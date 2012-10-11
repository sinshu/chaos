using System;

namespace MiswGame2007
{
    public class BaboBullet : Bullet
    {
        private const double RADIUS = 4;
        private const int DAMAGE = 3;

        public BaboBullet(GameScene game, Vector position, Vector velocity)
            : base(game, RADIUS, position, velocity, DAMAGE)
        {
        }

        public override void Tick(ThingList targetThings)
        {
            velocity.Y += 0.5;
            if (velocity.Y > 16)
            {
                velocity.Y = 16;
            }


            base.Tick(targetThings);
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
