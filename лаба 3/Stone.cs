using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лаба_3
{
    public class Stone : PictureBox
    {
        private const int MaxSpeed = 5;
        private static readonly Random random = new Random();
        private int dx;
        private int dy;

        public Stone()
        {
            // Установка изображения для объекта 
            Image = Properties.Resources.камень;
            SizeMode = PictureBoxSizeMode.AutoSize;
        }


        public void ChangeDirection(Point collisionPoint)
        {
            // Вычисляем вектор направления отталкивания
             int dx1 = Left - collisionPoint.X;
            int  dy1 = Top - collisionPoint.Y;

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
    }
}
