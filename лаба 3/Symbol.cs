using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба_3
{
    public class Symbol : PictureBox
    {
        private const int MaxSpeed = 5;
        private static readonly Random random = new Random();
        private int type;
        private int dx;
        private int dy;

        private readonly object lockObject = new object();
        public Symbol()
        {
            // Установка изображения для объекта 
            Image = Properties.Resources.камень;
            SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public Symbol(int type)
        {
            this.type = type;
            switch (type)
            {
                case 1:
                    {
                        Image = Properties.Resources.paper3;
                        break;
                    }
                case 2:
                    {
                        Image = Properties.Resources.камень;
                        break;
                    }
                case 3:
                    {
                        Image = Properties.Resources.ножницы;
                        break;
                    }
                default: {
                        Image = Properties.Resources.камень;
                        break;
                    }
            }
            SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public void ChangeDirection(Point collisionPoint)
        {
            // Вычисляем вектор направления отталкивания
            int dx1 = Left - collisionPoint.X;
            int dy1 = Top - collisionPoint.Y;

            // Нормализуем вектор направления
            double magnitude = Math.Sqrt(dx * dx + dy * dy);
            dx1 = (int)Math.Round(MaxSpeed * dx / magnitude);
            dy1 = (int)Math.Round(MaxSpeed * dy / magnitude);

            // Изменяем направление движения объекта Stone
            //Left += dx1;
            //Top += dy1;
            dx = -dx1;
            dy = -dy1;
        }

        public void ChangeType(Symbol otherSymbol)
        {
            lock (lockObject)
            {
                if (this.type == otherSymbol.getType()) { return; }
                if (this.type == 1 && otherSymbol.getType() == 3)
                {
                    Image = Properties.Resources.ножницы;
                    //  this.type = 3;
                    return;
                }
                if (this.type == 2 && otherSymbol.getType() == 1)
                {
                    Image = Properties.Resources.paper3;
                    // this.type = 1;
                    return;
                }
                if (this.getType() == 3 && otherSymbol.getType() == 2)
                {
                    Image.Dispose();
                    Image = Properties.Resources.камень;
                    SizeMode = PictureBoxSizeMode.AutoSize;
                    this.type = 2;
                    // return;
                }
                return;
            }

        }

        public void getImage(Bitmap bitmap)
        {
            this.Image = bitmap;
        }

        public void clearImage()
        {
            this.Image.Dispose();
        }

        public int getDx()
        {
            return this.dx;
        }

        public int getDy()
        {
            return this.dy;
        }

        public void setDx(int dx)
        {
            this.dx = dx;
        }

        public void setDy(int dy)
        {
            this.dy = dy;
        }

        public int getType()
        {
            return this.type;
        }
        public void setType(int type)
        {
            this.type = type;
        }
    }
}

