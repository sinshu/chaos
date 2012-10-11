using System;

namespace MiswGame2007
{
    public class RobotRocket : Bullet
    {
        private const double RADIUS = 4;
        private const double MAX_SPEED = 16;
        private const int DAMAGE = 8;

        private double direction;
        private double speed;
        private double rotate;
        private int life;
        private int animation;

        public RobotRocket(GameScene game, Vector position, int direction, double rotate)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.direction = direction;
            speed = 8;
            this.rotate = rotate;
            life = 120 - (int)Math.Round(15 * rotate);
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
                if (Math.Abs(dr) < rotate)
                {
                    direction += dr;
                }
                else
                {
                    direction += rotate * Math.Sign(dr);
                }
            }

            if (speed < MAX_SPEED)
            {
                speed += 0.5;
            }

            this.velocity = speed * new Vector(Math.Cos((double)direction / 180.0 * Math.PI), Math.Sin((double)direction / 180.0 * Math.PI));

            double a = 2 * Math.PI * game.Random.NextDouble();
            // if (animation % 2 == 0)
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
