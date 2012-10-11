using System;

namespace MiswGame2007
{
    public class ItemEnemy : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum Item
        {
            Machinegun,
            Rocket,
            Flame,
            Shotgun,
            Health
        }

        private const int INIT_HEALTH = 30;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;
        private const int NUM_ANIMATIONS = 32;

        private static Vector SIZE = new Vector(32, 32);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(16, 32), SIZE);

        private Direction direction;
        private Item item;
        private int animation;
        private int moveCount;
        private int moveCount2;
        private bool jumping;

        public ItemEnemy(GameScene game, int row, int col, Direction direction, Item item)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            this.item = item;
            animation = 0;
            moveCount = 0;
            moveCount2 = 0;
            jumping = false;
        }

        public override void Tick(GameInput input)
        {
            if (moveCount2 > 0)
            {
                moveCount2--;
            }

            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            if (moveCount2 == 0)
            {
                if (moveCount > 0)
                {
                    moveCount--;

                    if (!jumping)
                    {
                        if (Math.Abs(dx) < 256 && Math.Abs(dy) < 64)
                        {
                            if (dx < 0)
                            {
                                direction = Direction.Left;
                                // velocity.X = 3;
                            }
                            else if (dx > 0)
                            {
                                direction = Direction.Right;
                                // velocity.X = -3;
                            }
                            velocity.X = 6 * game.Random.NextDouble() - 3;
                            velocity.Y = -4;
                            moveCount = 16 * game.Random.Next(2, 17);
                            moveCount2 = game.Random.Next(15, 30);
                            jumping = true;
                            animation = -1;
                        }
                        else
                        {
                            if (direction == Direction.Left)
                            {
                                velocity.X = -1;
                            }
                            else
                            {
                                velocity.X = 1;
                            }
                            animation = (animation + 1) % NUM_ANIMATIONS;
                        }
                    }
                }
                else
                {
                    moveCount = 16 * game.Random.Next(2, 17);
                    moveCount2 = game.Random.Next(60, 120);
                    if (!jumping)
                    {
                        velocity.X = 0;
                        animation = 0;
                    }
                }
            }

            if (jumping)
            {
                if (animation < 15)
                {
                    animation++;
                }
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
                if (!jumping)
                {
                    int textureCol = animation / 4;
                    graphics.DrawImageFix(GameImage.ItemEnemy, 64, 64, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    int textureCol = animation / 2;
                    graphics.DrawImageFix(GameImage.ItemEnemy, 64, 64, 1, textureCol, drawX, drawY, this);
                }
            }
            else
            {
                if (!jumping)
                {
                    int textureCol = animation / 4;
                    graphics.DrawImageFixFlip(GameImage.ItemEnemy, 64, 64, 0, textureCol, drawX, drawY, this);
                }
                else
                {
                    int textureCol = animation / 2;
                    graphics.DrawImageFixFlip(GameImage.ItemEnemy, 64, 64, 1, textureCol, drawX, drawY, this);
                }
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            switch (item)
            {
                case Item.Machinegun:
                    game.AddItem(new MachinegunItem(game, position + new Vector(16, 32), new Vector(0, -8)));
                    break;
                case Item.Rocket:
                    game.AddItem(new RocketItem(game, position + new Vector(16, 32), new Vector(0, -8)));
                    break;
                case Item.Shotgun:
                    game.AddItem(new ShotgunItem(game, position + new Vector(16, 32), new Vector(0, -8)));
                    break;
                case Item.Flame:
                    game.AddItem(new FlameItem(game, position + new Vector(16, 32), new Vector(0, -8)));
                    break;
                case Item.Health:
                    game.AddItem(new HealthItem(game, position + new Vector(16, 32), new Vector(0, -8)));
                    break;
            }

            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(16);
            Remove();
        }

        public override void Blodked_Left(GameInput input)
        {
            direction = Direction.Right;
        }

        public override void Blocked_Right(GameInput input)
        {
            direction = Direction.Left;
        }

        public override void Blocked_Bottom(GameInput input)
        {
            base.Blocked_Top(input);
            velocity.X = 0;
            if (jumping)
            {
                jumping = false;
                animation = 0;
            }
        }
    }
}
