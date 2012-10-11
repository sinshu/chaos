using System;

namespace MiswGame2007
{
    public class House : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum State
        {
            Sleep = 1,
            Move,
            Attack
        }

        private const int INIT_HEALTH = 100;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;
        private const int NUM_ANIMATIONS = 32;

        private static Vector SIZE = new Vector(32, 40);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(16, 24), SIZE);

        private Direction direction;
        private State currentState;
        private int openCount;
        private int attackCount;
        private int animation;

        public House(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            currentState = State.Sleep;
            openCount = 0;
            attackCount = 0;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            Player player = game.Player;
            double playerRange;
            double dx = 1 * (player.Center.X - Center.X);
            double dy = 2 * (player.Center.Y - Center.Y);
            playerRange = dx * dx + dy * dy;
            if (playerRange < 256 * 256 && ((direction == Direction.Left && dx < 0) || (direction == Direction.Right && dx > 0)))
            {
                if (currentState != State.Attack)
                {
                    currentState = State.Attack;
                }
            }
            else if (playerRange < 512 * 512)
            {
                currentState = State.Move;
            }
            else
            {
                currentState = State.Sleep;
            }



            if (currentState == State.Sleep || currentState == State.Move)
            {
                if (openCount > -1)
                {
                    openCount--;
                }
            }
            else if (currentState == State.Attack)
            {
                if (openCount < 15)
                {
                    if (openCount == 12)
                    {
                        game.PlaySound(GameSound.Hi1);
                    }
                    openCount++;
                }
            }

            if (openCount == -1)
            {
                if (dx < 0)
                {
                    direction = Direction.Left;
                }
                else if (dx > 0)
                {
                    direction = Direction.Right;
                }
            }
            else
            {
                velocity.X = 0;
            }

            switch (currentState)
            {
                case State.Sleep:
                    if (openCount == -1)
                    {
                        velocity.X = 0;
                        animation = 0;
                    }
                    break;
                case State.Move:
                    if (openCount == -1)
                    {
                        if (Math.Abs(dx) < 0.5)
                        {
                            velocity.X = dx;
                        }
                        else
                        {
                            if (direction == Direction.Left)
                            {
                                velocity.X = -0.5;
                            }
                            else
                            {
                                velocity.X = 0.5;
                            }
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                    break;
                case State.Attack:
                    if (openCount == 15)
                    {
                        if (attackCount == 0)
                        {
                            if (direction == Direction.Left)
                            {
                                game.AddEnemyBullet(new HouseBullet(game, position + new Vector(32 - 2, 24), new Vector(-4 * game.Random.NextDouble(), 4 * game.Random.NextDouble() - 8)));
                            }
                            else
                            {
                                game.AddEnemyBullet(new HouseBullet(game, position + new Vector(32 + 2, 24), new Vector(4 * game.Random.NextDouble(), 4 * game.Random.NextDouble() - 8)));
                            }
                            attackCount = 16;
                        }
                        else
                        {
                            attackCount--;
                        }
                    }
                    break;
            }
            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }
            MoveBy(input, velocity);

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                int textureCol = animation / 4;
                if (openCount == -1)
                {
                    graphics.DrawImageFix(GameImage.House, 64, 64, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFix(GameImage.House, 64, 64, 1, openCount / 2, drawX, drawY, this);
                }
            }
            else
            {
                int textureCol = animation / 4;
                if (openCount == -1)
                {
                    graphics.DrawImageFixFlip(GameImage.House, 64, 64, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFixFlip(GameImage.House, 64, 64, 1, openCount / 2, drawX, drawY, this);
                }
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(16);
            Remove();
        }
    }
}
