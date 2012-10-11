using System;

namespace MiswGame2007
{
    public class Item : Thing
    {
        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;

        public Item(GameScene game, Vector position, Vector velocity)
            : base(game, new Rectangle(new Vector(4, 4), new Vector(24, 24)), position, velocity, 666)
        {
            MoveBy_Left(GameInput.Empty, 0);
            MoveBy_Right(GameInput.Empty, 0);
        }

        public override void Tick(GameInput input)
        {
            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }
            MoveBy(input, velocity);
        }

        public override void Blocked_Bottom(GameInput input)
        {
            base.Blocked_Bottom(input);
            velocity.X = 0;
        }

        public virtual void Get()
        {
        }
    }
}
