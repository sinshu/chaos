using System;

namespace MiswGame2007
{
    public class EndingScene
    {
        public enum State
        {
            None = 1,
            Exit
        }

        private enum Message
        {
            Macoto = 1,
            Sinshu,
            Zhon,
            Crimson,
            Iori,
            Yutaka,
            Yousuke,
            Tetsu,
            MiswMember,
            ProjectLeader,
            Program,
            Music,
            Graphics,
            StageConstruction,
            Voice,
            SpecialThanks,
            ChaoslugStaff,
            ThankYouForPlaying
        }

        private State currentState;
        private int numTicks;
        private bool forceExit;
        private int forceExitCount;

        private AudioDevice audio;

        public EndingScene()
        {
            currentState = State.None;
            numTicks = 0;
            forceExit = false;
            forceExitCount = 0;
            audio = null;
        }

        public void Tick(EndingInput input)
        {
            if (numTicks == 0)
            {
                audio.StopMusic();
            }
            if (numTicks == 60)
            {
                audio.PlayMusic(GameMusic.Ending);
            }
            if (numTicks < 90 * 60)
            {
                numTicks++;
            }
            else
            {
                currentState = State.Exit;
            }

            if (input.Exit && !forceExit)
            {
                forceExit = true;
            }
            if (forceExit)
            {
                if (forceExitCount < 16)
                {
                    forceExitCount++;
                }
                else
                {
                    currentState = State.Exit;
                }
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            if (numTicks < 128)
            {
                int color = 255 - 2 * numTicks;
                graphics.FillScreen(color, color, color, 255);
            }
            else
            {
                graphics.FillScreen(0, 0, 0, 255);
            }
            DrawMessage(4 * 60, 16 * 60, 0, graphics, Message.ChaoslugStaff);
            DrawMessage(16 * 60, 24 * 60, 0, graphics, Message.ProjectLeader, Message.Macoto);
            DrawMessage(24 * 60, 32 * 60, 0, graphics, Message.Program, Message.Sinshu);
            DrawMessage(32 * 60, 40 * 60, 0, graphics, Message.Music, Message.Yutaka, Message.Yousuke, Message.Sinshu);
            DrawMessage(40 * 60, 48 * 60, 0, graphics, Message.Graphics, Message.Macoto, Message.Sinshu, Message.Iori);
            DrawMessage(48 * 60, 56 * 60, 0, graphics, Message.StageConstruction, Message.Crimson, Message.Zhon, Message.Tetsu, Message.Sinshu);
            DrawMessage(56 * 60, 64 * 60, 0, graphics, Message.Voice, Message.Zhon);
            DrawMessage(64 * 60, 72 * 60, 0, graphics, Message.SpecialThanks, Message.MiswMember);
            DrawMessage(72 * 60, 88 * 60, -128, graphics, Message.ThankYouForPlaying);
            DrawPenguin(72 * 60, 88 * 60, 32, graphics);

            if (forceExit)
            {
                if (forceExitCount < 16)
                {
                    graphics.FillScreen(0, 0, 0, 16 * forceExitCount);
                }
                else
                {
                    graphics.FillScreen(0, 0, 0, 255);
                }
            }
        }

        private void DrawMessage(int beginTick, int endTick, int dy, GraphicsDevice graphics, Message message)
        {
            if (numTicks < beginTick)
            {
                return;
            }
            if (endTick < numTicks)
            {
                return;
            }

            int alpha = 255;
            if (numTicks < (beginTick + endTick) / 2)
            {
                if (numTicks - beginTick < 64)
                {
                    alpha = 4 * (numTicks - beginTick);
                }
            }
            else if ((beginTick + endTick) / 2 < numTicks)
            {
                if (endTick - numTicks < 64)
                {
                    alpha = 4 * (endTick - numTicks);
                }
            }

            switch (message)
            {
                case Message.Macoto:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 0, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Sinshu:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 1, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Zhon:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 2, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Crimson:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 3, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Iori:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 4, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Yutaka:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 5, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Yousuke:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 6, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Tetsu:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 7, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.MiswMember:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 8, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.ProjectLeader:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 0, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Program:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 1, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Music:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 2, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Graphics:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 3, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.StageConstruction:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 4, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.Voice:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 5, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.SpecialThanks:
                    graphics.DrawImageAdd(GameImage.Staff, 256, 32, 6, 1, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 32) / 2 + dy, alpha);
                    break;
                case Message.ChaoslugStaff:
                    graphics.DrawImageAdd(GameImage.Staff, 512, 64, 6, 0, (Settings.SCREEN_WIDTH - 512) / 2, (Settings.SCREEN_HEIGHT - 64) / 2 + dy, alpha);
                    break;
                case Message.ThankYouForPlaying:
                    graphics.DrawImageAdd(GameImage.Staff, 512, 64, 7, 0, (Settings.SCREEN_WIDTH - 512) / 2, (Settings.SCREEN_HEIGHT - 64) / 2 + dy, alpha);
                    break;
            }
        }

        private void DrawMessage(int beginTick, int endTick, int dy, GraphicsDevice graphics, Message msg1, Message msg2)
        {
            DrawMessage(beginTick, endTick, dy - 16, graphics, msg1);
            DrawMessage(beginTick, endTick, dy + 16, graphics, msg2);
        }

        private void DrawMessage(int beginTick, int endTick, int dy, GraphicsDevice graphics, Message msg1, Message msg2, Message msg3)
        {
            DrawMessage(beginTick, endTick, dy - 32, graphics, msg1);
            DrawMessage(beginTick, endTick, dy, graphics, msg2);
            DrawMessage(beginTick, endTick, dy + 32, graphics, msg3);
        }

        private void DrawMessage(int beginTick, int endTick, int dy, GraphicsDevice graphics, Message msg1, Message msg2, Message msg3, Message msg4)
        {
            DrawMessage(beginTick, endTick, dy - 32, graphics, msg1, msg2);
            DrawMessage(beginTick, endTick, dy + 32, graphics, msg3, msg4);
        }

        private void DrawMessage(int beginTick, int endTick, int dy, GraphicsDevice graphics, Message msg1, Message msg2, Message msg3, Message msg4, Message msg5)
        {
            DrawMessage(beginTick, endTick, dy - 64, graphics, msg1);
            DrawMessage(beginTick, endTick, dy, graphics, msg2, msg3, msg4);
            DrawMessage(beginTick, endTick, dy + 64, graphics, msg5);
        }

        private void DrawPenguin(int beginTick, int endTick, int dy, GraphicsDevice graphics)
        {
            if (numTicks < beginTick)
            {
                return;
            }
            if (endTick < numTicks)
            {
                return;
            }

            int alpha = 255;
            if (numTicks < (beginTick + endTick) / 2)
            {
                if (numTicks - beginTick < 64)
                {
                    alpha = 4 * (numTicks - beginTick);
                }
            }
            else if ((beginTick + endTick) / 2 < numTicks)
            {
                if (endTick - numTicks < 64)
                {
                    alpha = 4 * (endTick - numTicks);
                }
            }

            graphics.DrawImageAlpha(GameImage.Penguin, 256, 256, 0, 0, (Settings.SCREEN_WIDTH - 256) / 2, (Settings.SCREEN_HEIGHT - 256) / 2 + dy, alpha);
        }

        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public AudioDevice AudioDevice
        {
            set
            {
                audio = value;
            }
        }
    }
}
