using System;

namespace MiswGame2007
{
    public class Mushroom : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum State
        {
            Invisible = 1,
            Rise,
            Wait,
            Attack
        }

        private const int INIT_HEALTH = 5;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;

        private static Vector SIZE = new Vector(16, 24);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(8, 8), SIZE);

        private Direction direction;
        private State currentState;
        private int attackCount;
        private int attackCount2;
        private double angle;
        private int animation;

        private bool aggressive;

        public Mushroom(GameScene game, int row, int col, Direction direction, bool visible)
            : base(game, new Rectangle(new Vector(8, 24), new Vector(16, 8)), new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            if (visible)
            {
                currentState = State.Rise;
            }
            else
            {
                currentState = State.Invisible;
            }
            attackCount = 0;
            attackCount2 = 0;
            angle = 270;
            animation = 0;
            aggressive = false;
        }

        public Mushroom(GameScene game, Vector position)
            : base(game, new Rectangle(new Vector(8, 24), new Vector(16, 8)), position, Vector.Zero, INIT_HEALTH / 2)
        {
            direction = game.Random.Next(0, 2) == 0 ? Direction.Left : Direction.Right;
            currentState = State.Rise;
            attackCount = 0;
            attackCount2 = 0;
            angle = 270;
            animation = 0;
            aggressive = true;
        }

        public override void Tick(GameInput input)
        {
            if (attackCount2 > 0)
            {
                attackCount2--;
            }

            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            switch (currentState)
            {
                case State.Invisible:
                    if (Math.Abs(dx) < 256 && Math.Abs(dy) < 64)
                    {
                        currentState = State.Rise;
                    }
                    break;
                case State.Rise:
                    if (animation < 15)
                    {
                        animation++;
                    }
                    else
                    {
                        rectangle = RECTANGLE;
                        currentState = State.Wait;
                        animation = 0;
                    }
                    break;
                case State.Wait:
                    if (Math.Abs(velocity.X) < 1)
                    {
                        velocity.X = 0;
                    }
                    else
                    {
                        velocity.X -= Math.Sign(velocity.X);
                    }
                    if (aggressive || (Math.Abs(dx) < 320 && Math.Abs(dy) < 128))
                    {
                        if (attackCount2 == 0)
                        {
                            attackCount = game.Random.Next(aggressive ? 60 : 30, aggressive ? 120 : 60);
                            currentState = State.Attack;
                        }
                    }
                    break;
                case State.Attack:
                    if (attackCount > 0)
                    {
                        double dr = (Math.Atan2(dy, dx) / Math.PI * 180) - angle;
                        dr = (dr + 180) % 360;
                        if (dr < 0) dr += 360;
                        dr -= 180;
                        if (Math.Abs(dr) < 5)
                        {
                            angle += dr;
                        }
                        else
                        {
                            angle += 5 * Math.Sign(dr);
                        }

                        velocity = 4 * new Vector(Math.Cos((double)angle / 180.0 * Math.PI), Math.Sin((double)angle / 180.0 * Math.PI));

                        attackCount--;
                        animation = (animation + 1) % 16;
                    }
                    else
                    {
                        attackCount2 = game.Random.Next(30, 60);
                        currentState = State.Wait;
                        angle = 270;
                        animation = 0;
                    }
                    break;
            }

            if (currentState != State.Attack)
            {
                velocity.Y += ACCELERATION_FALLING;
                if (velocity.Y > MAX_FALLING_SPEED)
                {
                    velocity.Y = MAX_FALLING_SPEED;
                }
            }

            MoveBy(input, velocity);

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            int textureCol = animation / 2;
            switch (currentState)
            {
                case State.Rise:
                    if (direction == Direction.Left)
                    {
                        graphics.DrawImageFix(GameImage.Mushroom, 32, 32, 0, textureCol, drawX, drawY, this);
                    }
                    else
                    {
                        graphics.DrawImageFixFlip(GameImage.Mushroom, 32, 32, 0, textureCol, drawX, drawY, this);
                    }
                    break;
                case State.Wait:
                    if (direction == Direction.Left)
                    {
                        graphics.DrawImageFix(GameImage.Mushroom, 32, 32, 0, 7, drawX, drawY, this);
                    }
                    else
                    {
                        graphics.DrawImageFixFlip(GameImage.Mushroom, 32, 32, 0, 7, drawX, drawY, this);
                    }
                    break;
                case State.Attack:
                    graphics.DrawImageRotate(GameImage.Mushroom, 32, 32, 1, textureCol, drawX + 16, drawY + 16, 16, 16, (int)Math.Round(angle));
                    break;
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            if (aggressive && game.Items.Count < 3)
            {
                if (game.Random.Next(0, 4) == 0)
                {
                    game.AddItem(new FlameItem(game, position, new Vector(0, 0)));
                }
            }

            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.PlaySound(GameSound.Explode2);
            SpreadDebris(4);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            angle = -angle + 180;
        }

        public override void Blocked_Right(GameInput input)
        {
            angle = -angle + 180;
        }

        public override void Blocked_Top(GameInput input)
        {
            angle = -(angle + 90) + 90;
        }

        public override void Blocked_Bottom(GameInput input)
        {
            angle = -(angle + 90) + 90;
        }

        public override bool IgnoreEnemyBlock
        {
            get
            {
                return true;
            }
        }

        public bool Aggressive
        {
            get
            {
                return aggressive;
            }
        }
    }
}
