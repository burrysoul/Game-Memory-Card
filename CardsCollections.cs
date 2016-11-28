using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MemoryGame
{
    public class CardsCollections
    {
        internal  List<MemoryCard> GameOfThronesCollection(GameLogic Logic) //Метод возвращает список карт
        {
            List<MemoryCard> cards = new List<MemoryCard>();
            cards.Add(new MemoryCard(1, Properties.Resources.rubashka, Properties.Resources.starks,Logic));
            cards.Add(new MemoryCard(1, Properties.Resources.rubashka, Properties.Resources.starks, Logic));

            cards.Add(new MemoryCard(2, Properties.Resources.rubashka, Properties.Resources.lannisters, Logic));
            cards.Add(new MemoryCard(2, Properties.Resources.rubashka, Properties.Resources.lannisters, Logic));

            cards.Add(new MemoryCard(3, Properties.Resources.rubashka, Properties.Resources.arryn, Logic));
            cards.Add(new MemoryCard(3, Properties.Resources.rubashka, Properties.Resources.arryn, Logic));

            cards.Add(new MemoryCard(4, Properties.Resources.rubashka, Properties.Resources.greyjoy, Logic));
            cards.Add(new MemoryCard(4, Properties.Resources.rubashka, Properties.Resources.greyjoy, Logic));

            cards.Add(new MemoryCard(5, Properties.Resources.rubashka, Properties.Resources.martells, Logic));
            cards.Add(new MemoryCard(5, Properties.Resources.rubashka, Properties.Resources.martells, Logic));

            cards.Add(new MemoryCard(6, Properties.Resources.rubashka, Properties.Resources.targaryen, Logic));
            cards.Add(new MemoryCard(6, Properties.Resources.rubashka, Properties.Resources.targaryen, Logic));

            
            //Два действия ниже перемешивают массив
            Random rnd = new Random();
            cards = cards.OrderBy(item=>rnd.Next()).ToList(); //Сортируем массив в рандомном порядке

            return cards;
        }
    }
}
