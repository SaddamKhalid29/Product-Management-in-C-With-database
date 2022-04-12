using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProducManagementSystem
{
    public class DatabaseConnectivity : Form1
    {

        public void show()
        {
            try
            {
                Form1 form = new Form1();

                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                MessageBox.Show("Connection is established!", "Connected");

                SqlCommand sqlCommand=sqlConnection.CreateCommand();

                sqlCommand.Connection=sqlConnection;

                sqlCommand.CommandText=string.Format("Select * from Products");

                SqlDataReader sqlDataReader=sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {

                    while (sqlDataReader.Read()) {
                       
                      form.txtID.Text=Convert.ToString(sqlDataReader.GetInt32(0));
                      form.txtName.Text=sqlDataReader.GetString(1);
                      form.txtPrice.Text= Convert.ToString(sqlDataReader.GetInt32(2));
                      form.txtQty.Text= Convert.ToString(sqlDataReader.GetInt32(3));
                      form.comboBoxWeight.Text=Convert.ToString(sqlDataReader.GetInt32(4));
                      form.Manufact.Value= (DateTime)sqlDataReader.GetSqlValue(5);
                      form.ExpiryDate.Value= (DateTime)sqlDataReader.GetSqlValue(6);
                      form.ImagePathLocation.Text=sqlDataReader.GetString(7);
                    }

                }
                sqlConnection.Close();
               
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }
    }
}
