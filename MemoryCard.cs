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
 public class MemoryCard:PictureBox //Наследуем от PictureBox для того, чтобы можно было работать с объектом, как с обычным контролом
    {
        Image backImage, frontImage; //рубашка и основная картинка соответственно
        
        bool isOpened; //"открыта ли карта рубашкой"
        GameLogic parentLogic; //ссылка на экземпляр класса GameLogic, необходим для доступа к другим картам, общему счету и прочим необходимым методам
        int pairNumber; //Парный номер, только у двух карт на столе одинаковое значение этой переменной, по сути, нужна, чтобы распознавать "брата-близнеца" текущей карты
      
        //Обертка над полями в виде свойств
        public Image BackImage
        {
            get
            {
                return backImage;
            }

            set
            {
                backImage = value;
            }
        }

        public Image FrontImage
        {
            get
            {
                return frontImage;
            }

            set
            {
                frontImage = value;
            }
        }

       
        //Сюда нужно заглянуть
        public bool IsOpened
        {
            get
            {
                return isOpened;
            }

            set
            {
               //Этот код означает, что при изменении этого свойства, автоматически поменяется и основная картинка карты - на рубашку или переднюю картинку
                if (value) //Если передаваемое значение имеет тип "true"
                    Image = FrontImage; // Если true - нужно сделать основную картинку картинкой фронта
                else Image = BackImage; // Если false - нужно "перевернуть карту и сделать основной картинкой рубашку
                isOpened = value;
            }
        }

        internal GameLogic ParentLogic
        {
            get
            {
                return parentLogic;
            }

            set
            {
                parentLogic = value;
            }
        }

        public int PairNumber
        {
            get
            {
                return pairNumber;
            }

            set
            {
                pairNumber = value;
            }
        }

      public MemoryCard()
        {

        }  

        public MemoryCard(int pairNumb, Image back, Image front,GameLogic parLogic)
        {
            //Класс принимает в конструктор парный номер, картинку переда, рубашки, а также используемый программой экземпляр GameLogic, о котором говорилось выше
            
            BackImage = back;
            FrontImage = front;
            ParentLogic = parLogic;
          
            PairNumber = pairNumb;

            IsOpened = false;
          
            //Для того чтобы использовать экземпляры класса как контролы, им надо дать размер и (необязательно, но так красивее) способ отображения картинки в контроле
            SizeMode = PictureBoxSizeMode.StretchImage; //Делаем так, чтобы картинка(текущая) растягивалась на всю ширину нашего контрола
            Size = new Size(80, 120);
            
            this.Click += Opener; //Делаем обработчик для клика

        }
        //Класс-обработчик клика
        public void Opener(object sender, EventArgs e)
        {
           
            
     if (parentLogic.CanPlay) //"Если ничего не мешает игре", подробнее об этой переменной можно прочитать в комментариях класса GameLogic
        if (ParentLogic.OpenedPairNumber!=0) // Если эта переменная равна нулю, это означает, что ни одна карта не открыта
                {
                  if (!IsOpened) //Избегаем ситуации, когда щелчок по одной и той же карте будет накручивать очки, если карта уже открыта,по щелчку на ней действий происходить не будет
                    if (ParentLogic.OpenedPairNumber==pairNumber) //Сравниваем Pair Number уже открытой и текущей карты, как и было сказано, всего может быть две карты с одним и тем же Pair Number
                    {
                        IsOpened = true; //Открываем карту
                        ParentLogic.OpenedPairNumber = 0; //Раз карта открыта, можно приступать к угадыванию других карт, поэтому сбрасываем значение текущей открытой карты
                        ParentLogic.OpenedCard = null; //Обнуляем это значение, тк карта, которую мы только что открыли, перестает считаться "текущей открытой картой"


                        ParentLogic.Score++; //Увеличиваем счет
                        ParentLogic.CurrentForm.ChangeScore(Convert.ToString(ParentLogic.Score)); //Транслируем этот счет в label на форме

                    }
                    else
                    {
                        ParentLogic.OpenedCard.IsOpened = false; //Если пользователь ошибся с картой, закрываем текущую открытую карту
                        OpenCardTemporary();//Показываем временно карту, по которой только что щелкнули, подробнее реализацию смотрите в методе
                        ParentLogic.OpenedPairNumber = 0; //Обнуляем значение текущей открытой карты
                        ParentLogic.Score--;//Штраф за ошибку
                        ParentLogic.CurrentForm.ChangeScore(Convert.ToString(ParentLogic.Score)); //Передаем счет в форму
                    }
                }
                else //Если ни одной карты еще не открыто, тогда просто открываем текущую карту, чтобы затем найти ее двойника
                {
                    IsOpened = true; //Открываем карту
                    ParentLogic.OpenedCard = this; //Передаем текущую карту в специальное свойство в экземпляре GameLogic
                    ParentLogic.OpenedPairNumber = PairNumber; //Передаем парный номер в экземпляр GameLogic, теперь по этому номеру будут сверяться другие карты, чтобы узнать, являются ли они парными
                }
                
           
        }
        
        //Это асинхронный метод, при его выполнениее создается отдельный поток, который его выполняет, async указывает, что внутри данного метода есть await, который возвращает Task (поток/задачу)
        public async void OpenCardTemporary()
        {
            await OpenTemporaryTask(); //await должен стоять перед методом, возвращающем поток, await чем-то похоже на return, только return возвращает одно конкретное значение, и программа ждет получение этого значение, в случае с await программа не дожидается выполнения метода, на который он указывает, а просто запускает его в отдельный поток и идет дальше
        }
        Task OpenTemporaryTask()
        {
            //Возращаем поток
            return Task.Run(() =>
            {
                ParentLogic.CanPlay = false; //Этим мы запретим игру на момент "временного открытия карты"
                IsOpened = true;//Открываем карту
                Thread.Sleep(1000);//Ждем секунду, чтобы пользователь успел разглядеть карту
                IsOpened = false;//Закрываем карту после ожидания
                ParentLogic.CanPlay = true;//Разрешаем игру
                //
            }
            );
        }
        
    }
}
