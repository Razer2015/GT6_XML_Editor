using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GT6_XML_Editor
{
    public partial class ProcessDialogForm : Form
    {
        Form parent;

        public ProcessDialogForm(Form parent)
        {
            InitializeComponent();
            this.ControlBox = false;
            this.parent = parent;
        }

        private void ProcessDialogForm_Load(object sender, EventArgs e)
        {
            if (this.StartPosition == FormStartPosition.CenterParent)
            {
                var x = parent.Location.X + (parent.Width - this.Width) / 2;
                var y = parent.Location.Y + (parent.Height - this.Height) / 2;
                this.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
            }
        }
    }
}
