using System;

namespace MiswGame2007
{
    public class Kyoro : Enemy
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

        private static Vector SIZE = new Vector(28, 44);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(18, 20), SIZE);

        private Direction direction;
        private bool playerDetected;
        private bool attacking;
        private int attackCount;
        private int attackCount2;
        private int playerRange;
        private int animation;

        public Kyoro(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            playerDetected = false;
            attacking = false;
            attackCount = 0;
            attackCount2 = 0;
            playerRange = 256;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            if (attackCount2 > 0)
            {
                attackCount2--;
            }

            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            playerDetected = false;
            if (attacking)
            {
                velocity.X = 0;
                if (attackCount < 32)
                {
                    if (attackCount == 0)
                    {
                        game.PlaySound(GameSound.Kue);
                    }
                    attackCount++;
                    if (attackCount == 16)
                    {
                        Vector posFix;
                        if (direction == Direction.Left)
                        {
                            posFix = new Vector(16, 16);
                        }
                        else
                        {
                            posFix = new Vector(48, 16);
                        }
                        game.AddEnemyBullet(new KyoroRocket(game, position + posFix, direction == Direction.Left ? 180 + 15 : -15));
                        game.PlaySound(GameSound.Rocket);
                        playerRange = game.Random.Next(128, 320);
                    }
                }
                if (attackCount >= 32)
                {
                    attacking = false;
                    attackCount = 0;
                }
                playerDetected = true;
                animation = 0;
            }
            else if (Math.Abs(dx) < 320 && Math.Abs(dy) < 128)
            {
                playerDetected = true;
                if (dx < 0)
                {
                    direction = Direction.Left;
                }
                else if (dx > 0)
                {
                    direction = Direction.Right;
                }
                
                if (attackCount2 == 0)
                {
                    attacking = true;
                    attackCount2 = game.Random.Next(60, 120);
                }
                else
                {
                    if (Math.Abs(Math.Abs(dx) - playerRange) < 4)
                    {
                        velocity.X = 0;
                        animation = 0;
                    }
                    else if (Math.Abs(dx) < playerRange)
                    {
                        if (dx < 0)
                        {
                            velocity.X = 1;
                        }
                        else if (dx > 0)
                        {
                            velocity.X = -1;
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                    else
                    {
                        if (dx < 0)
                        {
                            velocity.X = -1;
                        }
                        else if (dx > 0)
                        {
                            velocity.X = 1;
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                }
            }
            else
            {
                if (direction == Direction.Left)
                {
                    velocity.X = -1;
                }
                else
                {
                    velocity.X = 1;
                }
                animation = (animation + 1) % NUM_ANIMATIONS;
            }
            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }
            MoveBy(input, velocity);

            base.Tick(input);

            // Console.Write(attackCount + ",");
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                int textureCol = animation / 4;
                if (!attacking)
                {
                    graphics.DrawImageFix(GameImage.Kyoro, 64, 64, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    if (attackCount < 8)
                    {
                        graphics.DrawImageFix(GameImage.Kyoro, 64, 64, 1, attackCount / 2, drawX, drawY, this);
                    }
                    else if (attackCount < 24)
                    {
                        graphics.DrawImageFix(GameImage.Kyoro, 64, 64, 1, 3, drawX, drawY, this);
                    }
                    else
                    {
                        graphics.DrawImageFix(GameImage.Kyoro, 64, 64, 1, 3 - (attackCount - 24) / 2, drawX, drawY, this);
                    }
                }
            }
            else
            {
                int textureCol = animation / 4;
                if (!attacking)
                {
                    graphics.DrawImageFixFlip(GameImage.Kyoro, 64, 64, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    if (attackCount < 8)
                    {
                        graphics.DrawImageFixFlip(GameImage.Kyoro, 64, 64, 1, attackCount / 2, drawX, drawY, this);
                    }
                    else if (attackCount < 24)
                    {
                        graphics.DrawImageFixFlip(GameImage.Kyoro, 64, 64, 1, 3, drawX, drawY, this);
                    }
                    else
                    {
                        graphics.DrawImageFixFlip(GameImage.Kyoro, 64, 64, 1, 3 - (attackCount - 24) / 2, drawX, drawY, this);
                    }
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
            if (!playerDetected)
            {
                direction = Direction.Right;
            }
        }

        public override void Blocked_Right(GameInput input)
        {
            if (!playerDetected)
            {
                direction = Direction.Left;
            }
        }
    }
}
