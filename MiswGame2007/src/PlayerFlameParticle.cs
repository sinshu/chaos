using System;

namespace MiswGame2007
{
    public class PlayerFlameParticle : Particle
    {
        private int animation;

        public PlayerFlameParticle(GameScene game, Vector position, Vector velocity)
            : base(game, position, velocity)
        {
            animation = 0;
        }

        public PlayerFlameParticle(GameScene game, Vector position, Vector velocity, int animation)
            : this(game, position, velocity)
        {
            if (animation == 32)
            {
                this.animation = 31;
            }
            else
            {
                this.animation = animation;
            }
        }

        public override void Tick()
        {
            base.Tick();
            animation++;
            if (animation >= 32)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX - 16;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY - 16;
            graphics.DrawImageAdd(GameImage.PlayerBullet, 32, 32, animation / 2 / 8 + 6, animation / 2 % 8, drawX, drawY, 192);
        }
    }
}
