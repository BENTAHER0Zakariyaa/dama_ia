using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Dama_IA
{
    public static class MyFrameWork
    {
        public static Panel CreateCell(Point location, Size size, Color color, String name)
        {
            Panel panel =  new Panel();

            panel.Location = location;
            panel.Size = size;
            panel.BackColor = color;
            panel.Name = name;

            return panel;
        }

        public static Panel CreateMap (Point location, Size size, String name, Color color, String text = "")
        {
            Panel panel =  new Panel();

            panel.Location = location;
            panel.BackColor = color;
            panel.Size = size;
            panel.Name = name;
            panel.Text = text;

            return panel;
        }
        public static Button CreatePiece(Point location, Size size, String name, Color color, Image image, string text = "")
        {
            Button piece =  new Button();

            piece.Location = location;
            piece.BackColor = color;
            piece.Size = size;
            piece.Name = name;
            piece.BackgroundImage = image;
            if(name[0].Equals('B'))
            {
                piece.ForeColor = Color.White;
            }
            else
            {
                piece.ForeColor = Color.Black;
            }
            piece.BackgroundImageLayout = ImageLayout.Zoom;
            piece.FlatStyle = FlatStyle.Flat;
            piece.FlatAppearance.BorderSize = 0;
            piece.Text = text;

            return piece;
        }
    }
}
