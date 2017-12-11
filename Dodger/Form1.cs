using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Dodger
{
    public partial class Form1 : Form
    {
        List<Baddie> baddies = new List<Baddie>(); //list of baddies
        Player player = new Player();
        Random rand = new Random();
        Font fontBig = new Font("Arial", 24);
        Font fontSmall = new Font("Arial", 18);
        SoundPlayer gameOverSnd = new SoundPlayer(Properties.Resources.gameover);
        bool gameOver = true;
        int score = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!gameOver)
            {
                
                //genrate a random number between 0 and 10
                //if it's 0, create a new Baddie and add
                //it to the list of baddies
                
                if (rand.Next(0, 10) == 0)
                {
                    Baddie b = new Baddie();
                    baddies.Add(b);
                }

               
                //Use a for loop or foreach loop
                //to call the Move() function no each Baddie 
                //in the list, baddies.

                foreach (Baddie b in baddies)
                {
                    b.Move();
                }

           
                score++;

              
                CheckForCollision();
                DeleteOffscreenBaddies();
            }

            //trigger a paint event
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

       
            //Call the player's Draw() function and pass in
            //e.Graphics as the single parameter
            player.Draw(e.Graphics);

            
            //Loop over the list of baddies
            //for each one, call its Draw() function and pass in
            //e.Graphics as the single parameter
            foreach (Baddie b in baddies)
            {
                b.Draw(e.Graphics);
            }


            //don't change any of this
            e.Graphics.DrawString("SCORE: " + score.ToString(), fontSmall, Brushes.White, 10, 10);

            if (gameOver)
            {
                e.Graphics.DrawString("GAME OVER", fontBig, Brushes.White, 125, 125);
                e.Graphics.DrawString("Click the mouse to play again", fontSmall, Brushes.White, 50, 175);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
      
            //if the game is over...
            //  set the score to 0
            //  call the Clear() function in the baddies list (to empty it)
            //  set gameOver = false
            //  call the player's Reset() function
            if (gameOver == true)
            {
                score = 0;
                baddies.Clear();
                gameOver = false;
                player.Reset();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
          
            //where the mouse is The mouse coords are in the e parameter
            if (!gameOver)
            {
                player.x = e.X;
                player.y = e.Y;
            }
        }

        void CheckForCollision()
        {
            //create rectangle around the player
            Rectangle prect = new Rectangle(player.x, player.y, player.image.Width, player.image.Width);

            foreach (Baddie b in baddies)
            {
                //create a rectangle around the current baddie, b.
                Rectangle brect = new Rectangle(b.x, b.y, b.image.Width, b.image.Height);

                //if the rects intersect, the game is over
                if (brect.IntersectsWith(prect))
                {
                    gameOverSnd.Play();
                    gameOver = true;
                    break;
                }
            }
        }


        void DeleteOffscreenBaddies()
        {
            for (int i = 0; i < baddies.Count; i++)
            {
                if (baddies[i].y > 700)
                {
                    baddies.RemoveAt(i);
                    i--;
                }
            }
        }

    }

    class Baddie
    {
        public int x;
        public int y;
        public int speed=5;
        static Random rand = new Random();
        public Image image = Properties.Resources.baddie;

        public Baddie()
        {
            y = 0;
            x = rand.Next(40,600);
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(Properties.Resources.baddie, x, y);
        }

        public void Move()
        {
            y += speed;
        }

    }

    class Player
    {
        public int x=320;
        public int y=240;
        public Image image = Properties.Resources.player;

        public void Draw(Graphics g)
        {
            g.DrawImage(Properties.Resources.player, x, y);
        }

        public void Reset()
        {
            x = 320;
            y = 240;
        }

    }

}
