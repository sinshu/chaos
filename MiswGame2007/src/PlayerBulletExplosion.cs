using System;

namespace MiswGame2007
{
    public class PlayerBulletExplosion : Particle
    {
        private int animation;
        private int flipRotate;

        public PlayerBulletExplosion(GameScene game, Vector position, Vector velocity)
            : base(game, position, velocity)
        {
            animation = 0;
            flipRotate = game.Random.Next(0, 8);
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
            graphics.DrawImageAddFlipRotate90(GameImage.PlayerBulletExplosion, 64, 64, animation / 8 + 2, animation % 8, drawX, drawY, flipRotate, 255, 255, 255);
        }
    }
}
