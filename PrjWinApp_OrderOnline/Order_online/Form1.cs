using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Order_online
{
    public partial class Form1 : Form
    {
        OrderAccessoriesEntities db = new OrderAccessoriesEntities();
        public Form1()
        {
            InitializeComponent();
        }

        //============= Method to Clear the Form ==============//
        private void ClearForm()
        {
            txtEmpID.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtProductID.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtEmpOrderId.Text = string.Empty;
            txtProdOrderName.Text = string.Empty;
            txtEmpID.Focus();
            txtProductID.Focus();
            txtEmpOrderId.Focus();
        }

        //============= Method to Populate List Employee ==============//
        private void PopulateListEmployee()
        {
            var empList = from emp in db.Employees
                          select emp;
            listViewEmployee.Items.Clear();
            foreach (var emp in empList)
            {
                ListViewItem item = new ListViewItem(Convert.ToString(emp.EmployeeID));
                item.SubItems.Add(emp.FirstName);
                item.SubItems.Add(emp.LastName);
                item.SubItems.Add(emp.PhoneNumber);
                item.SubItems.Add(emp.Email);
                listViewEmployee.Items.Add(item);
            }
        }

        //============= Method to Populate List Product ==============//
        private void PopulateListProduct()
        {
            var productList = from prod in db.Products
                              orderby prod.ProductID
                              select prod;
            listViewProduct.Items.Clear();
            foreach (var prod in productList)
            {
                ListViewItem item = new ListViewItem(Convert.ToString(prod.ProductID));
                item.SubItems.Add(prod.ProductName);
                listViewProduct.Items.Add(item);
            }
        }

        //============= Method to Populate List Order ==============//
        private void PopulateListOrder()
        {
            var orderList = from order in db.orders
                              select order;
            listViewOrder.Items.Clear();
            foreach (var order in orderList)
            {
                ListViewItem item = new ListViewItem(Convert.ToString(order.EmployeeId));
                item.SubItems.Add(order.ProductName);
                item.SubItems.Add(order.DateCreated.ToString());
                listViewOrder.Items.Add(item);
            }
        }

        //============= Form Load Employee ==============//
        private void Form1_Load(object sender, EventArgs e)
        {
            //PopulateListEmployee();
            //listViewEmployee.FullRowSelect = true;
        }

        //============= Click on List View Employee ==============//
        private void listViewEmployee_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listViewEmployee.SelectedItems[0];
            txtEmpID.Text = item.SubItems[0].Text;
            txtFName.Text = item.SubItems[1].Text;
            txtLName.Text = item.SubItems[2].Text;
            txtPhone.Text = item.SubItems[3].Text;
            txtEmail.Text = item.SubItems[4].Text;
        }

        //============= Click on List View Product ==============//
        private void listViewProduct_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listViewProduct.SelectedItems[0];
            txtProductID.Text = item.SubItems[0].Text;
            txtProductName.Text = item.SubItems[1].Text;
        }

        //============= Click on List View Order ==============//
        private void listViewOrder_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listViewOrder.SelectedItems[0];
            txtEmpOrderId.Text = item.SubItems[0].Text;
            txtProdOrderName.Text = item.SubItems[1].Text;
        }

        //============= Save Employee Button ==============//
        private void btnSave_Click(object sender, EventArgs e)
        {
            var emp = new Employee();

            int id = Convert.ToInt32(txtEmpID.Text.Trim());
            var foundEmp = db.Employees.Find(id);
            if (foundEmp != null)
            {
                MessageBox.Show("Employee Id is already there!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmpID.Clear();
                txtEmpID.Focus();
            }
            else if (txtEmpID.Text.Length < 5 || txtEmpID.Text.Length > 5)
            {
                MessageBox.Show("Please enter 5-digits only!!!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                emp.EmployeeID = Convert.ToInt32(txtEmpID.Text);
                emp.FirstName = txtFName.Text;
                emp.LastName = txtLName.Text;
                emp.PhoneNumber = txtPhone.Text;
                emp.Email = txtEmail.Text;
                db.Employees.Add(emp);
                db.SaveChanges();
                MessageBox.Show("Employee saved successfully!");
                PopulateListEmployee();
                ClearForm();
            }
        }

        //============= Update Button ==============//
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Do you want to update this record? ", "UPDATE",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txtEmpID.Text.Trim());
                var updateEmp = db.Employees.Find(id);

                if (txtEmpID.Text.Length < 5 || txtEmpID.Text.Length > 5)
                {
                    MessageBox.Show("Please enter 5-digits only!!!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (updateEmp != null)
                {
                    updateEmp.FirstName = txtFName.Text;
                    updateEmp.LastName = txtLName.Text;
                    updateEmp.PhoneNumber = txtPhone.Text;
                    updateEmp.Email = txtEmail.Text;

                    db.SaveChanges();
                    MessageBox.Show("Record updated successfully!");
                    PopulateListEmployee();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Employee Id is already there!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
                
        }

        //============= List Employee Button ==============//
        private void btnListEmployee_Click(object sender, EventArgs e)
        {
            PopulateListEmployee();
        }

        //============= List Product Button ==============//
        private void btnListProduct_Click(object sender, EventArgs e)
        {
            PopulateListProduct();
            listViewProduct.FullRowSelect = true;
        }

        //============= List Order Button ==============//
        private void btnListOrder_Click(object sender, EventArgs e)
        {
            PopulateListOrder();
            listViewOrder.FullRowSelect = true;
        }

        //============= Add Order Button ==============//
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var order = new order();

            int id = Convert.ToInt32(txtEmpOrderId.Text.Trim());
            var foundEmp = db.orders.Find(id);
            if (foundEmp != null)
            {
                MessageBox.Show("Employee Id is already there!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmpOrderId.Clear();
                txtEmpOrderId.Focus();
            }
            else
            {
                order.EmployeeId = Convert.ToInt32(txtEmpOrderId.Text);
                order.ProductName = txtProdOrderName.Text;
                
                db.orders.Add(order);
                db.SaveChanges();
                MessageBox.Show("Order added successfully!");
                ClearForm();
            }
        }

        //============= Delete Order Button ==============//
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Do you want to delete this record? ", "DELETE",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txtEmpOrderId.Text.Trim());

                order order = db.orders.Find(id);
                if (order != null)
                {
                    db.orders.Remove(order);
                    db.SaveChanges();
                    PopulateListOrder();
                    MessageBox.Show("Order deleted successfully!");
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Error", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
                
        }

        //============= Exit Button ==============//
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure you want to exit the application?", "EXIT",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Thank you for using my application!\n Created by: ** Mina Dawood **");
                this.Close();
            }
        }

    }
}
