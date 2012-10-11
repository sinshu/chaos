using System;

namespace MiswGame2007
{
    public class StarmanBullet : Bullet
    {
        private const double RADIUS = 4;
        private const int DAMAGE = 1;

        private Starman.Direction direction;
        private double baseHeight;
        private int count;
        private double x;

        public StarmanBullet(GameScene game, Vector position, Starman.Direction direction, double x)
            : base(game, RADIUS, position, new Vector(direction == Starman.Direction.Left ? -1 : 1, 0), DAMAGE)
        {
            this.direction = direction;
            baseHeight = position.Y;
            count = 0;
            this.x = x;
        }

        public override void Tick(ThingList targetThings)
        {
            count++;
            double a;
            if (count < 16)
            {
                a = 2 * count;
            }
            else
            {
                a = 32;
            }
            double newHeight = baseHeight + a * Math.Sin((double)count / 16.0 * Math.PI + x);
            velocity.Y = newHeight - position.Y;
            if (Math.Abs(velocity.X) < 8)
            {
                velocity.X += Math.Sign(velocity.X) * 0.125;
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
