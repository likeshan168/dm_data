using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ImportData
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            string serverIP = txtServerIP.Text.Trim(),
                   dataBaseName = txtDataBaseName.Text.Trim(),
                   dataUserName = txtDataUserName.Text.Trim(),
                   dataUserPwd = txtDataUserPwd.Text.Trim(),
                   tableName = txtTableName.Text.Trim();
            if (string.IsNullOrEmpty(serverIP) || string.IsNullOrEmpty(dataBaseName) || string.IsNullOrEmpty(dataUserName) || string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("请将数据库属性字段填写完整", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string connStr = string.Format("server={0};initial catalog={1};uid={2};pwd={3}", serverIP, dataBaseName, dataUserName, dataUserPwd);
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand();
                    string sqlStr = string.Format("select * from [dbo].[{0}]", tableName);
                    cmd.CommandText = sqlStr;
                    cmd.Connection = con;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    ShowAndEditRowData.DataSource = dt.DefaultView;
                    SqlCommandBuilder sb = new SqlCommandBuilder(da);
                    da.Update(dt.GetChanges());
                    dt.AcceptChanges();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rows = ShowAndEditRowData.SelectedRows;
            
        }

        
    }
}
