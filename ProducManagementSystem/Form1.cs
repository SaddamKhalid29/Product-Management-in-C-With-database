using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;

namespace ProducManagementSystem
{
    public partial class Form1 : Form
    {
        List<Products> products =new List<Products>();
        Products CurrentProduct;
        int currentIndex = -1;

        //For while loop in database show record
        int count=0;
        public Form1()
        {
            InitializeComponent();
        }

     //   DatabaseConnectivity db=new DatabaseConnectivity();
        private void Form1_Load(object sender, EventArgs e)
        {
            //if (File.Exists("products.txt"))
            //    ReadDataFromFile();

            show();
            if (products.Count > 0)
            {
                currentIndex = products.Count - 1;
                CurrentProduct = products[currentIndex];

            }
            if (CurrentProduct != null)
              // FromDataToUI();
              
            EnableControls();
            sqlRowCheck();
        }
        private void EnableControls()
        {
            if (CurrentProduct == null)
            {
                btn_Delete.Enabled = false;
                btn_next.Enabled = false;
                btn_prev.Enabled = false;
                btn_Save.Enabled = false;
                btnUpdate.Enabled = false;
                txtID.Enabled = false;
                txtName.Enabled = false;
                txtPrice.Enabled = false;
                txtQty.Enabled = false;
                comboBoxWeight.Enabled = false;
                Manufact.Enabled = false;
                ExpiryDate.Enabled = false;
                btnBrowse.Enabled = false;
                pictureBox.Enabled=false;
            }
            else
            {
                // btn_Delete.Enabled = false;
                btn_Save.Enabled = true;
               
                txtID.Enabled = true;
                txtName.Enabled = true;
                txtPrice.Enabled = true;
                txtQty.Enabled = true;
                comboBoxWeight.Enabled = true;
                Manufact.Enabled = true;
                ExpiryDate.Enabled = true;
                btnBrowse.Enabled = true;
                pictureBox.Enabled = true;
                btn_next.Enabled=true;
                btn_prev.Enabled=true;  
               
            }
            //if (products.Count >= 1)
            //{
            //    btn_Delete.Enabled=true;
            //    btnUpdate.Enabled = true;
            //}
            //if (currentIndex > 0)
            //{
            //    btn_prev.Enabled = true;
            //}
            //if (currentIndex <= 0)
            //{
            //    btn_prev.Enabled=false;
            //}
            //if (currentIndex == products.Count)
            //{
            //    btn_next.Enabled=false;
            //}
            
            //if(currentIndex < products.Count - 1)
            //{
            //    btn_next.Enabled = true;
            //}
            //else
            //{
            //    btn_next.Enabled = false;
            //}
        }
        
        private void btn_new_Click(object sender, EventArgs e)
        {
            CurrentProduct =new Products();
            currentIndex = products.Count();
            EnableControls();
             FromDataToUI();
            


        }

        private void FromDataToUI()
        {
            txtID.Text = Convert.ToString(CurrentProduct.Product_ID);
            txtName.Text = CurrentProduct.Product_Name;
            txtPrice.Text = Convert.ToString(CurrentProduct.Product_Price);
            txtQty.Text = Convert.ToString(CurrentProduct.Produt_Quantity);
            //string weight = Convert.ToString(comboBoxWeight.SelectedIndex);
            //weight = CurrentProduct.Product_Weight;
            comboBoxWeight.SelectedIndex =CurrentProduct.product_Weight == Products.PRODUCTWEIGHT._10mg ? 0 : 1;
            Manufact.Value = CurrentProduct.Product_ManufacturingDate;
            ExpiryDate.Value = CurrentProduct.Product_ExpiryDate;
            ImagePathLocation.Text = CurrentProduct.ImageLocation;
            pictureBox.ImageLocation = CurrentProduct.ImageLocation;
            
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

            insert();
            //FromUIToData();
            if (currentIndex == products.Count)
            {
                products.Add(CurrentProduct);
            }


            //  WriteDataToFile();
        }

        private void FromUIToData()
        {
            CurrentProduct.Product_ID=Convert.ToInt32(txtID.Text);
            CurrentProduct.Product_Name = txtName.Text;
            CurrentProduct.Product_Price=Convert.ToInt32(txtPrice.Text);
            CurrentProduct.Produt_Quantity=Convert.ToInt32(txtQty.Text);
            //CurrentProduct.Product_Weight = comboBoxWeight.Text;
            CurrentProduct.product_Weight = comboBoxWeight.SelectedIndex == 0 ? Products.PRODUCTWEIGHT._10mg : Products.PRODUCTWEIGHT._200mg;
            CurrentProduct.Product_ManufacturingDate = Manufact.Value;
            CurrentProduct.Product_ExpiryDate = ExpiryDate.Value;
            CurrentProduct.ImageLocation= ImagePathLocation.Text;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd=new OpenFileDialog();
            ofd.Filter = "Image Files (*.png)|*.png|(*.jpg)|*.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox.ImageLocation= ofd.FileName;
                ImagePathLocation.Text= ofd.FileName;
            }
            else
            {
                MessageBox.Show("Image Selection is cancelled","Cancellation",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            count = count - 1;
            PreviousRecord();
            //if (currentIndex <= 0)
            //{
            //    return;
            //}
             //currentIndex--;
            //CurrentProduct = products[currentIndex];
           // FromDataToUI();
           // EnableControls();

        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            count = count + 1;
            NextRecord();
           // if (currentIndex >= products.Count - 1)
           // {

           //     return;
           // }
           currentIndex++;
           // CurrentProduct = products[currentIndex];
           //FromDataToUI();
            EnableControls();
        }

        public void WriteDataToFile()
        {
            FileStream filestream = new FileStream("products.txt", FileMode.OpenOrCreate);
            TextWriter textWriter = new StreamWriter(filestream);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Products>));
            xmlSerializer.Serialize(textWriter, products);
            textWriter.Close();
            filestream.Close();
        }
        public void ReadDataFromFile()
        {


            FileStream filestream = new FileStream("products.txt", FileMode.OpenOrCreate);
            TextReader textReader = new StreamReader(filestream);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Products>));
            products = (List<Products>)xmlSerializer.Deserialize(textReader);


            textReader.Close();
            filestream.Close();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {

            delete();
            show();

            //if(products.Count != 0)
            //{
            //    products.RemoveAt(currentIndex);
            //    currentIndex = currentIndex + 1;
           //     List<Products> list = new List<Products>();
           //     products.Add(products[currentIndex]);
           //     //  currentIndex--;
                
           //     MessageBox.Show("Product is removed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

           // }
           // if (products.Count != 0)
           // {
           //     foreach(Products product in products)
           //     {
           //         //if (product.Product_ID == CurrentProduct.Product_ID)
           //         //{
           //         //    //products.Remove(product);
           //         //    //  currentIndex--;
           //         //    //currentIndex = products.Count;
                       
           //         //   //  break;
           //         //}

           //         WriteDataToFile();
                    

           //     }
             
           // }
           // if (products.Count == 0)
           // {
           //     MessageBox.Show("There's is no product to delete.");
           // }
           // else
           // {
           //     currentIndex = 0;
           //     CurrentProduct = products[currentIndex];
           // }
           //FromDataToUI();
           //// WriteDataToFile();
           // EnableControls();
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {

            Search_byID();
            //int searchId = Convert.ToInt32(txtSearchByID.Text);
            //int count = 0;
            //bool flag = false;
            //foreach(Products product in products)
            //{
            //    if (searchId== product.Product_ID)
            //    {
            //        CurrentProduct = product;
            //        currentIndex = count;
                   
            //      //  FromDataToUI();
            //        EnableControls();
            //        flag = true;
            //        break;

            //    }
                
            //    count++;
            //}
            //if (flag == false)
            //{
            //    MessageBox.Show("This entered id record doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        public void show()
        {

            try
            {

                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                MessageBox.Show("Connection is established!", "Connected");

                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = string.Format("Select * from Products");

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    CurrentProduct = new Products();

                    while (sqlDataReader.Read())
                    {

                        txtID.Text = Convert.ToString(sqlDataReader.GetInt32(0));
                        txtName.Text = sqlDataReader.GetString(1);
                        txtPrice.Text = Convert.ToString(sqlDataReader.GetInt32(2));
                        txtQty.Text = Convert.ToString(sqlDataReader.GetInt32(3));
                        comboBoxWeight.Text = Convert.ToString(sqlDataReader.GetInt32(4));
                        Manufact.Value = (DateTime)sqlDataReader.GetSqlValue(5);
                        ExpiryDate.Value = (DateTime)sqlDataReader.GetSqlValue(6);
                        ImagePathLocation.Text = sqlDataReader.GetString(7);
                        pictureBox.ImageLocation = ImagePathLocation.Text;
                        count++;
                    }
                    
                }
                sqlConnection.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void update()
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                MessageBox.Show("Connection is established!", "Connectivity");

                SqlCommand sqlcommand = sqlConnection.CreateCommand();

                sqlcommand.Connection = sqlConnection;


                //SQL Query to insert date
                //insert into Products values(103,'Gabs',200,2,34,'2021 Feb 22','2022 Jan 22','\Downloads\Pencil') ;

                sqlcommand.CommandText = string.Format("update Products "+
                    "set Product_Name='{0}',Product_Price={1},Product_Quantity={2},Product_Weight={3},Manufacturing_Date='{4}',Expiry_date='{5}',Image_Path='{6}' where (Product_id={7})", txtName.Text, txtPrice.Text, txtQty.Text, comboBoxWeight.Text, Manufact.Value,ExpiryDate.Value,ImagePathLocation.Text,txtID.Text);
                
       

                int row = sqlcommand.ExecuteNonQuery();

                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            update();
        }

        public void insert()
        {
            try
            {

                //Creating an object of sqlconnection
                SqlConnection sqlConnection = new SqlConnection();

                //Setting up the strng
                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";
               
                //Opening the connection
                sqlConnection.Open();

                //Checking the connection if it is open
                MessageBox.Show("Connection is open", "Connectivity");

                // To write the sql command
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                //setting the command connection with sql
                sqlCommand.Connection = sqlConnection;

                //setting up the command
                sqlCommand.CommandText = string.Format("insert into Products" +
                    "(Product_id,Product_Name,Product_Price,Product_Quantity,Product_Weight,Manufacturing_Date,Expiry_date,Image_Path)" +
                    "values ({0},'{1}',{2},{3},{4},'{5}','{6}','{7}')", txtID.Text, txtName.Text, txtPrice.Text,txtQty.Text,comboBoxWeight.Text,Manufact.Value,ExpiryDate.Value,ImagePathLocation.Text);
               
                //Reading data from sql
                int rows = sqlCommand.ExecuteNonQuery();
               
                //Closing the connection
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void delete()
        {
            try
            {

                //Creating an object of sqlconnection
                SqlConnection sqlConnection = new SqlConnection();

                //Setting up the strng
                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";
               
                //Opening the connection
                sqlConnection.Open();
               
                // To write the sql command
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                //setting the command connection with sql
                sqlCommand.Connection = sqlConnection;

                //setting up the command
                sqlCommand.CommandText = string.Format("delete from Products where (Product_id={0})", txtID.Text);

                //Reading data from sql
                int rows = sqlCommand.ExecuteNonQuery();
               
                //Closing the connection
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Search_byID()
        {
            try
            {

                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                MessageBox.Show("Connection is established!", "Connected");

                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = string.Format("Select * from Products where Product_id={0}",txtSearchByID.Text);

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    CurrentProduct = new Products();

                    while (sqlDataReader.Read())
                    {

                        txtID.Text = Convert.ToString(sqlDataReader.GetInt32(0));
                        txtName.Text = sqlDataReader.GetString(1);
                        txtPrice.Text = Convert.ToString(sqlDataReader.GetInt32(2));
                        txtQty.Text = Convert.ToString(sqlDataReader.GetInt32(3));
                        comboBoxWeight.Text = Convert.ToString(sqlDataReader.GetInt32(4));
                        Manufact.Value = (DateTime)sqlDataReader.GetSqlValue(5);
                        ExpiryDate.Value = (DateTime)sqlDataReader.GetSqlValue(6);
                        ImagePathLocation.Text = sqlDataReader.GetString(7);
                        pictureBox.ImageLocation = ImagePathLocation.Text;
                    }

                }
                else
                {
                    MessageBox.Show("The existing record do not exist","No Record Found");
                }
                
                sqlConnection.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void NextRecord()
        {
            try
            {

                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = string.Format("Select * from Products");

                int index=0;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    CurrentProduct = new Products();

                    while (sqlDataReader.Read() && index<count)
                    {

                        txtID.Text = Convert.ToString(sqlDataReader.GetInt32(0));
                        txtName.Text = sqlDataReader.GetString(1);
                        txtPrice.Text = Convert.ToString(sqlDataReader.GetInt32(2));
                        txtQty.Text = Convert.ToString(sqlDataReader.GetInt32(3));
                        comboBoxWeight.Text = Convert.ToString(sqlDataReader.GetInt32(4));
                        Manufact.Value = (DateTime)sqlDataReader.GetSqlValue(5);
                        ExpiryDate.Value = (DateTime)sqlDataReader.GetSqlValue(6);
                        ImagePathLocation.Text = sqlDataReader.GetString(7);
                        pictureBox.ImageLocation = ImagePathLocation.Text;
                        index++;
                        
                    }
                
                }
                sqlConnection.Close();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PreviousRecord()
        {
            try
            {

                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = string.Format("Select * from Products");

                int index = 0;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    CurrentProduct = new Products();

                    while (sqlDataReader.Read() && index <= count)
                    {

                        txtID.Text = Convert.ToString(sqlDataReader.GetInt32(0));
                        txtName.Text = sqlDataReader.GetString(1);
                        txtPrice.Text = Convert.ToString(sqlDataReader.GetInt32(2));
                        txtQty.Text = Convert.ToString(sqlDataReader.GetInt32(3));
                        comboBoxWeight.Text = Convert.ToString(sqlDataReader.GetInt32(4));
                        Manufact.Value = (DateTime)sqlDataReader.GetSqlValue(5);
                        ExpiryDate.Value = (DateTime)sqlDataReader.GetSqlValue(6);
                        ImagePathLocation.Text = sqlDataReader.GetString(7);
                        pictureBox.ImageLocation = ImagePathLocation.Text;
                        index++;

                    }
                }
                
                sqlConnection.Close();
   }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void sqlRowCheck()
        {
            try
            {

                SqlConnection sqlConnection = new SqlConnection();

                sqlConnection.ConnectionString = "Data Source=DESKTOP-F42J1IK;Initial Catalog=Product_Management_System;Integrated Security=True";

                sqlConnection.Open();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;

                sqlCommand.CommandText = string.Format("Select * from Products");

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();



                if (sqlDataReader.HasRows)
                {
                    CurrentProduct = new Products();
                    btn_next.Enabled = true;
                    btn_prev.Enabled = true;
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
