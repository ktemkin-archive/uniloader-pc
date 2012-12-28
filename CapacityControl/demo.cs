using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FilledContainer
{
    public partial class demo : Form
    {
        public demo()
        {
            InitializeComponent();
        }

        private void capacityControl1_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            filledContainer.Value = float.Parse(txtFree.Text);
        }

        private void btnDemoTwo_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void monoFilledContainer1_Load(object sender, EventArgs e)
        {

        }
    }
}
