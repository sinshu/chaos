using System;

namespace MiswGame2007
{
    public class Rain : Particle
    {
        private bool background;

        public Rain(GameScene game, bool background)
            : base(game, Vector.Zero, Vector.Zero)
        {
            this.background = background;
            if (!background)
            {
                position = new Vector(game.Map.Width * game.Random.NextDouble(), game.Map.Height * game.Random.NextDouble());
                velocity.Y = 16 + 16 * game.Random.NextDouble();
            }
            else
            {
                position = new Vector(1024 * game.Random.NextDouble(), 512 * game.Random.NextDouble());
                velocity.Y = 8 + 8 * game.Random.NextDouble();
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (!background)
            {
                if (position.Y > game.Map.Height + 32)
                {
                    position.X = game.Map.Width * game.Random.NextDouble();
                    position.Y = -32;
                    velocity.Y = 16 + 16 * game.Random.NextDouble();
                }
            }
            else
            {
                if (position.Y > 512 + 16)
                {
                    position.X = 1024 * game.Random.NextDouble();
                    position.Y = -16;
                    velocity.Y = 8 + 8 * game.Random.NextDouble();
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            if (!background)
            {
                int drawX = (int)Math.Round(position.X) - game.IntCameraX;
                int drawY = (int)Math.Round(position.Y) - game.IntCameraY - 32;
                graphics.DrawImageAlpha(GameImage.Rain, 1, 64, 0, 0, drawX, drawY, 64);
            }
            else
            {
                int drawX = (int)Math.Round(position.X) + game.IntBackgroundX;
                int drawY = (int)Math.Round(position.Y) + game.IntBackgroundY - 32;
                graphics.DrawImageAlpha(GameImage.Rain, 1, 64, 0, 1, drawX, drawY, 32);
            }
        }
    }
}
