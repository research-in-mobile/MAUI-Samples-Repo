using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.GraphicsView.Drawables
{
    public class BouncingBall
    {
        public double X;
        public double Y;
        public double Radius = 5;
        public double XVel;
        public double YVel;
        public byte R, G, B;

        public void Draw(ICanvas canvas)
        {
            canvas.FillColor = Color.FromRgb(R, G, B);
            canvas.FillCircle((float)X, (float)Y, (float)Radius);
            canvas.DrawCircle((float)X, (float)Y, (float)Radius);
        }

        public void Advance(double timeDelta, double width, double height)
        {
            MoveForward(timeDelta);
            Bounce(width, height);
        }

        private void MoveForward(double timeDelta)
        {
            X += XVel * timeDelta;
            Y += YVel * timeDelta;
        }

        private void Bounce(double width, double height)
        {
            double minX = Radius;
            double minY = Radius;
            double maxX = width - Radius;
            double maxY = height - Radius;

            if (X < minX)
            {
                X = minX + (minX - X);
                XVel = -XVel;
            }
            else if (X > maxX)
            {
                X = maxX - (X - maxX);
                XVel = -XVel;
            }

            if (Y < minY)
            {
                Y = minY + (minY - Y);
                YVel = -YVel;
            }
            else if (Y > maxY)
            {
                Y = maxY - (Y - maxY);
                YVel = -YVel;
            }
        }
    }
}
