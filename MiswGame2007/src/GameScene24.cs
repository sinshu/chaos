using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene24 : GameScene
    {
        private int mafiaCount;
        private bool spawnMafia;

        public GameScene24(StageData data)
            : base(data)
        {
            mafiaCount = 3;
            spawnMafia = false;
        }

        public GameScene24(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            mafiaCount = 10;
            spawnMafia = false;
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                PlayMusic(GameMusic.Stage5);
            }

            base.Tick(input);

            if (Enemies.Count == 0)
            {
                spawnMafia = true;
            }
            if (spawnMafia)
            {
                if (mafiaCount > 0 && Ticks % 60 == 30)
                {
                    AddEnemy(new Mafia(this, 55, 1, Mafia.Direction.Right));
                    mafiaCount--;
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background5, 1024, 512, IntBackgroundX, IntBackgroundY);
        }
    }
}
