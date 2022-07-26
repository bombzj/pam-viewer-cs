using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PamViewer
{
    public partial class PamForm : Form
    {
        public PamForm(Program program)
        {
            this.program = program;
            InitializeComponent();

            TopLevel = false;
            Location = new System.Drawing.Point(0, 300);
            Control.FromHandle(program.Window.Handle).Controls.Add(this);
            Show();
            textBoxSearch.Focus();
        }

        private Program program;

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            var groups = program.searchGroup(textBoxSearch.Text);

            comboBoxGroup.Items.Clear();
            foreach (string name in groups)
            {
                comboBoxGroup.Items.Add(name);
            }
            comboBoxGroup.Focus();
        }

        private void comboBoxGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = comboBoxGroup.Text;
            var pams = program.listPam(selectedName);

            comboBoxPam.Items.Clear();
            foreach (string name in pams)
            {
                comboBoxPam.Items.Add(name);
            }
            comboBoxPam.SelectedIndex = 0;
        }

        private void comboBoxPam_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedName = comboBoxPam.Text;
            var sprites = program.listSprite(selectedName);

            comboBoxSprite.Items.Clear();
            foreach (string name in sprites)
            {
                comboBoxSprite.Items.Add(name);
            }
            comboBoxSprite.SelectedIndex = 0;
        }

        private void comboBoxSprite_SelectedIndexChanged(object sender, EventArgs e)
        {
            program.playSprite(comboBoxGroup.Text, comboBoxPam.Text, comboBoxSprite.Text);
        }

        private void textBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                buttonSearch.PerformClick();
            }
        }
    }
}
