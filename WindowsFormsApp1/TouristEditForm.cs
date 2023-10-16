using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class TouristEditForm : Form
    {
        public TouristEditForm()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string touristCode = textBox5.Text; // Код туриста
            string name = textBox1.Text;
            string birthDate = textBox2.Text;
            string otherData = textBox3.Text;


            // Далее, вы можете использовать данные, чтобы сохранить их в базе данных.
            // Создайте соединение с базой данных, используя строку подключения из конфигурации.
            string connectionString = ConfigurationManager.AppSettings["connectionStr"];
            SqlConnection connection = new SqlConnection(connectionString);

            // Создайте команду SQL для вставки данных в таблицу "Туристы".
            string sql = "INSERT INTO Туристы ([Серия паспорта], Фамилия, Имя, Отчество, Фото) VALUES (@СерияПаспорта, @Фамилия, @Имя, @Отчество, @Фото)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@СерияПаспорта", touristCode); // Поле "Код туриста"
            command.Parameters.AddWithValue("@Фамилия", name); // Поле "Фамилия"
            command.Parameters.AddWithValue("@Имя", birthDate); // Поле "Имя"
            command.Parameters.AddWithValue("@Отчество", otherData); // Поле "Отчество"

            // Загрузка изображения в виде массива байтов в параметр @Фото
            if (pictureBox1.Image != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] photoData = ms.ToArray();
                command.Parameters.Add(new SqlParameter("@Фото", SqlDbType.VarBinary, -1)).Value = photoData;
            }
            else
            {
                command.Parameters.Add(new SqlParameter("@Фото", SqlDbType.VarBinary, -1)).Value = DBNull.Value;
            }

            try
            {
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Данные успешно сохранены в таблицу 'Туристы'.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении данных: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Показать диалоговое окно открытия файла
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
            openFileDialog1.Title = "Выберите изображение";
            openFileDialog1.FileName = ""; // Устанавливает начальное имя файла (необязательно)

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog1.FileName;
                // Здесь вы можете использовать selectedImagePath для загрузки изображения.
                // Загрузите изображение
                Image selectedImage = new Bitmap(selectedImagePath);

                // Масштабируйте изображение
                Image scaledImage = ScaleImage(selectedImage, pictureBox1.Width, pictureBox1.Height);
                // Например, вы можете отобразить изображение в PictureBox.
                pictureBox1.Image = new Bitmap(selectedImagePath);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
