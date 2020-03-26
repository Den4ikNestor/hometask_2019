using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.IO;


namespace AddDataOnDatabase
{
    public partial class Form1 : Form
    {
        private string path;
        private List<List<string>> dataMatrix = new List<List<string>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                path = textBox1.Text = openFileDialog1.FileName;
        }

        private void CreateDataMatrix()
        {
            var svcFileText = File.ReadAllText(path);
            var buf = svcFileText.Split('\n');

            for (int i = 0; i < buf.Length-1; i++)
            {
                //Убираем лишние символы
                buf[i] = buf[i].Replace(", ", ",");
                buf[i] = buf[i].Replace("\r", "");
                buf[i] = buf[i].Replace(" (", ",");
                buf[i] = buf[i].Replace(")", "");
                //Добавляем в массив
                dataMatrix.Add(buf[i].Split(',').ToList());

                if (dataMatrix[i].Count < 3)
                    if (i == 0) dataMatrix[i].Add("Year");
                    else dataMatrix[i].Add("0");

                if (dataMatrix[i][2].Length > 4)
                    dataMatrix[i][2] = dataMatrix[i][2].Remove(4);
            }
        }

        private void FillingDatabase()
        {
            string connectionStr = "Server=localhost ; Port=5454; User Id = postgres;Password = su;Database = methods_of_information_retrieval_mirvoda;";
            NpgsqlConnection connection = new NpgsqlConnection(connectionStr);
   

            for(int i = 1; i<dataMatrix.Count;i++)
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("insert into movies values (@id, @year, @name)", connection);
                command.Parameters.AddWithValue("@id", dataMatrix[i][0]);
                command.Parameters.AddWithValue("@year", dataMatrix[i][2]);
                command.Parameters.AddWithValue("@name", dataMatrix[i][1]);
                command.ExecuteNonQuery();
                connection.Close();
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreateDataMatrix();
            FillingDatabase();
        }
    }
}
