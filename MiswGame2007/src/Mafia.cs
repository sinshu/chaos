using System;

namespace MiswGame2007
{
    public class Mafia : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 1;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;

        private static Vector SIZE = new Vector(32, 64);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(0, 0), SIZE);

        private Direction direction;
        private bool running;
        private int stateCount;
        private int animation;

        public Mafia(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            running = true;
            stateCount = game.Random.Next(30, 60);
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            if (stateCount > 0)
            {
                stateCount--;
            }
            if (stateCount == 0)
            {
                running = !running;
                stateCount = game.Random.Next(30, 60);
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
                animation = 0;
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
                graphics.DrawImageFix(GameImage.Mafia, 32, 64, 0, animation, drawX, drawY, this);
            }
            else
            {
                graphics.DrawImageFixFlip(GameImage.Mafia, 32, 64, 0, animation, drawX, drawY, this);
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
            SpreadDebris(8);
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
