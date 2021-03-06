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
   
    public partial class Forms : Form
    {
        bool retrievedForUpdate;
        int GlobalId = 0;
        private int userId, deptId, roleId;
        private string formName;
        public Forms(int userId, int deptId, int roleId, string formName)
        {
            InitializeComponent();
            this.userId = userId;
            this.deptId = deptId;
            this.roleId = roleId;
            this.formName = formName;
        }
        // Pre Load
        #region

        private void AddForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'financeDataSet1.FormMST' table. You can move, or remove it, as needed.
            this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
            if (new AdministratorLogin().administratorLogin(userId))
            {
                prepareaction();
            }
        }

        #endregion

        // DML     
        #region
        // add the record
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (validateRecord())
            {
                //Connection String 
                string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

                //Instantiate SQL Connection
                SqlConnection con = new SqlConnection(cs);

                // Open the connection
                    con.Open();



                // Get the number of the row in database
                SqlCommand cmd = new SqlCommand("Form_addoreditordelete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormName", textBox1.Text.ToString().ToUpper());
                cmd.Parameters.AddWithValue("@FormDescription", richTextBox1.Text.ToString().ToUpper());
                cmd.Parameters.AddWithValue("@FormId", -1);

                try
                {
                    bool success = Convert.ToBoolean(cmd.ExecuteScalar());
                    MessageBox.Show("Department is Added", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
                    cleartextbox();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("Name_FormMST"))
                    {
                        MessageBox.Show("Record already added. Perhaps you want to update.", "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Focus();
                    }
                    else
                        MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
                    cleartextbox();
                    con.Close();
                }
            }
        }
        // exit the form
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // update the record
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //If Form Controls are validated proceed to update record
            if (validateRecord())
            {
                //Check if we are not Updating Record
                if (retrievedForUpdate)
                {
                    //Connection String 
                    string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

                    //Instantiate SQL Connection
                    SqlConnection con = new SqlConnection(cs);
                    // Open the connection
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Form_addoreditordelete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FormId", GlobalId);
                    cmd.Parameters.AddWithValue("@FormName", textBox1.Text.ToString().ToUpper());
                    cmd.Parameters.AddWithValue("@FormDescription", richTextBox1.Text.ToString().ToUpper());

                    try
                    {
                        Convert.ToBoolean(cmd.ExecuteScalar());
                        MessageBox.Show("Form is Updated", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
                        cleartextbox();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("Name_FormMST"))
                        {
                            MessageBox.Show("Record already added. Perhaps you want to update.", "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBox1.Focus();
                        }
                        else
                            MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
                        cleartextbox();
                        con.Close();
                    }                                    
                }
            }
        }
        // delete the record
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Delete the department?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //If Form Controls are validated proceed to update record
                if (validateRecord())
                {
                    //Check if we are not Updating Record
                    if (retrievedForUpdate)
                    {
                        //Connection String 
                        string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

                        //Instantiate SQL Connection
                        SqlConnection con = new SqlConnection(cs);
                        // Open the connection
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Form_addoreditordelete", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FormId", 0);
                        cmd.Parameters.AddWithValue("@FormName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@FormDescription", richTextBox1.Text);

                        try
                        {
                            Convert.ToBoolean(cmd.ExecuteScalar());
                            MessageBox.Show("Form is Deleted", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
                            cleartextbox();
                        }
                        catch (SqlException ex)
                        {
                                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        finally
                        {
                            this.formMSTTableAdapter1.Fill(this.financeDataSet1.FormMST);
                            cleartextbox();
                            con.Close();
                        }                    
                    }
                }
            }
        }
        #endregion

        // support
        #region
        //  clear the text
        private void cleartextbox()
        {
            textBox1.Text = String.Empty;
            richTextBox1.Text = String.Empty;
            retrievedForUpdate = false;
        }
        // search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder SearchStatement = new StringBuilder();
            try
            {
                SearchStatement.Clear();
                if (txtFormNameSearch.Text.Length > 0)
                {
                    if (SearchStatement.Length > 0)
                    {
                        SearchStatement.Append(" and ");
                    }
                    SearchStatement.Append("FormName like '%" + txtFormNameSearch.Text + "%'");
                }


                //Refresh DGV 
                formMSTBindingSource.Filter = SearchStatement.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // clear the search
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtFormNameSearch.Text = "";
            formMSTBindingSource.Filter = null;
        }

        private void DGVFrom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = DGVForm.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                richTextBox1.Text = DGVForm.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                retrievedForUpdate = true;
                string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                con.Open();
                SqlCommand myCommand = new SqlCommand("SELECT FormId FROM [finance].[dbo].[FormMST] where FormName = '" + textBox1.Text + "'", con);
                GlobalId = int.Parse(myCommand.ExecuteScalar().ToString());
                con.Close();
            }
        }
        //prepare the action add, delete, update
        private void prepareaction()
        {
            string CanAdd = "CanAdd";
            if (new CheckingPrivileges().CheckingPrivilegesaction(userId, deptId, roleId, CanAdd, formName))
            {
                btnAdd.Enabled = true;
            }
            else
                btnAdd.Enabled = false;
            string CanUpdate = "CanUpdate";
            if (new CheckingPrivileges().CheckingPrivilegesaction(userId, deptId, roleId, CanUpdate, formName))
            {
                btnUpdate.Enabled = true;
            }
            else
                btnUpdate.Enabled = false;
            string CanDelete = "CanDelete";
            if (new CheckingPrivileges().CheckingPrivilegesaction(userId, deptId, roleId, CanDelete, formName))
            {
                btnDelete.Enabled = true;
            }
            else
                btnDelete.Enabled = false;
        }

        private bool validateRecord()
        {
            bool validationResult = true;
            string validationMessage = "";
            if (textBox1.Text.Length == 0)
            {
                validationMessage += "Please provide Form Name\n";
                validationResult = false;
            }
            if (richTextBox1.Text.Length == 0)
            {
                validationMessage += "Please provide Description\n";
                validationResult = false;
            }
            if (richTextBox1.Text.Length >= 250)
            {
                validationMessage += "Please provide only 250 charecters in description box\n";
                validationResult = false;
            }
            if (validationResult == false)
            {
                MessageBox.Show(validationMessage, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
                return true;
        }

        private void btClearRecord_Click(object sender, EventArgs e)
        {
            cleartextbox();
        }
        #endregion

    }
}
