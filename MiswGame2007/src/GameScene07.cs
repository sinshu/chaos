using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene07 : GameScene
    {
        private int initEnemyCount;
        private int skyColorCount;

        public GameScene07(StageData data)
            : base(data)
        {
            initEnemyCount = Enemies.Count;
            skyColorCount = 255;
        }

        public GameScene07(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            initEnemyCount = Enemies.Count;
            skyColorCount = 255;
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                PlayMusic(GameMusic.Stage2);
            }

            base.Tick(input);

            int targetColorCount = (int)Math.Round(256.0 * (double)Enemies.Count / (double)initEnemyCount);
            if (targetColorCount < 0) targetColorCount = 0;
            else if (targetColorCount > 255) targetColorCount = 255;
            if (targetColorCount < skyColorCount && Ticks % 4 == 0)
            {
                skyColorCount--;
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            int color = 192 + skyColorCount / 4;
            graphics.DrawImage(GameImage.Background2, 1024, 512, IntBackgroundX, IntBackgroundY, color, color, color);
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            int color = 224 + skyColorCount / 8;
            Map.Draw(graphics, color, color, color);
        }
    }
}
