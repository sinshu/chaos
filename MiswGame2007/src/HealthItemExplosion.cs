using System;

namespace MiswGame2007
{
    public class HealthItemExplosion : Particle
    {
        private int animation;

        public HealthItemExplosion(GameScene game, Vector position, Vector velocity)
            : base(game, position, velocity)
        {
            animation = 0;
        }

        public override void Tick()
        {
            base.Tick();
            animation++;
            if (animation == 16)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX - 32;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY - 32;
            graphics.DrawImageAdd(GameImage.ItemExplosion, 64, 64, animation / 8 + 2, animation % 8, drawX, drawY, 255);
        }
    }
}
