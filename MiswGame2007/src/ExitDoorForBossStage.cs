using System;

namespace MiswGame2007
{
    public class ExitDoorForBossStage : ExitDoor
    {
        private int delay;

        public ExitDoorForBossStage(GameScene game, int row, int col)
            : base(game, row, col)
        {
            delay = 256;
        }

        public override void Tick()
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
                            game.PlayMusic(GameMusic.Clear);
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
                        if (delay > 0)
                        {
                            delay--;
                        }
                        else
                        {
                            game.Clear();
                        }
                    }
                }
            }
        }
    }
}
