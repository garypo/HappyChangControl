using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Chang_Player
{
    public partial class FormHappyChangPlayer : Form
    {
        public FormHappyChangPlayer()
        {
            InitializeComponent();
        }

        private void buttonBasicWin_Click(object sender, EventArgs e)
        {
            FormBasic w = new FormBasic();
            w.Show();
        }
    }
}
