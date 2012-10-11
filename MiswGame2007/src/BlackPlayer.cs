using System;

namespace MiswGame2007
{
    public class BlackPlayer : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum Weapon
        {
            Pistol = 1,
            Machinegun,
            Shotgun,
            Rocket,
            Flamethrower
        }

        private const int INIT_HEALTH = 30;

        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_FALLING_SPEED = 16;
        private const int NUM_ANIMATIONS = 16;

        private static Vector SIZE = new Vector(20, 36);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(6, 28), SIZE);

        private Direction direction;
        private Weapon weapon;
        private bool playerDetected;
        private bool attacking;
        private int attackCount;
        private int attackCount2;
        private int attackWaitCount;
        private int playerRange;
        private int animation;
        private int fireAnimation;

        public BlackPlayer(GameScene game, int row, int col, Direction direction, Weapon weapon)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            this.weapon = weapon;
            playerDetected = false;
            attacking = false;
            attackCount = 0;
            attackCount2 = 0;
            attackWaitCount = 0;
            playerRange = 256;
            animation = 0;
            fireAnimation = 0;
            if (weapon != Weapon.Pistol)
            {
                health = 50;
            }
        }

        public override void Tick(GameInput input)
        {
            if (attackCount2 > 0)
            {
                attackCount2--;
            }

            double dx = game.Player.Center.X - Center.X;
            double dy = game.Player.Center.Y - Center.Y;

            playerDetected = false;
            if (attacking)
            {
                velocity.X = 0;
                if (attackWaitCount > 0)
                {
                    attackWaitCount--;
                    fireAnimation = 0;
                }
                else if (attackCount < 64)
                {
                    Vector posFix = new Vector(0, 30);
                    int angle = 180;
                    if (direction == Direction.Left)
                    {
                    }
                    else
                    {
                        posFix.X = 32 - posFix.X;
                        angle = 180 - angle;
                    }

                    switch (weapon)
                    {
                        case Weapon.Pistol:
                            if (attackCount < 48)
                            {
                                if (attackCount % 16 == 0)
                                {
                                    game.AddEnemyBullet(new PlayerBullet(game, position + posFix, angle, true));
                                    game.PlaySound(GameSound.Pistol);
                                }
                                fireAnimation = attackCount / 2 % 8;
                            }
                            else
                            {
                                fireAnimation = 0;
                            }
                            break;
                        case Weapon.Machinegun:
                            if (attackCount == 24)
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
                            if (attackCount % 32 < 16)
                            {
                                if (attackCount % 4 == 0)
                                {
                                    game.AddEnemyBullet(new PlayerBullet2(game, position + posFix, angle, true));
                                    game.PlaySound(GameSound.Pistol);
                                }
                                fireAnimation = (2 * attackCount) % 8;
                            }
                            else
                            {
                                fireAnimation = 0;
                            }
                            break;
                        case Weapon.Rocket:
                            if (attackCount % 32 == 0)
                            {
                                game.AddEnemyBullet(new KyoroRocket(game, position + posFix, angle, true));
                                game.PlaySound(GameSound.Rocket);
                            }
                            fireAnimation = attackCount / 4 % 8;
                            break;
                        case Weapon.Shotgun:
                            if (attackCount == 0)
                            {
                                Random random = game.Random;
                                for (int i = 0; i < 16; i++)
                                {
                                    game.AddEnemyBullet(new PlayerBullet3(game, position + posFix, angle + 10 * random.NextDouble() + 10 * random.NextDouble() - 10, 8 + 16 * random.NextDouble(), true));
                                }
                                game.Quake(4);
                                game.Flash(32);
                                game.PlaySound(GameSound.Shotgun);
                            }
                            if (attackCount < 16)
                            {
                                fireAnimation = attackCount / 2;
                            }
                            else
                            {
                                fireAnimation = 0;
                            }
                            break;
                        case Weapon.Flamethrower:
                            {
                                if (attackCount == 0)
                                {
                                    game.PlaySound(GameSound.Flame);
                                }
                                if (dx < 0)
                                {
                                    direction = Direction.Left;
                                }
                                else if (dx > 0)
                                {
                                    direction = Direction.Right;
                                }
                                Random random = game.Random;
                                game.AddEnemyBullet(new PlayerFlame(game, position + posFix, angle + 6 * random.NextDouble() + 6 * random.NextDouble() - 6, true));
                            }
                            break;
                    }
                    attackCount++;
                }
                if (attackCount >= 64)
                {
                    playerRange = game.Random.Next(128, 320);
                    attacking = false;
                    attackCount = 0;
                    attackCount2 = game.Random.Next(60, 120);
                    fireAnimation = 0;
                }
                playerDetected = true;
                animation = 0;
            }
            else if (Math.Abs(dx) < 320 && Math.Abs(dy) < 128)
            {
                playerDetected = true;
                if (dx < 0)
                {
                    direction = Direction.Left;
                }
                else if (dx > 0)
                {
                    direction = Direction.Right;
                }

                if (attackCount2 == 0)
                {
                    attacking = true;
                    attackWaitCount = 30;
                }
                else
                {
                    if (Math.Abs(Math.Abs(dx) - playerRange) < 4)
                    {
                        velocity.X = 0;
                        animation = 0;
                    }
                    else if (Math.Abs(dx) < playerRange)
                    {
                        if (dx < 0)
                        {
                            velocity.X = 2;
                        }
                        else if (dx > 0)
                        {
                            velocity.X = -2;
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                    else
                    {
                        if (dx < 0)
                        {
                            velocity.X = -2;
                        }
                        else if (dx > 0)
                        {
                            velocity.X = 2;
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                }
            }
            else
            {
                if (direction == Direction.Left)
                {
                    velocity.X = -2;
                }
                else
                {
                    velocity.X = 2;
                }
                animation = (animation + 1) % NUM_ANIMATIONS;
            }
            velocity.Y += ACCELERATION_FALLING;
            if (velocity.Y > MAX_FALLING_SPEED)
            {
                velocity.Y = MAX_FALLING_SPEED;
            }
            MoveBy(input, velocity);

            base.Tick(input);

            // Console.Write(attackCount + ",");
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (direction == Direction.Left)
            {
                if (!attacking)
                {
                    graphics.DrawImageFix(GameImage.BlackPlayer, 32, 64, 0, animation / 2, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFix(GameImage.BlackPlayer, 32, 64, 1, fireAnimation, drawX, drawY, this);
                }
            }
            else
            {
                if (!attacking)
                {
                    graphics.DrawImageFixFlip(GameImage.BlackPlayer, 32, 64, 0, animation / 2, drawX, drawY, this);
                }
                else
                {
                    graphics.DrawImageFixFlip(GameImage.BlackPlayer, 32, 64, 1, fireAnimation, drawX, drawY, this);
                }
            }
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            switch (weapon)
            {
                case Weapon.Machinegun:
                    game.AddItem(new MachinegunItem(game, position + new Vector(0, 32), new Vector(0, -8)));
                    break;
                case Weapon.Rocket:
                    game.AddItem(new RocketItem(game, position + new Vector(0, 32), new Vector(0, -8)));
                    break;
                case Weapon.Shotgun:
                    game.AddItem(new ShotgunItem(game, position + new Vector(0, 32), new Vector(0, -8)));
                    break;
                case Weapon.Flamethrower:
                    game.AddItem(new FlameItem(game, position + new Vector(0, 32), new Vector(0, -8)));
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
            if (!playerDetected)
            {
                direction = Direction.Right;
            }
        }

        public override void Blocked_Right(GameInput input)
        {
            if (!playerDetected)
            {
                direction = Direction.Left;
            }
        }
    }
}
