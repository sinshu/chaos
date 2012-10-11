using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene20 : GameScene
    {
        public GameScene20(StageData data)
            : base(data)
        {
            Init();
        }

        public GameScene20(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            Init();
        }

        private void Init()
        {
            Enemies.AddThing(new Father(this, 4, 10));
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

            if (Items.Count == 0 && Ticks % 180 == 90 && (Player.CurrentWeapon == Player.Weapon.Pistol || (Player.CurrentWeapon == Player.Weapon.Rocket && Player.Ammo <= 15) || (Player.CurrentWeapon == Player.Weapon.Machinegun && Player.Ammo <= 50)))
            {
                if (Random.Next(0, 2) == 0)
                {
                    Items.AddThing(new MachinegunItem(this, new Vector(32 + Random.NextDouble() * (Map.Width - 96), 32), Vector.Zero));
                }
                else
                {
                    Items.AddThing(new RocketItem(this, new Vector(32 + Random.NextDouble() * (Map.Width - 96), 32), Vector.Zero));
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background4, 1024, 512, IntBackgroundX, IntBackgroundY, 64, 64, 64);
            for (int i = 0; i < 512; i++)
            {
                int y = (int)Math.Round(16 * Math.Sin(2 * Math.PI * (i + 0.5 * Ticks) / 128));
                int color = i + 2 * Ticks;
                graphics.DrawImageAdd(GameImage.Aurora, 2, 256, 0, i, IntBackgroundX + 2 * i, y, 128, GetAuroraColorR(color), GetAuroraColorG(color), GetAuroraColorB(color));
            }
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            Map.Draw(graphics, 128, 128, 128);
        }

        private int GetAuroraColorR(int index)
        {
            index %= 1536;
            if (index <= 256)
            {
                return 255;
            }
            else if (index < 512)
            {
                return 512 - index;
            }
            else if (index < 1024)
            {
                return 0;
            }
            else if (index < 1280)
            {
                return index - 1024;
            }
            else
            {
                return 255;
            }
        }

        private int GetAuroraColorG(int index)
        {
            index %= 1536;
            if (index < 256)
            {
                return index;
            }
            else if (index <= 768)
            {
                return 255;
            }
            else if (index < 1024)
            {
                return 1024 - index;
            }
            else
            {
                return 0;
            }
        }

        private int GetAuroraColorB(int index)
        {
            index %= 1536;
            if (index < 512)
            {
                return 0;
            }
            else if (index < 768)
            {
                return index - 512;
            }
            else if (index <= 1280)
            {
                return 255;
            }
            else
            {
                return 1536 - index;
            }
        }
    }
}
