using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene05 : GameScene
    {
        private BossRobot boss;
        private int initBossHealth;
        private int skyColorCount;

        public GameScene05(StageData data)
            : base(data)
        {
            Init();
        }

        public GameScene05(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            Init();
        }

        private void Init()
        {
            boss = new BossRobot(this, 11, 21, BossRobot.Direction.Left);
            Enemies.AddThing(boss);
            initBossHealth = boss.Health;
            skyColorCount = 0;
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                StopMusic();
            }
            else if (Ticks == 60)
            {
                PlayMusic(GameMusic.Boss1);
            }

            base.Tick(input);

            int targetColorCount = 256 - (int)Math.Round(256.0 * (double)(boss.Health - 500) / (double)(initBossHealth - 500));
            if (targetColorCount < 0) targetColorCount = 0;
            else if (targetColorCount > 255) targetColorCount = 255;
            if (skyColorCount < targetColorCount && Ticks % 4 == 0)
            {
                skyColorCount++;
            }

            if (Items.Count == 0 && Ticks % 180 == 90 && (Player.CurrentWeapon == Player.Weapon.Pistol || (Player.CurrentWeapon == Player.Weapon.Machinegun && Player.Ammo <= 50)))
            {
                Items.AddThing(new MachinegunItem(this, new Vector(32 + Random.NextDouble() * (Map.Width - 96), 32), Vector.Zero));
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background1, 1024, 512, IntBackgroundX, IntBackgroundY, 32 + (255 - skyColorCount) * 7 / 8, 32 + (255 - skyColorCount) * 3 / 8, 32);
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            int color = 128 + (255 - skyColorCount) / 4;
            Map.Draw(graphics, color, color, color);
        }
    }
}
