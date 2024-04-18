using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба_3
{
    public partial class Form1 : Form
    {

        private const int NumStones = 20; // Количество объектов Stone для создания
        private const int NumPaper = 10;
        private const int MaxSpeed = 10;
        private readonly Random random = new Random();
        private readonly object lockObject = new object();
        private readonly List<Stone> stones = new List<Stone>();
        private readonly List<Symbol> symbols = new List<Symbol>();
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Создаем потоки для создания и размещения объектов Stone
            for (int i = 0; i < NumStones; i++)
            {
                Thread thread = new Thread(CreateAndMoveSymbol);
                thread.Start();
            }
        }

        private void CreateAndMoveSymbol()
        {
            try {
                // Создаем новый объект Stone
                // Stone stone = new Stone();
                Symbol symbol = new Symbol();
                //T box = new T();
                // Генерируем случайные координаты для объекта Stone
                int x = random.Next(Width - symbol.Width);
                int y = random.Next(Height - symbol.Height);

                // Генерируем случайное направление движения
                //   int dx = random.Next(-MaxSpeed, MaxSpeed + 1);
                //   int dy = random.Next(-MaxSpeed, MaxSpeed + 1);
                symbol.setDx(random.Next(-MaxSpeed, MaxSpeed + 1));
                symbol.setDy(random.Next(-MaxSpeed, MaxSpeed + 1));
                // Размещаем объект Stone на форме
                symbol.Location = new System.Drawing.Point(x, y);
                AddSymbol(symbol);

                // Запускаем бесконечный цикл для перемещения объекта Stone
                while (true)
                {
                    // Перемещаем объект Stone
                    MoveSymbol(symbol);

                    // Проверяем, достиг ли объект Stone границы формы
                    if (symbol.Left <= 0 || symbol.Right >= ClientSize.Width)
                    {
                        // dx = -dx; // Изменяем направление движения по оси X
                        symbol.setDx(-symbol.getDx());
                    }

                    if (symbol.Top <= 0 || symbol.Bottom >= ClientSize.Height)
                    {
                        //  dy = -dy; // Изменяем направление движения по оси Y
                        symbol.setDy(-symbol.getDy());
                    }

                    // Проверяем столкновения с другими объектами Stone
                    CheckCollisions(symbol);
                    // Приостанавливаем поток на некоторое время
                    Thread.Sleep(100);
                }
            } catch (ObjectDisposedException e)
            {
                Console.WriteLine("Error " + e.Message);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error " + exp.Message);
            }
        }

        private void AddSymbol(Symbol symbol)
        {
            // Выполняем добавление объекта Stone на форму из главного потока
            if (InvokeRequired)
            {
                Invoke(new Action<Symbol>(AddSymbol), symbol);
            }
            else
            {
                symbols.Add(symbol);
                Controls.Add(symbol);
            }
        }

        private void MoveSymbol(Symbol symbol)
        {
            // Выполняем перемещение объекта Stone на форме из главного потока
            if (InvokeRequired)
            {
                Invoke(new Action<Symbol>(MoveSymbol), symbol);
            }
            else
            {
                // Используем блокировку, чтобы предотвратить одновременный доступ к объекту Stone из разных потоков
                lock (lockObject)
                {
                    symbol.Left += symbol.getDx();
                    symbol.Top += symbol.getDy();
                }
            }
        }

        private void CheckCollisions(Symbol symbol)
        {
            lock (lockObject)
            {
                foreach (Symbol otherSymbol in symbols)
                {
                    if (otherSymbol != symbol && symbol.Bounds.IntersectsWith(otherSymbol.Bounds))
                    {
                        // Обнаружено столкновение
                        Point collisionPoint = GetCollisionPoint(symbol, otherSymbol);
                        symbol.ChangeDirection(collisionPoint);
                        otherSymbol.ChangeDirection(collisionPoint);
                    }
                }
            }
        }

        private Point GetCollisionPoint(Symbol symbol1, Symbol symbol2)
        {
            // Вычисляет центр пересечения границ двух объектов Stone
            int x1 = symbol1.Left + symbol1.Width / 2;
            int y1 = symbol1.Top + symbol1.Height / 2;
            int x2 = symbol2.Left + symbol2.Width / 2;
            int y2 = symbol2.Top + symbol2.Height / 2;

            int collisionX = (x1 + x2) / 2;
            int collisionY = (y1 + y2) / 2;

            return new Point(collisionX, collisionY);
        }
    }
}
