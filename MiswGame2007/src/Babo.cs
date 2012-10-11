using System;

namespace MiswGame2007
{
    public class Babo : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 30;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;
        private const int NUM_ANIMATIONS = 16;

        private static Vector SIZE = new Vector(32, 40);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(16, 24), SIZE);

        private Direction direction;
        private bool canJump;
        private bool normalJump;
        private bool attackJump;
        private int jumpCount;
        private int animation;

        public Babo(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            canJump = false;
            normalJump = false;
            attackJump = false;
            jumpCount = 60;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            if (jumpCount > 0)
            {
                jumpCount--;
            }

            if (canJump && jumpCount == 0)
            {
                double dx = game.Player.Center.X - Center.X;
                double dy = game.Player.Center.Y - Center.Y;
                if (Math.Abs(dx) < 256 && Math.Abs(dy) < 64)
                {
                    if (dx < 0)
                    {
                        velocity.X = -4 * game.Random.NextDouble();
                        direction = Direction.Left;
                    }
                    else if (dx > 0)
                    {
                        velocity.X = 4 * game.Random.NextDouble();
                        direction = Direction.Right;
                    }
                    velocity.Y = 8 * game.Random.NextDouble() - 16;
                    attackJump = game.Random.Next(2) == 0;
                    normalJump = !attackJump;
                    jumpCount = game.Random.Next(15, 30);
                }
                else
                {
                    if (direction == Direction.Left)
                    {
                        velocity.X = -4 * game.Random.NextDouble();
                    }
                    else
                    {
                        velocity.X = 4 * game.Random.NextDouble();
                    }
                    velocity.Y = 4 * game.Random.NextDouble() - 8;
                    attackJump = false;
                    normalJump = true;
                    jumpCount = game.Random.Next(30, 60);
                }
            }

            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }

            canJump = false;
            MoveBy(input, velocity);

            base.Tick(input);

            if (canJump || attackJump)
            {
                animation = (animation + 1) % NUM_ANIMATIONS;
            }
            else if (velocity.Y < 0)
            {
                animation = 0;
            }
            else if (animation <= 8)
            {
                animation++;
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            int textureRow = 0;
            int textureCol = animation / 2;
            if (direction == Direction.Left)
            {
                if (!attackJump)
                {
                    graphics.DrawImageFix(GameImage.Babo, 64, 64, textureRow, textureCol, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFix(GameImage.Babo, 64, 64, textureRow + 1, textureCol, drawX, drawY, this);
                }
            }
            else
            {
                if (!attackJump)
                {
                    graphics.DrawImageFixFlip(GameImage.Babo, 64, 64, textureRow, textureCol, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFixFlip(GameImage.Babo, 64, 64, textureRow + 1, textureCol, drawX, drawY, this);
                }
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(16);
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

        public override void Blocked_Bottom(GameInput input)
        {

            velocity = Vector.Zero;
            if (attackJump)
            {
                for (int i = 0; i < 5; i++)
                {
                    game.AddEnemyBullet(new BaboBullet(game, position + new Vector(32, 56), new Vector(8 * game.Random.NextDouble() - 4, 8 * game.Random.NextDouble() - 16)));
                }
                game.AddParticle(new BaboBulletExplosion(game, position + new Vector(32, 64), Vector.Zero));
                game.Quake(4);
                animation = 0;
            }
            if (normalJump)
            {
                animation = 0;
            }
            canJump = true;
            attackJump = false;
            normalJump = false;
        }
    }
}
