using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene11 : GameScene
    {
        private int initEnemyCount;
        private int skyColorCount;

        public GameScene11(StageData data)
            : base(data)
        {
            initEnemyCount = Enemies.Count;
            skyColorCount = 255;
        }

        public GameScene11(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            initEnemyCount = Enemies.Count;
            skyColorCount = 255;
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                PlayMusic(GameMusic.Stage3);
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
            int color = 64 + skyColorCount * 3 / 4;
            graphics.DrawImage(GameImage.Background3, 1024, 512, IntBackgroundX, IntBackgroundY, color, color, color);
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            int color = 128 + skyColorCount / 2;
            Map.Draw(graphics, color, color, color);
        }
    }
}
