using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene03 : GameScene
    {
        public GameScene03(StageData data)
            : base(data)
        {
        }

        public GameScene03(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                PlayMusic(GameMusic.Stage1);
            }

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background1, 1024, 512, IntBackgroundX, IntBackgroundY);
        }
    }
}
