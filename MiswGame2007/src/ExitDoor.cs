using System;

namespace MiswGame2007
{
    public class ExitDoor
    {
        protected GameScene game;
        protected Vector position;
        protected bool visible;
        protected bool playerExited;
        protected int fadeCount;
        protected int slideCount;

        public ExitDoor(GameScene game, int row, int col)
        {
            this.game = game;
            position.X = col * Settings.BLOCK_WDITH;
            position.Y = row * Settings.BLOCK_WDITH;
            visible = false;
            playerExited = false;
            fadeCount = 0;
            slideCount = 0;
        }

        public virtual void Tick()
        {
            if (!visible)
            {
                if (game.Enemies.Count == 0)
                {
                    visible = true;
                }
            }
            else if (!playerExited)
            {
                if (fadeCount < 64)
                {
                    fadeCount++;
                }
                else
                {
                    if (slideCount < 20)
                    {
                        slideCount++;
                    }
                    else
                    {
                        Player player = game.Player;
                        if (player.CurrentLandState == Player.LandState.OnGround && Math.Abs(player.Center.X - (position.X + 32)) < 4 && Math.Abs(player.Center.Y - (position.Y + 32)) < 16)
                        {
                            playerExited = true;
                            player.Freeze();
                        }
                    }
                }
            }
            else
            {
                if (slideCount > 0)
                {
                    slideCount--;
                    if (slideCount == 0) game.Player.Disappear();
                }
                else
                {
                    if (fadeCount > 0)
                    {
                        fadeCount--;
                    }
                    else
                    {
                        game.Clear();
                    }
                }
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            if (visible)
            {
                if (fadeCount < 64)
                {
                    graphics.DrawImageAlpha(GameImage.Block, 64, 64, 3, 0, drawX, drawY, 4 * fadeCount);
                    graphics.DrawImageAlpha(GameImage.Block, 64, 64, 3, 1, drawX, drawY, 4 * fadeCount);
                }
                else if (slideCount < 20)
                {
                    graphics.DrawImage(GameImage.Block, 64, 64, 3, 0, drawX, drawY);
                    graphics.DrawImage2(GameImage.Block, 76 + slideCount, 196, 20 - slideCount, 60, drawX + 12, drawY + 4);
                    graphics.DrawImage2(GameImage.Block, 96, 196, 20 - slideCount, 60, drawX + 32 + slideCount, drawY + 4);
                }
                else
                {
                    graphics.DrawImage(GameImage.Block, 64, 64, 3, 0, drawX, drawY);
                }
            }
        }

        public void Draw2(GraphicsDevice graphics)
        {
            if (playerExited)
            {
                int drawX = (int)Math.Round(position.X - game.IntCameraX);
                int drawY = (int)Math.Round(position.Y - game.IntCameraY);
                if (slideCount > 0)
                {
                    graphics.DrawImage2(GameImage.Block, 76 + slideCount, 196, 20 - slideCount, 60, drawX + 12, drawY + 4);
                    graphics.DrawImage2(GameImage.Block, 96, 196, 20 - slideCount, 60, drawX + 32 + slideCount, drawY + 4);
                }
            }
        }

        public Vector Center
        {
            get
            {
                return position + new Vector(32, 32);
            }
        }
    }
}
