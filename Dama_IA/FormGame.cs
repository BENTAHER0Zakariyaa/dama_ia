using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dama_IA
{
    public partial class FormGame : Form
    {
        Board board = new Board();
        public FormGame()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.board.DrawGame(this);            
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            this.board.DrawGame(this);
            
        }

    }
}
