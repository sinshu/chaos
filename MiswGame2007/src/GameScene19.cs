using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene19 : GameScene
    {
        public GameScene19(StageData data)
            : base(data)
        {
        }

        public GameScene19(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                PlayMusic(GameMusic.Stage4);
            }

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background4, 1024, 512, IntBackgroundX, IntBackgroundY, 64, 64, 64);
            if (Ticks < 1024)
            {
                for (int i = 0; i < 512; i++)
                {
                    int y = (int)Math.Round(16 * Math.Sin(2 * Math.PI * (i + 0.5 * Ticks) / 128));
                    int color = i + 2 * Ticks;
                    graphics.DrawImageAdd(GameImage.Aurora, 2, 256, 0, i, IntBackgroundX + 2 * i, y, Ticks / 8, GetAuroraColorR(color), GetAuroraColorG(color), GetAuroraColorB(color));
                }
            }
            else
            {
                for (int i = 0; i < 512; i++)
                {
                    int y = (int)Math.Round(16 * Math.Sin(2 * Math.PI * (i + 0.5 * Ticks) / 128));
                    int color = i + 2 * Ticks;
                    graphics.DrawImageAdd(GameImage.Aurora, 2, 256, 0, i, IntBackgroundX + 2 * i, y, 128, GetAuroraColorR(color), GetAuroraColorG(color), GetAuroraColorB(color));
                }
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
