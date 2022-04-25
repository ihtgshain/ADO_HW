using HomeWork_All;
using MyHW.Properties;
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

namespace MyHomeWork
{
    public partial class FrmLogon : Form
    {
        public FrmLogon()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;

            try
            {
                using(SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand cmdCheck = new SqlCommand();
                    cmdCheck.CommandText = "checkExistence";
                    cmdCheck.Connection = conn;
                    cmdCheck.Parameters.Add("@UserName", SqlDbType.NVarChar, 16).Value = userName;
                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmdCheck.ExecuteReader();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("該使用者已存在，請重新取名");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "InsertMember";
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@userName", SqlDbType.NVarChar, 16).Value = userName;
                    cmd.Parameters.Add("@password", SqlDbType.NVarChar, 40).Value = password;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Insert Member successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void OK_Click(object sender, EventArgs e)
        {
            string userName = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.Default.NorthwindConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "LogOnMember";
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 16).Value = userName;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 40).Value = password;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("LogOn successful");
                        Form1 f = new Form1();
                        f.Show();


                    }
                    else
                    {
                        MessageBox.Show("LogOn failed");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SqlConnection conn2 = new SqlConnection(Settings.Default.NorthwindConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter($"select * from MyMember where userName='{userName}' and password='{password}'", conn2);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            UsernameTextBox.Text = PasswordTextBox.Text = "";
        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
