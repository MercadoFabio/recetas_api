﻿using RecetasFront;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViajesFinal.Forms
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

     
        private void consultaEditarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmGet v = new FrmGet();
            v.ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Alta v = new Frm_Alta();
            v.ShowDialog();
        }
    }
}
