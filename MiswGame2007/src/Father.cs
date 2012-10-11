using System;

namespace MiswGame2007
{
    public class Father : Enemy
    {
        private const int INIT_HEALTH = 2500;

        private static Vector SIZE = new Vector(64, 128);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(32, 96), SIZE);

        private double baseHeight;
        private Vector basePos;
        private Vector targetPos;
        private int stateCount;
        private bool teleporting;
        private bool attacking;
        private bool wideAttack;
        private int numDeathTicks;

        public Father(GameScene game, int row, int col)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            baseHeight = position.Y;
            basePos = position;
            targetPos = position;
            stateCount = 0;
            teleporting = false;
            attacking = false;
            wideAttack = false;
            numDeathTicks = 0;
        }

        public override void Tick(GameInput input)
        {
            if (health <= 0)
            {
                teleporting = false;
                attacking = false;
                DeathTick(input);
                base.Tick(input);
                return;
            }

            if (!teleporting && !attacking)
            {
                if (stateCount < 32)
                {
                    stateCount++;
                }
                if (stateCount == 32)
                {
                    if (game.Random.Next(0, 4) < ((health > INIT_HEALTH / 2) ? 3 : 1))
                    {
                        basePos = position;
                        targetPos = new Vector(32 + game.Random.NextDouble() * (game.Map.Width - 64 - 128), baseHeight + 128 * game.Random.NextDouble() - 64);
                        Vector v = targetPos - basePos;
                        if (v.X * v.X + v.Y * v.Y >= 256 * 256)
                        {
                            teleporting = true;
                        }
                        stateCount = 0;
                    }
                    else
                    {
                        attacking = true;
                        wideAttack = game.Random.Next(0, 2) == 1;
                        targetPos = new Vector(128 + game.Random.NextDouble() * (game.Map.Width - 256 - 128), baseHeight + 128 * game.Random.NextDouble() - 64);
                        stateCount = 0;
                    }
                }
                velocity = 0.015625 * (targetPos - position);
                double d = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                if (d > 16)
                {
                    velocity = 16 / d * velocity;
                }
            }

            if (teleporting)
            {
                if (stateCount == 0)
                {
                    velocity = Vector.Zero;
                    game.PlaySound(GameSound.Warp);
                }
                if (stateCount < 6)
                {
                    position = stateCount / 6.0 * targetPos + (6 - stateCount) / 6.0 * basePos;
                    game.AddBackgroundParticle(new FatherGhost(game, position, Vector.Zero, stateCount));
                    stateCount++;
                }
                if (stateCount == 6)
                {
                    position = targetPos;
                    stateCount = 0;
                    teleporting = false;
                }
            }
            else if (attacking)
            {
                if (stateCount < 256)
                {
                    if (stateCount == 32)
                    {
                        game.PlaySound(GameSound.Laser);
                    }
                    if (stateCount >= 32 && stateCount < 160)
                    {
                        int timer = (int)Math.Round(128.0 * (health - 500) / (INIT_HEALTH - 500));
                        if (timer < 16) timer = 16;
                        if (!wideAttack)
                        {
                            if (stateCount % 4 == 0)
                            {
                                game.AddEnemyBullet(new FatherBullet(game, Center + new Vector(64 * game.Random.NextDouble() - 32, 32 * game.Random.NextDouble() - 16), Center + new Vector(128 * game.Random.NextDouble() - 64, -64 * game.Random.NextDouble() - 64), true, timer));
                            }
                        }
                        else
                        {
                            if (stateCount % 4 == 0)
                            {
                                game.AddEnemyBullet(new FatherBullet(game, Center + new Vector(64 * game.Random.NextDouble() - 32, 32 * game.Random.NextDouble() - 16), new Vector(48 + (game.Map.Width - 96) * game.Random.NextDouble(), Center.Y - 64 * game.Random.NextDouble()), false, timer));
                            }
                        }
                    }
                    stateCount++;
                }
                if (stateCount == 256)
                {
                    attacking = false;
                    stateCount = 0;
                }

                if (stateCount > 0)
                {
                    velocity = 0.015625 * (targetPos - position);
                    double d = Math.Sqrt(velocity.X * velocity.X + velocity.Y * velocity.Y);
                    if (d > 16)
                    {
                        velocity = 16 / d * velocity;
                    }
                }
            }

            MoveBy(input, velocity);

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            if (!teleporting && !attacking)
            {
                DrawNormal(graphics, stateCount / 2);
            }
            else if (teleporting)
            {
                DrawNormal(graphics, 0);
            }
            else
            {
                if (stateCount < 32)
                {
                    DrawSpin(graphics, stateCount / 2);
                }
                else if (stateCount < 160)
                {
                    DrawSpin(graphics, stateCount);
                }
                else if (stateCount < 192)
                {
                    DrawSpin(graphics, stateCount / 2);
                }
                else
                {
                    DrawSpin(graphics, stateCount / 4);
                }
            }
        }

        private void DrawNormal(GraphicsDevice graphics, int animation)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageFix(GameImage.Father, 128, 256, 0, animation % 8, drawX, drawY, this);
        }

        private void DrawSpin(GraphicsDevice graphics, int animation)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            int rotate = animation % 16;

            if (rotate < 8)
            {
                graphics.DrawImageFix(GameImage.Father, 128, 256, 1, rotate, drawX, drawY, this);
            }
            else
            {
                graphics.DrawImageFixFlip(GameImage.Father, 128, 256, 1, 15 - rotate, drawX, drawY, this); 
            }
        }

        public void DeathTick(GameInput input)
        {
            if (numDeathTicks < 128)
            {
                if (numDeathTicks == 0)
                {
                    game.EnemyBullets.BreakAll();
                    game.StopMusic();
                }
                if (numDeathTicks % 8 == 0)
                {
                    DoSomeExplode();
                    game.Quake(4);
                    game.PlaySound(GameSound.Shotgun);
                    damageFlash = 256;
                }
                if (numDeathTicks % 16 == 0)
                {
                    game.Flash(16);
                }
                numDeathTicks++;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X - 32, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top)), Vector.Zero));
                }
                for (int i = 0; i < 5; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X, Top + 0.25 * i * (Bottom - Top)), Vector.Zero));
                }
                for (int i = 0; i < 4; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X + 32, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top)), Vector.Zero));
                }
                game.Quake(16);
                game.Flash(128);
                game.PlaySound(GameSound.Explode);
                SpreadDebris(32);
                game.Items.AddThing(new HealthItem(game, new Vector(32 + game.Random.NextDouble() * (game.Map.Width - 96), 32), Vector.Zero));
                Remove();
            }
        }

        public override bool Shootable
        {
            get
            {
                return !teleporting;
            }
        }
    }
}
