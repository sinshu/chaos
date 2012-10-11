using System;

namespace MiswGame2007
{
    public class Worm : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 3;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;

        private static Vector SIZE = new Vector(32, 16);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(0, 16), SIZE);

        private Direction direction;
        private int stateCount;
        private bool running;
        private int animation;

        public Worm(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            stateCount = game.Random.Next(30, 60);
            running = true;
            animation = 0;
        }

        public Worm(GameScene game, Vector position)
            : base(game, new Rectangle(new Vector(8, 24), new Vector(16, 8)), position, Vector.Zero, INIT_HEALTH / 2)
        {
            direction = game.Random.Next(0, 2) == 0 ? Direction.Left : Direction.Right;
            stateCount = game.Random.Next(30, 60);
            running = true;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            if (stateCount > 0)
            {
                stateCount--;
            }
            if (stateCount == 0)
            {
                if (Math.Abs(dx) < 320 && Math.Abs(dy) < 32)
                {
                    if (dx < 0)
                    {
                        direction = Direction.Left;
                    }
                    else if (dx > 0)
                    {
                        direction = Direction.Right;
                    }
                    stateCount = game.Random.Next(15, 30);
                    running = true;
                }
                else
                {
                    stateCount = game.Random.Next(30, 60);
                    running = game.Random.Next(0, 4) != 0;
                    if (!running)
                    {
                        animation = 4;
                    }
                }
            }

            if (running)
            {
                if (direction == Direction.Left)
                {
                    velocity.X = -4;
                }
                else
                {
                    velocity.X = 4;
                }
                animation = (animation + 1) % 8;
            }
            else
            {
                velocity.X = 0;
            }
            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }
            MoveBy(input, velocity);

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                graphics.DrawImageFix(GameImage.Worm, 32, 32, 0, animation, drawX, drawY, this);
            }
            else
            {
                graphics.DrawImageFixFlip(GameImage.Worm, 32, 32, 0, animation, drawX, drawY, this);
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.PlaySound(GameSound.Explode2);
            SpreadDebris(4);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            direction = Direction.Right;
        }

        public override void Blocked_Right(GameInput input)
        {
            direction = Direction.Left;
        }
    }
}
