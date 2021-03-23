using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Dama_IA
{
    public class Piece
    {
        //public 
        public Point Location{ get; set; }
        public bool IsUpgraded { get; set; }

        public Piece(Point location, bool isUpgraded)
        {
            this.Location = location;
            this.IsUpgraded = isUpgraded;
        }
    }
}
