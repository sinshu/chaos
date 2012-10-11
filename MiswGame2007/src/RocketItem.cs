using System;

namespace MiswGame2007
{
    public class RocketItem : Item
    {
        int animation;

        public RocketItem(GameScene game, Vector position, Vector velocity)
            : base(game, position, velocity)
        {
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            base.Tick(input);
            animation = (animation + 1) % 16;
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            graphics.DrawImage(GameImage.Item, 32, 32, 2, animation / 2, drawX, drawY);
        }

        public override void Get()
        {
            game.AddParticle(new WeaponItemExplosion(game, position + new Vector(16, 16), Vector.Zero));
            Remove();
        }
    }
}
