using System;

namespace MiswGame2007
{
    public class StageSelectScene
    {
        public enum State
        {
            None = 1,
            GotoGame,
            Exit
        }

        private int numUnlockedStages;
        private int currentStageIndex;
        private State currentState;
        private bool gotoGame;
        private int gotoGameCount;

        private AudioDevice audio;

        public StageSelectScene(int numUnlockedStages, int initStageIndex)
        {
            this.numUnlockedStages = numUnlockedStages;
            currentStageIndex = initStageIndex;
            currentState = State.None;
            gotoGame = false;
            gotoGameCount = 0;

            audio = null;
        }

        public void Tick(StageSelectInput input)
        {
            if (numUnlockedStages > 0 && !gotoGame && currentState != State.Exit)
            {
                if (input.Left && !input.Right)
                {
                    currentStageIndex--;
                    if (currentStageIndex < 0)
                    {
                        currentStageIndex += numUnlockedStages;
                    }
                }
                else if (input.Right && !input.Left)
                {
                    currentStageIndex = (currentStageIndex + 1) % numUnlockedStages;
                }
            }

            if (input.Start)
            {
                gotoGame = true;
            }

            if (input.Exit && !gotoGame)
            {
                currentState = State.Exit;
            }

            if (gotoGame)
            {
                if (gotoGameCount < 16)
                {
                    if (gotoGameCount == 0)
                    {
                        audio.StopMusic();
                    }
                    gotoGameCount++;
                }
                else
                {
                    currentState = State.GotoGame;
                }
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            graphics.FillScreen(0, 0, 0, 192);

            graphics.DrawImageAdd(GameImage.Message, 256, 32, 3, 0, (Settings.SCREEN_WIDTH - 256) / 2, Settings.SCREEN_HEIGHT / 2 - 80, 255);

            if (currentStageIndex + 1 < 10)
            {
                graphics.DrawImageAdd(GameImage.Number, 32, 64, 0, currentStageIndex + 1, (Settings.SCREEN_WIDTH - 32) / 2, (Settings.SCREEN_HEIGHT - 64) / 2, 255);
            }
            else
            {
                graphics.DrawImageAdd(GameImage.Number, 32, 64, 0, (currentStageIndex + 1) / 10, (Settings.SCREEN_WIDTH - 64) / 2, (Settings.SCREEN_HEIGHT - 64) / 2, 255);
                graphics.DrawImageAdd(GameImage.Number, 32, 64, 0, (currentStageIndex + 1) % 10, (Settings.SCREEN_WIDTH - 64) / 2 + 32, (Settings.SCREEN_HEIGHT - 64) / 2, 255);
            }

            if (currentStageIndex > 0)
            {
                graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, Settings.SCREEN_WIDTH / 2 - 80, Settings.SCREEN_HEIGHT / 2 - 16);
            }
            if (currentStageIndex + 1 < numUnlockedStages)
            {
                graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, Settings.SCREEN_WIDTH / 2 + 48, Settings.SCREEN_HEIGHT / 2 - 16);
            }

            if (gotoGameCount > 0)
            {
                if (gotoGameCount < 16)
                {
                    graphics.FillScreen(255, 255, 255, 16 * gotoGameCount);
                }
                else
                {
                    graphics.FillScreen(255, 255, 255, 255);
                }
            }
        }

        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public int SelectedStage
        {
            get
            {
                return currentStageIndex;
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
