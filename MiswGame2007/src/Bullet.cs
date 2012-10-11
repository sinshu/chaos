using System;

namespace MiswGame2007
{
    public class Bullet
    {
        protected GameScene game;
        protected double radius;
        protected Vector position;
        protected Vector velocity;
        protected int damage;

        private bool removed;

        public Bullet(GameScene game, double radius, Vector position, Vector velocity, int damage)
        {
            this.game = game;
            this.radius = radius;
            this.position = position;
            this.velocity = velocity;
            this.damage = damage;
            removed = false;
        }

        public void Remove()
        {
            removed = true;
        }

        public virtual void Tick(ThingList targetThings)
        {
            MoveBy(velocity, targetThings);
        }

        public virtual void Draw(GraphicsDevice graphics)
        {
        }

        public virtual void Hit()
        {
        }

        public virtual void MoveBy(Vector d, ThingList targetThings)
        {
            if (removed)
            {
                return;
            }
            MoveBy_Horizontal(d.X, targetThings);
            MoveBy_Vertical(d.Y, targetThings);
        }

        public virtual void MoveBy_Horizontal(double d, ThingList targetThings)
        {
            if (d < 0)
            {
                MoveBy_Left(d, targetThings);
            }
            else if (d > 0)
            {
                MoveBy_Right(d, targetThings);
            }
        }

        public virtual void MoveBy_Vertical(double d, ThingList targetThings)
        {
            if (d < 0)
            {
                MoveBy_Up(d, targetThings);
            }
            else if (d > 0)
            {
                MoveBy_Down(d, targetThings);
            }
        }

        public virtual void MoveBy_Left(double d, ThingList targetThings)
        {
            position.X += d;
            foreach (Thing target in targetThings)
            {
                if (!target.Shootable) continue;
                if (Left < target.Right && target.Left < Right && Top < target.Bottom && target.Top < Bottom)
                {
                    if (position.X > target.Center.X)
                    {
                        Left = target.Right;
                    }
                    target.Damage(damage);
                    Hit();
                    return;
                }
            }
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int leftCol = LeftCol;
            Map map = game.Map;
            if (map.IsObstacle(topRow, leftCol) || map.IsObstacle(bottomRow, leftCol))
            {
                Left = (leftCol + 1) * Settings.BLOCK_WDITH;
                Hit();
            }
        }

        public virtual void MoveBy_Up(double d, ThingList targetThings)
        {
            position.Y += d;
            foreach (Thing target in targetThings)
            {
                if (!target.Shootable) continue;
                if (Left < target.Right && target.Left < Right && Top < target.Bottom && target.Top < Bottom)
                {
                    if (position.Y > target.Center.Y)
                    {
                        Top = target.Bottom;
                    }
                    target.Damage(damage);
                    Hit();
                    return;
                }
            }
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int topRow = TopRow;
            Map map = game.Map;
            if (map.IsObstacle(topRow, leftCol) || map.IsObstacle(topRow, rightCol))
            {
                Top = (topRow + 1) * Settings.BLOCK_WDITH;
                Hit();
            }
        }

        public virtual void MoveBy_Right(double d, ThingList targetThings)
        {
            position.X += d;
            foreach (Thing target in targetThings)
            {
                if (!target.Shootable) continue;
                if (Left < target.Right && target.Left < Right && Top < target.Bottom && target.Top < Bottom)
                {
                    if (position.X < target.Center.X)
                    {
                        Right = target.Left;
                    }
                    target.Damage(damage);
                    Hit();
                    return;
                }
            }
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int rightCol = RightCol;
            Map map = game.Map;
            if (map.IsObstacle(topRow, rightCol) || map.IsObstacle(bottomRow, rightCol))
            {
                Right = rightCol * Settings.BLOCK_WDITH;
                Hit();
            }
        }

        public virtual void MoveBy_Down(double d, ThingList targetThings)
        {
            position.Y += d;
            foreach (Thing target in targetThings)
            {
                if (!target.Shootable) continue;
                if (Left < target.Right && target.Left < Right && Top < target.Bottom && target.Top < Bottom)
                {
                    if (position.Y < target.Center.Y)
                    {
                        Bottom = target.Top;
                    }
                    target.Damage(damage);
                    Hit();
                    return;
                }
            }
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int bottomRow = BottomRow;
            Map map = game.Map;
            if (map.IsObstacle(bottomRow, leftCol) || map.IsObstacle(bottomRow, rightCol))
            {
                Bottom = bottomRow * Settings.BLOCK_WDITH;
                Hit();
            }
        }

        public void SpreadDebris(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                game.AddParticle(new Debris(game, position, new Vector(8 - 16 * game.Random.NextDouble(), 4 - 16 * game.Random.NextDouble()), game.Random.Next(0, 4)));
            }
        }

        public bool Removed
        {
            get
            {
                return removed;
            }
        }

        public Vector Position
        {
            get
            {
                return position;
            }
        }

        public Vector Velocity
        {
            get
            {
                return velocity;
            }
        }

        public double Left
        {
            get
            {
                return position.X - radius;
            }

            set
            {
                position.X = value + radius;
            }
        }

        public double Right
        {
            get
            {
                return position.X + radius;
            }

            set
            {
                position.X = value - radius;
            }
        }

        public double Top
        {
            get
            {
                return position.Y - radius;
            }

            set
            {
                position.Y = value + radius;
            }
        }

        public double Bottom
        {
            get
            {
                return position.Y + radius;
            }

            set
            {
                position.Y = value - radius;
            }
        }

        public int TopRow
        {
            get
            {
                return (int)Math.Floor(Top / Settings.BLOCK_WDITH);
            }
        }

        public int BottomRow
        {
            get
            {
                return (int)Math.Ceiling(Bottom / Settings.BLOCK_WDITH) - 1;
            }
        }

        public int LeftCol
        {
            get
            {
                return (int)Math.Floor(Left / Settings.BLOCK_WDITH);
            }
        }

        public int RightCol
        {
            get
            {
                return (int)Math.Ceiling(Right / Settings.BLOCK_WDITH) - 1;
            }
        }
    }
}
