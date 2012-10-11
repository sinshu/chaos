using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene09 : GameScene
    {
        private static int[] HANABI_COLOR_INDEX = { 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 3, 4, 4, 4, 4, 4, 4, 4, 5 };

        private int initEnemyCount;
        private int skyColorCount;

        public GameScene09(StageData data)
            : base(data)
        {
            initEnemyCount = Enemies.Count;
            skyColorCount = 255;
        }

        public GameScene09(StageData data, PlayerState playerState)
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
            if (Ticks % 60 == 0)
            {
                AddBackgroundParticle(new Hanabi(this, new Vector(1024 * Random.NextDouble(), 256 * Random.NextDouble()), Random.Next(0, 4) == 0, HANABI_COLOR_INDEX[Random.Next(0, HANABI_COLOR_INDEX.Length)]));
            }

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
            int color = 64 + skyColorCount / 4;
            graphics.DrawImage(GameImage.Background2, 1024, 512, IntBackgroundX, IntBackgroundY, color, color, color);
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            int color = 160 + skyColorCount / 8;
            Map.Draw(graphics, color, color, color);
        }
    }
}
