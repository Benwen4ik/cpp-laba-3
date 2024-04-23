﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace лаба_3
{
    public partial class Form1 : Form
    {

        private int NumStones = 3; // Количество объектов Stone для создания
        private int NumPaper = 3;
        private int NumScissors = 3;
        private const int MaxSpeed = 5;
        private const int MinDistance = 50;
        private readonly Random random = new Random();
        List<Symbol> controlsToRemove = new List<Symbol>();
        private readonly object lockObject = new object();
        private readonly List<Symbol> symbols = new List<Symbol>();
       // private List<Point> coordinates = new List<Point>();
        private List<Thread> threads = new List<Thread>();
        private bool Pause = true;
        private volatile bool isRunning = true;
        public Form1()
        {
            InitializeComponent();
            stonecount.Text = "3";
            papercount.Text = "3";
            scissorscount.Text = "3";
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                isRunning = true;
                NumStones = Convert.ToInt32(stonecount.Text);
                if (NumStones < 0 || NumStones > 15)
                {
                    MessageBox.Show("Элементов должно быть от 0 до 15");
                    NumStones = 3;
                    stonecount.Text = "3";
                }
                NumScissors = Convert.ToInt32(scissorscount.Text);
                if (NumScissors < 0 || NumScissors > 15)
                {
                    MessageBox.Show("Элементов должно быть от 0 до 15");
                    NumScissors = 3;
                    scissorscount.Text = "3";
                }
                NumPaper = Convert.ToInt32(papercount.Text);
                if (NumPaper < 0 || NumPaper > 15)
                {
                    MessageBox.Show("Элементов должно быть от 0 до 15");
                    NumPaper = 3;
                    papercount.Text = "3";
                }
                if (threads.Count == 0)
                {
                    // Создаем потоки для создания и размещения объектов Stone
                    for (int i = 0; i < NumStones; i++)
                    {
                        Thread thread = new Thread(CreateSymbol);
                        threads.Add(thread);
                        thread.Start(2);
                    }
                    for (int i = 0; i < NumPaper; i++)
                    {
                        Thread thread = new Thread(CreateSymbol);
                        threads.Add(thread);
                        thread.Start(1);
                    }
                    for (int i = 0; i < NumScissors; i++)
                    {
                        Thread thread = new Thread(CreateSymbol);
                        threads.Add(thread);
                        thread.Start(3);
                    }
                }
                else
                {
                    for (int i = 0; i < threads.Count; i++)
                    {
                        //threads[i].Start() ;
                    }
                }
            } catch (FormatException fe)
            {
                MessageBox.Show("Error: " + fe.Message);
                scissorscount.Text = "3";
                stonecount.Text = "3";
                papercount.Text = "3";
            }
            catch (Exception exp1)
            {
                MessageBox.Show("Error: " + exp1.Message);
            }
        }

        private void CreateSymbol (object type)
        {
            try
            {
                // Создаем новый объект Stone
                Symbol symbol = new Symbol((int)type);
                // Генерируем случайные координаты для объекта Stone
                int x = random.Next(20,Width - 2 * symbol.Width);
                int y = random.Next(70,Height - 2 * symbol.Height);

                // Проверяем, нет ли других объектов Stone вблизи сгенерированных координат
                while (HasCollision(symbols, x, y))
                {
                    x = random.Next(20,Width - 2 * symbol.Width);
                    y = random.Next(70,Height - 2 * symbol.Height);
                }
                // Генерируем случайное направление движения
                symbol.setDx(random.Next(-MaxSpeed, MaxSpeed + 1));
                symbol.setDy(random.Next(-MaxSpeed, MaxSpeed + 1));
                // Размещаем объект Stone на форме
                symbol.Location = new System.Drawing.Point(x, y);
                AddSymbol(symbol);
                CycleMoveSymbol(symbol);
            } catch (Exception e)
            {
                MessageBox.Show("Error: " +  e.Message);
            }
        }



        private void CycleMoveSymbol(Symbol symbol)
        {
            try { 
                // Создайте таймер с указанной периодичностью
                TimerCallback tm = new TimerCallback(CheckCollisionsType);
                // создаем таймер
                Timer timer = new Timer(tm, symbol, 1000, 10);

               // TimerCallback collis = new TimerCallback(CheckCollisions);
                // создаем таймер
              //  Timer timerCollis = new Timer(collis, symbol, 0, 100);
                // Запускаем бесконечный цикл для перемещения объекта Stone
                while (isRunning)
                {
                    if (!isRunning) {
                        timer.Dispose();
                        break; 
                    }
                    while (!Pause)
                    {
                         Thread.Sleep(500);
                    }
                    // Перемещаем объект Stone
                    MoveSymbol(symbol);

                    // Проверяем, достиг ли объект Stone границы формы
                    if (symbol.Left <= 5 || symbol.Right >= (ClientSize.Width-10))
                    {
                        // dx = -dx; // Изменяем направление движения по оси X
                        symbol.setDx(-symbol.getDx());
                    }

                    if (symbol.Top <= 50 || symbol.Bottom >= (ClientSize.Height-10))
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
            catch(NullReferenceException ex)
            {
                Console.WriteLine("Error " + ex.Message);
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
                controlsToRemove.Add(symbol);
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

        private void CheckCollisions(object symbolobj)
        {
                Symbol symbol = (Symbol)symbolobj;
                foreach (Symbol otherSymbol in symbols)
                {
                    if (otherSymbol != symbol && symbol.Bounds.IntersectsWith(otherSymbol.Bounds))
                    {
                        // Обнаружено столкновение
                       // Point collisionPoint = GetCollisionPoint(symbol, otherSymbol);
                    //symbol.ChangeDirection(collisionPoint);
                    //otherSymbol.ChangeDirection(collisionPoint);
                    symbol.setDx(-symbol.getDx());
                    symbol.setDy(-symbol.getDy());
                    otherSymbol.setDx(-symbol.getDx());
                    otherSymbol.setDy(-symbol.getDy());
                    //ChangeType(symbol,otherSymbol);
                    // otherSymbol.ChangeType(symbol);
                }
                }

        }

        private void CheckCollisionsType(object symbolobj)
        {
            try
            {
                if (!isRunning) return;
                if (symbols.Count == 0) return;
                Symbol symbol = (Symbol)symbolobj;
                if (InvokeRequired)
                {
                    Invoke(new Action<Symbol>(CheckCollisionsType), symbol);
                }
                else
                {
                    lock (lockObject)
                    {
                        foreach (Symbol otherSymbol in symbols)
                        {
                            if (otherSymbol != symbol && symbol.Bounds.IntersectsWith(otherSymbol.Bounds))
                            {
                                ChangeType(symbol, otherSymbol);
                               // otherSymbol.ChangeType(symbol);
                            }
                        }
                    }
                }
            } catch (ObjectDisposedException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            catch (ArgumentOutOfRangeException argexc)
            {
                Console.WriteLine("Error: " + argexc.Message);
                return;
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

        private bool HasCollision(List<Symbol> symbols, int x, int y)
        {
            foreach (Symbol symbol in symbols)
            {
                if (Math.Abs(symbol.Left - x) < (symbol.Width + MinDistance) && Math.Abs(symbol.Top - y) < (symbol.Height + MinDistance))
                    return true;
            }

            return false;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            Pause = !Pause;
        }

        public void ChangeType(Symbol symbol, Symbol otherSymbol)
        {
            lock (lockObject)
            {
                if (symbol.getType() == otherSymbol.getType()) { return; }
                if (symbol.getType() == 1 && otherSymbol.getType() == 3)
                {
                    // Image = Properties.Resources.ножницы
                    symbol.clearImage();
                    symbol.getImage(Properties.Resources.ножницы);
                    symbol.setType(3);
                }
                if (symbol.getType() == 2 && otherSymbol.getType() == 1)
                {
                    //Image = Properties.Resources.paper3;
                    symbol.clearImage();
                    symbol.getImage(Properties.Resources.бумага3);
                    symbol.setType(1);
                }
                if (symbol.getType() == 3 && otherSymbol.getType() == 2)
                {
                    symbol.clearImage();
                    symbol.getImage(Properties.Resources.камень);
                    symbol.setType(2);
                }
                //
                if (symbol.getType() == 3 && otherSymbol.getType() == 1)
                {
                    // Image = Properties.Resources.ножницы
                    otherSymbol.clearImage();
                    otherSymbol.getImage(Properties.Resources.ножницы);
                    otherSymbol.setType(3);
                }
                if (symbol.getType() == 1 && otherSymbol.getType() == 2)
                {
                    //Image = Properties.Resources.paper3;
                    otherSymbol.clearImage();
                    otherSymbol.getImage(Properties.Resources.бумага3);
                    otherSymbol.setType(1);
                }
                if (symbol.getType() == 2 && otherSymbol.getType() == 3)
                {
                    otherSymbol.clearImage();
                    otherSymbol.getImage(Properties.Resources.камень);
                    otherSymbol.setType(2);
                }
                return;
            }
        }

        private void abort_Click(object sender, EventArgs e)
        {
            isRunning = false;
            RemoveControlsFromUI();
            threads.Clear();
        }
        private void RemoveControlsFromUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RemoveControlsFromUI));
                return;
            }

            foreach (Symbol symbol in controlsToRemove)
            {
                symbols.Remove(symbol);
                Controls.Remove(symbol);
                symbol.Dispose();
            }

            controlsToRemove.Clear();
        }
    }
}
