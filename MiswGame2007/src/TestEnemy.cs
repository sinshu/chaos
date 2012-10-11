using System;

namespace MiswGame2007
{
    public class TestEnemy : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private const int INIT_HEALTH = 30;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;
        private const int NUM_ANIMATIONS = 16;

        private static Vector SIZE = new Vector(24, 40);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(4, 24), SIZE);

        private Direction direction;
        private int animation;

        public TestEnemy(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            if (direction == Direction.Left)
            {
                velocity.X = -2;
            }
            else
            {
                velocity.X = 2;
            }
            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }
            MoveBy(input, velocity);
            animation = (animation + 1) % NUM_ANIMATIONS;

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                int textureCol = animation / 2;
                graphics.DrawImageFix(GameImage.Player, 32, 64, 0, textureCol, drawX, drawY, this);
            }
            else
            {
                int textureCol = animation / 2;
                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, 0, textureCol, drawX, drawY, this);
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            if (game.Random.NextDouble() < 0.2)
            {
                game.AddItem(new MachinegunItem(game, position + new Vector(0, 32), new Vector(0, -8)));
            }
            else if (game.Random.NextDouble() < 0.3)
            {
                game.AddItem(new RocketItem(game, position + new Vector(0, 32), new Vector(0, -8)));
            }
            else if (game.Random.NextDouble() < 0.5)
            {
                game.AddItem(new ShotgunItem(game, position + new Vector(0, 32), new Vector(0, -8)));
            }
            else
            {
                game.AddItem(new FlameItem(game, position + new Vector(0, 32), new Vector(0, -8)));
            }
            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(16);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            direction = Direction.Right;
        }

        public override void Blocked_Right(GameInput input)
        {
            direction = Direction.Left;
        }
    }
}
