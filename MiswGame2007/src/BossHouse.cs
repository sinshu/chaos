using System;

namespace MiswGame2007
{
    public class BossHouse : Enemy
    {
        private enum OutEnemy
        {
            Byaa = 1,
            Nurunuru
        }

        private const int INIT_HEALTH = 2000;
        private static Vector SIZE = new Vector(128, 128);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(64, 128), SIZE);

        private int attackCount;
        private bool attacking;
        private int openCount;
        private OutEnemy enemy;
        private int numDeathTicks;

        public BossHouse(GameScene game, int row, int col)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            attackCount = 120;
            attacking = false;
            openCount = 0;
            if (game.Random.Next(0, 2) == 0)
            {
                enemy = OutEnemy.Byaa;
            }
            else
            {
                enemy = OutEnemy.Nurunuru;
            }
            numDeathTicks = 0;
        }

        public override void Tick(GameInput input)
        {
            if (health <= 0)
            {
                DeathTick(input);
                base.Tick(input);
                return;
            }

            if (attackCount > 0)
            {
                attackCount--;
            }

            if (attackCount == 0 && !attacking && game.Enemies.Count < 6)
            {
                attacking = true;
                if (game.Random.Next(0, 2) == 0)
                {
                    enemy = OutEnemy.Byaa;
                }
                else
                {
                    enemy = OutEnemy.Nurunuru;
                }
            }

            if (attacking)
            {
                if (openCount < 256)
                {
                    if (openCount == 0)
                    {
                        game.PlaySound(GameSound.RobotJump);
                        game.PlaySound(GameSound.Hi2);
                    }
                    if (openCount == 248)
                    {
                        game.PlaySound(GameSound.DoorClose);
                    }
                    openCount++;
                }
                if (openCount == 128)
                {
                    if (enemy == OutEnemy.Byaa)
                    {
                        game.AddEnemy(new Byaa(game, position + new Vector(64, 0), Byaa.Direction.Left, false));
                    }
                    else if (enemy == OutEnemy.Nurunuru)
                    {
                        game.AddEnemy(new Nurunuru(game, position + new Vector(64, 0), Nurunuru.Direction.Left, false));
                    }
                }
                if (openCount >= 256)
                {
                    openCount = 0;
                    if (health > 1000)
                    {
                        attackCount = 180;
                    }
                    else
                    {
                        attackCount = 90;
                    }
                    attacking = false;
                }
            }

            /*
            if (health < 1000 && game.Ticks % 900 == 0)
            {
                game.AddEnemy(new House(game, 1, 1, House.Direction.Right));
            }
            */

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            int animation = 0;
            if (openCount < 32)
            {
                animation = openCount / 4;
            }
            else if (openCount < 224)
            {
                animation = 7;
            }
            else
            {
                animation = 7 - (openCount - 224) / 4;
            }

            if (attacking && openCount < 128)
            {
                int enemyX = (int)Math.Round(position.X + 64) - game.IntCameraX;
                int enemyY = 0;
                if (openCount < 96)
                {
                    enemyY = (int)Math.Round(position.Y + 128) - game.IntCameraY;
                }
                else
                {
                    enemyY = (int)Math.Round(position.Y + 128 - 4 * (openCount - 96)) - game.IntCameraY;
                }
                if (enemy == OutEnemy.Byaa)
                {
                    graphics.DrawImageFix(GameImage.Byaa, 128, 128, 0, 0, enemyX, enemyY, this);
                }
                else if (enemy == OutEnemy.Nurunuru)
                {
                    graphics.DrawImageFix(GameImage.Nurunuru, 128, 128, 0, 0, enemyX, enemyY, this);
                }
            }

            graphics.DrawImageFix(GameImage.HouseBoss, 256, 256, animation / 4, animation % 4, drawX, drawY, this);
        }

        private void DeathTick(GameInput input)
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
                game.AddParticle(new BigExplosion(game, Center - new Vector(-64, -64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-16, -64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(16, -64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(64, -64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-64, -16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-16, -16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(16, -16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(64, -16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-64, 16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-16, 16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(16, 16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(64, 16 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-64, 64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(-16, 64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(16, 64 + 16), Vector.Zero));
                game.AddParticle(new BigExplosion(game, Center - new Vector(64, 64 + 16), Vector.Zero));
                game.Quake(16);
                game.Flash(128);
                game.PlaySound(GameSound.Explode);
                SpreadDebris(256);
                game.Items.AddThing(new HealthItem(game, new Vector(32 + game.Random.NextDouble() * (game.Map.Width - 96), 32), Vector.Zero));
                Remove();
            }
        }

        public int BossHealth
        {
            get
            {
                return health;
            }
        }
    }
}
