using System;

namespace MiswGame2007
{
    public class MushroomSpore : Bullet
    {
        private const double RADIUS = 2;
        private const int DAMAGE = 10;

        public MushroomSpore(GameScene game, Vector position, Vector velocity)
            : base(game, RADIUS, position, velocity, DAMAGE)
        {
        }

        public override void Tick(ThingList targetThings)
        {
            velocity.Y += 0.125;
            if (velocity.Y > 2)
            {
                velocity.Y = 2;
            }

            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;

            graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 0, 4, drawX - 16, drawY - 16, 255);
        }

        public override void Hit()
        {
            game.AddBackgroundParticle(new BaakaBulletExplosion(game, position, Vector.Zero));
            Remove();
        }

        public override void MoveBy_Left(double d, ThingList targetThings)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int leftCol = LeftCol;
            Map map = game.Map;
            if (map.IsObstacle(topRow, leftCol) || map.IsObstacle(bottomRow, leftCol))
            {
                Left = (leftCol + 1) * Settings.BLOCK_WDITH;
                velocity.X = -velocity.X;
            }
        }

        public override void MoveBy_Up(double d, ThingList targetThings)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int topRow = TopRow;
            Map map = game.Map;
            if (map.IsObstacle(topRow, leftCol) || map.IsObstacle(topRow, rightCol))
            {
                Top = (topRow + 1) * Settings.BLOCK_WDITH;
                velocity.Y = -velocity.Y;
            }
        }

        public override void MoveBy_Right(double d, ThingList targetThings)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int rightCol = RightCol;
            Map map = game.Map;
            if (map.IsObstacle(topRow, rightCol) || map.IsObstacle(bottomRow, rightCol))
            {
                Right = rightCol * Settings.BLOCK_WDITH;
                velocity.X = -velocity.X;
            }
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
                if (game.Enemies.Count < 64)
                {
                    game.AddEnemy(new Mushroom(game, new Vector(position.X - 16, Bottom - 32)));
                }
                Hit();
            }
        }
    }
}
