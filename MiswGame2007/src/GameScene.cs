using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene
    {
        public enum State
        {
            None = 1,
            Clear,
            Gameover
        }

        private enum ThingScreenPosition
        {
            InScreen = 1,
            Left,
            LeftUpper,
            Upper,
            RightUpper,
            Right,
            RightLower,
            Lower,
            LeftLower
        }

        private Random random;
        private Map map;
        private Player player;
        private Vector camera;
        private ThingList players;
        private ThingList enemies;
        private ThingList enemyAddList;
        private BulletList playerBullets;
        private BulletList enemyBullets;
        private ThingList items;
        private ExitDoor door;
        private ParticleList particles;
        private ParticleList particleAddList;
        private double quakeRadius;
        private Vector quakeVector;
        private int flash;
        private int numTicks;
        private bool cleared;
        private int clearTimer;
        private int gameoverTimer;
        private bool gameover;
        private ParticleList backgroundParticles;

        private AudioDevice audio;

        public GameScene(int numRows, int numCols)
        {
            random = new Random();
            map = new Map(this, numRows, numCols);
            player = null;
            camera = Vector.Zero;
            players = new ThingList();
            enemies = new ThingList();
            enemyAddList = new ThingList();
            playerBullets = new BulletList();
            enemyBullets = new BulletList();
            items = new ThingList();
            door = null;
            particles = new ParticleList();
            particleAddList = new ParticleList();
            quakeRadius = 0;
            quakeVector = Vector.Zero;
            flash = 0;
            numTicks = 0;
            cleared = false;
            clearTimer = 0;
            gameoverTimer = 0;
            gameover = false;
            backgroundParticles = new ParticleList();
            audio = null;
        }

        public GameScene(StageData data)
        {
            random = new Random();
            map = data.GetMap(this);
            // player = new Player(this, 6, 1, Player.Direction.Right);
            player = data.GetPlayer(this);
            MoveCameraFast(player.Focus);
            players = new ThingList();
            players.AddThing(player);
            // enemies = new ThingList();
            enemies = data.GetEnemies(this);
            enemyAddList = new ThingList();
            playerBullets = new BulletList();
            enemyBullets = new BulletList();
            items = new ThingList();
            door = data.GetExitDoor(this);
            particles = new ParticleList();
            particleAddList = new ParticleList();
            quakeRadius = 0;
            quakeVector = Vector.Zero;
            flash = 0;
            numTicks = 0;
            cleared = false;
            clearTimer = 0;
            gameoverTimer = 0;
            gameover = false;
            backgroundParticles = new ParticleList();

            audio = null;

            /*
            enemies.AddThing(new TestEnemy(this, 7, 24, TestEnemy.Direction.Left));
            enemies.AddThing(new TestEnemy(this, 12, 11, TestEnemy.Direction.Left));
            enemies.AddThing(new TestEnemy(this, 12, 24, TestEnemy.Direction.Left));
            enemies.AddThing(new TestEnemy(this, 16, 8, TestEnemy.Direction.Left));
            enemies.AddThing(new TestEnemy(this, 22, 17, TestEnemy.Direction.Left));
            */
        }

        public GameScene(StageData data, PlayerState playerState)
            : this(data)
        {
            player.State = playerState;
        }

        public virtual void Tick(GameInput input)
        {
            particles.Tick();
            if (door != null)
            {
                door.Tick();
            }
            items.Tick(input);
            enemyBullets.Tick(players);
            playerBullets.Tick(enemies);
            enemies.Tick(input);
            if (player != null)
            {
                player.Tick(input);
            }
            backgroundParticles.Tick();

            foreach (Thing enemy in enemyAddList)
            {
                enemies.AddThing(enemy);
            }
            enemyAddList.Clear();
            foreach (Particle particle in particleAddList)
            {
                particles.AddParticle(particle);
            }
            particleAddList.Clear();

            if (player != null)
            {
                MoveCamera(player.Focus);
            }

            players.SweepRemovedThings();
            enemies.SweepRemovedThings();
            playerBullets.SweepRemovedBullets();
            enemyBullets.SweepRemovedBullets();
            items.SweepRemovedThings();
            particles.SweepRemovedParticles();
            backgroundParticles.SweepRemovedParticles();

            if (quakeRadius > 0)
            {
                double angle = 2 * Math.PI * random.NextDouble();
                quakeVector = quakeRadius * new Vector(Math.Cos(angle), Math.Sin(angle));
                quakeRadius -= 0.25;
            }

            if (flash > 0)
            {
                flash--;
            }

            numTicks++;
            if (cleared)
            {
                if (clearTimer < 16)
                {
                    clearTimer++;
                }
            }

            if (input.Exit && !gameover)
            {
                gameover = true;
                gameoverTimer = 240;
            }

            if (player != null)
            {
                if (player.Removed)
                {
                    gameover = true;
                }
            }

            if (gameover)
            {
                if (gameoverTimer == 240)
                {
                    StopMusic();
                }
                if (gameoverTimer < 256)
                {
                    gameoverTimer++;
                }
            }
        }

        public virtual void Draw(GraphicsDevice graphics)
        {
            DrawBackground(graphics);
            backgroundParticles.Draw(graphics);
            DrawMap(graphics);
            if (door != null)
            {
                door.Draw(graphics);
            }
            // particles.Draw(graphics);
            enemies.Draw(graphics);
            if (player != null)
            {
                player.Draw(graphics);
            }
            items.Draw(graphics);
            particles.Draw(graphics);
            playerBullets.Draw(graphics);
            enemyBullets.Draw(graphics);
            if (door != null)
            {
                door.Draw2(graphics);
            }

            DrawSomething(graphics);

            if (flash > 0)
            {
                graphics.FillScreen(255, 255, 255, flash);
            }

            if (player != null)
            {
                if (player.DamageCount > 0)
                {
                    graphics.FillScreen(255, 0, 0, player.DamageCount / 2);
                }

                if (numTicks < 16)
                {
                    graphics.FillScreen(255, 255, 255, 255 - numTicks * 16);
                }

                if (cleared)
                {
                    if (clearTimer < 16)
                    {
                        graphics.FillScreen(255, 255, 255, clearTimer * 16);
                    }
                    else
                    {
                        graphics.FillScreen(255, 255, 255, 255);
                    }
                }

                DrawHud(graphics);

                if (gameover)
                {
                    if (gameoverTimer < 256)
                    {
                        if (gameoverTimer > 240)
                        {
                            graphics.FillScreen(0, 0, 0, (gameoverTimer - 240) * 16);
                        }
                    }
                    else
                    {
                        graphics.FillScreen(0, 0, 0, 255);
                    }
                }
            }
        }

        public virtual void DrawBackground(GraphicsDevice graphics)
        {
        }

        public virtual void DrawMap(GraphicsDevice graphics)
        {
            map.Draw(graphics);
        }

        public virtual void DrawSomething(GraphicsDevice graphics)
        {
        }

        public void AddEnemy(Thing thing)
        {
            enemyAddList.AddThing(thing);
        }

        public void AddPlayerBullet(Bullet bullet)
        {
            playerBullets.AddBullet(bullet);
        }

        public void AddEnemyBullet(Bullet bullet)
        {
            enemyBullets.AddBullet(bullet);
        }

        public void AddItem(Item item)
        {
            items.AddThing(item);
        }

        public void AddParticle(Particle particle)
        {
            particleAddList.AddParticle(particle);
        }

        public void AddBackgroundParticle(Particle particle)
        {
            backgroundParticles.AddParticle(particle);
        }

        private void MoveCameraFast(Vector target)
        {
            Vector focus = target - (0.5 * new Vector(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT));
            if (focus.X < 0)
            {
                focus.X = 0;
            }
            else if (map.Width - Settings.SCREEN_WIDTH < focus.X)
            {
                focus.X = map.Width - Settings.SCREEN_WIDTH;
            }
            if (focus.Y < 0)
            {
                focus.Y = 0;
            }
            else if (map.Height - Settings.SCREEN_HEIGHT < focus.Y)
            {
                focus.Y = map.Height - Settings.SCREEN_HEIGHT;
            }
            camera = focus;
        }

        private void MoveCamera(Vector target)
        {
            Vector focus = target - (0.5 * new Vector(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT));
            if (focus.X < 0)
            {
                focus.X = 0;
            }
            else if (map.Width - Settings.SCREEN_WIDTH < focus.X)
            {
                focus.X = map.Width - Settings.SCREEN_WIDTH;
            }
            if (focus.Y < 0)
            {
                focus.Y = 0;
            }
            else if (map.Height - Settings.SCREEN_HEIGHT < focus.Y)
            {
                focus.Y = map.Height - Settings.SCREEN_HEIGHT;
            }
            camera = 0.9375 * camera + 0.0625 * focus;
        }

        public void Quake(double radius)
        {
            if (quakeRadius < radius)
            {
                quakeRadius = radius;
            }
        }

        public void Flash(int flash)
        {
            if (this.flash < flash)
            {
                this.flash = flash;
            }
            if (this.flash > 255)
            {
                this.flash = 255;
            }
        }

        public void DrawHud(GraphicsDevice graphics)
        {
            bool hudTransparent = false;
            if (enemies.Count > 0)
            {
                hudTransparent = DrawWhereEnemyIs(graphics);
            }
            else
            {
                hudTransparent = DrawWhereExitIs(graphics);
            }
            if (hudTransparent)
            {
                DrawPlayerInfo(graphics, 128);
            }
            else
            {
                DrawPlayerInfo(graphics, 255);
            }
        }

        public void DrawPlayerInfo(GraphicsDevice graphics, int alpha)
        {
            graphics.DrawImageAlpha(GameImage.Hud, 32, 32, 2, 0, 8, Settings.SCREEN_HEIGHT - 40, alpha);
            for (int i = 0; i < 6; i++)
            {
                graphics.DrawImageAlpha(GameImage.Hud, 32, 32, 2, 1, 8 + 32 + 32 * i, Settings.SCREEN_HEIGHT - 40, alpha);
            }
            graphics.DrawImageAlpha(GameImage.Hud, 32, 32, 2, 2, 8 + 32 + 192, Settings.SCREEN_HEIGHT - 40, alpha);
            if (player.DrawHealth > 0)
            {
                int damageCount = player.DamageCount;
                graphics.DrawRect(8 + 32, Settings.SCREEN_HEIGHT - 40 + 10, 2 * player.DrawHealth, 1, 255, 128 + damageCount / 2, 128 + damageCount / 2, alpha);
                graphics.DrawRect(8 + 32, Settings.SCREEN_HEIGHT - 40 + 10 + 1, 2 * player.DrawHealth, 12 - 2, 255, damageCount, damageCount, alpha);
                graphics.DrawRect(8 + 32, Settings.SCREEN_HEIGHT - 40 + 10 + 11, 2 * player.DrawHealth, 1, 128 + damageCount / 2, damageCount, damageCount, alpha);
            }

            if (player.CurrentWeapon != Player.Weapon.Pistol)
            {
                graphics.DrawImageAlpha(GameImage.Hud, 96, 32, 3, 0, 8, Settings.SCREEN_HEIGHT - 80, alpha);

                switch (player.CurrentWeapon)
                {
                    case Player.Weapon.Machinegun:
                        graphics.DrawImageAlpha(GameImage.Item, 32, 32, 0, 0, 9, Settings.SCREEN_HEIGHT - 80, alpha);
                        break;
                    case Player.Weapon.Rocket:
                        graphics.DrawImageAlpha(GameImage.Item, 32, 32, 2, 0, 9, Settings.SCREEN_HEIGHT - 80, alpha);
                        break;
                    case Player.Weapon.Shotgun:
                        graphics.DrawImageAlpha(GameImage.Item, 32, 32, 1, 0, 9, Settings.SCREEN_HEIGHT - 80, alpha);
                        break;
                    case Player.Weapon.Flamethrower:
                        graphics.DrawImageAlpha(GameImage.Item, 32, 32, 3, 0, 9, Settings.SCREEN_HEIGHT - 80, alpha);
                        break;
                }
            }
            int ammo = player.Ammo;
            if (ammo / 100 > 0)
            {
                DrawNumber(graphics, ammo / 100 % 10, 40, Settings.SCREEN_HEIGHT - 80, alpha, player.AmmoNumColorCount);
            }
            if (ammo / 10 > 0)
            {
                DrawNumber(graphics, ammo / 10 % 10, 40 + 19, Settings.SCREEN_HEIGHT - 80, alpha, player.AmmoNumColorCount);
            }
            if (ammo > 0)
            {
                DrawNumber(graphics, ammo % 10, 40 + 19 + 19, Settings.SCREEN_HEIGHT - 80, alpha, player.AmmoNumColorCount);
            }

            if (player.AmmoHudColorCount > 0)
            {
                graphics.DrawImageAlpha(GameImage.Hud, 96, 32, 3, 1, 8, Settings.SCREEN_HEIGHT - 80, player.AmmoHudColorCount * alpha / 255);
            }
        }

        private void DrawNumber(GraphicsDevice graphics, int n, int x, int y, int alpha, int white)
        {
            graphics.DrawImageAlpha(GameImage.Hud, 32, 32, 4 + n / 8, n % 8, x, y, alpha);
            if (white > 0)
            {
                graphics.DrawImageAlpha(GameImage.Hud, 32, 32, 6 + n / 8, n % 8, x, y, white * alpha / 255);
            }
        }

        public void Clear()
        {
            cleared = true;
        }

        public bool DrawWhereEnemyIs(GraphicsDevice graphics)
        {
            double minRange = double.MaxValue;
            Thing target = null;
            ThingScreenPosition targetPos = ThingScreenPosition.InScreen;
            foreach (Thing enemy in enemies)
            {
                ThingScreenPosition sp = GetScreenPosition(enemy.Center);
                if (sp == ThingScreenPosition.InScreen)
                {
                    return false;
                }
                if (enemy is AtField || enemy is EggMachine)
                {
                    continue;
                }
                Vector d = enemy.Center - player.Center;
                double range = d.X * d.X + d.Y * d.Y;
                if (range < minRange)
                {
                    minRange = range;
                    target = enemy;
                    targetPos = sp;
                }
            }
            if (target != null)
            {
                Vector focus = target.Center - camera;
                switch (targetPos)
                {
                    case ThingScreenPosition.Left:
                        if (focus.Y < 16)
                        {
                            graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, 0, 0);
                            graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 32, 0);
                        }
                        else if (focus.Y > Settings.SCREEN_HEIGHT - 16)
                        {
                            graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, 0, Settings.SCREEN_HEIGHT - 32);
                            graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 32, Settings.SCREEN_HEIGHT - 32);
                        }
                        else
                        {
                            graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, 0, (int)Math.Round(focus.Y) - 16);
                            graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 32, (int)Math.Round(focus.Y) - 16);
                        }
                        return true;
                    case ThingScreenPosition.LeftUpper:
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 1, 0, 0);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 32, 32);
                        return true;
                    case ThingScreenPosition.Upper:
                        if (focus.X < 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 2, 0, 0);
                        else if (focus.X > Settings.SCREEN_WIDTH - 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 2, Settings.SCREEN_WIDTH - 32, 0);
                        else graphics.DrawImage(GameImage.Hud, 32, 32, 0, 2, (int)Math.Round(focus.X) - 16, 0);
                        if (focus.X < 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 0, 32);
                        else if (focus.X > Settings.SCREEN_WIDTH - 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 64, 32);
                        else graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, (int)Math.Round(focus.X) - 32, 32);
                        return true;
                    case ThingScreenPosition.RightUpper:
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 3, Settings.SCREEN_WIDTH - 32, 0);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 96, 32);
                        return true;
                    case ThingScreenPosition.Right:
                        if (focus.Y < 16)
                        {
                            graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH - 32, 0);
                            graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 96, 0);
                        }
                        else if (focus.Y > Settings.SCREEN_HEIGHT - 16)
                        {
                            graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH - 32, Settings.SCREEN_HEIGHT - 32);
                            graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 96, Settings.SCREEN_HEIGHT - 32);
                        }
                        else
                        {
                            graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH - 32, (int)Math.Round(focus.Y) - 16);
                            graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 96, (int)Math.Round(focus.Y) - 16);
                        }
                        return true;
                    case ThingScreenPosition.RightLower:
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 5, Settings.SCREEN_WIDTH - 32, Settings.SCREEN_HEIGHT - 32);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 96, Settings.SCREEN_HEIGHT - 64);
                        return true;
                    case ThingScreenPosition.Lower:
                        if (focus.X < 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 6, 0, Settings.SCREEN_HEIGHT - 32);
                        else if (focus.X > Settings.SCREEN_WIDTH - 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 6, Settings.SCREEN_WIDTH - 32, Settings.SCREEN_HEIGHT - 32);
                        else graphics.DrawImage(GameImage.Hud, 32, 32, 0, 6, (int)Math.Round(focus.X) - 16, Settings.SCREEN_HEIGHT - 32);
                        if (focus.X < 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 0, Settings.SCREEN_HEIGHT - 64);
                        else if (focus.X > Settings.SCREEN_WIDTH - 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, Settings.SCREEN_WIDTH - 64, Settings.SCREEN_HEIGHT - 64);
                        else graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, (int)Math.Round(focus.X) - 32, Settings.SCREEN_HEIGHT - 64);
                        return true;
                    case ThingScreenPosition.LeftLower:
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 7, 0, Settings.SCREEN_HEIGHT - 32);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 0, 32, Settings.SCREEN_HEIGHT - 64);
                        return true;
                }
            }
            return false;
        }

        public bool DrawWhereExitIs(GraphicsDevice graphics)
        {
            Vector focus = door.Center - camera;
            ThingScreenPosition sp = GetScreenPosition(door.Center);
            switch (sp)
            {
                case ThingScreenPosition.Left:
                    if (focus.Y < 16)
                    {
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, 0, 0);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 32, 0);
                    }
                    else if (focus.Y > Settings.SCREEN_HEIGHT - 16)
                    {
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, 0, Settings.SCREEN_HEIGHT - 32);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 32, Settings.SCREEN_HEIGHT - 32);
                    }
                    else
                    {
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, 0, (int)Math.Round(focus.Y) - 16);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 32, (int)Math.Round(focus.Y) - 16);
                    }
                    return true;
                case ThingScreenPosition.LeftUpper:
                    graphics.DrawImage(GameImage.Hud, 32, 32, 0, 1, 0, 0);
                    graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 32, 32);
                    return true;
                case ThingScreenPosition.Upper:
                    if (focus.X < 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 2, 0, 0);
                    else if (focus.X > Settings.SCREEN_WIDTH - 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 2, Settings.SCREEN_WIDTH - 32, 0);
                    else graphics.DrawImage(GameImage.Hud, 32, 32, 0, 2, (int)Math.Round(focus.X) - 16, 0);
                    if (focus.X < 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 0, 32);
                    else if (focus.X > Settings.SCREEN_WIDTH - 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 64, 32);
                    else graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, (int)Math.Round(focus.X) - 32, 32);
                    return true;
                case ThingScreenPosition.RightUpper:
                    graphics.DrawImage(GameImage.Hud, 32, 32, 0, 3, Settings.SCREEN_WIDTH - 32, 0);
                    graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 96, 32);
                    return true;
                case ThingScreenPosition.Right:
                    if (focus.Y < 16)
                    {
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH - 32, 0);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 96, 0);
                    }
                    else if (focus.Y > Settings.SCREEN_HEIGHT - 16)
                    {
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH - 32, Settings.SCREEN_HEIGHT - 32);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 96, Settings.SCREEN_HEIGHT - 32);
                    }
                    else
                    {
                        graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH - 32, (int)Math.Round(focus.Y) - 16);
                        graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 96, (int)Math.Round(focus.Y) - 16);
                    }
                    return true;
                case ThingScreenPosition.RightLower:
                    graphics.DrawImage(GameImage.Hud, 32, 32, 0, 5, Settings.SCREEN_WIDTH - 32, Settings.SCREEN_HEIGHT - 32);
                    graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 96, Settings.SCREEN_HEIGHT - 64);
                    return true;
                case ThingScreenPosition.Lower:
                    if (focus.X < 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 6, 0, Settings.SCREEN_HEIGHT - 32);
                    else if (focus.X > Settings.SCREEN_WIDTH - 16) graphics.DrawImage(GameImage.Hud, 32, 32, 0, 6, Settings.SCREEN_WIDTH - 32, Settings.SCREEN_HEIGHT - 32);
                    else graphics.DrawImage(GameImage.Hud, 32, 32, 0, 6, (int)Math.Round(focus.X) - 16, Settings.SCREEN_HEIGHT - 32);
                    if (focus.X < 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 0, Settings.SCREEN_HEIGHT - 64);
                    else if (focus.X > Settings.SCREEN_WIDTH - 32) graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, Settings.SCREEN_WIDTH - 64, Settings.SCREEN_HEIGHT - 64);
                    else graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, (int)Math.Round(focus.X) - 32, Settings.SCREEN_HEIGHT - 64);
                    return true;
                case ThingScreenPosition.LeftLower:
                    graphics.DrawImage(GameImage.Hud, 32, 32, 0, 7, 0, Settings.SCREEN_HEIGHT - 32);
                    graphics.DrawImage(GameImage.Hud, 64, 32, 1, 1, 32, Settings.SCREEN_HEIGHT - 64);
                    return true;
            }
            return false;
        }

        private ThingScreenPosition GetScreenPosition(Vector realPosition)
        {
            Vector position = realPosition - camera;
            if (position.X < 0)
            {
                if (position.Y < 0)
                {
                    return ThingScreenPosition.LeftUpper;
                }
                else if (position.Y > Settings.SCREEN_HEIGHT)
                {
                    return ThingScreenPosition.LeftLower;
                }
                else
                {
                    return ThingScreenPosition.Left;
                }
            }
            else if (position.X > Settings.SCREEN_WIDTH)
            {
                if (position.Y < 0)
                {
                    return ThingScreenPosition.RightUpper;
                }
                else if (position.Y > Settings.SCREEN_HEIGHT)
                {
                    return ThingScreenPosition.RightLower;
                }
                else
                {
                    return ThingScreenPosition.Right;
                }
            }
            else if (position.Y < 0)
            {
                return ThingScreenPosition.Upper;
            }
            else if (position.Y > Settings.SCREEN_HEIGHT)
            {
                return ThingScreenPosition.Lower;
            }
            else
            {
                return ThingScreenPosition.InScreen;
            }
        }

        public void PlaySound(GameSound sound)
        {
            if (audio != null)
            {
                audio.PlaySound(sound);
            }
        }

        public void PlayMusic(GameMusic music)
        {
            if (audio != null)
            {
                audio.PlayMusic(music);
            }
        }

        public void StopMusic()
        {
            if (audio != null)
            {
                audio.StopMusic();
            }
        }

        public Map Map
        {
            get
            {
                return map;
            }
        }

        public Vector Camera
        {
            get
            {
                return camera;
            }

            set
            {
                camera = value;
            }
        }

        public Random Random
        {
            get
            {
                return random;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }
        }

        public ThingList Enemies
        {
            get
            {
                return enemies;
            }
        }

        public ThingList Items
        {
            get
            {
                return items;
            }
        }

        public BulletList EnemyBullets
        {
            get
            {
                return enemyBullets;
            }
        }

        public int Ticks
        {
            get
            {
                return numTicks;
            }
        }

        public State CurrentState
        {
            get
            {
                if (!gameover)
                {
                    if (clearTimer < 16)
                    {
                        return State.None;
                    }
                    else
                    {
                        return State.Clear;
                    }
                }
                else
                {
                    if (gameoverTimer < 256)
                    {
                        return State.None;
                    }
                    else
                    {
                        return State.Gameover;
                    }
                }
            }
        }

        public int IntCameraX
        {
            get
            {
                int x = (int)Math.Round(camera.X + quakeVector.X);
                if (x < 0)
                {
                    return 0;
                }
                else if (map.Width - Settings.SCREEN_WIDTH < x)
                {
                    return map.Width - Settings.SCREEN_WIDTH;
                }
                else
                {
                    return x;
                }
            }
        }

        public int IntCameraY
        {
            get
            {
                int y = (int)Math.Round(camera.Y + quakeVector.Y);
                if (y < 0)
                {
                    return 0;
                }
                else if (map.Height - Settings.SCREEN_HEIGHT < y)
                {
                    return map.Height - Settings.SCREEN_HEIGHT;
                }
                else
                {
                    return y;
                }
            }
        }

        public int IntBackgroundX
        {
            get
            {
                double maxScreenScrollX = map.Width - Settings.SCREEN_WIDTH;
                double maxBackgroundScrollX = 1024 - Settings.SCREEN_WIDTH;

                if (maxScreenScrollX < maxBackgroundScrollX * 2)
                {
                    double sx = camera.X - (map.Width - Settings.SCREEN_WIDTH) / 2;
                    return (int)Math.Round(-0.5 * sx - (1024 - Settings.SCREEN_WIDTH) / 2);
                }
                else
                {
                    double sx = camera.X / (map.Width - Settings.SCREEN_WIDTH);
                    return (int)Math.Round(-sx * maxBackgroundScrollX);
                }
            }
        }

        public int IntBackgroundY
        {
            get
            {
                double maxScreenScrollY = map.Height - Settings.SCREEN_HEIGHT;
                double maxBackgroundScrollY = 512 - Settings.SCREEN_HEIGHT;

                if (maxScreenScrollY < maxBackgroundScrollY * 2)
                {
                    double sy = camera.Y - (map.Height - Settings.SCREEN_HEIGHT) / 2;
                    return (int)Math.Round(-0.5 * sy - (512 - Settings.SCREEN_HEIGHT) / 2);
                }
                else
                {
                    double sy = camera.Y / (map.Height - Settings.SCREEN_HEIGHT);
                    return (int)Math.Round(-sy * maxBackgroundScrollY);
                }
            }
        }

        public AudioDevice AudioDevice
        {
            set
            {
                audio = value;
            }
        }

        public bool DebugMode = false;
    }
}
