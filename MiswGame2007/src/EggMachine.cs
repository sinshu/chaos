using System;

namespace MiswGame2007
{
    public class EggMachine : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        private static Vector SIZE = new Vector(48, 96);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(40, 32), SIZE);

        private Direction direction;
        private bool idle;
        private int moveCount;
        private int animation;

        public EggMachine(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, int.MaxValue)
        {
            this.direction = direction;
            idle = true;
            moveCount = 0;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            if (idle)
            {
                moveCount = (moveCount + 1) % 32;
                animation = moveCount / 4;
            }

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                if (idle)
                {
                    graphics.DrawImageFix(GameImage.EggMachine, 128, 128, 0, animation, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFix(GameImage.EggMachine, 128, 128, 1, animation, drawX, drawY, this);
                }
            }
            else
            {
                if (idle)
                {
                    graphics.DrawImageFixFlip(GameImage.EggMachine, 128, 128, 0, animation, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFixFlip(GameImage.EggMachine, 128, 128, 1, animation, drawX, drawY, this);
                }
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                game.AddParticle(new BigExplosion(game, new Vector(Center.X - 32, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top)), Vector.Zero));
            }
            for (int i = 0; i < 5; i++)
            {
                game.AddParticle(new BigExplosion(game, new Vector(Center.X, Top + 0.25 * i * (Bottom - Top)), Vector.Zero));
            }
            for (int i = 0; i < 4; i++)
            {
                game.AddParticle(new BigExplosion(game, new Vector(Center.X + 32, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top)), Vector.Zero));
            }
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(32);
            Remove();
        }

        public void BeginIdle()
        {
            idle = true;
            moveCount = 0;
            animation = 0;
        }

        public void SetAnimation(int index)
        {
            idle = false;
            animation = index;
        }

        public void FireBullet()
        {
            if (direction == Direction.Left)
            {
                game.AddEnemyBullet(new EggMachineBullet(game, position + new Vector(64, 96), new Vector(-4 * game.Random.NextDouble(), 2 * game.Random.NextDouble() - 6), false));
            }
            else
            {
                game.AddEnemyBullet(new EggMachineBullet(game, position + new Vector(64, 96), new Vector(4 * game.Random.NextDouble(), 2 * game.Random.NextDouble() - 6), false));
            }
        }

        public void FireWormEgg()
        {
            if (direction == Direction.Left)
            {
                game.AddEnemyBullet(new EggMachineBullet(game, position + new Vector(64, 96), new Vector(-4 * game.Random.NextDouble(), 2 * game.Random.NextDouble() - 6), true));
            }
            else
            {
                game.AddEnemyBullet(new EggMachineBullet(game, position + new Vector(64, 96), new Vector(4 * game.Random.NextDouble(), 2 * game.Random.NextDouble() - 6), true));
            }
        }
    }
}
