using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Server_Dau_Gia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private int highestBid = 0;
        private string highestBidder = "No bids yet";
        private string connectionString = "Server=PHONG-IT;Database=AuctionDB;Trusted_Connection=True;";
        private void btnStartServer_Click(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(StartServer);
            serverThread.IsBackground = true;
            serverThread.Start();
            stserver.AppendText("Server started...\r\n");
        }
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            TestConnection();
        }

        private void TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Kết nối SQL Server thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Lỗi SQL Server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            StopServer();
            stserver.AppendText("Server stopped...\r\n");
        }
        private void StartServer()
        {
            try
            {
                IPAddress localIP = GetLocalIPAddress();
                int port = 8888;

                server = new TcpListener(localIP, port);
                server.Start();

                stserver.AppendText($"Server started on {localIP}:{port}\r\n");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client);
                    UpdateClientList(); // Cập nhật danh sách client
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi động server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateClientList()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateClientList));
                return;
            }

            txtIP.Clear();
            foreach (var client in clients)
            {
                if (client.Connected)
                {
                    IPEndPoint endPoint = client.Client.RemoteEndPoint as IPEndPoint;
                    if (endPoint != null)
                    {
                        txtIP.AppendText($"{endPoint.Address}:{endPoint.Port}\r\n");
                    }
                }
            }
        }

        private void StopServer()
        {
            if (server != null)
            {
                try
                {
                    foreach (var client in clients)
                    {
                        client.Close();
                    }
                    clients.Clear();

                    server.Stop();
                    server = null;
                    stserver.AppendText("Server stopped...\r\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi dừng server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Server chưa khởi động!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // Lấy địa chỉ IP của máy chủ
        private IPAddress GetLocalIPAddress()
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            foreach (IPAddress ip in addresses)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
                    return ip;
            }
            return IPAddress.Loopback; // 127.0.0.1 nếu không tìm thấy IP
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);

                // Read the data type (TEXT or IMAGE)
                string dataType = Encoding.UTF8.GetString(reader.ReadBytes(5));

                if (dataType.StartsWith("IMAGE"))
                {
                    int imageSize = reader.ReadInt32();  // Read the image size
                    byte[] imageData = reader.ReadBytes(imageSize); // Read the image data

                    if (imageData.Length == imageSize)
                    {
                        DisplayReceivedImage(imageData);
                        BroadcastImage(imageData); // Broadcast the image to all connected clients
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                clients.Remove(client);
                UpdateClientList(); // Update the list of connected clients
            }
        }

        private void btnSendAuctionImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    byte[] imageData = File.ReadAllBytes(filePath);
                    DisplayReceivedImage(imageData); // Display the image on the server
                    BroadcastImage(imageData); // Broadcast the image to all connected clients
                }
            }
        }

        private void DisplayReceivedImage(byte[] imageBytes)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<byte[]>(DisplayReceivedImage), imageBytes);
                return;
            }

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                pictureBox1.Image = Image.FromStream(ms);
            }
        }

        private void BroadcastImage(byte[] imageData)
        {
            List<TcpClient> disconnectedClients = new List<TcpClient>();

            foreach (var client in clients)
            {
                try
                {
                    if (client.Connected)
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(Encoding.UTF8.GetBytes("IMAGE:"), 0, 6);
                        stream.Write(imageData, 0, imageData.Length);
                    }
                    else
                    {
                        disconnectedClients.Add(client);
                    }
                }
                catch
                {
                    disconnectedClients.Add(client);
                }
            }

            foreach (var client in disconnectedClients)
            {
                clients.Remove(client);
            }
        }


        private void BroadcastBid()
        {
            string message = $"Highest Bid: {highestBid} by {highestBidder}";
            txtLog.AppendText(message + "\r\n");

            byte[] data = Encoding.UTF8.GetBytes(message);
            List<TcpClient> disconnectedClients = new List<TcpClient>();

            foreach (var client in clients)
            {
                try
                {
                    if (client.Connected)
                    {
                        client.GetStream().Write(data, 0, data.Length);
                    }
                    else
                    {
                        disconnectedClients.Add(client);
                    }
                }
                catch
                {
                    disconnectedClients.Add(client);
                }
            }

            foreach (var client in disconnectedClients)
            {
                clients.Remove(client);
            }
        }
        private void LoadBidders()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BidderName AS 'Người đấu giá', BidAmount AS 'Giá thầu', BidTime AS 'Thời gian', Image FROM Bids ORDER BY BidAmount DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Add a new column to store Image objects for display
                    dt.Columns.Add(new DataColumn("ImageDisplay", typeof(Image)));

                    // Convert the Image column from byte array to Image
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Image"] != DBNull.Value)
                        {
                            byte[] imageData = (byte[])row["Image"];
                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                row["ImageDisplay"] = Image.FromStream(ms);
                                pictureBox1.Image = Image.FromStream(ms);
                            }
                        }
                    }

                    dgvBidders.DataSource = dt;
                }
            }
        }
        private void btnLoadBidders_Click(object sender, EventArgs e)
        {
            LoadBidders();
        }

        private void SaveBidToDatabase(string bidder, int bidAmount, byte[] imageData)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Bids (BidderName, BidAmount, BidTime, Image) VALUES (@Bidder, @BidAmount, GETDATE(), @Image)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Bidder", bidder);
                        cmd.Parameters.AddWithValue("@BidAmount", bidAmount);
                        cmd.Parameters.AddWithValue("@Image", (object)imageData ?? DBNull.Value); // Nếu không có ảnh, lưu NULL
                        cmd.ExecuteNonQuery();
                    }
                }
                txtLog.AppendText($"Đã lưu {bidder} với giá thầu {bidAmount} vào database.\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu vào database: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteHistory_Click(object sender, EventArgs e)
        {
            DeleteAuctionHistory();
        }

        private void DeleteAuctionHistory()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Bids";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                txtLog.AppendText("Đã xóa lịch sử đấu giá.\r\n");
                LoadBidders(); // Refresh the data grid view
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa lịch sử đấu giá: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvBidders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
