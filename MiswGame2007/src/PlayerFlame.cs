using System;

namespace MiswGame2007
{
    public class PlayerFlame : Bullet
    {
        private const double RADIUS = 8;
        private const double SPEED = 8;

        private const int NUM_ANIMATIONS = 32;

        private double direction;
        private int animation;

        public PlayerFlame(GameScene game, Vector position, double direction)
            : base(game, RADIUS, position, Vector.Zero, 4)
        {
            this.velocity = SPEED * new Vector(Math.Cos(direction / 180.0 * Math.PI), Math.Sin(direction / 180.0 * Math.PI));
            this.direction = direction;
            animation = 0;

            if (game.DebugMode) damage *= 8;
        }

        public PlayerFlame(GameScene game, Vector position, double direction, bool blackPlayer)
            : base(game, RADIUS, position, Vector.Zero, 4)
        {
            this.velocity = SPEED * new Vector(Math.Cos(direction / 180.0 * Math.PI), Math.Sin(direction / 180.0 * Math.PI));
            this.direction = direction;
            animation = 0;
        }

        public override void Tick(ThingList targetThings)
        {
            animation++;
            if (animation % 2 == 0)
            {
                if (damage > 1)
                {
                    damage--;
                }
            }

            if (animation >= NUM_ANIMATIONS)
            {
                Remove();
            }
            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            graphics.DrawImageRotateAdd(GameImage.PlayerBullet, 32, 32, animation / 2 / 8 + 6, animation / 2 % 8, drawX, drawY, 16, 16, (int)Math.Round(direction), 192);
        }

        public override void Hit()
        {
            double a = 2 * Math.PI * game.Random.NextDouble();
            /*
            if (game.Random.Next(2) == 0)
            {
                game.AddParticle(new PlayerRocketTail(game, position, new Vector(Math.Cos(a), Math.Sin(a)) + new Vector(0, -1), animation));
            }
            else
            */
            {
                game.AddParticle(new PlayerFlameParticle(game, position, 0.125 * velocity + 0.5 * new Vector(Math.Cos(a), Math.Sin(a)), animation));
            }
            Remove();
        }
    }
}
