using System;

namespace MiswGame2007
{
    public class BaakaBullet : Bullet
    {
        private const double RADIUS = 4;

        public BaakaBullet(GameScene game, Vector position, Vector velocity)
            : base(game, RADIUS, position, velocity, 3)
        {
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 0, 1, drawX - 16, drawY - 16, 255);
        }

        public override void Hit()
        {
            game.AddParticle(new BaakaBulletExplosion(game, position, Vector.Zero));
            Remove();
        }
    }
}
