using System;

namespace MiswGame2007
{
    public class BrokenAtField : Particle
    {
        private int part;
        private int animation;

        public BrokenAtField(GameScene game, Vector position, Vector velocity, int part)
            : base(game, position, velocity)
        {
            this.part = part;
            animation = 0;
        }

        public override void Tick()
        {
            velocity.Y += 0.5;
            base.Tick();
            animation++;
            if (animation >= 32)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (part == 0)
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet,16, 64, 2, (animation / 2 % 8) * 2, drawX - 16, drawY - 64, 255 - animation * 8);
            }
            else if (part == 1)
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet,16, 64, 2, (animation / 2 % 8) * 2 + 1, drawX, drawY - 64, 255 - animation * 8);
            }
            else if (part == 2)
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet, 16, 64, 3, (animation / 2 % 8) * 2, drawX - 16, drawY, 255 - animation * 8);
            }
            else
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet,16, 64, 3, (animation / 2 % 8) * 2 + 1, drawX, drawY, 255 - animation * 8);
            }
        }
    }
}
