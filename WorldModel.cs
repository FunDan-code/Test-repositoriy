using System;
using System.CodeDom;
using System.Windows.Forms;

namespace GravityBalls
{
	public class WorldModel
	{
		public double BallX;
		public double BallY;
		public double BallVx = 300;
        public double BallVy = 300;
        public double BallRadius;
		public double WorldWidth;
		public double WorldHeight;
		public double R = 0.2; //Сила сопротивления
		public double G = 500; //Сила притяжения
		public double F = 30000; //Сила отталкивания от курсора

		//Параметры игрока
		public double PlayerX;
        public double PlayerY;
		public double PlayerHeight = 80;
		public double PlayerWidth = 20;
		public double PlayerSpeed = 25;
		public int PlayerScore = 0;

        //Параметры противника
        public double EnemyX;
        public double EnemyY;
        public double EnemyHeight = 80;
        public double EnemyWidth = 20;
        public double EnemySpeed = 25;
        public int EnemyScore = 0;

        public void SimulateTimeframe(double dt)
		{
			MoveBall(dt);
			ApplyWallsBouncing();
			ApplyPlayerBouncing(dt);
			ApplyEnemyBouncing(dt);
			ApplyEnemyMove();
			ApplyPointsCount();
            //ApplyAirResistance(dt);

            //ApplyG(dt);
            //ApplyF(dt);
        }

		private void ApplyPlayerBouncing(double dt)
		{
			if (BallX + BallRadius >= PlayerX && BallY + BallRadius >= PlayerY && BallY - BallRadius <= PlayerY + PlayerHeight)
			{
				BallVx = -BallVx;
			}
		}

        private void ApplyEnemyBouncing(double dt)
        {
            if (BallX - BallRadius <= EnemyX + EnemyWidth && BallY + BallRadius >= EnemyY && BallY - BallRadius <= EnemyY + EnemyHeight)
            {
                BallVx = -BallVx;
            }
        }

		private void ApplyEnemyMove()
		{
			if (EnemyY + EnemyHeight / 2 < BallY)
			{
				EnemyY += EnemySpeed;
			}
            if (EnemyY + EnemyHeight / 2 > BallY)
            {
				EnemyY -= EnemySpeed;
			}
		}

		private void ApplyPointsCount()
		{
			if(BallX - BallRadius == 0) //Левая стена - балл нам
			{
				PlayerScore++;
			}
            if (BallX + BallRadius == WorldWidth) //Правая стена - балл противнику
            {
                EnemyScore++;
            }

        }

        private void MoveBall(double dt)
		{
			BallX += BallVx * dt;
			BallY += BallVy * dt;

			BallX = Math.Max(BallRadius, Math.Min(BallX, WorldWidth- BallRadius));
            BallY = Math.Max(BallRadius, Math.Min(BallY, WorldHeight - BallRadius));
        }

		private void ApplyWallsBouncing()
		{
			if (BallX - BallRadius <= 0 || BallX + BallRadius >= WorldWidth)
			{
				BallVx *=-1;
			}
            if (BallY - BallRadius <= 0 || BallY + BallRadius >= WorldHeight)
			{
				BallVy *=-1;
			}

        }

		private void ApplyAirResistance(double dt)
		{
			BallVx -= BallVx * R *dt;
            BallVy -= BallVy * R * dt;


        }
		private void ApplyG(double dt)
		{
			BallVy += G * dt;
		}
		private void ApplyF(double dt)
		{
			var cursorX = Cursor.Position.X;
			var cursorY = Cursor.Position.Y;
			var dx = BallX - cursorX;
			var dy = BallY - cursorY;
			var d = Math.Sqrt(dx * dx + dy * dy);
			var f = F/ (d*d);
			BallVx += dx * f * dt;
			BallVy += dy * f * dt;
		}
	}
}