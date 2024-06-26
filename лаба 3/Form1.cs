﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private int MaxSpeed = 5;
        private const int MinDistance = 50;
        private readonly Random random = new Random();
        List<Symbol> controlsToRemove = new List<Symbol>();
        private readonly object lockObject = new object();
       // private readonly object lockObject2 = new object();
        private readonly List<Symbol> symbols = new List<Symbol>();
       // private List<Point> coordinates = new List<Point>();
        private List<Thread> threads = new List<Thread>();
        private bool Pause = true;
        private volatile bool isRunning = true;
        string jsonfile = @"score.json";

        class Users
        {
            public string user;
            public int CountWin;
            public int CountLose;
            public Users(string str, int w, int l)
            {
                user = str;
                CountWin = w;
                CountLose = l;
            }
            public void getWin ()
            {
                CountWin ++;
            }
            public void getLose()
            {
               CountLose ++;
            }
        };

       // int CountWin = 0;
       // int CountLose = 0;
        List<Users> listUsers = new List<Users> { };


        public Form1()
        {
            try
            {
                InitializeComponent();
                stonecount.Text = "3";
                papercount.Text = "3";
                scissorscount.Text = "3";
                speedbox.Text = "5";
                winComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                winComboBox.Items.Add("Stone");
                winComboBox.Items.Add("Paper");
                winComboBox.Items.Add("Scissors");
                string jsonstring = "";
                if (File.Exists(jsonfile))
                {
                    jsonstring = File.ReadAllText(jsonfile);
                    listUsers = JsonConvert.DeserializeObject<List<Users>>(jsonstring);
                }
                changeLabel();
            }
            catch (Exception exp1)
            {
                MessageBox.Show("Error: " + exp1.Message);
            }
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
                MaxSpeed = Convert.ToInt32(speedbox.Text);
                if (MaxSpeed < 1 || MaxSpeed > 15)
                {
                    MessageBox.Show("Скорость должна быть от 1 до 15");
                    MaxSpeed = 5;
                    speedbox.Text = "5";
                }
                if (threads.Count == 0)
                {
                    if ((NumPaper + NumScissors + NumStones) > 20 && (ClientSize.Width < 1000 || ClientSize.Height < 1000))
                    {
                        MessageBox.Show("Элементов слишко много для маленького окна. Откройте полноэкранный режим");
                        return;
                    }
                    for (int i = 0; i < NumStones; i++)
                    {
                        Thread thread = new Thread(CreateSymbol);
                        threads.Add(thread);
                        thread.Name = "stone" + i;
                        thread.Start(2);
                     //   thread.Join();
                    }
                    for (int i = 0; i < NumPaper; i++)
                    {
                        Thread thread = new Thread(CreateSymbol);
                        threads.Add(thread);
                        thread.Name = "paper" + i;
                        thread.Start(1);
                     //   thread.Join();
                    }
                    for (int i = 0; i < NumScissors; i++)
                    {   
                        Thread thread = new Thread(CreateSymbol);
                        threads.Add(thread);
                        thread.Name = "scissors" + i;
                        thread.Start(3);
                    //    thread.Join();
                    }
                    SelectWinAsync();
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

        private  void CreateSymbol (object type)
        {
            try
            {
                Symbol symbol = new Symbol((int)type);
                int x = random.Next(20,Width - 2 * symbol.Width);
                int y = random.Next(70,Height - 2 * symbol.Height);

                bool hasCollision = true;

                while (hasCollision)
                {
                    x = random.Next(20, Width - 2 * symbol.Width);
                    y = random.Next(70, Height - 2 * symbol.Height);

                    Monitor.Enter(lockObject);

                    try
                    {
                        hasCollision = HasCollision(symbols, x, y);
                        if (!hasCollision)
                        {
                            symbol.setDx(random.Next(-MaxSpeed, MaxSpeed + 1));
                            symbol.setDy(random.Next(-MaxSpeed, MaxSpeed + 1));
                            symbol.Location = new System.Drawing.Point(x, y);
                            symbols.Add(symbol);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lockObject);
                    }
                }
                AddSymbol(symbol);
                CycleMoveSymbol(symbol);
            } catch (Exception e)
            {
                MessageBox.Show("Error: " +  e.Message);
            }
        }

        private async Task SelectWinAsync()
        {
            try
            {
                while (isRunning)
                {
                    if (NumStones == 0 && NumScissors == 0)
                    {
                        if (!foundUser(UserText.Text))
                            listUsers.Add(new Users(UserText.Text, 0, 0));
                        isRunning = false;
                        MessageBox.Show("Paper win");
                        getScore(1);
                        changeLabel();
                    }
                    if (NumStones == 0 && NumPaper == 0)
                    {
                        if (!foundUser(UserText.Text))
                            listUsers.Add(new Users(UserText.Text, 0, 0));
                        isRunning = false;
                        MessageBox.Show("Scissors win");
                        getScore(2);
                        changeLabel();
                    }
                    if (NumPaper == 0 && NumScissors == 0)
                    {
                        if (!foundUser(UserText.Text))
                            listUsers.Add(new Users(UserText.Text, 0, 0));
                        isRunning = false;
                        MessageBox.Show("Stone win");
                        getScore(0);
                        changeLabel();
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error " + exp.Message);
            }
        }

        private bool foundUser(string name)
        {
            if (listUsers == null) listUsers = new List<Users> { };
            foreach(Users user in listUsers)
            {
                if (user.user == name) return true;
            }
            return false;
        }

        private void getScore(int a)
        {
            foreach (Users us in listUsers)
            {
                if (us.user == UserText.Text)
                {
                    if (winComboBox.SelectedIndex == a) us.CountWin += 1;
                    else us.getLose();
                }
            }
            Pause = !Pause;
            isRunning = false;
            RemoveControlsFromUI();
            threads.Clear();
            Pause = !Pause;
        }

       

        private void changeLabel()
        {
            labelScore.Text = "";
            if (listUsers == null)
            {
                listUsers = new List<Users> { };
                return;
            }
            foreach(Users us in listUsers) 
            labelScore.Text += us.user + " Win:" + us.CountWin + " Lose:" + us.CountLose + "\n";
        }



        private void CycleMoveSymbol(Symbol symbol)
        {
            try {
                // TimerCallback tm = new TimerCallback(SelectWin);
                // создаем таймер
                // Timer timer = new Timer(tm, symbol, 1000, 10);
               // TimerCallback tm = new TimerCallback(SelectWin);
               // Timer timer = new Timer(tm, "kek", 100, 100);

                //  TimerCallback collis = new TimerCallback(CheckCollisions);
                // создаем таймер
                //  Timer timerCollis = new Timer(collis, symbol, 0, 100);
                while (isRunning)
                {
                    if (!isRunning)
                    {
                       // timer.Dispose();
                        //break;
                        return;
                    }
                    while (!Pause)
                    {
                        if (!isRunning)
                        {
                          //  timer.Dispose();
                            //break;
                            return;
                        }
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

                   CheckCollisions(symbol);
                  // SelectWin();
                    //CheckCollisionsType(symbol);
                    //i/f (NumStones == 0 || NumScissors == 0 || NumPaper == 0)
                    //SelectWin();
                    Thread.Sleep(100);
                }
              //  timer.Dispose();
            }
            catch (ObjectDisposedException e)
            {
                MessageBox.Show("Error " + e.Message);
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error " + exp.Message);
            }
        }

        private void AddSymbol(Symbol symbol)
        { 
            if (InvokeRequired)
            {
                BeginInvoke(new Action<Symbol>(AddSymbol), symbol);
            }
            else
            {
              //  symbols.Add(symbol);
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
                        CheckCollisionsType(symbol);
                        symbol.setDx(-symbol.getDx());
                        symbol.setDy(-symbol.getDy());
                        otherSymbol.setDx(-symbol.getDx());
                        otherSymbol.setDy(-symbol.getDy());

                        symbol.setTime(  DateTime.Now);
                        otherSymbol.setTime(DateTime.Now); 

                    if (DateTime.Now - symbol.getTime() > TimeSpan.FromSeconds(0.5))
                    {
                        symbol.setDx(-symbol.getDx());
                        symbol.setDy(-symbol.getDy());
                        symbol.setTime(DateTime.MinValue);
                    }
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
              //  if (!Pause) return;
                Symbol symbol = (Symbol)symbolobj;
                if (InvokeRequired)
                {
                    Invoke(new Action<Symbol>(CheckCollisionsType), symbol);
                }
                else
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
            } catch (ObjectDisposedException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            catch (ArgumentOutOfRangeException argexc)
            {
                Console.WriteLine("Error: " + argexc.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
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
                    NumPaper--;
                    NumScissors++;
                }
                if (symbol.getType() == 2 && otherSymbol.getType() == 1)
                {
                    //Image = Properties.Resources.paper3;
                    symbol.clearImage();
                    symbol.getImage(Properties.Resources.бумага3);
                    symbol.setType(1);
                    NumStones--;
                    NumPaper++;
                }
                if (symbol.getType() == 3 && otherSymbol.getType() == 2)
                {
                    symbol.clearImage();
                    symbol.getImage(Properties.Resources.камень);
                    symbol.setType(2);
                    NumScissors--;
                    NumStones++;
                }
                //
                if (symbol.getType() == 3 && otherSymbol.getType() == 1)
                {
                    // Image = Properties.Resources.ножницы
                    otherSymbol.clearImage();
                    otherSymbol.getImage(Properties.Resources.ножницы);
                    otherSymbol.setType(3);
                    NumPaper--;
                    NumScissors++;
                }
                if (symbol.getType() == 1 && otherSymbol.getType() == 2)
                {
                    //Image = Properties.Resources.paper3;
                    otherSymbol.clearImage();
                    otherSymbol.getImage(Properties.Resources.бумага3);
                    otherSymbol.setType(1);
                    NumStones--;
                    NumPaper++;
                }
                if (symbol.getType() == 2 && otherSymbol.getType() == 3)
                {
                    otherSymbol.clearImage();
                    otherSymbol.getImage(Properties.Resources.камень);
                    otherSymbol.setType(2);
                    NumScissors--;
                    NumStones++;
                }
                return;
            }
        }

        private void abort_Click(object sender, EventArgs e)
        {
            Pause = !Pause;
            isRunning = false;
            RemoveControlsFromUI();
            threads.Clear();
            Pause = !Pause;
           
        }
        private void RemoveControlsFromUI()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RemoveControlsFromUI));
                return;
            }
            lock (lockObject)
            {
                List<Symbol> symbolsToRemove = new List<Symbol>(controlsToRemove);

                foreach (Symbol symbol in symbolsToRemove)
                {
                    symbols.Remove(symbol);
                    Controls.Remove(symbol);
                    symbol.Dispose();
                }
                
                controlsToRemove.RemoveAll(symbolsToRemove.Contains);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           // string jsonString = System.Text.Json.JsonSerializer.Serialize(listUsers);
            string json = JsonConvert.SerializeObject(listUsers);
            File.WriteAllText(jsonfile, json);
            Pause = !Pause;
            isRunning = false;
       //     foreach (Thread thr in threads)
         //   {
        //        thr.Join();
           // }
            RemoveControlsFromUI();
           // threads.Clear();
          //  Pause = !Pause;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void clearScoreButton_Click(object sender, EventArgs e)
        {
            labelScore.Text = "Score";
            listUsers.Clear();

        }
    }
}
