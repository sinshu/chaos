using System;

namespace MiswGame2007
{
    public class PlayerBullet2 : Bullet
    {
        private const double RADIUS = 8;
        private const double SPEED = 16;
        private const int DAMAGE = 4;

        private const int NUM_ANIMATIONS = 8;

        private double direction;
        private int life;
        private int animation;

        public PlayerBullet2(GameScene game, Vector position, int direction)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.direction = (double)direction + 6.0 * game.Random.NextDouble() - 3.0;
            this.velocity = SPEED * new Vector(Math.Cos(this.direction / 180.0 * Math.PI), Math.Sin(this.direction / 180.0 * Math.PI));
            life = 40;
            animation = 0;

            if (game.DebugMode) damage *= 8;
        }

        public PlayerBullet2(GameScene game, Vector position, int direction, bool blackPlayer)
            : base(game, RADIUS, position, Vector.Zero, 3)
        {
            this.direction = (double)direction + 3.0 * game.Random.NextDouble() - 1.5;
            this.velocity = 12 * new Vector(Math.Cos(this.direction / 180.0 * Math.PI), Math.Sin(this.direction / 180.0 * Math.PI));
            life = int.MaxValue;
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
            animation = (animation + 1) % NUM_ANIMATIONS;
            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageRotateAdd(GameImage.PlayerBullet, 32, 32, 0, animation, drawX, drawY, 28, 16, (int)Math.Round(direction), 255);
        }

        public override void Hit()
        {
            game.AddParticle(new PlayerBulletExplosion(game, position, Vector.Zero));
            /*
            if (life != 0)
            {
                SpreadDebris(game.Random.Next(0, 2));
            }
            */
            Remove();
        }
    }
}
