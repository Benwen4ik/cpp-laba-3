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

        private const int NumStones = 10; // Количество объектов Stone для создания
        private const int NumPaper = 10;
        private const int MaxSpeed = 5;
        private readonly Random random = new Random();
        private readonly object lockObject = new object();
        private readonly List<Stone> stones = new List<Stone>();
        private readonly List<Paper> papers = new List<Paper>();
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Создаем потоки для создания и размещения объектов Stone
            for (int i = 0; i < NumStones; i++)
            {
                Thread thread = new Thread(CreateAndMoveStone);
                thread.Start();
            }
        }

        private void CreateAndMoveStone()
        {
            try { 
            // Создаем новый объект Stone
            Stone stone = new Stone();

                // Генерируем случайные координаты для объекта Stone
                int x = random.Next(Width - stone.Width);
                int y = random.Next(Height - stone.Height);

                // Генерируем случайное направление движения
             //   int dx = random.Next(-MaxSpeed, MaxSpeed + 1);
             //   int dy = random.Next(-MaxSpeed, MaxSpeed + 1);
                stone.setDx(random.Next(-MaxSpeed, MaxSpeed + 1));
                stone.setDy(random.Next(-MaxSpeed, MaxSpeed + 1));
                // Размещаем объект Stone на форме
                stone.Location = new System.Drawing.Point(x, y);
                AddStone(stone);

                // Запускаем бесконечный цикл для перемещения объекта Stone
                while (true)
                {
                    // Перемещаем объект Stone
                    MoveStone(stone);

                    // Проверяем, достиг ли объект Stone границы формы
                    if (stone.Left <= 0 || stone.Right >= ClientSize.Width)
                    {
                        // dx = -dx; // Изменяем направление движения по оси X
                        stone.setDx(-stone.getDx());
                    }

                    if (stone.Top <= 0 || stone.Bottom >= ClientSize.Height)
                    {
                        //  dy = -dy; // Изменяем направление движения по оси Y
                        stone.setDy(-stone.getDy());
                    }

                    // Проверяем столкновения с другими объектами Stone
                    CheckCollisions(stone);
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

        private void AddStone(Stone stone)
        {
            // Выполняем добавление объекта Stone на форму из главного потока
            if (InvokeRequired)
            {
                Invoke(new Action<Stone>(AddStone), stone);
            }
            else
            {
                stones.Add(stone);
                Controls.Add(stone);
            }
        }

        private void MoveStone(Stone stone)
        {
            // Выполняем перемещение объекта Stone на форме из главного потока
            if (InvokeRequired)
            {
                Invoke(new Action<Stone>(MoveStone), stone);
            }
            else
            {
                // Используем блокировку, чтобы предотвратить одновременный доступ к объекту Stone из разных потоков
                lock (lockObject)
                {
                    stone.Left += stone.getDx();
                    stone.Top += stone.getDy();
                }
            }
        }

        private void CheckCollisions(Stone stone)
        {
            lock (lockObject)
            {
                foreach (Stone otherStone in stones)
                {
                    if (otherStone != stone && stone.Bounds.IntersectsWith(otherStone.Bounds))
                    {
                        // Обнаружено столкновение
                        Point collisionPoint = GetCollisionPoint(stone, otherStone);
                        stone.ChangeDirection(collisionPoint);
                        otherStone.ChangeDirection(collisionPoint);
                    }
                }
            }
        }

        private Point GetCollisionPoint(Stone stone1, Stone stone2)
        {
            // Вычисляет центр пересечения границ двух объектов Stone
            int x1 = stone1.Left + stone1.Width / 2;
            int y1 = stone1.Top + stone1.Height / 2;
            int x2 = stone2.Left + stone2.Width / 2;
            int y2 = stone2.Top + stone2.Height / 2;

            int collisionX = (x1 + x2) / 2;
            int collisionY = (y1 + y2) / 2;

            return new Point(collisionX, collisionY);
        }
    }
}
