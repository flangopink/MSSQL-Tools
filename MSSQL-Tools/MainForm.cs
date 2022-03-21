using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using flangoLib;

namespace MSSQL_Tools
{
    public partial class MainForm : Form
    {
        //readonly List<string> columns = new();
        readonly List<ComboBox> cbs = new();
        public MainForm()
        {
            InitializeComponent();
            Initialize();

            //columns = MakeColumns();

            FLib.Print("--= Thank you for using flangoLib! =--");

            rtb_out.Text = "\n\n\n\t\t\t--= Welcome to SQL Tools by flangopink! =--\n\t\t\t\t--= Powered by flangoLib =--";
        }

        private void B_GenRandomInsert_Click(object sender, EventArgs e)
        {
            List<string> selectedTypes = new();
            foreach (ComboBox cb in cbs)
                selectedTypes.Add(cb.Text);
            
            rtb_out.Text = FLib.GenerateInsertQuery(selectedTypes, int.Parse(tb_Count.Text), tb_TableName.Text);
            B_InsertIntoTable.Enabled = true;
        }

        /*List<string> MakeColumns()
        {
            return new List<string>() { tb_c1.Text, tb_c2.Text, tb_c3.Text, tb_c4.Text, tb_c5.Text, tb_c6.Text, tb_c7.Text, tb_c8.Text };
        }*/

        void Initialize()
        {
            cbs.Add(cb_c1); cbs.Add(cb_c2); cbs.Add(cb_c3); cbs.Add(cb_c4);
            cbs.Add(cb_c5); cbs.Add(cb_c6); cbs.Add(cb_c7); cbs.Add(cb_c8);

            foreach (ComboBox cb in cbs)
                foreach (string t in FLib.ColumnTypes)
                    cb.Items.Add(t);
        }

        private void B_Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(rtb_out.Text);
        }

        private void B_Clear_Click(object sender, EventArgs e)
        {
            rtb_out.Text = "";
            B_InsertIntoTable.Enabled = false;
        }

        private void B_InsertIntoTable_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"This will insert {tb_Count.Text} row(s) into a *REAL* SQL database. Continue?", "Insert into SQL database.", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string connetionString = $"Data Source={tb_Server.Text};Initial Catalog={tb_Database.Text};Integrated Security=SSPI;Encrypt=True;TrustServerCertificate=True;";
                SqlConnection connection;
                SqlCommand command;
                string sql = rtb_out.Text;

                connection = new SqlConnection(connetionString);
                try
                {
                    connection.Open();
                    command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    MessageBox.Show("Success!");
                    rtb_out.Text = "";
                    B_InsertIntoTable.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't open connection!\n\n"+ex);
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void B_CurrencyRate_Click(object sender, EventArgs e)
        {
            //rtb_out.Text = FLib.GetCurrencyRates().Keys;
        }
    }
}
