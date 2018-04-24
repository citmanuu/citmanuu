﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MANUUFinance
{
    public partial class BudgetPurpose : Form
    {
        private int userId, deptId, roleId;
        string formName;
        public BudgetPurpose(int userId, int deptId, int roleId, string formName)
        {
            InitializeComponent();
            this.userId = userId;
            this.deptId = deptId;
            this.roleId = roleId;
            this.formName = formName;
        }
        private void BudgetPurpose_Load(object sender, EventArgs e)
        {
            PrepareDeptCombo();
            PrepareFYCombo();
            PrepareSL1Combo();
            PrepareSL2Combo("0");
            PrepareSL3Combo("0");
            PrepareAccountsCombo("0");
            load_DataGridView();
            if (new AdministratorLogin().administratorLogin(userId))
            {
                comboDept.Enabled = false;
                txtAppAmount.Enabled = false;
            }
            else
            {
                txtRBECY.Enabled = false;
            }
        }

        private void PrepareDeptCombo()
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT DeptId, DeptName FROM [Finance].[dbo].[Department] Order by 1";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboDept.DisplayMember = "ListItemDesc"; // will display Name property
                comboDept.ValueMember = "ListItemID"; // will select Value property
                comboDept.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void PrepareFYCombo()
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT PKFYID, FYName FROM [Finance].[dbo].[FinancialYear] Order by 1";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboFY.DisplayMember = "ListItemDesc"; // will display Name property
                comboFY.ValueMember = "ListItemID"; // will select Value property
                comboFY.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void PrepareSL1Combo()
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "Select Distinct A.SL1ID, A.SL1Name from Finance.dbo.SL1 A, Finance.dbo.AccountsView B " +
                                   "where  A.SL1ID = B.SL1ID order by 2";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboSL1.DisplayMember = "ListItemDesc"; // will display Name property
                comboSL1.ValueMember = "ListItemID"; // will select Value property
                comboSL1.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }
        }
        private void PrepareSL2Combo(string fkSL1)
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "Select Distinct A.PKSL2, A.SL2Name from Finance.dbo.SL2 A, Finance.dbo.AccountsView B " +
                                    "where B.SL1ID = " + fkSL1 + " and A.PKSL2 = B.PKSL2 order by 2";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboSL2.DisplayMember = "ListItemDesc"; // will display Name property
                comboSL2.ValueMember = "ListItemID"; // will select Value property
                comboSL2.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void PrepareSL3Combo(string fkSL2)
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "Select Distinct A.PKSL3, A.SL3Name from Finance.dbo.SL3 A, Finance.dbo.AccountsView B " +
                                    "where B.PKSL2 = " + fkSL2 + "and A.PKSL3 = B.FKSL3ID order by 2";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboSL3.DisplayMember = "ListItemDesc"; // will display Name property
                comboSL3.ValueMember = "ListItemID"; // will select Value property
                comboSL3.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void comboSL1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareSL2Combo(Convert.ToString(comboSL1.SelectedValue));
        }

        private void comboSL2_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareSL3Combo(Convert.ToString(comboSL2.SelectedValue));
        }

        private void comboSL3_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrepareAccountsCombo(Convert.ToString(comboSL3.SelectedValue));
        }

        private void DGVAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //D.FYName(0), DEPNAME(1), SL3Code(2), AccountName(3), BECY(4), RBECY(5), BENY(6), SL1Name (7), 
                //SL2Name (8), SL3Name (9), SL1ID (10), PKSL2 (11), FKSL3ID (12), C.PKDEPID (13), D.PKFYID (14), ACID (15), PKBUDGETID (16)

                comboFY.SelectedValue = Convert.ToInt32(DGVAccounts.Rows[e.RowIndex].Cells[15].FormattedValue.ToString());
                comboDept.SelectedValue = Convert.ToInt32(DGVAccounts.Rows[e.RowIndex].Cells[14].FormattedValue.ToString());
                comboSL1.SelectedValue = Convert.ToInt32(DGVAccounts.Rows[e.RowIndex].Cells[11].FormattedValue.ToString());
                comboSL2.SelectedValue = Convert.ToInt32(DGVAccounts.Rows[e.RowIndex].Cells[12].FormattedValue.ToString());
                comboSL3.SelectedValue = Convert.ToInt32(DGVAccounts.Rows[e.RowIndex].Cells[13].FormattedValue.ToString());
                comboAccount.SelectedValue = Convert.ToInt32(DGVAccounts.Rows[e.RowIndex].Cells[16].FormattedValue.ToString());

                txtBECY.Text = DGVAccounts.Rows[e.RowIndex].Cells[5].FormattedValue.ToString();
                txtRBECY.Text = DGVAccounts.Rows[e.RowIndex].Cells[6].FormattedValue.ToString();
                txtBENY.Text = DGVAccounts.Rows[e.RowIndex].Cells[7].FormattedValue.ToString();
                txtPKBudgetID.Text = DGVAccounts.Rows[e.RowIndex].Cells[17].FormattedValue.ToString();
                txtAppAmount.Text = DGVAccounts.Rows[e.RowIndex].Cells[18].FormattedValue.ToString();
                //if (DGVAccounts.Rows[e.RowIndex].Cells[7].FormattedValue.ToString() == "DUMMY SL1")
                //{
                //    //btnMapVirtualAccount.Enabled = true;
                //}
                //else
                //    //btnMapVirtualAccount.Enabled = false;
                LockKeys();
            }
        }

        private void LockKeys()
        {
            comboFY.Enabled = false;
            comboSL1.Enabled = false;
            comboSL2.Enabled = false;
            comboSL3.Enabled = false;
        }

        private void PrepareAccountsCombo(string fkSL3)
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "Select PKACID, ACCOUNTNAME from Finance.dbo.Accounts where FKSL3ID = " + fkSL3 + "order by 2";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboAccount.DisplayMember = "ListItemDesc"; // will display Name property
                comboAccount.ValueMember = "ListItemID"; // will select Value property
                comboAccount.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void load_DataGridView()
        {
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            if (new AdministratorLogin().administratorLogin(userId))
            {
                SqlDataAdapter sqldb = new SqlDataAdapter("Select D.FYName, DeptName, SL3Code, AccountName, BECY, RBECY, BENY, SL1Name, SL2Name, SL3Name, SL1ID, PKSL2, FKSL3ID, C.DeptId, D.PKFYID, B.PKACID, A.PKBUDGETID, A.ApprAmount from Budget A, AccountsView B, Department C, FinancialYear D where A.FKACID = B.PKACID  AND A.FKDEPID = C.DeptId AND A.FKFYID = D.PKFYID AND C.DeptId = '" + deptId + "' ", objSqlConnection);
                DataTable dtb1 = new DataTable();
                sqldb.Fill(dtb1);
                DGVAccounts.DataSource = dtb1;
            }
            else
            {
                SqlDataAdapter sqldb = new SqlDataAdapter("Select D.FYName, DeptName, SL3Code, AccountName, BECY, RBECY, BENY, SL1Name, SL2Name, SL3Name, SL1ID, PKSL2, FKSL3ID, C.DeptId, D.PKFYID, B.PKACID, A.PKBUDGETID, A.ApprAmount from Budget A, AccountsView B, Department C, FinancialYear D where A.FKACID = B.PKACID  AND A.FKDEPID = C.DeptId AND A.FKFYID = D.PKFYID", objSqlConnection);
            DataTable dtb1 = new DataTable();
            sqldb.Fill(dtb1);
            DGVAccounts.DataSource = dtb1;
            }
        }
    }
}