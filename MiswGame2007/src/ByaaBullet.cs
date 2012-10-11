using System;

namespace MiswGame2007
{
    public class ByaaBullet : Bullet
    {
        private const double RADIUS = 4;
        private const int DAMAGE = 1;

        public ByaaBullet(GameScene game, Vector position, Thing target)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            double angle = Math.Atan2(target.Center.Y - position.Y, target.Center.X - position.X);
            velocity = 8 * new Vector(Math.Cos(angle), Math.Sin(angle));
        }

        public ByaaBullet(GameScene game, Vector position, Vector velocity)
            : base(game, RADIUS, position, velocity, DAMAGE)
        {
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 0, 2, drawX - 16, drawY - 16, 255);
        }

        public override void Hit()
        {
            game.AddParticle(new BaboBulletExplosion(game, position, Vector.Zero));
            Remove();
        }
    }
}
