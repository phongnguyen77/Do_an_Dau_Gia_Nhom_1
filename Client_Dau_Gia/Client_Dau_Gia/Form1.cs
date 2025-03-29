using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace Client_Dau_Gia
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private int highestBid = 0;
        private string highestBidder = "";
        private string connectionString = "Server=PHONG-IT;Database=AuctionDB;Trusted_Connection=True;";
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect("192.168.1.21", 8888); // Kết nối đến Server
                stream = client.GetStream();

                // Lấy địa chỉ IP và cổng của Client
                IPEndPoint localEndPoint = (IPEndPoint)client.Client.LocalEndPoint;
                stclient.AppendText($"Connected from {localEndPoint.Address}:{localEndPoint.Port}\r\n");

                // Nhận dữ liệu từ Server trên luồng riêng
                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}");
            }
        }

        private void btnBid_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("Bạn chưa kết nối tới Server!");
                return;
            }

            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Nhập tên của bạn!");
                return;
            }

            if (!int.TryParse(txtBid.Text, out int bidAmount) || bidAmount <= 0)
            {
                MessageBox.Show("Nhập một số hợp lệ!");
                return;
            }

            if (pictureBox.Image == null)
            {
                MessageBox.Show("Chọn một hình ảnh!");
                return;
            }

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                    byte[] imageBytes = ms.ToArray();

                    string message = $"{name}:{bidAmount}";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    byte[] data = new byte[messageBytes.Length + imageBytes.Length + 1];
                    data[0] = 1; // Flag to indicate image is included
                    Buffer.BlockCopy(messageBytes, 0, data, 1, messageBytes.Length);
                    Buffer.BlockCopy(imageBytes, 0, data, messageBytes.Length + 1, imageBytes.Length);

                    if (stream != null && stream.CanWrite)
                    {
                        stream.Write(data, 0, data.Length);
                        // Save bid to database
                        SaveBidToDatabase(name, bidAmount, imageBytes);
                    }
                    else
                    {
                        MessageBox.Show("Stream is not writable.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi dữ liệu: {ex.Message}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024 * 1024]; // Increase buffer size for image data
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (buffer[0] == 1) // Check if the first byte is the flag for image
                {
                    int messageLength = Array.IndexOf(buffer, (byte)0, 1) - 1;
                    string message = Encoding.UTF8.GetString(buffer, 1, messageLength);
                    string[] parts = message.Split(':');
                    string bidder = parts[0];
                    int bidAmount = int.Parse(parts[1]);

                    byte[] imageBytes = new byte[bytesRead - messageLength - 1];
                    Buffer.BlockCopy(buffer, messageLength + 1, imageBytes, 0, imageBytes.Length);

                    if (bidAmount > highestBid)
                    {
                        highestBid = bidAmount;
                        highestBidder = bidder;
                        SaveBidToDatabase(bidder, bidAmount, imageBytes);
                        BroadcastBid();
                    }
                }
                else
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // Handle non-image messages if needed
                }
            }

            client.Close();
        }

        // Hàm lưu vào SQL Server
        private void SaveBidToDatabase(string bidder, int bidAmount, byte[] imageBytes)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Bids (BidderName, BidAmount, BidTime, Image) VALUES (@Bidder, @BidAmount, GETDATE(), @Image)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Bidder", bidder);
                    cmd.Parameters.AddWithValue("@BidAmount", bidAmount);
                    cmd.Parameters.AddWithValue("@Image", imageBytes);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void BroadcastBid()
        {
            try
            {
                string message = $"Highest bid: {highestBid} by {highestBidder}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi phát sóng giá thầu: {ex.Message}");
            }
        }
        private void ReceiveData()
        {
            try
            {
                byte[] buffer = new byte[1024 * 1024]; // Increase buffer size for image data
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (buffer[0] == 1) // Check if the first byte is the flag for image
                    {
                        int messageLength = Array.IndexOf(buffer, (byte)0, 1) - 1;
                        string message = Encoding.UTF8.GetString(buffer, 1, messageLength);
                       

                        using (MemoryStream ms = new MemoryStream(buffer, messageLength + 1, bytesRead - messageLength - 1))
                        {
                            Image image = Image.FromStream(ms);
                            pictureBox.Image = image;
                        }
                    }
                    else
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhận dữ liệu: " + ex.Message);
            }

        }
        private void SendImage(TcpClient client, byte[] imageData)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);

                writer.Write(Encoding.UTF8.GetBytes("IMAGE"));  // Gửi flag báo là ảnh
                writer.Write(imageData.Length);                // Gửi kích thước ảnh
                writer.Write(imageData);                        // Gửi dữ liệu ảnh

                writer.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi ảnh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadBids()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT BidderName AS 'Người đấu giá', BidAmount AS 'Giá thầu', BidTime AS 'Thời gian', Image FROM Bids ORDER BY BidTime DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Thêm cột hình ảnh vào DataTable
                    DataColumn imageColumn = new DataColumn("Hình ảnh", typeof(Image));
                    dt.Columns.Add(imageColumn);

                    // Chuyển đổi dữ liệu hình ảnh từ byte sang Image
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Image"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])row["Image"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                row["Hình ảnh"] = Image.FromStream(ms);
                            }
                        }
                    }

                    // Gán dữ liệu vào DataGridView
                    dgvBids.DataSource = dt;

                    // Ẩn cột byte array gốc
                    dgvBids.Columns["Image"].Visible = false;
                }
            }
        }
        private void btnClearHistory_Click(object sender, EventArgs e)
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

                // Clear the DataGridView
                dgvBids.DataSource = null;
                MessageBox.Show("Lịch sử đấu giá đã được xóa.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa lịch sử đấu giá: " + ex.Message);
            }
        }
        private void btnLoadBids_Click(object sender, EventArgs e)
        {
            LoadBids();
        }



        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null && client.Connected)
                {
                    stream.Close();
                    client.Close();
                    stclient.AppendText("Disconnected from server\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ngắt kết nối: " + ex.Message);
            }
        }
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }
        private void RemoveImagePathColumn()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "ALTER TABLE Bids DROP COLUMN ImagePath";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("Database connection successful!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvBids_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
