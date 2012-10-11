using System;

namespace MiswGame2007
{
    public class PlayerBulletExplosion2 : Particle
    {
        private int animation;
        private int flipRotate;

        public PlayerBulletExplosion2(GameScene game, Vector position, Vector velocity)
            : base(game, position, velocity)
        {
            animation = 0;
            flipRotate = game.Random.Next(0, 8);
        }

        public override void Tick()
        {
            base.Tick();
            animation++;
            if (animation == 8)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX - 16;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY - 16;
            graphics.DrawImageAddFlipRotate90(GameImage.PlayerBullet, 32, 32, 4, animation, drawX, drawY, flipRotate, 255, 255, 255);
        }
    }
}
