using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene15 : GameScene
    {
        public GameScene15(StageData data)
            : base(data)
        {
            Init();
        }

        public GameScene15(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            Init();
        }

        private void Init()
        {
            Enemies.AddThing(new BossMushroom(this));
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
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background3, 1024, 512, IntBackgroundX, IntBackgroundY, 128, 128, 128);
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            Map.Draw(graphics, 128, 128, 128);
        }
    }
}
