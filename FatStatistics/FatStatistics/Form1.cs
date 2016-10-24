using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FatStatistics
{
    public partial class Form1 : Form
    {

        private string DBhost = "HOST";
        private string DBuser = "USER";
        private string DBpass = "PASS";
        private string DB = "DATABASE";

        int TechRes;
        int HumRes;
        int user;
        string userType;
        string userSex;

        public Form1()
        {
            InitializeComponent();
            getData();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (tech.Checked) user = 1;
            else if (hum.Checked) user = 0;

            if (weight.Text != "" && age.Text != "" && sex.SelectedIndex.ToString() != "-1")
            {

                MySqlCommand command = new MySqlCommand(); ;
                string connectionString, commandString;
                connectionString = "Data source=" + DBhost + ";UserId=" + DBuser + ";Password=" + DBpass + ";database=" + DB + ";";
                MySqlConnection connection = new MySqlConnection(connectionString);
                commandString = @"INSERT INTO `users` (`user_type`, `weight`, `age`, `sex`) 
                                VALUES ('" + user + "', '" + weight.Text + "', '" + age.Text + "', '" + sex.SelectedIndex.ToString() + "');";
                command.CommandText = commandString;
                command.Connection = connection;
                MySqlDataReader reader;

                try
                {
                    command.Connection.Open();
                    reader = command.ExecuteReader();
                    reader.Close();

                    if (Convert.ToInt32(user) == 1) userType = "технарь";
                    else userType = "гуманитарий";

                    if (Convert.ToInt32(sex.SelectedIndex) == 0) userSex = "М";
                    else userSex = "Ж";

                    int rowNumber = dataGridView1.Rows.Add();
                    
                    dataGridView1.Rows[rowNumber].Cells["user_type"].Value = userType;
                    dataGridView1.Rows[rowNumber].Cells["user_weight"].Value = weight.Text;
                    dataGridView1.Rows[rowNumber].Cells["user_age"].Value = age.Text;
                    dataGridView1.Rows[rowNumber].Cells["user_sex"].Value = userSex;


                    label.ForeColor = Color.Green;
                    label.Text = "Новые данные успешно добавлены!";
                    sex.SelectedIndex = -1;
                    age.Text = "";
                    weight.Text = "";

                }
                catch (MySqlException ex)
                {
                    label.ForeColor = Color.Red;
                    label.Text = ex.ToString();
                }
                finally
                {
                    command.Connection.Close();
                }

            }
            else
            {
                label.ForeColor = Color.Red;
                label.Text = "Вы не заполнили одно из полей!";
            }


        }

        private void getData()
        {

            MySqlCommand command = new MySqlCommand(); ;
            string connectionString, commandString;
            connectionString = "Data source=" + DBhost + ";UserId=" + DBuser + ";Password=" + DBpass + ";database=" + DB + ";";
            MySqlConnection connection = new MySqlConnection(connectionString);
            commandString = "SELECT * FROM `users`;";
            command.CommandText = commandString;
            command.Connection = connection;
            MySqlDataReader reader;
            try
            {
                command.Connection.Open();
                reader = command.ExecuteReader();
                TechRes = 0;
                HumRes = 0;
                while (reader.Read())
                {

                    if (Convert.ToInt32(reader["user_type"]) == 1) userType = "технарь";
                    else userType = "гуманитарий";

                    if (Convert.ToInt32(reader["sex"]) == 0) userSex = "М";
                    else userSex = "Ж";

                    int rowNumber = dataGridView1.Rows.Add();
                    
                    dataGridView1.Rows[rowNumber].Cells["user_type"].Value = userType;
                    dataGridView1.Rows[rowNumber].Cells["user_weight"].Value = reader["weight"];
                    dataGridView1.Rows[rowNumber].Cells["user_age"].Value = reader["age"];
                    dataGridView1.Rows[rowNumber].Cells["user_sex"].Value = userSex;

                    /*----------------*/

                    

                    if (Convert.ToInt32(reader["user_type"]) == 1) TechRes += Convert.ToInt32(reader["weight"]);
                    else if (Convert.ToInt32(reader["user_type"]) == 0) HumRes += Convert.ToInt32(reader["weight"]);



                }

                if (TechRes < HumRes) result.Text = "Технари легче, чем гуманитарии!";
                else if (TechRes > HumRes) result.Text = "Технари тяжелее, чем гуманитарии!";
                else if (TechRes == HumRes) result.Text = "Технари и гуманитарии весят одинакого!";

                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                command.Connection.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
