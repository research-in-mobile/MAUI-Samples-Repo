using MAUI.GraphicsView.Drawables;
using Microsoft.Maui.Dispatching;
using System.Timers;

namespace MAUI.GraphicsView;

public partial class MainPage : ContentPage
{
    public int Speed = 3;
    System.Timers.Timer _ballUpdateTimer;
    System.Timers.Timer _petUpdateTimer;

    private readonly BallContainer ballContainer;


    public MainPage()
    {
        InitializeComponent();

        ballContainer = new BallContainer(10);
        ballContainer.Randomize(BallContainerView.Width, BallContainerView.Height);

        BallContainerView.Drawable = ballContainer;

        _ballUpdateTimer = new System.Timers.Timer(20);
        _petUpdateTimer = new System.Timers.Timer(33);

        _ballUpdateTimer.Elapsed += new ElapsedEventHandler(UpdateBallContainer);
        _petUpdateTimer.Elapsed += new ElapsedEventHandler(UpdatePetView);

        _ballUpdateTimer.Start();

    }

    private void UpdateBallContainer(object sender, EventArgs e)
    {
        ballContainer.Advance(10, BallContainerView.Width, BallContainerView.Height);

        BallContainerView.Invalidate();
    }


    public void UpdatePetView(object source, ElapsedEventArgs e)
    {
        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        var stepPoint = petDrawable.ResetStep();

        if (stepPoint.X != 0)
            petDrawable.HorizontalSteps += (int)stepPoint.X;
        if (stepPoint.Y != 0)
            petDrawable.VerticleSteps += (int)stepPoint.Y;

        if (stepPoint.X == 0 && stepPoint.Y == 0)
            _petUpdateTimer.Stop();

        this.PetGraphicsView.Invalidate();
    }


    private void MoveButton_Pressed(object sender, EventArgs e)
    {
        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        petDrawable.SetRandomMove();
        _petUpdateTimer.Start();

    }
    private void PetGraphicsView_StartInteraction(object sender, TouchEventArgs e)
    {
        PointF firstPoint = e.Touches.FirstOrDefault();

        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        petDrawable.MoveTo = new PointF((int)firstPoint.X, (int)firstPoint.Y);
        _petUpdateTimer.Start();
    }

    private void UpButton_Pressed(object sender, EventArgs e)
    {
        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        petDrawable.VerticleSteps -= Speed;

        petView.Invalidate();
    }
    private void DownButton_Pressed(object sender, EventArgs e)
    {
        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        petDrawable.VerticleSteps += Speed;

        petView.Invalidate();
    }
    private void LeftButton_Pressed(object sender, EventArgs e)
    {
        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        petDrawable.HorizontalSteps -= Speed;

        petView.Invalidate();
    }
    private void RightButton_Pressed(object sender, EventArgs e)
    {
        var petView = this.PetGraphicsView;
        var petDrawable = (PetDrawable)petView.Drawable;

        petDrawable.HorizontalSteps += Speed;

        petView.Invalidate();
    }

}

