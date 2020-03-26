using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Laba1
{

    public partial class Form1 : Form
    {
        string connectionStr = "Server=localhost ; Port=5454; User Id = postgres;Password = su;Database = methods_of_information_retrieval_mirvoda;";



        public Form1()
        {
            InitializeComponent();
        }

        //0 - ищем по названию(частично и не очень), 1 - ищем по году, 2 - ищем по году и имени.
        private string[] GetSearchElements()
        {
            string year = "", name = "";

            string text = textBox1.Text;
            int numCount = 0;

            for (int i = 0; i < text.Length; i++)
            {

                if (text[i] >= '0' && text[i] <= '9')
                {
                    numCount++;
                    if (numCount == 4)
                        year = text.Substring(i - 3, 4);
                }
                else numCount = 0;
            }


            if (name != "")
            {
                if (name[0] == ' ') name = name.Remove(0, 1);
                if (name[name.Length - 1] == ' ') name = name.Remove(name.Length - 1, 1);
            }
            if (year != "") name = text.Replace(year, "");
            else name = text;

            return new string[] { name, year };
        }
        private void Search()
        {
            string[] elements = GetSearchElements();
            string name = elements[0];
            string year = elements[1];

            string request = "SELECT * FROM  movies WHERE ";
            if (name.Length != 0 && year.Length != 0)
            {
                //ищем по имени и году
                request += "name LIKE '%' || \'" + name + "\' || '%' and year =" + year + " LIMIT 10";
            }
            else if (name.Length != 0)
            {
                //ищем по названию
                request += "name LIKE '%' || \'" + name + "\' || '%' LIMIT 10";
            }
            else
            {
                //ищем по году
                request += "year =" + year + " LIMIT 10";
            }

            
            NpgsqlConnection connection = new NpgsqlConnection(connectionStr);
            connection.Open();
            var command = new NpgsqlCommand(request, connection);

            NpgsqlDataReader reader = command.ExecuteReader();



            while (reader.Read())
            {
                dataGridView1.Rows.Add(reader[0], reader.GetString(2), reader[1]);
            }

            connection.Close();

            MessageBox.Show("Поиск завершен");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            if (textBox1.Text == "" || textBox1.Text.Replace(" ", "") == "")
                MessageBox.Show("Поле поиска не заполнено");
            else
                Search();

        }
    }
}