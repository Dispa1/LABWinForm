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
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class TouristEditForm2 : Form
    {
        public TouristEditForm2()
        {
            InitializeComponent();
        }

        private void TouristEditForm2_Load(object sender, EventArgs e)
        {
        }

        public void LoadData(string touristID, string name, string birthDate, string otherData, Image photo)
        {
            // Загрузите данные из выбранной строки в текстовые поля формы
            textBox5.Text = touristID;
            textBox1.Text = name;
            textBox2.Text = birthDate;
            textBox3.Text = otherData;
            pictureBox1.Image = photo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string touristID = textBox5.Text; // Код туриста
            string name = textBox1.Text;
            string birthDate = textBox2.Text;
            string otherData = textBox3.Text;


            // Создайте соединение с базой данных, используя строку подключения из конфигурации.
            string connectionString = ConfigurationManager.AppSettings["connectionStr"];
            SqlConnection connection = new SqlConnection(connectionString);

            // Создайте команду SQL для обновления данных в таблице "Туристы".
            string sql = "UPDATE Туристы SET Фамилия = @Фамилия, Имя = @Имя, Отчество = @Отчество, Фото = @Фото WHERE [Серия паспорта] = @СерияПаспорта";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@СерияПаспорта", touristID); // Условие WHERE
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
                    MessageBox.Show("Данные успешно обновлены в таблице 'Туристы'.");
                }
                else
                {
                    MessageBox.Show("Не удалось обновить данные. Возможно, запись с указанным 'Серия паспорта' не найдена.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при обновлении данных: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
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
                // Здесь вы можете использовать selectedImagePath для загрузки новой фотографии.
                // Загрузите новую фотографию
                Image selectedImage = new Bitmap(selectedImagePath);

                // Масштабируйте изображение
                Image scaledImage = ScaleImage(selectedImage, pictureBox1.Width, pictureBox1.Height);

                // Установите масштабированное изображение в PictureBox
                pictureBox1.Image = scaledImage;
            }
        }

       

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
