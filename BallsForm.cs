using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GravityBalls
{
	public class BallsForm : Form
	{
		private Timer timer;
		private WorldModel world;
		private Font font;

		private WorldModel CreateWorldModel()
		{
			var w = new WorldModel
			{
				WorldHeight = ClientSize.Height,
				WorldWidth = ClientSize.Width,
				BallRadius = 10
			};
			w.PlayerX = w.WorldWidth - 40; //местоположение ракетки игрока
            w.PlayerY = w.WorldHeight / 2;
            w.EnemyX = w.EnemyWidth + 2;
            w.EnemyY = w.EnemyHeight / 2;
            w.BallX = w.WorldHeight / 2;
			w.BallY = w.BallRadius;
			return w;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
            world.PlayerX = world.WorldWidth - 40;
            world.PlayerY = world.WorldHeight / 2;
            world.EnemyX = world.EnemyWidth /  2;
            world.WorldHeight = ClientSize.Height;
			world.WorldWidth = ClientSize.Width;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			DoubleBuffered = true;
			BackColor = Color.Black;
			world = CreateWorldModel();
			timer = new Timer { Interval = 30 };
			timer.Tick += TimerOnTick;
			timer.Start();
			world.WorldHeight = ClientSize.Height;
			world.WorldWidth = ClientSize.Width;

			font = new Font(FontFamily.GenericSerif, 12.0F);
		}

		private void TimerOnTick(object sender, EventArgs eventArgs)
		{
			world.SimulateTimeframe(timer.Interval / 1000d);
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			var g = e.Graphics;
			g.SmoothingMode = SmoothingMode.HighQuality;

			var points = string.Format("{0} | {1}", world.EnemyScore, world.PlayerScore);
			g.DrawString(points, font, Brushes.White, (float)world.WorldWidth / 2, (float)10);

			//Отрисовка ракетки врага по заданным свойствам
			g.FillRectangle(Brushes.Blue,
                (float)(world.PlayerX),
                (float)(world.PlayerY - 40),
                (float)world.PlayerWidth,
                (float)world.PlayerHeight);

            g.FillRectangle(Brushes.Red,
                (float)(world.EnemyX),
                (float)(world.EnemyY - 2),
                (float)world.EnemyWidth,
                (float)world.EnemyHeight);

            g.FillEllipse(Brushes.White,
				(float)(world.BallX - world.BallRadius),
				(float)(world.BallY - world.BallRadius),
				2 * (float)world.BallRadius,
				2 * (float)world.BallRadius);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Text = string.Format("Cursor ({0}, {1})", e.X, e.Y);
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BallsForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "BallsForm";
            this.ResumeLayout(false);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.W)
            {
                world.PlayerY -= world.PlayerSpeed;
            }

            if (e.KeyCode == Keys.S)
            {
                world.PlayerY += world.PlayerSpeed;
            }
        }
    }
}