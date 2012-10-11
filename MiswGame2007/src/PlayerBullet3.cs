using System;

namespace MiswGame2007
{
    public class PlayerBullet3 : Bullet
    {
        private const double RADIUS = 8;
        private const int DAMAGE = 2;

        private const int NUM_ANIMATIONS = 8;

        private double direction;
        private int life;
        private int animation;

        public PlayerBullet3(GameScene game, Vector position, double direction, double speed)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.direction = direction;
            this.velocity = speed * new Vector(Math.Cos(this.direction / 180.0 * Math.PI), Math.Sin(this.direction / 180.0 * Math.PI));
            life = 8;
            animation = 0;

            if (game.DebugMode) damage *= 8;
        }

        public PlayerBullet3(GameScene game, Vector position, double direction, double speed, bool blackPlayer)
            : base(game, RADIUS, position, Vector.Zero, 1)
        {
            this.direction = direction;
            this.velocity = speed * new Vector(Math.Cos(this.direction / 180.0 * Math.PI), Math.Sin(this.direction / 180.0 * Math.PI));
            life = 8;
            animation = 0;
        }

        public override void Tick(ThingList targetThings)
        {
            if (life > 0)
            {
                life--;
            }
            if (life == 0)
            {
                Hit();
                return;
            }

            animation = (animation + 1) % NUM_ANIMATIONS;
            base.Tick(targetThings);
            base.Tick(targetThings);
            base.Tick(targetThings);
            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageRotateAdd(GameImage.PlayerBullet, 32, 32, 5, animation, drawX, drawY, 28, 16, (int)Math.Round(direction), 255);
        }

        public override void Hit()
        {
            if (Removed)
            {
                return;
            }

            if (life != 0)
            {
                game.AddParticle(new PlayerBulletExplosion2(game, position, velocity));
                /*
                if (game.Random.Next(0, 4) == 0)
                {
                    SpreadDebris(1);
                }
                */
            }
            Remove();
        }
    }
}
