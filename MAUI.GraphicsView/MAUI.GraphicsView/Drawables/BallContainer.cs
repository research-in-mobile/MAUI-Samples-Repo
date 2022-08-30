
using Android.Telecom;
using System.Drawing;

namespace MAUI.GraphicsView.Drawables
{
    public class BallContainer : IDrawable
    {
        //private readonly List<BouncingBall> Balls;
        private readonly BouncingBall[] Balls;
        private int BallCount = 4; 

        public BallContainer(int ballCount)
        {
            Balls = new BouncingBall[ballCount];

            //Balls = new List<BouncingBall>()
            //{
            //    new BouncingBall(),
            //    new BouncingBall(),
            //    new BouncingBall(),
            //    new BouncingBall()
            //};
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.DarkViolet;
            canvas.FillRectangle(dirtyRect);

            foreach (var ball in Balls)
            {
                ball?.Draw(canvas);
            }
        }

        public void Randomize(double width, double height)
        {
            Random rand = new();
            //for (int i = 0; i < Balls.Count; i++)
            for (int i = 0; i < Balls.Length; i++)
                {
                Balls[i] = new()
                {
                    X = rand.NextDouble() * width,
                    Y = rand.NextDouble() * height,
                    Radius = rand.NextDouble() * 5 + 5,
                    XVel = rand.NextDouble() - .5,
                    YVel = rand.NextDouble() - .5,
                    R = (byte)rand.Next(50, 255),
                    G = (byte)rand.Next(50, 255),
                    B = (byte)rand.Next(50, 255),
                };
            }
        }

        public void Advance(double timeDelta, double width, double height)
        {
            foreach (var ball in Balls)
            {
                ball?.Advance(timeDelta, width, height);
            }
        }
    }
}
