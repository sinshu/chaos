using System;

namespace MiswGame2007
{
    public class KyoroRocket : Bullet
    {
        private const double RADIUS = 4;
        private const double MAX_SPEED = 8;
        private const int DAMAGE = 8;

        private double direction;
        private double speed;
        private int life;
        private double maxRotAngle;
        private int animation;

        public KyoroRocket(GameScene game, Vector position, int direction)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.direction = direction;
            speed = 0;
            life = 600;
            maxRotAngle = 0.25;
            animation = 0;
        }

        public KyoroRocket(GameScene game, Vector position, int direction, bool blackPlayer)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.direction = direction;
            speed = 0;
            life = 600;
            maxRotAngle = 0.125;
            animation = 0;
        }

        public override void Tick(ThingList targetThings)
        {
            if (life > 0)
            {
                life--;
            }
            else
            {
                Hit();
                return;
            }

            double minRange = double.MaxValue;
            Thing target = null;
            foreach (Thing thing in targetThings)
            {
                double dx = thing.Center.X - position.X;
                double dy = thing.Center.Y - position.Y;
                double range = dx * dx + dy * dy;
                if (range < minRange)
                {
                    minRange = range;
                    target = thing;
                }
            }
            if (target != null)
            {
                double dx = target.Center.X - position.X;
                double dy = target.Center.Y - position.Y;
                double dr = (Math.Atan2(dy, dx) / Math.PI * 180) - direction;
                dr = (dr + 180) % 360;
                if (dr < 0) dr += 360;
                dr -= 180;
                if (Math.Abs(dr) < maxRotAngle * speed)
                {
                    direction += dr;
                }
                else
                {
                    direction += maxRotAngle * speed * Math.Sign(dr);
                }
            }

            if (speed < MAX_SPEED)
            {
                speed += 0.5;
            }

            velocity = speed * new Vector(Math.Cos(direction / 180.0 * Math.PI), Math.Sin(direction / 180.0 * Math.PI));

            double a = 2 * Math.PI * game.Random.NextDouble();
            if (animation % 2 == 0)
            {
                game.AddParticle(new PlayerRocketTail(game, position, 0.125 * new Vector(Math.Cos(a), Math.Sin(a))));
            }

            animation = (animation + 1) % 8;

            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageRotate(GameImage.PlayerBullet, 32, 32, 1, animation, drawX, drawY, 28, 16, (int)Math.Round(direction));
        }

        public override void Hit()
        {
            game.AddParticle(new BigExplosion(game, position, Vector.Zero));
            game.Quake(2);
            game.Flash(6);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(4);
            Remove();
        }
    }
}
