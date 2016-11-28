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
  public class GameLogic
    {
        List<MemoryCard> cardsList; //список всех карт, необходим для групповых операций (например, чтобы парой строк открыть все карты)

        int openedPairNumber; //Парный номер, один и тот же номер есть только у двух карт
        Form1 currentForm; //Ссылка на форму, нам же надо как-то добавить в нее все карты
        MemoryCard openedCard; //Текущая открытая карта, нужна только для того, чтобы ее можно было закрыть, обрабатывая клик по другой карте
        bool canPlay; //Есть ситуации, когда нельзя выполнять игровую логику, например, в начале игры, когда открыты все карты, было бы не очень хорошо, если бы пользователь щелкал по картам, и обработчик выполнял бы соответствующие действия - сравнивал бы пары, считал очки и тд. Для этого и нужна эта переменная - чтобы заблокировать нежелательные действия в обработчике
        int score; //счет игры

        //Ниже представлены свойства для полей выше
        internal List<MemoryCard> CardsList
        {
            get
            {
                return cardsList;
            }

            set
            {
                cardsList = value;
            }
        }
             
        public Form1 CurrentForm
        {
            get
            {
                return currentForm;
            }

            set
            {
                currentForm = value;
            }
        }

        internal MemoryCard OpenedCard
        {
            get
            {
                return openedCard;
            }

            set
            {
                openedCard = value;
            }
        }

        public int OpenedPairNumber
        {
            get
            {
                return openedPairNumber;
            }

            set
            {
                openedPairNumber = value;
            }
        }

        public bool CanPlay
        {
            get
            {
                return canPlay;
            }

            set
            {
                canPlay = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        public GameLogic()
        {
           
            
        }
        public void SetCards(Form1 formToSet,List<MemoryCard> collectionToSet)
        {
           
            CurrentForm = formToSet;
            CardsList = collectionToSet;
            OpenedPairNumber = 0;
            //Начальные позиции для добавления карт
            int xpos = 40; 
            int ypos = 100;

            
           
            int counter = 0;
            //Ниже предсавлена логика для добавления карт в форму
            for (int i=0;i< CardsList.Count; i++)
            {
                
                if (counter%4==0 && counter!=0) //Карты представлены по 4 в ряд, если ряд заполняется, нужно перейти к заполнению следующего
                {

                    ypos += 140;//Опускаем ряд на 140 пикселей
                    xpos = 40; //Первая карта в новом ряду, координата X возвращается в начало
                }
                CardsList[i].Location = new Point(xpos, ypos); //Даем карте вычисленные координаты

                xpos += 120;//Следующая карта будет правее предыдущей (если ряд не кончится)
                counter++;//Обновляем счетчик карт для отсчета карт, в принципе, можно было использовать i, но тут банально проще ориентироваться с такой переменной 

                CurrentForm.Controls.Add(CardsList[i]);//Добавляем карту в форму
                  
                
            }
            
        }
        public void CloseAllCards() //Метод для закрытия всех карт
        {
            foreach(MemoryCard mc in CardsList)
            {
                mc.IsOpened = false;
            }
        }
        public void OpenAllCards() //Метод для открытия всех карт
        {
            foreach (MemoryCard mc in CardsList)
            {
                mc.IsOpened = true;
            }
        }
        //Подробно подобные асинхронные методы описаны в комментариях класса MemoryCard
        public async void OpenCardsTemporary()
        {
            await OpenCardsTask();
        }
        public Task OpenCardsTask()
        {
            return Task.Run(() =>
            {
                CanPlay = false;
               OpenAllCards();
                Thread.Sleep(5000);
                CloseAllCards();
                CanPlay = true;
            }
            );
        }
    }
}
