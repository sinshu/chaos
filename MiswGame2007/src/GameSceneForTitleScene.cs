using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameSceneForTitleScene : GameScene
    {
        private bool showTitle;

        public GameSceneForTitleScene(int numRows, int numCols)
            : base(numRows, numCols)
        {
            showTitle = false;
        }

        public override void Tick(GameInput input)
        {
            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
        }

        public override void DrawSomething(GraphicsDevice graphics)
        {
            if (showTitle)
            {
                graphics.DrawImage(GameImage.Title, 512, 256, (Settings.SCREEN_WIDTH - 512) / 2 - IntCameraX + 16, (Settings.SCREEN_HEIGHT - 256) / 2 - IntCameraY + 16);
            }
        }

        public void ShowTitle()
        {
            showTitle = true;
        }
    }
}
