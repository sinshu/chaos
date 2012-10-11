using System;

namespace MiswGame2007
{
    public class EggMachineBullet : Bullet
    {
        private const double RADIUS = 4;
        private const int DAMAGE = 3;

        private bool isWormEgg;

        public EggMachineBullet(GameScene game, Vector position, Vector velocity, bool isWormEgg)
            : base(game, RADIUS, position, velocity, DAMAGE)
        {
            this.isWormEgg = isWormEgg;
        }

        public override void Tick(ThingList targetThings)
        {
            velocity.Y += 0.0625;
            if (velocity.Y > 16)
            {
                velocity.Y = 16;
            }


            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 0, 3, drawX - 16, drawY - 16, 255);
        }

        public override void Hit()
        {
            game.AddParticle(new EggMachineBulletExplosion(game, position, Vector.Zero));
            Remove();
        }

        public override void MoveBy_Down(double d, ThingList targetThings)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int bottomRow = BottomRow;
            Map map = game.Map;
            if (map.IsObstacle(bottomRow, leftCol) || map.IsObstacle(bottomRow, rightCol))
            {
                Bottom = bottomRow * Settings.BLOCK_WDITH;
                if (isWormEgg)
                {
                    if (game.Enemies.Count < 6)
                    {
                        if (!(position.X < game.Map.Width - 256 && position.X > 256) && position.Y > game.Map.Height - 128)
                        {
                            game.AddEnemy(new Worm(game, new Vector(position.X - 16, Bottom - 32)));
                        }
                    }
                }
                Hit();
            }
            else
            {
                foreach (Thing target in targetThings)
                {
                    if (!target.Shootable) continue;
                    if (Left < target.Right && target.Left < Right && Top < target.Bottom && target.Top < Bottom)
                    {
                        if (position.Y < target.Center.Y)
                        {
                            Bottom = target.Top;
                        }
                        target.Damage(damage);
                        Hit();
                        break;
                    }
                }
            }
        }
    }
}
