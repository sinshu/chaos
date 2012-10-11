using System;

namespace MiswGame2007
{
    public class Hanabi : Particle
    {
        private double explodeY;
        private bool exploding;
        private int animation;
        private bool big;
        private int colorIndex;
        private bool flip;

        public Hanabi(GameScene game, Vector explosion, bool big, int colorIndex)
            : base(game, new Vector(explosion.X, 512), new Vector(0, -8))
        {
            explodeY = explosion.Y;
            exploding = false;
            animation = -1;
            this.big = big;
            this.colorIndex = colorIndex;
            flip = game.Random.Next(0, 2) == 0;
        }

        public override void Tick()
        {
            base.Tick();
            if (position.Y < explodeY)
            {
                position.Y = explodeY;
                exploding = true;
            }
            if (exploding)
            {
                if (animation < 64)
                {
                    animation++;
                }
                else
                {
                    Remove();
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) + game.IntBackgroundX;
            int drawY = (int)Math.Round(position.Y) + game.IntBackgroundY;

            if (big)
            {
                if (!exploding)
                {
                    graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 1, 0, drawX - 16, drawY - 16, 255, GetRed(), GetGreen(), GetBlue());
                }
                else
                {
                    if (!flip)
                    {
                        graphics.DrawImageAdd(GameImage.BigHanabi, 256, 256, animation / 2 / 4, animation / 2 % 4, drawX - 128, drawY - 128, 255, GetRed(), GetGreen(), GetBlue());
                    }
                    else
                    {
                        graphics.DrawImageAddFlipRotate90(GameImage.BigHanabi, 256, 256, animation / 2 / 4, animation / 2 % 4, drawX - 128, drawY - 128, 1, GetRed(), GetGreen(), GetBlue());
                    }
                }
            }
            else
            {
                if (!exploding)
                {
                    graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 1, 1, drawX - 16, drawY - 16, 255, GetRed(), GetGreen(), GetBlue());
                }
                else
                {
                    if (!flip)
                    {
                        graphics.DrawImageAdd(GameImage.SmallHanabi, 128, 128, animation / 2 / 4, animation / 2 % 4, drawX - 64, drawY - 64, 255, GetRed(), GetGreen(), GetBlue());
                    }
                    else
                    {
                        graphics.DrawImageAddFlipRotate90(GameImage.SmallHanabi, 128, 128, animation / 2 / 4, animation / 2 % 4, drawX - 64, drawY - 64, 1, GetRed(), GetGreen(), GetBlue());
                    }
                }
            }
        }

        private int GetRed()
        {
            if (colorIndex <= 1 || colorIndex == 5)
            {
                return 255;
            }
            else
            {
                return 0;
            }
        }

        private int GetGreen()
        {
            if (colorIndex >= 1 && colorIndex <= 3)
            {
                return 255;
            }
            else
            {
                return 0;
            }
        }

        private int GetBlue()
        {
            if (colorIndex >= 3 && colorIndex <= 5)
            {
                return 255;
            }
            else
            {
                return 0;
            }
        }
    }
}
