using Hnatyshyn.Nazar._5i.ArkanoidV1.Models;
using System;
using System.Diagnostics;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Hnatyshyn.Nazar._5i.ArkanoidV1
{
    //Hnatyshyn Nazar 5I
    //Gioco: Arkanoid
    //06/11/2015
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int ButtomBorderSize = 15, lifes = 3, score = 0, StartBricksYPos = 100, stage = 1, MaxScore = 0;
        Paddle Paddle;
        Ball Ball;
        Brick[] Bricks;

        SoundPlayer SoundCollisionPaddle = new SoundPlayer("Sounds/paddle.wav");
        SoundPlayer SoundCollisionBrick = new SoundPlayer("Sounds/brick.wav");
        SoundPlayer SoundLose = new SoundPlayer("Sounds/lose.wav");
        //MusicPlayerOld SoundTrack;

        GameStates GameState;

        public enum GameStates
        {
            WaitToStart,
            Playing,
            Pause,
            Lose,
            Win,
            Menu,
            Closing
        }

        DispatcherTimer ball_timer;
        DispatcherTimer lbl_states_timer;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameState = GameStates.WaitToStart;

            NativeMethods.SetCursorPos(Convert.ToInt32(this.Width / 2), Convert.ToInt32(this.Height / 2));
            lbl_lifes.Content = "Lifes: " + lifes;
            lbl_score.Content = "Score: " + score;
            lbl_stage.Content = "Stage: " + stage;

            //----Creazione della pallina e paddle
            Random R = new Random();
            float BallXDirection = R.Next(-150, 151) / 100;
            Paddle = new Paddle(this.Width / 2 - 135 / 2, this.Height - ButtomBorderSize - 30, 30, 135, new SolidColorBrush(Color.FromArgb(255, 106, 106, 106)));
            Ball = new Ball(Paddle.PosX + Paddle.Width / 2, Paddle.PosY - 25, 25, 25, new SolidColorBrush(Color.FromArgb(255, 106, 106, 106)), BallXDirection, -1, 3);
            Container.Children.Add(Ball.Ellipse);
            Container.Children.Add(Paddle.Rectangle);

            //----Creazione dei mattoncini
            CreateBricks(StartBricksYPos);

            SoundCollisionPaddle.Load();
            SoundCollisionBrick.Load();
            //try
            //{
            //    SoundTrack = new MusicPlayerOld("soundtrack.wav");
            //    SoundTrack.Play(true);
            //}
            //catch { }
        }

        private void CreateBricks(int StartYPos)
        {
            int StartYPosition = StartYPos;
            int BricksNumberInLine = Convert.ToInt32(Container.ActualWidth / 110);
            int BrickLines = 7;
            int StartIndex = Convert.ToInt32((this.Width - (BricksNumberInLine * 110)) / 2);
            int BricksNumber = BricksNumberInLine * BrickLines;

            Bricks = new Brick[BricksNumber];
            Brush Color = Brushes.DarkBlue;
            for (int i = 0; i < BrickLines; i++)
            {
                switch (i)
                {
                    case 0: Color = Brushes.Red; break;
                    case 1: Color = Brushes.Orange; break;
                    case 2: Color = Brushes.Yellow; break;
                    case 3: Color = Brushes.Lime; break;
                    case 4: Color = Brushes.Cyan; break;
                    case 5: Color = Brushes.Blue; break;
                    case 6: Color = Brushes.DarkViolet; break;

                }
                for (int j = 0; j < BricksNumberInLine; j++)
                {
                    Bricks[i * BricksNumberInLine + j] = new Brick(StartIndex + j * (100 + 10), StartYPosition + i * (30 + 10), 30, 100, Color, (7 - i) * 100);
                    Bricks_Container.Children.Add(Bricks[i * BricksNumberInLine + j].Rectangle);
                }
            }
            foreach (Brick Br in Bricks)
            {
                MaxScore += Br.Score;
                    }
            //----del timer della pallina
            ball_timer = new DispatcherTimer();
            ball_timer.Tick += new EventHandler(Ball_Timer_Tick);
            ball_timer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            lbl_states_timer = new DispatcherTimer();
            lbl_states_timer.Tick += new EventHandler(lbl_states_timer_Tick);
            lbl_states_timer.Interval = new TimeSpan(0, 0, 0, 1);
        }

        public void Ball_Timer_Tick(object sender, EventArgs e)
        {
            if (GameState == GameStates.Playing)
            {
                //----Movimento della pallina
                Ball.MoveBall();
                //----Controllo se la pallina va contro le pareti
                Ball.BorderCollision(this.Width);
                //----Controllo collisione contro paddle
                if (Ball.IntersectsWithPaddle(Paddle.Rect))
                    SoundCollisionPaddle.Play();
                //----Controllo collisione con mattoncini
                foreach (Brick Br in Bricks)
                {
                    if (Ball.IntersectsWithBrick(Br.Rect))
                    {
                        SoundCollisionBrick.Play();
                        Br.Rectangle.Opacity = 0;
                        Br.Rect.Location = new Point(-200, -200);
                            Ball.Speed += 0.2;
                        Ball.Ellipse.Fill = Br.Rectangle.Fill;
                        score += Br.Score;
                        lbl_score.Content = "Score: " + score;
                        if (score == MaxScore)
                        {
                            if (stage > 5)
                            {
                                Win();
                                break;
                            }
                            stage++;
                            lbl_stage.Content = "Stage: " + stage;
                            StartBricksYPos += 60;
                            Bricks_Container.Children.Clear();
                            CreateBricks(StartBricksYPos);
                            ball_timer.Stop();
                            Random R = new Random();
                            float BallXDirection = R.Next(-100, 101) / 100;
                            Ball.Reset(Paddle.PosX + Paddle.Width / 2 - Ball.Width / 2, Paddle.PosY - 25, 25, 25, new SolidColorBrush(Color.FromArgb(255, 106, 106, 106)), BallXDirection, -1, 3); //new Ball(Paddle.PosX + Paddle.Width / 2, Paddle.PosY - 25, 25, 25, Brushes.Orange, +1, -1, 3);
                            GameState = GameStates.WaitToStart;

                        }
                        break;
                    }
                }
                //----Controllo se la pallina esce dal bordo inferiore
                if (Ball.LeavesScreen(this.Height))
                {
                    SoundLose.Play();
                    ball_timer.Stop();
                    Random R = new Random();
                    float BallXDirection = R.Next(-150, 151) / 100;
                    Ball.Reset(Paddle.PosX + Paddle.Width / 2 - Ball.Width / 2, Paddle.PosY - 25, 25, 25, new SolidColorBrush(Color.FromArgb(255, 106, 106, 106)), BallXDirection, -1, 5); //new Ball(Paddle.PosX + Paddle.Width / 2, Paddle.PosY - 25, 25, 25, Brushes.Orange, +1, -1, 3);
                    lifes--;
                    GameState = GameStates.WaitToStart;
                    if (lifes == -1)
                    {
                        Lose();
                        lifes = 0;
                    }
                    lbl_lifes.Content = "Lifes: " + lifes;
                }
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.Close();
                    break;
                case Key.PageUp:
                    Ball.Speed++;
                    break;
                case Key.PageDown:
                    Ball.Speed--;
                    break;
                case Key.P:
                    Pause();
                    break;
                case Key.Space:
                    if (GameState == GameStates.Lose || GameState == GameStates.Win)
                    {
                        Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                    if (GameState == GameStates.WaitToStart)
                    {
                        ball_timer.Start();
                        SoundCollisionPaddle.Play();
                        GameState = GameStates.Playing;
                    }
                    break;
                case Key.Left:
                    Paddle.PosX -= 20;
                    if (GameState == GameStates.WaitToStart)
                        Ball.PosX = Paddle.PosX + Paddle.Width / 2 - Ball.Width / 2;
                    break;
                case Key.Right:
                    Paddle.PosX += 20;
                    if (GameState == GameStates.WaitToStart)
                        Ball.PosX = Paddle.PosX + Paddle.Width / 2 - Ball.Width / 2;
                    break;
            }
        }

        private void Pause()
        {
            if (GameState != GameStates.Pause)
            {
                lbl_state.Content = "Pause";
                GameState = GameStates.Pause;
                lbl_states_timer.Start();
            }
            else
            {
                lbl_state.Content = "";
                GameState = GameStates.Playing;
                lbl_states_timer.Stop();
            }
        }
        private void Menu()                                 // Non viene utilizzato
        {
            if (GameState != GameStates.Menu)
            {
                lbl_state.Content = "";
                GameState = GameStates.Menu;
                lbl_states_timer.Start();
            }
            else
            {
                lbl_state.Content = "";
                GameState = GameStates.Playing;
                lbl_states_timer.Stop();
            }
        }

        private void Lose()
        {
            if (GameState != GameStates.Lose)
            {
                lbl_state.Content = "You Lose \n Press \"space\" to restart or \"Esc\" to exit";
                GameState = GameStates.Lose;
                lbl_states_timer.Start();
            }
        }

        private void Win()                                                                          //Non viene utilizzato
        {
            if (GameState != GameStates.Win)
            {
                lbl_state.Content = "You Win \n Press \"space\" to restart or \"Esc\" to exit";
                GameState = GameStates.Win;
                lbl_states_timer.Start();
            }
        }

        private void lbl_states_timer_Tick(object sender, EventArgs e)
        {
            if (lbl_state.Opacity != 0)
            lbl_state.Opacity = 0;
            else
                lbl_state.Opacity = 100;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (GameState == GameStates.WaitToStart)
            {
                ball_timer.Start();
                SoundCollisionPaddle.Play();
                GameState = GameStates.Playing;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //SoundTrack.StopPlaying();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (GameState == GameStates.Playing || GameState == GameStates.WaitToStart)
            {
                //----Il paddle segue il mouse
                Point MousePoint = e.GetPosition(null);
                if (MousePoint.X > Paddle.Width / 2 && MousePoint.X < this.Width - Paddle.Width / 2)        //Movimento della barra
                                                                                                            //Posso aggiornare le coordinate del paddle in 2 modi diversi
                                                                                                            //Paddle.UpdatePosition(MousePoint.X - Paddle.Width / 2);                               //2-*

                    Paddle.PosX = MousePoint.X - Paddle.Width / 2;                                          //3-*

                //----La pallina segue il paddle se il timer è disattivato
                if (GameState == GameStates.WaitToStart)
                    Ball.PosX = Paddle.PosX + Paddle.Width / 2 - Ball.Width / 2;
            }
        }
        public partial class NativeMethods
        {
            /// Return Type: BOOL->int  
            ///X: int  
            ///Y: int  
            [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
            [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool SetCursorPos(int X, int Y);
        }
    }
}
