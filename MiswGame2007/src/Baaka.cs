using System;

namespace MiswGame2007
{
    public class Baaka : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum State
        {
            Move,
            ChasePlayer,
            Attack,
            OpenField
        }

        private const int INIT_HEALTH = 100;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;
        private const int NUM_ANIMATIONS = 16;

        private static Vector SIZE = new Vector(32, 80);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(32, 16), SIZE);

        private Direction direction;
        private int atFieldCount;
        private int atFieldCount2;
        private int attackCount;
        private int attackCount2;
        private State currentState;
        private State nextState;
        private AtField atField;
        private double attackAngle;
        private int animation;

        public Baaka(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            atFieldCount = 0;
            atFieldCount2 = 0;
            attackCount = 0;
            attackCount2 = 0;
            currentState = State.Move;
            nextState = State.Move;
            atField = null;
            attackAngle = 0;
            animation = 0;
        }

        public override void Tick(GameInput input)
        {
            if (atField != null)
            {
                if (atField.Removed)
                {
                    atField = null;
                }
            }

            if (atFieldCount2 > 0)
            {
                atFieldCount2--;
            }

            if (attackCount2 > 0)
            {
                attackCount2--;
            }

            Player player = game.Player;

            switch (currentState)
            {
                case State.Move:
                    if (direction == Direction.Left)
                    {
                        velocity.X = -2;
                    }
                    else
                    {
                        velocity.X = 2;
                    }
                    if (atField != null)
                    {
                        atField.Remove();
                        atField = null;
                    }
                    if (Math.Abs(player.Center.X - Center.X) < 320 && Math.Abs(player.Center.Y - Center.Y) < 64)
                    {
                        nextState = State.ChasePlayer;
                    }
                    if (nextState != State.Move)
                    {
                        if (nextState == State.OpenField)
                        {
                            if (atFieldCount2 == 0)
                            {
                                currentState = State.OpenField;
                            }
                        }
                        else
                        {
                            currentState = nextState;
                        }
                    }
                    animation = (animation + 1) % NUM_ANIMATIONS;
                    break;
                case State.ChasePlayer:
                    double dx = player.Center.X - Center.X;
                    if (Math.Abs(dx) < 256 && attackCount2 == 0)
                    {
                        nextState = State.Attack;
                    }
                    else if (Math.Abs(dx) < 2)
                    {
                        velocity.X = dx;
                    }
                    else
                    {
                        if (dx < 0)
                        {
                            velocity.X = -2;
                            direction = Direction.Left;
                        }
                        else
                        {
                            velocity.X = 2;
                            direction = Direction.Right;
                        }
                    }
                    if (Math.Abs(player.Center.Y - Center.Y) > 64)
                    {
                        nextState = State.Move;
                    }
                    if (atField != null)
                    {
                        atField.Remove();
                        atField = null;
                    }
                    if (nextState != State.ChasePlayer)
                    {
                        if (nextState == State.OpenField)
                        {
                            if (atFieldCount2 == 0)
                            {
                                currentState = State.OpenField;
                            }
                        }
                        else
                        {
                            currentState = nextState;
                        }
                    }
                    animation = (animation + 1) % NUM_ANIMATIONS;
                    break;
                case State.OpenField:
                    velocity.X = 0;
                    if (nextState == State.OpenField)
                    {
                        if (atFieldCount < 15)
                        {
                            atFieldCount++;
                        }
                        else
                        {
                            if (atField == null)
                            {
                                atField = new AtField(game, (AtField.Direction)direction, this);
                                game.AddEnemy(atField);
                            }
                        }
                    }
                    else
                    {
                        if (atFieldCount > 0)
                        {
                            atFieldCount--;
                        }
                        else
                        {
                            if (atField != null)
                            {
                                atField.Remove();
                                atField = null;
                            }
                            currentState = nextState;
                        }
                    }
                    if ((direction == Direction.Left && Center.X < player.Center.X) || (direction == Direction.Right && player.Center.X < Center.X))
                    {
                        nextState = State.ChasePlayer;
                    }
                    animation = 0;
                    break;
                case State.Attack:
                    velocity.X = 0;
                    if (attackCount < 20)
                    {
                        if (attackCount == 0)
                        {
                            Vector attackPos = Center + new Vector(0, -20);
                            attackAngle = Math.Atan2(game.Player.Center.Y - attackPos.Y, game.Player.Center.X - attackPos.X);
                            game.PlaySound(GameSound.Baaka);
                        }
                        if (attackCount % 4 == 2)
                        {
                            game.AddEnemyBullet(new BaakaBullet(game, Center + new Vector(0, -20), 8 * new Vector(Math.Cos(attackAngle), Math.Sin(attackAngle))));
                        }
                        attackCount++;
                    }
                    else
                    {
                        attackCount = 0;
                        attackCount2 = game.Random.Next(60, 120);
                        nextState = State.ChasePlayer;
                    }
                    if (nextState != State.Attack && attackCount == 0)
                    {
                        if (nextState == State.OpenField)
                        {
                            if (atFieldCount2 == 0)
                            {
                                currentState = State.OpenField;
                            }
                        }
                        else
                        {
                            currentState = nextState;
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
            int textureCol = animation / 2;
            if (direction == Direction.Left)
            {
                if (currentState != State.Attack)
                {
                    graphics.DrawImageFix(GameImage.Baaka, 96, 96, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFix(GameImage.Baaka, 96, 96, 0, 8, drawX, drawY, this);
                }
            }
            else
            {
                if (currentState != State.Attack)
                {
                    graphics.DrawImageFixFlip(GameImage.Baaka, 96, 96, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFixFlip(GameImage.Baaka, 96, 96, 0, 8, drawX, drawY, this);
                }
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            if (atField != null)
            {
                atField.Remove();
                atField = null;
            }

            game.AddParticle(new BigExplosion(game, Center + new Vector(32, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, -32), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(-32, 0), Vector.Zero));
            game.AddParticle(new BigExplosion(game, Center + new Vector(0, 32), Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(32);
            Remove();
        }

        public override void Damage(int amount)
        {
            base.Damage(amount);
            if (atFieldCount2 == 0)
            {
                nextState = State.OpenField;
            }
            else
            {
                nextState = State.ChasePlayer;
            }
        }

        public override void Blodked_Left(GameInput input)
        {
            direction = Direction.Right;
        }

        public override void Blocked_Right(GameInput input)
        {
            direction = Direction.Left;
        }

        public void AtFieldBroken()
        {
            atField = null;
            atFieldCount2 = 120;
            attackCount2 = 30;
            nextState = State.ChasePlayer;
        }
    }
}
