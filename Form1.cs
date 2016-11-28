using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        GameLogic Logic;
        public Form1()
        {
            InitializeComponent();
            ScoreLabel.Text = "0";
            Logic = new GameLogic();
            Logic.SetCards(this, (new CardsCollections()).GameOfThronesCollection(Logic));
            Logic.OpenCardsTemporary();
        }
       
       public void ChangeScore(string score)
        {
            ScoreLabel.Text = score;
        }

       
    }
}
