using System;

namespace MiswGame2007
{
    public class Enemy : Thing
    {
        public Enemy(GameScene game, Rectangle rectangle, Vector position, Vector velocity, int health)
            : base(game, rectangle, position, velocity, health)
        {
        }

        public override void MoveBy_Left(GameInput input, double d)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int leftCol = LeftCol;
            Map map = game.Map;
            for (int row = topRow; row <= bottomRow; row++)
            {
                if (map.IsObstacleForEnemy(row, leftCol, this))
                {
                    Left = (leftCol + 1) * Settings.BLOCK_WDITH;
                    Blodked_Left(input);
                    break;
                }
            }
        }

        public override void MoveBy_Up(GameInput input, double d)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int topRow = TopRow;
            Map map = game.Map;
            for (int col = leftCol; col <= rightCol; col++)
            {
                if (map.IsObstacleForEnemy(topRow, col, this))
                {
                    Top = (topRow + 1) * Settings.BLOCK_WDITH;
                    Blocked_Top(input);
                    break;
                }
            }
        }

        public override void MoveBy_Right(GameInput input, double d)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int rightCol = RightCol;
            Map map = game.Map;
            for (int row = topRow; row <= bottomRow; row++)
            {
                if (map.IsObstacleForEnemy(row, rightCol, this))
                {
                    Right = rightCol * Settings.BLOCK_WDITH;
                    Blocked_Right(input);
                    break;
                }
            }
        }

        public override void MoveBy_Down(GameInput input, double d)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int bottomRow = BottomRow;
            Map map = game.Map;
            for (int col = leftCol; col <= rightCol; col++)
            {
                if (map.IsObstacleForEnemy(bottomRow, col, this))
                {
                    Bottom = bottomRow * Settings.BLOCK_WDITH;
                    Blocked_Bottom(input);
                    break;
                }
            }
        }

        public virtual bool IgnoreEnemyBlock
        {
            get
            {
                return false;
            }
        }
    }
}
