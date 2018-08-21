using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Updater
{
    public partial class FormName : Form
    {
        Form1 frmMain;
        /// <summary>
        /// 
        /// </summary>
        public String ResultName
        {
            get
            {
                return textBox1.Text;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public FormName(Form1 prm_frm, String prm_text,String prm_button,String prm_title)
        {
            InitializeComponent();
            frmMain = prm_frm;
            textBox1.Text = prm_text;
            Text = prm_title;
            this.DialogResult = DialogResult.Cancel;
            this.buttonOK.Text = prm_button + "(&O)";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            frmMain.NameResult = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
