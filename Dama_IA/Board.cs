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
    public class Board
    {
        //Cell Props
        private int cellHight = 50;
        private Size cellSize = new Size(50, 50);
        private Point mapLocation;
        private Color darkColor = ColorTranslator.FromHtml("#987654");
        private Color lightColor = ColorTranslator.FromHtml("#f2f2f2");

        //Map
        private Panel map;
        private int rowLength = 8;
        private int columnLength = 8;
        private int height;
        private int width;

        // Mouse
        private Point MouseLocationInPiece;
        private Point FirstLocation = new Point();

        // Check
        private bool isMoved = true;

        //players
        private List<Player> players;
        private Player playerTurn;

        public void DrawGame(FormGame formGame)
        {
            //
            this.DrawBoard(formGame);
            this.InitPlayers();
            this.playerTurn=players[1];
            this.DrawPlayersPieces();
            this.DrawCells();
        }

        //this method for drawing broard
        public void DrawBoard(FormGame formGame)
        {
            //calc width & height
            this.width = this.columnLength * cellHight;
            this.height = this.rowLength * cellHight;
            //
            this.mapLocation = new Point(10, 10);
            Size mapSize = new Size(width, this.height);
            //
            formGame.Controls.Remove(this.map);
            this.map = MyFrameWork.CreateMap(mapLocation, mapSize, "Board", Color.White);
            this.map.BorderStyle = BorderStyle.FixedSingle;
            this.map.BorderStyle = BorderStyle.FixedSingle;
            //
            formGame.Controls.Add(this.map);

        }
        public void DrawCells()
        {

            // // for 0 to 7 rows
            for (int row = 0; row < this.rowLength; row++)
            {
                // for 0 to 7 cols
                for (int col = 0; col < this.columnLength; col++)
                {
                    Panel cell = new Panel();
                    Point cellLocation = new Point(col * this.cellHight, row * this.cellHight);
                    if ((row % 2).Equals(0))
                    {
                        if ((col % 2).Equals(1))
                        {
                            string name = $"LC:{col}:{row}";
                            //light cell
                            cell = MyFrameWork.CreateCell(cellLocation, this.cellSize, this.lightColor, name);
                        }
                        else
                        {
                            //dark cell
                            string name = $"DC:{col}:{row}";
                            cell = cell = MyFrameWork.CreateCell(cellLocation, this.cellSize, this.darkColor, name);
                        }
                    }
                    else
                    {
                        if ((col % 2).Equals(0))
                        {
                            //light cell
                            string name = $"LC:{col}:{row}";
                            cell = MyFrameWork.CreateCell(cellLocation, this.cellSize, this.lightColor, name);
                        }
                        else
                        {
                            //dark cell
                            string name = $"DC:{col}:{row}";
                            cell = cell = MyFrameWork.CreateCell(cellLocation, this.cellSize, this.darkColor, name);
                        }
                    }
                    this.map.Controls.Add(cell);
                }
            }

            //BackgroundFix();
        }

        void InitPlayers()
        {
            //Create new list for players
            this.players = new List<Player>();

            //Create 2 players (black and white)
            Player player1 = new Player(1,"BLACK");
            Player player2 = new Player(2,"WHITE");

            //Init potions of pieces
            player1.InitPlayer(player1.Name);
            player2.InitPlayer(player2.Name);

            //Add players to list 
            this.players.Add(player1);
            this.players.Add(player2);
        }
        public void DrawPlayersPieces()
        {
            foreach (Player player in this.players)
            {
                foreach (Piece piece in player.pieces)
                {
                    string name = player.Id % 2 == 1 ? $"B:{piece.Location.X}:{piece.Location.Y}" : $"W:{piece.Location.X}:{piece.Location.Y}";
                    Image image = player.Id % 2 == 1 ? global::Dama_IA.Properties.Resources.black : global::Dama_IA.Properties.Resources.white;
                    Point pieceLocation = new Point(piece.Location.X * this.cellHight, piece.Location.Y * this.cellHight);
                    Button p = MyFrameWork.CreatePiece(pieceLocation, this.cellSize, name, this.darkColor,image);
                    
                    p.MouseDown += this.Piece_MouseDown;
                    p.MouseUp += this.Piece_MouseUp;
                    p.MouseMove += this.Piece_MouseMove;

                    this.map.Controls.Add(p);
                }
            }
        }

        private void Piece_MouseDown(object sender, MouseEventArgs e)
        {
            Button piece = (sender as Button);
            piece.BringToFront();
            this.map.Controls.SetChildIndex(piece, 0);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && IsMyPiece(piece))
            {
                this.MouseLocationInPiece = e.Location;
                this.FirstLocation = piece.Location;
            }
        }

        private void Piece_MouseUp(object sender, MouseEventArgs e)
        {
            Button piece = (sender as Button);
            if(IsMyPiece(piece))
            {
                Point Mouse = new Point(piece.Location.X + MouseLocationInPiece.X, piece.Location.Y + MouseLocationInPiece.Y);

                //Simple dark piece Move coordinates
                Point darkPieceMovingRight = new Point((this.FirstLocation.X + cellHight), (this.FirstLocation.Y + cellHight));
                Point darkPieceMovingLeft = new Point((this.FirstLocation.X - cellHight), (this.FirstLocation.Y + cellHight));

                //Simple light piece Move coordinates
                Point lightPieceMovingRight = new Point((this.FirstLocation.X + cellHight), (this.FirstLocation.Y - cellHight));
                Point lightPieceMovingLeft = new Point((this.FirstLocation.X - cellHight), (this.FirstLocation.Y - cellHight));

               
               //Simple move
                if(piece.Text.Equals(""))
                {
                    //Player 1 
                    if (this.playerTurn.Name.ToUpper().Equals("BLACK"))
                    {
                        //Moving
                        if((this.FirstLocation.Y + cellHight) <= Mouse.Y && (this.FirstLocation.Y + (2 * cellHight)) >= Mouse.Y)
                        {
                            
                            //Right
                                if((this.FirstLocation.X + cellHight) <= Mouse.X && (this.FirstLocation.X + (2*cellHight)) >= Mouse.X)
                                {
                                    if(IsOpen(darkPieceMovingRight))
                                    {
                                       piece.Location = darkPieceMovingRight;
                                    }
                                    else{
                                        piece.Location = this.FirstLocation;
                                    }
                                }
                                //Left
                                else if((this.FirstLocation.X - cellHight) <= Mouse.X && (this.FirstLocation.X) >= Mouse.X)
                                {
                                    if(IsOpen(darkPieceMovingRight))
                                    {
                                       piece.Location = darkPieceMovingLeft;
                                    }
                                    else{
                                        piece.Location = this.FirstLocation;
                                    }
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            
                        }
                        //Eating
                        else if(
                            (this.FirstLocation.Y + (2*cellHight)) <= Mouse.Y && (this.FirstLocation.Y + (3 * cellHight)) >= Mouse.Y
                        )
                        {
                            //Right
                            if((this.FirstLocation.X + (2*cellHight)) <= Mouse.X && (this.FirstLocation.X + (3*cellHight)) >= Mouse.X)
                            {
                                
                                if(!IsOpen(new Point (this.FirstLocation.X + cellHight, this.FirstLocation.Y + cellHight)) && 
                                    IsOpen(new Point ((this.FirstLocation.X + (2*cellHight)), (this.FirstLocation.Y + (2*cellHight))))
                                )
                                {
                                    if(!IsMyPiece(GetPiece(new Point (this.FirstLocation.X + cellHight, this.FirstLocation.Y + cellHight))))
                                    {
                                        this.RemoveEnemy(this.players[1].pieces, new Point (this.FirstLocation.X + cellHight, this.FirstLocation.Y + cellHight));
                                        piece.Location = new Point ((this.FirstLocation.X + (2*cellHight)), (this.FirstLocation.Y + (2*cellHight)));
                                    }
                                    else{
                                    piece.Location = this.FirstLocation;
                                    }
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            }
                            
                            //left
                            else if((this.FirstLocation.X - (cellHight)) >= Mouse.X && (this.FirstLocation.X - (2*cellHight)) <= Mouse.X)
                            {
                                if(!IsOpen(new Point (this.FirstLocation.X - cellHight, this.FirstLocation.Y + cellHight)) &&
                                    IsOpen(new Point ((this.FirstLocation.X - (2*cellHight)), (this.FirstLocation.Y + (2*cellHight))))
                                )
                                {
                                    if(!IsMyPiece(GetPiece(new Point (this.FirstLocation.X - cellHight, this.FirstLocation.Y + cellHight))))
                                    {
                                        this.RemoveEnemy(this.players[1].pieces, new Point (this.FirstLocation.X - cellHight, this.FirstLocation.Y + cellHight));
                                        piece.Location = new Point ((this.FirstLocation.X - (2*cellHight)), (this.FirstLocation.Y + (2*cellHight)));
                                    }
                                    else{
                                    piece.Location = this.FirstLocation;
                                    }
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            }
                            else{
                                piece.Location = this.FirstLocation;
                            }
                        }
                        else{
                            piece.Location = this.FirstLocation;
                        }
                    }
                    else{
                        //Moving
                        if((this.FirstLocation.Y - cellHight) <= Mouse.Y && (this.FirstLocation.Y) >= Mouse.Y)
                        {
                            //Right
                            if((this.FirstLocation.X + cellHight) <= Mouse.X && (this.FirstLocation.X + (2*cellHight)) >= Mouse.X)
                            {
                                if(IsOpen(lightPieceMovingRight))
                                {
                                    piece.Location = lightPieceMovingRight;
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            }
                            //Left
                            else if((this.FirstLocation.X) >= Mouse.X && (this.FirstLocation.X - cellHight) <= Mouse.X)
                            {
                                if(IsOpen(lightPieceMovingLeft))
                                {
                                    piece.Location = lightPieceMovingLeft;
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            }
                            else{
                                piece.Location = this.FirstLocation;
                            }
                        }
                        //Eating
                        else if(
                            (this.FirstLocation.Y + (2*cellHight)) <= Mouse.Y && (this.FirstLocation.Y + (3 * cellHight)) >= Mouse.Y
                        )
                        {
                            //Right
                            if((this.FirstLocation.X + (2*cellHight)) <= Mouse.X && (this.FirstLocation.X + (3*cellHight)) >= Mouse.X)
                            {
                                
                                if(!IsOpen(new Point (this.FirstLocation.X + cellHight, this.FirstLocation.Y + cellHight)) && 
                                    IsOpen(new Point ((this.FirstLocation.X + (2*cellHight)), (this.FirstLocation.Y + (2*cellHight))))
                                )
                                {
                                    if(!IsMyPiece(GetPiece(new Point (this.FirstLocation.X + cellHight, this.FirstLocation.Y + cellHight))))
                                    {
                                        this.RemoveEnemy(this.players[1].pieces, new Point (this.FirstLocation.X + cellHight, this.FirstLocation.Y + cellHight));
                                        piece.Location = new Point ((this.FirstLocation.X + (2*cellHight)), (this.FirstLocation.Y + (2*cellHight)));
                                    }
                                    else{
                                    piece.Location = this.FirstLocation;
                                    }
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            }
                            
                            //left
                            else if((this.FirstLocation.X - (cellHight)) >= Mouse.X && (this.FirstLocation.X - (2*cellHight)) <= Mouse.X)
                            {
                                if(!IsOpen(new Point (this.FirstLocation.X - cellHight, this.FirstLocation.Y + cellHight)) &&
                                    IsOpen(new Point ((this.FirstLocation.X - (2*cellHight)), (this.FirstLocation.Y + (2*cellHight))))
                                )
                                {
                                    if(!IsMyPiece(GetPiece(new Point (this.FirstLocation.X - cellHight, this.FirstLocation.Y + cellHight))))
                                    {
                                        this.RemoveEnemy(this.players[1].pieces, new Point (this.FirstLocation.X - cellHight, this.FirstLocation.Y + cellHight));
                                        piece.Location = new Point ((this.FirstLocation.X - (2*cellHight)), (this.FirstLocation.Y + (2*cellHight)));
                                    }
                                    else{
                                    piece.Location = this.FirstLocation;
                                    }
                                }
                                else{
                                    piece.Location = this.FirstLocation;
                                }
                            }
                            else{
                                piece.Location = this.FirstLocation;
                            }
                        }
                        else{
                            piece.Location = this.FirstLocation;
                        }
                    }
                }
            }

            //     if (
            //         (this.FirstLocation.Y + cellHight) <= (piece.Location.Y + MouseLocationInPiece.Y) &&
            //         (this.FirstLocation.Y + (2 * cellHight)) >= (piece.Location.Y + MouseLocationInPiece.Y)
            //     )
            //     {
            //         if (
            //             (this.FirstLocation.X + cellHight) <= (piece.Location.X + MouseLocationInPiece.X) &&
            //             (this.FirstLocation.X + (2 * cellHight)) >= (piece.Location.X + MouseLocationInPiece.X))
            //         {
            //             piece.Location = new Point(
            //                     this.FirstLocation.X + cellHight,
            //                     this.FirstLocation.Y + cellHight
            //                 );
            //         }
            //         else if ((this.FirstLocation.X - cellHight) <= (e.X + piece.Left - MouseLocationInPiece.X) && (this.FirstLocation.X) >= (e.X + piece.Left - MouseLocationInPiece.X))
            //         {
            //             piece.Location = new Point(
            //                     this.FirstLocation.X - cellHight,
            //                     this.FirstLocation.Y + cellHight
            //                 );
            //         }
            //         else
            //         {
            //             piece.Location = new Point(
            //                         this.FirstLocation.X,
            //                         this.FirstLocation.Y
            //                     );
            //         }
            //     }
            //     else
            //     {
            //         piece.Location = new Point(
            //                     this.FirstLocation.X,
            //                     this.FirstLocation.Y
            //                 );
            //     }
            //  }
            // else
            // {
            //     if (
            //         (this.FirstLocation.Y - cellHight) <= (piece.Location.Y - MouseLocationInPiece.Y) &&
            //         (this.FirstLocation.Y - (2 * cellHight)) >= (piece.Location.Y - MouseLocationInPiece.Y)
            //     )
            //     {
            //         if (
            //             (this.FirstLocation.X + cellHight) <= (piece.Location.X + MouseLocationInPiece.X) &&
            //             (this.FirstLocation.X + (2 * cellHight)) >= (piece.Location.X + MouseLocationInPiece.X))
            //         {
            //             piece.Location = new Point(
            //                     this.FirstLocation.X + cellHight,
            //                     this.FirstLocation.Y + cellHight
            //                 );
            //         }
            //         else if ((this.FirstLocation.X - cellHight) <= (e.X + piece.Left - MouseLocationInPiece.X) && (this.FirstLocation.X) >= (e.X + piece.Left - MouseLocationInPiece.X))
            //         {
            //             piece.Location = new Point(
            //                     this.FirstLocation.X - cellHight,
            //                     this.FirstLocation.Y + cellHight
            //                 );
            //         }
            //         else
            //         {
            //             piece.Location = new Point(
            //                         this.FirstLocation.X,
            //                         this.FirstLocation.Y
            //                     );
            //         }
            //     }
            //     else
            //     {
            //         piece.Location = new Point(
            //                     this.FirstLocation.X,
            //                     this.FirstLocation.Y
            //                 );
            //     }
            // }

            // this.FirstLocation = new Point();
            // this.isMoved = true;

        }

        void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            Button piece = (sender as Button);

            int maxWidth = this.cellHight * this.columnLength - this.cellHight;
            int minWidth = 0;

            int maxHeight = this.cellHight * this.rowLength - this.cellHight;
            int minHeight = 0;

            if (e.Button == System.Windows.Forms.MouseButtons.Left && IsMyPiece(piece))
            {

                if (
                    (e.X + piece.Left - MouseLocationInPiece.X) <= maxWidth &&
                    (e.X + piece.Left - MouseLocationInPiece.X) >= minWidth &&
                    (e.Y + piece.Top - MouseLocationInPiece.Y) <= maxHeight &&
                    (e.Y + piece.Top - MouseLocationInPiece.Y) >= minHeight
                )
                {
                    piece.Left = e.X + piece.Left - MouseLocationInPiece.X;
                    piece.Top = e.Y + piece.Top - MouseLocationInPiece.Y;
                }
            }

        }


        //functions 

        bool IsOpen(Point location)
        {
            bool result = true;
            foreach (Control c in this.map.Controls)
            {

                if(c is Button)
                {
                    Button piece = (c as Button);
                    if((location.Y == piece.Location.Y && location.X == piece.Location.X))
                    {
                        result = false;
                    }
                }
                
                
            }
            return result;
        }
        bool IsMyPiece(Button piece)
        {
            return piece.Name[0].Equals(this.playerTurn.Name[0]);
        }
        bool IsEnemy(Button piece, Player player, Point location)
        {
            return !piece.Name[0].Equals(this.playerTurn.Name[0]) && !IsOpen(location);
        }
        
        void RemoveEnemy(List<Piece> pieces, Point location)
        {
            foreach (Control c in this.map.Controls)
            {
                if(c is Button)
                {
                    if((c as Button).Location.X == location.X && (c as Button).Location.Y == location.Y)
                        {
                            this.RemovePiece(pieces,(c as Button).Name);
                            this.map.Controls.Remove(c);
                        }
                }
            }
        }
        void RemovePiece(List<Piece> pieces, String Name)
        {
            string[] split = Name.Split(':');
            foreach (Piece p in pieces)
            {
                if(p.Location.X==Int32.Parse(split[1]) && p.Location.Y==Int32.Parse(split[2]))
                {
                        pieces.Remove(p);
                        break;
                }
            }
        }

        Button GetPiece(Point location)
        {
            Button piece= new Button();
            foreach (Control c in this.map.Controls)
            {
                if(c is Button)
                {
                    if((c as Button).Location.X == location.X && (c as Button).Location.Y == location.Y)
                        piece = (c as Button);
                }
            }
            return piece;
        }





        //void BackgroundFix()
        //{
        //    foreach (Control b in this.parent.Controls)
        //    {
        //        if(b is Button)
        //        { 
        //            Button button = (b as Button);
        //            Position pos = new Position(int.Parse((button.Name.Split(':'))[1]), int.Parse((button.Name.Split(':'))[2]));
        //            string name = $"p{pos.Row}:{pos.Col}";
        //            Control c = getControl(name);
        //            b.BackColor = (c as Panel).BackColor;
        //        }
        //    }

        //}

        //Control getControl(string name)
        //{
        //    Control control= new Control();
        //        foreach (Control c in this.parent.Controls)
        //        {
        //            if(c.Name.Equals(name))
        //            {
        //                control = c;
        //                break;
        //            }
        //        }
        //    return control;
        //}


















































        // private void C_LastMove(object sender, EventArgs e)
        // {
        //     int row = int.Parse(((sender as Button).Name.Split(':'))[0]);
        //     int col = int.Parse(((sender as Button).Name.Split(':'))[1]);


        //     if (!(((row % 2).Equals(0) && (col % 2).Equals(1)) || (row % 2).Equals(1) && (col % 2).Equals(0)))
        //     {
        //         foreach (Control c in this.parent.Controls)
        //         {

        //             if (c is Button)
        //             {
        //                 if ((c as Button).Name.Equals($"{row + 1}:{col - 1}"))
        //                 {
        //                     (c as Button).BackColor = System.Drawing.ColorTranslator.FromHtml("#987654");
        //                 }
        //             }

        //         }
        //         foreach (Control c in this.parent.Controls)
        //         {

        //             if (c is Button)
        //             {
        //                 if ((c as Button).Name.Equals($"{row + 1}:{col + 1}"))
        //                 {
        //                     (c as Button).BackColor = System.Drawing.ColorTranslator.FromHtml("#987654");
        //                 }
        //             }

        //         }
        //     }

        // }

        // private void C_Moves(object sender, System.EventArgs e)
        // {
        //     int row = int.Parse(((sender as Button).Name.Split(':'))[0]);
        //     int col = int.Parse(((sender as Button).Name.Split(':'))[1]);


        //     if (!(((row % 2).Equals(0) && (col % 2).Equals(1)) || (row % 2).Equals(1) && (col % 2).Equals(0)))
        //     {
        //         foreach (Control c in this.parent.Controls)
        //         {

        //             if (c is Button)
        //             {
        //                 if ((c as Button).Name.Equals($"{row + 1}:{col - 1}"))
        //                 {
        //                     (c as Button).BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f879");
        //                 }
        //             }

        //         }
        //         foreach (Control c in this.parent.Controls)
        //         {

        //             if (c is Button)
        //             {
        //                 if ((c as Button).Name.Equals($"{row + 1}:{col + 1}"))
        //                 {
        //                     (c as Button).BackColor = System.Drawing.ColorTranslator.FromHtml("#f6f879");
        //                 }
        //             }

        //         }
        //     }

        // }
    }
}
