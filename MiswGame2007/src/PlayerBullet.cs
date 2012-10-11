using System;

namespace MiswGame2007
{
    public class PlayerBullet : Bullet
    {
        private const double RADIUS = 8;
        private const double SPEED = 16;
        private const int DAMAGE = 5;

        private const int NUM_ANIMATIONS = 8;

        private int direction;
        private int life;
        private int animation;

        public PlayerBullet(GameScene game, Vector position, int direction)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.velocity = SPEED * new Vector(Math.Cos((double)direction / 180.0 * Math.PI), Math.Sin((double)direction / 180.0 * Math.PI));
            this.direction = direction;
            life = 40;
            animation = 0;

            if (game.DebugMode) damage *= 8;
        }

        public PlayerBullet(GameScene game, Vector position, int direction, bool blackPlayer)
            : base(game, RADIUS, position, Vector.Zero, 3)
        {
            this.velocity = 0.5 * SPEED * new Vector(Math.Cos((double)direction / 180.0 * Math.PI), Math.Sin((double)direction / 180.0 * Math.PI));
            this.direction = direction;
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

            graphics.DrawImageRotateAdd(GameImage.PlayerBullet, 32, 32, 0, animation, drawX, drawY, 24, 16, direction, 255);
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
