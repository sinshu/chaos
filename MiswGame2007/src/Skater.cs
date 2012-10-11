using System;

namespace MiswGame2007
{
    public class Skater : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 50;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;

        private static Vector SIZE = new Vector(24, 64);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(36, 32), SIZE);

        private Direction direction;
        private int stateCount;
        private int stateCount2;
        private bool flipAnimation;

        public Skater(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            stateCount = 0;
            stateCount2 = 0;
            flipAnimation = false;
        }

        public override void Tick(GameInput input)
        {
            if (Math.Abs(velocity.X) < 0.125)
            {
                velocity.X = 0;
            }
            else
            {
                velocity.X -= Math.Sign(velocity.X) * 0.125;
            }

            if (stateCount == 0)
            {
                double dx = game.Player.Center.X - Center.X;
                double dy = game.Player.Center.Y - Center.Y;
                if (Math.Abs(dy) < 128)
                {
                    if (dx < 0)
                    {
                        direction = Direction.Left;
                    }
                    else if (dx > 0)
                    {
                        direction = Direction.Right;
                    }
                    stateCount2 = game.Random.Next(45, 60);
                }
                else
                {
                    stateCount2 = game.Random.Next(45, 75);
                }
            }
            if (stateCount == 12)
            {
                if (direction == Direction.Left)
                {
                    velocity.X = -8;
                }
                else
                {
                    velocity.X = 8;
                }
            }
            stateCount++;
            if (stateCount >= stateCount2)
            {
                stateCount = 0;
                flipAnimation = !flipAnimation;
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
            int textureCol = 3;
            if (stateCount < 16)
            {
                textureCol = stateCount / 4;
            }
            if (flipAnimation)
            {
                textureCol = 3 - textureCol;
            }
            if (direction == Direction.Left)
            {
                graphics.DrawImageFix(GameImage.Skater, 96, 96, 0, textureCol, drawX, drawY, this);
            }
            else
            {
                graphics.DrawImageFixFlip(GameImage.Skater, 96, 96, 0, textureCol, drawX, drawY, this);
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            for (int i = 0; i < 32; i++)
            {
                double x = Left + (Right - Left) * game.Random.NextDouble();
                double y = Top + (Bottom - Top) * game.Random.NextDouble();
                double vx = (x - Center.X) * (0.5 - 0.25 * game.Random.NextDouble());
                double vy = (y - Center.Y) * (0.5 - 0.25 * game.Random.NextDouble()) - 2;
                game.AddEnemyBullet(new HouseBullet(game, new Vector(x, y), new Vector(vx, vy)));
            }

            game.AddParticle(new BigExplosion(game, Center + new Vector(16, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, -32), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(-16, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, 32), Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(16);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            velocity.X = -velocity.X;
        }

        public override void Blocked_Right(GameInput input)
        {
            velocity.X = -velocity.X;
        }
    }
}
