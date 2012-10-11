using System;

namespace MiswGame2007
{
    public class AtField : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 30;

        private const int NUM_ANIMATIONS = 16;

        private static Vector SIZE = new Vector(32, 96);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(0, 16), SIZE);

        private Direction direction;
        private Baaka parent;
        private int animation;
        private int previousHealth;
        private int energy;

        public AtField(GameScene game, Direction direction, Baaka parent)
            : base(game, RECTANGLE, direction == Direction.Left ? parent.Center + new Vector(-32 - 16, -64) : parent.Center + new Vector(32 - 16, -64), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            this.parent = parent;
            animation = 0;
            previousHealth = health;
            energy = 192;
        }

        public override void Tick(GameInput input)
        {
            if (parent.Removed)
            {
                Remove();
                return;
            }
            if (energy > 0)
            {
                energy--;
            }
            else
            {
                parent.AtFieldBroken();
                Remove();
                return;
            }
            if (direction == Direction.Left)
            {
                position = parent.Center + new Vector(-32 - 16, -64);
            }
            else
            {
                position = parent.Center + new Vector(32 - 16, -64);
            }
            animation = (animation + 1) % NUM_ANIMATIONS;
            if (health < previousHealth)
            {
                game.PlaySound(GameSound.AtField);
            }
            previousHealth = health;
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 128, 1, animation / 2, drawX - 16, drawY, energy < 16 ? 16 * energy : 255);
            }
            else
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 128, 1, animation / 2, drawX + 16, drawY, energy < 16 ? 16 * energy : 255);
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            parent.AtFieldBroken();

            Random random = game.Random;
            game.AddParticle(new BrokenAtField(game, direction == Direction.Left ? Center - new Vector(16, 0) : Center + new Vector(16, 0), new Vector(2 * random.NextDouble() - 4, 4 * random.NextDouble() - 8), 0));
            game.AddParticle(new BrokenAtField(game, direction == Direction.Left ? Center - new Vector(16, 0) : Center + new Vector(16, 0), new Vector(2 * random.NextDouble() + 2, 4 * random.NextDouble() - 8), 1));
            game.AddParticle(new BrokenAtField(game, direction == Direction.Left ? Center - new Vector(16, 0) : Center + new Vector(16, 0), new Vector(2 * random.NextDouble() - 4, 4 * random.NextDouble()), 2));
            game.AddParticle(new BrokenAtField(game, direction == Direction.Left ? Center - new Vector(16, 0) : Center + new Vector(16, 0), new Vector(2 * random.NextDouble() + 2, 4 * random.NextDouble()), 3));

            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Glass);
            Remove();
        }
    }
}
