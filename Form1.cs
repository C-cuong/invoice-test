using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace invoice_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void bt_add_pic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Choose Image(*.jpg;*.jpeg;*.pnp)|*.jpg;*.jpeg;*.pnp";
            if(ofd.ShowDialog() == DialogResult.OK )
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dgvItems.Rows.Add(tb_ItemID.Text, tb_InvoiceID.Text, tb_ProductID.Text, tb_Quantity.Text, tb_UnitPrice.Text,pictureBox1.Image);
            tb_ItemID.Clear();
            tb_ProductID.Clear(); 
            tb_Quantity.Clear();
            tb_UnitPrice.Clear();
            pictureBox1.Image=null;
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void btnSaveInvoice_Click(object sender, EventArgs e)
        {


            string connectionString = "Data Source=(LocalDb)\\Local;Initial Catalog=\"update database test\";Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insert invoice
                string insertInvoiceQuery = "INSERT INTO Invoices (InvoiceID, InvoiceDate, SupplierID, TotalAmount) VALUES (@InvoiceID, @InvoiceDate, @SupplierID, @TotalAmount)";
                using (SqlCommand command = new SqlCommand(insertInvoiceQuery, connection))
                {
                    command.Parameters.AddWithValue("@InvoiceID", tb_InvoiceID.Text);
                    command.Parameters.AddWithValue("@InvoiceDate", tb_InvoiceDate.Text);
                    command.Parameters.AddWithValue("@SupplierID", tb_SupplierID.Text);
                    command.Parameters.AddWithValue("@TotalAmount", tb_TotalAmount.Text);
                    command.ExecuteNonQuery();
                }

                // Insert invoice items
                string insertItemsQuery = "INSERT INTO InvoiceItems (ItemID, InvoiceID, ProductID, Quantity, UnitPrice, Image) VALUES (@ItemID, @InvoiceID, @ProductID, @Quantity, @UnitPrice, @Image)";
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.IsNewRow) continue;

                    using (SqlCommand command = new SqlCommand(insertItemsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID",row.Cells["ItemID"].Value.ToString());
                        command.Parameters.AddWithValue("@InvoiceID", tb_InvoiceID.Text);
                        command.Parameters.AddWithValue("@ProductID", row.Cells["ProductID"].Value.ToString());
                        command.Parameters.AddWithValue("@Quantity", row.Cells["Quantity"].Value.ToString());
                        command.Parameters.AddWithValue("@UnitPrice", row.Cells["UnitPrice"].Value.ToString());
                        command.Parameters.AddWithValue("@Image", (byte[])row.Cells["grImage"].Value);
                        command.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Invoice saved successfully!");
        }
    }
}