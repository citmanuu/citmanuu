﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MANUUFinance
{
    public partial class frmView : Form
    {
        public frmView()
        {
            InitializeComponent();
        }

        private void View_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'financeDataSet3.UserDeptRoleview' table. You can move, or remove it, as needed.
            this.userDeptRoleviewTableAdapter1.Fill(this.financeDataSet3.UserDeptRoleview);            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder SearchStatement = new StringBuilder();
            try
            {
                SearchStatement.Clear();
                if (txtNameSearch.Text.Length > 0)
                {
                    if (SearchStatement.Length > 0)
                    {
                        SearchStatement.Append(" and ");
                    }
                    SearchStatement.Append("Name like '%" + txtNameSearch.Text + "%'");
                }
                //Refresh DGV 
                userDeptRoleviewBindingSource.Filter = SearchStatement.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtNameSearch.Text = "";
            userDeptRoleviewBindingSource.Filter = null;
        }
    }
}
