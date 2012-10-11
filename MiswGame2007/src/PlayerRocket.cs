using System;

namespace MiswGame2007
{
    public class PlayerRocket : Bullet
    {
        private const double RADIUS = 4;
        private const double MAX_SPEED = 16;
        private const int DAMAGE = 16;

        private double direction;
        private double speed;
        private int life;
        private int animation;

        public PlayerRocket(GameScene game, Vector position, int direction)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.direction = direction;
            speed = 0;
            life = 48;
            animation = 0;

            if (game.DebugMode) damage *= 8;
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
                if (!thing.Shootable || thing is AtField || thing is EggMachine)
                {
                    continue;
                }
                double dx = thing.Center.X - position.X;
                double dy = thing.Center.Y - position.Y;
                double angle = direction / 180.0 * Math.PI;
                double dr = dx * Math.Cos(angle) + dy * Math.Sin(angle);
                if (dr < 0)
                {
                    continue;
                }
                double ds = 4 * (dx * Math.Cos(angle + Math.PI / 2) + dy * Math.Sin(angle + Math.PI / 2));
                double range = dr * dr + ds * ds;
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
                if (Math.Abs(dr) < 0.0625 * speed)
                {
                    direction += dr;
                }
                else
                {
                    direction += 0.0625 * speed * Math.Sign(dr);
                }
            }

            if (speed < MAX_SPEED)
            {
                velocity = speed * new Vector(Math.Cos(direction / 180.0 * Math.PI), Math.Sin(direction / 180.0 * Math.PI));
            }
            else
            {
                velocity = MAX_SPEED * new Vector(Math.Cos(direction / 180.0 * Math.PI), Math.Sin(direction / 180.0 * Math.PI));
            }
            if (speed < 2 * MAX_SPEED)
            {
                speed++;
            }

            double a = 2 * Math.PI * game.Random.NextDouble();
            game.AddParticle(new PlayerRocketTail(game, position, 0.125 * new Vector(Math.Cos(a), Math.Sin(a))));

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
