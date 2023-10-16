using AppConnectionFactory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DataSet ds;

        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ds = MyProviderFactory.GetMyConn();



            // Отобразите таблицу Туристы в dataGridView1
            DataViewManager dsview = ds.DefaultViewManager;
            dataGridView1.DataSource = dsview;
            dataGridView1.DataMember = "Туристы";

            // Отобразите таблицу Туры в dataGridView2
            //DataView dvTours = ds.Tables["Туры"].DefaultView;
            //dataGridView2.DataSource = dvTours;

            // Отобразите таблицу "Информация о туристах" в dataGridView3
            //DataView dvInfoTourists = ds.Tables["Информация о туристах"].DefaultView;
            //dataGridView3.DataSource = dvInfoTourists;

            // Отобразите другие таблицы, если они есть, аналогичным образом
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


        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Фото"].Index && e.RowIndex >= 0)
            {
                // Получите значение из ячейки
                Image cellValue = e.Value as Image;

                if (cellValue != null)
                {
                    // Получите размер ячейки
                    int cellWidth = dataGridView1.Columns["Фото"].Width;
                    int cellHeight = dataGridView1.Rows[e.RowIndex].Height;

                    // Масштабируйте изображение под размер ячейки
                    Image scaledImage = ScaleImage(cellValue, cellWidth, cellHeight);

                    // Установите масштабированное изображение в ячейку
                    e.Value = scaledImage;
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TouristEditForm editForm = new TouristEditForm();
            editForm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем выбранную строку
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Получаем данные из выбранной строки
                string touristID = selectedRow.Cells["Серия паспорта"].Value.ToString();
                string name = selectedRow.Cells["Фамилия"].Value.ToString();
                string birthDate = selectedRow.Cells["Имя"].Value.ToString();
                string otherData = selectedRow.Cells["Отчество"].Value.ToString();

                // Получаем изображение из выбранной строки
                Image photo = selectedRow.Cells["Фото"].Value as Image;

                // Создаем новую форму редактирования и передаем данные и фотографию
                TouristEditForm2 editForm = new TouristEditForm2();
                editForm.LoadData(touristID, name, birthDate, otherData, photo);

                // Открываем форму редактирования
                editForm.Show();

            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Проверяем, что в `dataGridView1` выбрана хотя бы одна строка
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем выбранную строку
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Получаем `Серия паспорта` (или другой уникальный идентификатор) из выбранной строки
                string touristCode = selectedRow.Cells["Серия паспорта"].Value.ToString();

                // Создайте соединение с базой данных, используя строку подключения из конфигурации.
                string connectionString = ConfigurationManager.AppSettings["connectionStr"];
                SqlConnection connection = new SqlConnection(connectionString);

                // Создайте команду SQL для удаления данных из таблицы "Туристы".
                string sql = "DELETE FROM Туристы WHERE [Серия паспорта] = @СерияПаспорта";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@СерияПаспорта", touristCode); // Условие WHERE

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Запись успешно удалена из таблицы 'Туристы'.");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти запись для удаления.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при удалении записи: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                // После удаления записи, обновите отображение данных в dataGridView1
                button1_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.");
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
           
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
