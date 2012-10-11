using System;

namespace MiswGame2007
{
    public class OyajiThunder : Particle
    {
        private int animation;

        public OyajiThunder(GameScene game, double x)
            : base(game, new Vector(x, game.Map.Height - 32), Vector.Zero)
        {
            animation = 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (animation < 48)
            {
                if (animation == 32)
                {
                    if (game.Player.Visible && Math.Abs(game.Player.Center.X - position.X) < 20)
                    {
                        game.Player.Damage(10);
                        game.Player.Velocity = new Vector(16 * Math.Sign(game.Player.Center.X - position.X), game.Player.Velocity.Y);
                    }
                    game.AddParticle(new BigExplosion(game, position, Vector.Zero));
                    for (int i = 0; i < 4; i++)
                    {
                        game.AddParticle(new Debris(game, position, new Vector(8 - 16 * game.Random.NextDouble(), 4 - 16 * game.Random.NextDouble()), game.Random.Next(0, 4)));
                    }
                    game.Quake(4);
                    game.Flash(16);
                    game.PlaySound(GameSound.Shotgun);
                }
                animation++;
            }
            if (animation == 48)
            {
                Remove();
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (animation < 32)
            {
                graphics.DrawRect(drawX - 2, drawY - 512, 4, 512, animation % 2 == 0 ? 255 : 0, 255, 255, 4 * animation);
                graphics.DrawRect(drawX - 1, drawY - 512, 2, 512, animation % 2 == 0 ? 255 : 0, 255, 255, 8 * animation);
            }
            else
            {
                graphics.DrawImageAdd(GameImage.OyajiThunder, 32, 512, 0, animation / 2 % 8, drawX - 16, drawY - 512, 255 - 16 * (animation - 32), animation % 2 == 0 ? 255 : 0, 255, 255);
            }
        }
    }
}
