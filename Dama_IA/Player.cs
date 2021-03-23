using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Dama_IA
{
    public class Player
    {
        public int Id{get; set;}
        public string Name{get; set;}
        public List<Piece> pieces {get; set;}

        public Player()
        {
            this.pieces = new List<Piece>();
        }

        public Player(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.pieces = new List<Piece>();
        }

        public void InitPlayer(string playerSide)
        {

            if(playerSide.ToLower().Equals("black"))
            {
                this.pieces.Add(new Piece(new Point(0, 0), false));
                this.pieces.Add(new Piece(new Point(2, 0), false));
                this.pieces.Add(new Piece(new Point(4, 0), false));
                this.pieces.Add(new Piece(new Point(6, 0), false));

                this.pieces.Add(new Piece(new Point(1, 1), false));
                this.pieces.Add(new Piece(new Point(3, 1), false));
                this.pieces.Add(new Piece(new Point(5, 1), false));
                this.pieces.Add(new Piece(new Point(7, 1), false));

                this.pieces.Add(new Piece(new Point(0, 2), false));
                this.pieces.Add(new Piece(new Point(2, 2), false));
                this.pieces.Add(new Piece(new Point(4, 2), false));
                this.pieces.Add(new Piece(new Point(6, 2), false));
            }
            else
            {
                // this.pieces.Add(new Piece(new Point(1, 7), false));
                // this.pieces.Add(new Piece(new Point(3, 7), false));
                // this.pieces.Add(new Piece(new Point(5, 7), false));
                this.pieces.Add(new Piece(new Point(7, 7), false)); 

                this.pieces.Add(new Piece(new Point(0, 6), false));
                this.pieces.Add(new Piece(new Point(2, 6), false));
                this.pieces.Add(new Piece(new Point(4, 6), false));
                // this.pieces.Add(new Piece(new Point(6, 6), false));

                this.pieces.Add(new Piece(new Point(1, 5), false));
                this.pieces.Add(new Piece(new Point(3, 5), false));
                this.pieces.Add(new Piece(new Point(5, 5), false));
                this.pieces.Add(new Piece(new Point(7, 5), false));
            }
        }

       

    //public int IsFull(Position p)
    //{
    //    int pos=-1, i=0;
    //    foreach (Piece piece in this.pieces)
    //    {
    //        if(piece.Position.Row.Equals(p.Row) && piece.Position.Col.Equals(p.Col))
    //        {
    //            pos=i;
    //            break;
    //        }
    //        i++;
    //    }
    //    return pos;
    //}
}
}
