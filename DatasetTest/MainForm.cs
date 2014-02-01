using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatasetTest.MSNorthwindDatasetTableAdapters;

namespace DatasetTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void bindingNavigator_RefreshItems(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'northwindDataset.Customers' table. You can move, or remove it, as needed.
            this.customersTableAdapter.Fill(this.northwindDataset.Customers);
            this.dataGridView1.DataSource = NorthwindDataset.GetCategories();
            var x= NorthwindDataset.GetCustomers().TestProp;


        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            tableAdapterManager.UpdateAll(northwindDataset);
        }
    }
}
