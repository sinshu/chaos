using System;

namespace MiswGame2007
{
    public class Norio : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 150;

        private static Vector SIZE = new Vector(48, 96);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(40, 16), SIZE);

        private Direction direction;
        private Vector attackPos;
        private bool attacking;
        private int attackCount;
        private int attackCount2;

        private bool ghost;

        public Norio(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            if (game.Random.Next(0, 2) == 0)
            {
                double rand = game.Random.NextDouble();
                attackPos = new Vector(-128 - 128 * rand, -128 - 128 * rand);
            }
            else
            {
                double rand = game.Random.NextDouble();
                attackPos = new Vector(128 + 128 * rand, -128 - 128 * rand);
            }
            attacking = false;
            attackCount = 0;
            attackCount2 = 0;
        }

        public Norio(GameScene game, int row, int col, Direction direction, bool ghost)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            if (game.Random.Next(0, 2) == 0)
            {
                double rand = game.Random.NextDouble();
                attackPos = new Vector(-128 - 128 * rand, -128 - 128 * rand);
            }
            else
            {
                double rand = game.Random.NextDouble();
                attackPos = new Vector(128 + 128 * rand, -128 - 128 * rand);
            }
            attacking = false;
            attackCount = 0;
            attackCount2 = 120;

            this.ghost = ghost;
        }

        public override void Tick(GameInput input)
        {
            if (attackCount2 > 0)
            {
                attackCount2--;
            }

            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            if (attacking)
            {
                if (attackCount < 64)
                {
                    /*
                    if (attackCount >= 32 && attackCount % 2 == 0)
                    {
                        if (direction == Direction.Left)
                        {
                            double angle = 0.75 * Math.PI + 0.25 * Math.PI * game.Random.NextDouble() + 0.25 * Math.PI * game.Random.NextDouble() - 0.25 * Math.PI;
                            // double angle = 0.75 * Math.PI + 0.5 * Math.PI * game.Random.NextDouble() - 0.25 * Math.PI;
                            Vector posFix = new Vector(32, 96);
                            game.AddEnemyBullet(new BaakaBullet(game, position + posFix, 4 * new Vector(Math.Cos(angle), Math.Sin(angle))));
                        }
                        else
                        {
                            double angle = 0.25 * Math.PI + 0.25 * Math.PI * game.Random.NextDouble() + 0.25 * Math.PI * game.Random.NextDouble() - 0.25 * Math.PI;
                            // double angle = 0.25 * Math.PI + 0.5 * Math.PI * game.Random.NextDouble() - 0.25 * Math.PI;
                            Vector posFix = new Vector(96, 96);
                            game.AddEnemyBullet(new BaakaBullet(game, position + posFix, 4 * new Vector(Math.Cos(angle), Math.Sin(angle))));
                        }
                    }
                    */
                    if (attackCount == 48)
                    {
                        for (int i = 0; i < 48; i++)
                        {
                            if (direction == Direction.Left)
                            {
                                double angle = 135 + 15 * game.Random.NextDouble() + 15 * game.Random.NextDouble() - 15;
                                // double angle = 0.75 * Math.PI + 0.5 * Math.PI * game.Random.NextDouble() - 0.25 * Math.PI;
                                Vector posFix = new Vector(16, 112);
                                game.AddEnemyBullet(new PlayerBullet3(game, position + posFix, angle, 8 + 16 * game.Random.NextDouble()));
                            }
                            else
                            {
                                double angle = 45 + 15 * game.Random.NextDouble() + 15 * game.Random.NextDouble() - 15;
                                // double angle = 0.25 * Math.PI + 0.5 * Math.PI * game.Random.NextDouble() - 0.25 * Math.PI;
                                Vector posFix = new Vector(112, 112);
                                game.AddEnemyBullet(new PlayerBullet3(game, position + posFix, angle, 8 + 16 * game.Random.NextDouble()));
                            }
                        }
                        game.Quake(4);
                        game.Flash(32);
                        game.PlaySound(GameSound.Shotgun);
                    }
                    attackCount++;
                }
                if (attackCount >= 64)
                {
                    if (game.Random.Next(0, 2) == 0)
                    {
                        double rand = game.Random.NextDouble();
                        attackPos = new Vector(-128 - 128 * rand, -128 - 128 * rand);
                    }
                    else
                    {
                        double rand = game.Random.NextDouble();
                        attackPos = new Vector(128 + 128 * rand, -128 - 128 * rand);
                    }
                    attacking = false;
                    attackCount = 0;
                    attackCount2 = game.Random.Next(30, 60);
                }
            }

            if ((Math.Abs(dx) < 256 && Math.Abs(dy) < 256) || ghost)
            {
                velocity = 0.015625 * (game.Player.Center + attackPos - Center);
                double d = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                if (d > 16)
                {
                    velocity = 16 / d * velocity;
                }
                if (!attacking && attackCount2 == 0)
                {
                    if (dy > 0)
                    {
                        attacking = true;
                    }
                    else
                    {
                        if (game.Random.Next(0, 2) == 0)
                        {
                            double rand = game.Random.NextDouble();
                            attackPos = new Vector(-128 - 128 * rand, -128 - 128 * rand);
                        }
                        else
                        {
                            double rand = game.Random.NextDouble();
                            attackPos = new Vector(128 + 128 * rand, -128 - 128 * rand);
                        }
                        attackCount2 = game.Random.Next(3, 60);
                    }
                }
                if (dx < 0)
                {
                    direction = Direction.Left;
                }
                else if (dx > 0)
                {
                    direction = Direction.Right;
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
                if (Math.Abs(dy - 192) < 1)
                {
                    velocity.Y = dy - 192;
                }
                else if (dy - 192 < 0)
                {
                    velocity.Y = -1;
                }
                else
                {
                    velocity.Y = 1;
                }
            }

            MoveBy(input, velocity);

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (!ghost)
            {
                if (direction == Direction.Left)
                {
                    if (!attacking)
                    {
                        graphics.DrawImageFix(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this);
                    }
                    else
                    {
                        if (attackCount < 32 && (attackCount / 4) % 2 == 0)
                        {
                            graphics.DrawImageFix(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this);
                        }
                        else
                        {
                            graphics.DrawImageFix(GameImage.Norio, 128, 128, 1, 0, drawX, drawY, this);
                        }
                    }
                }
                else
                {
                    if (!attacking)
                    {
                        graphics.DrawImageFixFlip(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this);
                    }
                    else
                    {
                        if (attackCount < 32 && (attackCount / 4) % 2 == 0)
                        {
                            graphics.DrawImageFixFlip(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this);
                        }
                        else
                        {
                            graphics.DrawImageFixFlip(GameImage.Norio, 128, 128, 1, 0, drawX, drawY, this);
                        }
                    }
                }
            }
            else
            {
                if (direction == Direction.Left)
                {
                    if (!attacking)
                    {
                        graphics.DrawImageAlphaFix(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this, 128);
                    }
                    else
                    {
                        if (attackCount < 32 && (attackCount / 4) % 2 == 0)
                        {
                            graphics.DrawImageAlphaFix(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this, 128);
                        }
                        else
                        {
                            graphics.DrawImageAlphaFix(GameImage.Norio, 128, 128, 1, 0, drawX, drawY, this, 128);
                        }
                    }
                }
                else
                {
                    if (!attacking)
                    {
                        graphics.DrawImageAlphaFixFlip(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this, 128);
                    }
                    else
                    {
                        if (attackCount < 32 && (attackCount / 4) % 2 == 0)
                        {
                            graphics.DrawImageAlphaFixFlip(GameImage.Norio, 128, 128, 0, 0, drawX, drawY, this, 128);
                        }
                        else
                        {
                            graphics.DrawImageAlphaFixFlip(GameImage.Norio, 128, 128, 1, 0, drawX, drawY, this, 128);
                        }
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

            game.AddParticle(new BigExplosion(game, Center + new Vector(32, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, -32), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(-32, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, 32), Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(32);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            if (!attacking)
            {
                direction = Direction.Right;
            }
        }

        public override void Blocked_Right(GameInput input)
        {
            if (!attacking)
            {
                direction = Direction.Left;
            }
        }

        public override bool Shootable
        {
            get
            {
                return !ghost;
            }
        }
    }
}
