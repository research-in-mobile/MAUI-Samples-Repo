using Java.Util;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MAUI.GraphicsView.Drawables
{
    public class PetDrawable : IDrawable
    {
        private System.Random _random = new System.Random();


        public int VerticleSteps = 0;
        public int HorizontalSteps = 0;
        public Point MoveTo = new Point(0, 0);
        public PointF Center = new Point(0, 0);

        public int X = 0;
        public int Y = 0;
        public float PetWidth = 50;

        bool _isInitilized = false;


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Navy;
            canvas.FillRectangle(dirtyRect);

            Center = dirtyRect.Center;
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 4;


            X = (int)(dirtyRect.Center.X - PetWidth / 2 + HorizontalSteps);
            Y = (int)(dirtyRect.Center.Y + VerticleSteps);

            //if(_isInitilized == false)
            //{
            //    MoveTo = new Point(X, Y);
            //    _isInitilized = true;
            //}

            RectF solidRectangle = new RectF(X, Y, PetWidth, PetWidth);

            
            SolidPaint solidPaint = new SolidPaint(Colors.AntiqueWhite);
            canvas.DrawRectangle(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Height); // Background
            canvas.SetFillPaint(solidPaint, solidRectangle);

            solidPaint = new SolidPaint(Colors.DarkSeaGreen);
            canvas.SetFillPaint(solidPaint, solidRectangle);
            canvas.SetShadow(new SizeF(10, 10), 10, Colors.Grey);
            canvas.FillRoundedRectangle(solidRectangle, 12);

            if (!MoveTo.IsEmpty)
            {
                canvas.DrawCircle((float)MoveTo.X, (float)MoveTo.Y, 3);
            }

        }

        public void SetRandomMove()
        {
            //VerticleSteps = _random.Next(-100,100);
            //HorizontalSteps = _random.Next(-100,100);

            MoveTo = new Point(_random.Next((int)Center.X - 100, (int)Center.X + 100), _random.Next((int)Center.Y - 100, (int)Center.Y + 100));
        }

        public Point ResetStep()
        {
            int x = 0;
            int y = 0;

            var moveX = MoveTo.X - (int)(PetWidth/2);
            var moveY = MoveTo.Y - (int)(PetWidth/2);

            if (X < moveX)
            {
                x++;
            }
            else if (X > moveX)
            {
                x--;
            }

            if (Y < moveY)
            {
                y++;
            }
            else if (Y > moveY)
            {
                y--;
            }

            return new Point(x, y);
        }
    }
}
