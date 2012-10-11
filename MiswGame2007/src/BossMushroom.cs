using System;

namespace MiswGame2007
{
    public class BossMushroom : Enemy
    {
        private const int INIT_HEALTH = 2000;

        private static Vector SIZE = new Vector(80, 128);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(88, 128), SIZE);

        private static Vector[] POSITION = {
            Settings.BLOCK_WDITH * new Vector(12, 3),
            Settings.BLOCK_WDITH * new Vector(4, 7),
            Settings.BLOCK_WDITH * new Vector(20, 7),
            Settings.BLOCK_WDITH * new Vector(12, 11)
        };

        private int currentPos;
        private int stateCount;
        private int animation;
        private int numDeathTicks;

        public BossMushroom(GameScene game)
            : base(game, RECTANGLE, POSITION[0], Vector.Zero, INIT_HEALTH)
        {
            currentPos = 0;
            stateCount = 0;
            animation = 0;
            numDeathTicks = 0;
        }

        public override void Tick(GameInput input)
        {
            if (health <= 0)
            {
                rectangle = RECTANGLE;
                stateCount = 256;
                DeathTick(input);
                base.Tick(input);
                return;
            }

            stateCount = (stateCount + 1) % 512;

            if (stateCount < 32)
            {
                rectangle = new Rectangle(new Vector(88, 256 - 4 * stateCount), new Vector(SIZE.X, 4 * stateCount));
                if (stateCount == 0)
                {
                    int nextPos = game.Random.Next(0, POSITION.Length - 1);
                    if (nextPos < currentPos)
                    {
                        position = POSITION[nextPos];
                        currentPos = nextPos;
                    }
                    else
                    {
                        position = POSITION[nextPos + 1];
                        currentPos = nextPos + 1;
                    }
                }
                animation = stateCount / 2;
            }
            else if (stateCount < 128)
            {
                if (stateCount == 32)
                {
                    rectangle = RECTANGLE;
                }
                animation = stateCount / 2 % 16;
            }
            else if (stateCount < 256)
            {
                if (stateCount % 32 == 0)
                {
                    game.PlaySound(GameSound.Mushroom);
                }
                if (health > INIT_HEALTH / 2)
                {
                    if (stateCount % 64 == 48)
                    {
                        SpreadSpores(8);
                    }
                }
                else
                {
                    if (stateCount % 32 == 24)
                    {
                        SpreadSpores(8);
                    }
                }
                animation = stateCount / 2 % 16;
            }
            else if (stateCount < 480)
            {
                animation = stateCount / 2 % 16;
            }
            else if (stateCount < 512)
            {
                rectangle = new Rectangle(new Vector(88, 128 + 4 * (stateCount - 480)), new Vector(SIZE.X, 128 - 4 * (stateCount - 480)));
                animation = 15 - (stateCount - 480) / 2;
            }

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (stateCount < 32)
            {
                graphics.DrawImageFix(GameImage.MushroomBoss2, 256, 256, animation / 8, animation % 8, drawX, drawY, this);
            }
            else if (stateCount < 128)
            {
                graphics.DrawImageFix(GameImage.MushroomBoss1, 256, 256, animation / 8, animation % 8, drawX, drawY, this);
            }
            else if (stateCount < 256)
            {
                if (animation % 8 == 0)
                {
                    graphics.DrawImageFix(GameImage.MushroomBoss1, 256, 256, 0, 0, drawX, drawY, this);
                }
                else if (animation <= 4)
                {
                    graphics.DrawImageFix(GameImage.MushroomBoss3, 256, 256, 0, animation - 1, drawX, drawY, this);
                }
                else if (animation <= 7)
                {
                    graphics.DrawImageFix(GameImage.MushroomBoss3, 256, 256, 0, 7 - animation, drawX, drawY, this);
                }
                else if (animation <= 12)
                {
                    graphics.DrawImageFix(GameImage.MushroomBoss3, 256, 256, 1, animation - 9, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFix(GameImage.MushroomBoss3, 256, 256, 1, 15 - animation, drawX, drawY, this);
                }
            }
            else if (stateCount < 480)
            {
                graphics.DrawImageFix(GameImage.MushroomBoss1, 256, 256, animation / 8, animation % 8, drawX, drawY, this);
            }
            else
            {
                graphics.DrawImageFix(GameImage.MushroomBoss2, 256, 256, animation / 8, animation % 8, drawX, drawY, this);
            }
        }

        public void SpreadSpores(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                double x = Left + (Right - Left) * game.Random.NextDouble();
                double y = Top + (Bottom - Top) * game.Random.NextDouble();
                double vx = (x - Center.X) * (0.0625 - 0.03125 * game.Random.NextDouble());
                double vy = (y - Center.Y) * (0.0625 - 0.03125 * game.Random.NextDouble()) - 4;
                game.AddEnemyBullet(new MushroomSpore(game, new Vector(x, y), new Vector(vx, vy)));
            }
        }

        public void DeathTick(GameInput input)
        {
            if (numDeathTicks < 128)
            {
                if (numDeathTicks == 0)
                {
                    game.Enemies.KillAll();
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
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X - 48, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top) - 16), Vector.Zero));
                }
                for (int i = 0; i < 5; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X, Top + 0.25 * i * (Bottom - Top) - 16), Vector.Zero));
                }
                for (int i = 0; i < 4; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X + 48, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top) - 16), Vector.Zero));
                }
                game.Quake(16);
                game.Flash(128);
                game.PlaySound(GameSound.Explode);
                SpreadDebris(64);
                game.Items.AddThing(new HealthItem(game, new Vector(32 + game.Random.NextDouble() * (game.Map.Width - 96), 32), Vector.Zero));
                Remove();
            }
        }
    }
}
