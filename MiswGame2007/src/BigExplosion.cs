using System;

namespace MiswGame2007
{
    public class BigExplosion : Particle
    {
        private int animation;
        private int flipRotate;

        public BigExplosion(GameScene game, Vector position, Vector velocity)
            : base(game, position, velocity)
        {
            animation = 0;
            flipRotate = game.Random.Next(0, 8);
        }

        public override void Tick()
        {
            base.Tick();
            animation++;
            if (animation == 32)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX - 64;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY - 64;

            graphics.DrawImageAddFlipRotate90(GameImage.BigExplosion, 128, 128, animation / 2 / 4, animation / 2 % 4, drawX, drawY, flipRotate, 255, 255, 255);
        }
    }
}
