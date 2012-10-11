using System;

namespace MiswGame2007
{
    public class Starman : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 250;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;

        private static Vector SIZE = new Vector(48, 80);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(24, 16), SIZE);

        private Direction direction;
        private bool jumping;
        private int jumpCount;
        private bool attacking;
        private int attackCount;
        private int attackRange;
        private double waveX;
        private bool waveAttack;
        private int animation;

        public Starman(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            jumping = false;
            jumpCount = game.Random.Next(30, 60);
            attacking = false;
            attackCount = 0;
            attackRange = game.Random.Next(128, 256);
            waveX = 2 * Math.PI * game.Random.NextDouble();
            waveAttack = game.Random.Next(0, 2) == 0;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            if (Math.Abs(dx) < 320 && Math.Abs(dy) < 128 && !attacking)
            {
                if (dx < 0)
                {
                    direction = Direction.Left;
                }
                else if (dx > 0)
                {
                    direction = Direction.Right;
                }
            }

            if (jumpCount > 0)
            {
                jumpCount--;
            }
            if (jumpCount == 0)
            {
                if (!jumping && !attacking)
                {
                    jumping = true;
                    if (Math.Abs(dx) < 320 && Math.Abs(dy) < 128)
                    {
                        double dx2 = dx - Math.Sign(dx) * attackRange;
                        if (Math.Abs(dx2) < 4)
                        {
                            velocity.X = 0;
                        }
                        else if (dx2 < 0)
                        {
                            velocity.X = -2;
                        }
                        else
                        {
                            velocity.X = 2;
                        }
                    }
                    else
                    {
                        if (direction == Direction.Left)
                        {
                            velocity.X = -2;
                        }
                        else
                        {
                            velocity.X = 2;
                        }
                    }
                    velocity.Y = -4 * game.Random.NextDouble() - 8;
                }
            }

            if (jumping)
            {
                animation = 3;
            }
            else
            {
                animation = 0;
            }

            if (attacking)
            {
                if (waveAttack)
                {
                    if (attackCount < 128)
                    {
                        if (attackCount == 0)
                        {
                            game.PlaySound(GameSound.Flame);
                        }
                        if (attackCount < 32)
                        {
                            animation = attackCount / 2 % 3;
                        }
                        else if (attackCount < 96)
                        {
                            if (attackCount % 2 == 0)
                            {
                                if (direction == Direction.Left)
                                {
                                    game.AddEnemyBullet(new StarmanBullet(game, Center + new Vector(-8, -16), direction, waveX));
                                }
                                else
                                {
                                    game.AddEnemyBullet(new StarmanBullet(game, Center + new Vector(8, -16), direction, waveX));
                                }
                            }
                            animation = attackCount % 3;
                        }
                        else
                        {
                            animation = attackCount / 2 % 3;
                        }
                        attackCount++;
                    }
                    if (attackCount == 128)
                    {
                        attacking = false;
                        attackCount = 0;
                        attackRange = game.Random.Next(128, 256);
                        waveX = 2 * Math.PI * game.Random.NextDouble();
                        waveAttack = game.Random.Next(0, 2) == 0;
                    }
                }
                else
                {
                    if (attackCount < 64)
                    {
                        if (attackCount < 16)
                        {
                        }
                        else if (attackCount < 64 && attackCount % 32 == 0)
                        {
                            if (direction == Direction.Left)
                            {
                                game.AddParticle(new BigExplosion(game, position + new Vector(0, 44), Vector.Zero));
                                for (int i = 0; i < 9; i++)
                                {
                                    game.AddEnemyBullet(new PlayerBullet(game, position + new Vector(0, 44), 176 + i, true));
                                }
                            }
                            else
                            {
                                game.AddParticle(new BigExplosion(game, position + new Vector(96, 44), Vector.Zero));
                                for (int i = 0; i < 9; i++)
                                {
                                    game.AddEnemyBullet(new PlayerBullet(game, position + new Vector(96, 44), -4 + i, true));
                                }
                            }
                            game.PlaySound(GameSound.Explode2);
                        }
                        attackCount++;
                    }
                    if (attackCount == 64)
                    {
                        attacking = false;
                        attackCount = 0;
                        attackRange = game.Random.Next(128, 256);
                        waveX = 2 * Math.PI * game.Random.NextDouble();
                        waveAttack = game.Random.Next(0, 2) == 0;
                    }
                }
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
                graphics.DrawImageFix(GameImage.Starman, 96, 96, 0, animation, drawX, drawY, this);
            }
            else
            {
                graphics.DrawImageFixFlip(GameImage.Starman, 96, 96, 0, animation, drawX, drawY, this);
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            game.AddParticle(new BigExplosion(game, Center + new Vector(16, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, -32), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(-16, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, 32), Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(24);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;
            if (!attacking && !(Math.Abs(dx) < 320 && Math.Abs(dy) < 128))
            {
                direction = Direction.Right;
            }
        }

        public override void Blocked_Right(GameInput input)
        {
            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;
            if (!attacking && !(Math.Abs(dx) < 320 && Math.Abs(dy) < 128))
            {
                direction = Direction.Left;
            }
        }

        public override void Blocked_Bottom(GameInput input)
        {
            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;
            velocity = Vector.Zero;
            if (jumping)
            {
                jumping = false;
                jumpCount = game.Random.Next(30, 60);
                if (Math.Abs(dx) < 320 && Math.Abs(dy) < 96)
                {
                    attacking = true;
                }
            }
        }
    }
}
