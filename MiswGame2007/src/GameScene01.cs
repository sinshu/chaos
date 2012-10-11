using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene01 : GameScene
    {
        public GameScene01(StageData data)
            : base(data)
        {
            Init();
        }

        public GameScene01(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            Init();
        }

        private void Init()
        {
            // AddEnemy(new BlackPlayer(this, 1, 1, BlackPlayer.Direction.Left, BlackPlayer.Weapon.Flamethrower));
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
