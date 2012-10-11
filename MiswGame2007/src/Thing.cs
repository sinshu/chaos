using System;

namespace MiswGame2007
{
    public abstract class Thing
    {
        protected GameScene game;
        protected Rectangle rectangle;
        protected Vector position;
        protected Vector velocity;
        protected int health;
        protected int damageFlash;

        private bool removed;

        public Thing(GameScene game, Rectangle rectangle, Vector position, Vector velocity, int health)
        {
            this.game = game;
            this.rectangle = rectangle;
            this.position = position;
            this.velocity = velocity;
            this.health = health;
            damageFlash = 0;
            removed = false;
        }

        public void Remove()
        {
            removed = true;
        }

        public virtual void Tick(GameInput input)
        {
            if (damageFlash < 32)
            {
                damageFlash = 0;
            }
            else
            {
                damageFlash -= 32;
            }
        }

        public virtual void Draw(GraphicsDevice graphics)
        {
        }

        public virtual void MoveBy(GameInput input, Vector d)
        {
            MoveBy_Horizontal(input, d.X);
            MoveBy_Vertical(input, d.Y);
        }

        public virtual void Damage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
            damageFlash = 256;
            game.PlaySound(GameSound.Damage);
        }

        public virtual void Die()
        {
        }

        public virtual void Blodked_Left(GameInput input)
        {
            velocity.X = 0;
        }

        public virtual void Blocked_Right(GameInput input)
        {
            velocity.X = 0;
        }

        public virtual void Blocked_Top(GameInput input)
        {
            velocity.Y = 0;
        }

        public virtual void Blocked_Bottom(GameInput input)
        {
            velocity.Y = 0;
        }

        public virtual void MoveBy_Horizontal(GameInput input, double d)
        {
            if (d < 0)
            {
                MoveBy_Left(input, d);
            }
            else if (d > 0)
            {
                MoveBy_Right(input, d);
            }
        }

        public virtual void MoveBy_Vertical(GameInput input, double d)
        {
            if (d < 0)
            {
                MoveBy_Up(input, d);
            }
            else if (d > 0)
            {
                MoveBy_Down(input, d);
            }
        }

        public virtual void MoveBy_Left(GameInput input, double d)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int leftCol = LeftCol;
            Map map = game.Map;
            for (int row = topRow; row <= bottomRow; row++)
            {
                if (map.IsObstacle(row, leftCol))
                {
                    Left = (leftCol + 1) * Settings.BLOCK_WDITH;
                    Blodked_Left(input);
                    break;
                }
            }
        }

        public virtual void MoveBy_Up(GameInput input, double d)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int topRow = TopRow;
            Map map = game.Map;
            for (int col = leftCol; col <= rightCol; col++)
            {
                if (map.IsObstacle(topRow, col))
                {
                    Top = (topRow + 1) * Settings.BLOCK_WDITH;
                    Blocked_Top(input);
                    break;
                }
            }
        }

        public virtual void MoveBy_Right(GameInput input, double d)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int rightCol = RightCol;
            Map map = game.Map;
            for (int row = topRow; row <= bottomRow; row++)
            {
                if (map.IsObstacle(row, rightCol))
                {
                    Right = rightCol * Settings.BLOCK_WDITH;
                    Blocked_Right(input);
                    break;
                }
            }
        }

        public virtual void MoveBy_Down(GameInput input, double d)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int bottomRow = BottomRow;
            Map map = game.Map;
            for (int col = leftCol; col <= rightCol; col++)
            {
                if (map.IsObstacle(bottomRow, col))
                {
                    Bottom = bottomRow * Settings.BLOCK_WDITH;
                    Blocked_Bottom(input);
                    break;
                }
            }
        }

        public static bool Overlappes(Thing a, Thing b)
        {
            return a.Left < b.Right && b.Left < a.Right && a.Top < b.Bottom && b.Top < a.Bottom;
        }

        public void SpreadDebris(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                double x = Left + (Right - Left) * game.Random.NextDouble();
                double y = Top + (Bottom - Top) * game.Random.NextDouble();
                double vx = (x - Center.X) * (0.5 - 0.25 * game.Random.NextDouble());
                double vy = (y - Center.Y) * (0.5 - 0.25 * game.Random.NextDouble()) - 4;
                game.AddParticle(new Debris(game, new Vector(x, y), new Vector(vx, vy), game.Random.Next(0, 4)));
            }
        }

        public void DoSomeExplode()
        {
            double x = Left + (Right - Left) * game.Random.NextDouble();
            double y = Top + (Bottom - Top) * game.Random.NextDouble();
            game.AddParticle(new BigExplosion(game, new Vector(x, y), Vector.Zero));
        }

        public bool Removed
        {
            get
            {
                return removed;
            }
        }

        public GameScene Game
        {
            get
            {
                return game;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return rectangle;
            }
        }

        public Vector Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public double Left
        {
            get
            {
                return position.X + rectangle.Left;
            }

            set
            {
                position.X = value - rectangle.Left;
            }
        }

        public double Right
        {
            get
            {
                return position.X + rectangle.Right;
            }

            set
            {
                position.X = value - rectangle.Right;
            }
        }

        public double Top
        {
            get
            {
                return position.Y + rectangle.Top;
            }

            set
            {
                position.Y = value - rectangle.Top;
            }
        }

        public double Bottom
        {
            get
            {
                return position.Y + rectangle.Bottom;
            }

            set
            {
                position.Y = value - rectangle.Bottom;
            }
        }

        public Vector LeftTop
        {
            get
            {
                return position;
            }
        }

        public Vector RightTop
        {
            get
            {
                return new Vector(position.X + rectangle.Right, position.Y);
            }
        }

        public Vector LeftBottom
        {
            get
            {
                return new Vector(position.X, position.Y + rectangle.Bottom);
            }
        }

        public Vector RightBottom
        {
            get
            {
                return position + rectangle.RightBottom;
            }
        }

        public Vector Center
        {
            get
            {
                return position + rectangle.Center;
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

        public int DamageCount
        {
            get
            {
                if (damageFlash < 0)
                {
                    return 0;
                }
                else if (damageFlash > 255)
                {
                    return 255;
                }
                else
                {
                    return damageFlash;
                }
            }
        }

        public int DamageColorGreen
        {
            get
            {
                int color = 256 - damageFlash / 2;
                if (color > 255)
                {
                    return 255;
                }
                else
                {
                    return color;
                }
            }
        }

        public int DamageColorBlue
        {
            get
            {
                int color = 256 - damageFlash;
                if (color < 0)
                {
                    return 0;
                }
                else if (color > 255)
                {
                    return 255;
                }
                else
                {
                    return color;
                }
            }
        }

        public virtual bool Shootable
        {
            get
            {
                return true;
            }
        }
    }
}
