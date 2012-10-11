using System;

namespace MiswGame2007
{
    public class FatherGhost : Particle
    {
        private int animation;
        private int color;

        public FatherGhost(GameScene game, Vector position, Vector velocity, int color)
            : base(game, position, velocity)
        {
            animation = 0;
            this.color = color;
        }

        public override void Tick()
        {
            animation++;
            if (animation == 8)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            int r = (color <= 1 || color == 5) ? 255 : 0;
            int g = (color >= 3 && color <= 5) ? 255 : 0;
            int b = (color >= 1 && color <= 3) ? 255 : 0;
            graphics.DrawImageAdd(GameImage.Father, 128, 256, 0, 0, drawX, drawY, 255 - 32 * animation, r, g, b);
        }
    }
}
