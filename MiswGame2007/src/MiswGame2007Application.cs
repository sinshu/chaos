using System;
using System.IO;
using Yanesdk.Ytl;
using Yanesdk.System;
using Yanesdk.Draw;
using Yanesdk.Timer;

namespace MiswGame2007
{
    public class MiswGame2007Application : IDisposable
    {
        private enum State
        {
            Title = 1,
            StageSelect,
            Game,
            Gameover,
            Ending,
            Exit
        }

        private const int NUM_STAGES = 25;

        private Settings settings;
        private StageData[] stages;
        private SDLWindow window;
        private GraphicsDevice graphicsDevice;
        private AudioDevice audioDevice;
        private InputDevice inputDevice;
        private State currentState;

        private TitleScene title;
        private StageSelectScene stageSelect;
        private EndingScene ending;

        private int currentStageIndex;
        private GameScene currentGame;
        private int numUnlockedStages;

        private FpsTimer timer;

        private StreamWriter log;

        public MiswGame2007Application()
        {
            {
                FileArchiverZip zip = new FileArchiverZip();
                zip.ZipExtName = ".btw";
                FileSys.Archiver.Add(zip);
            }
            UnmanagedResourceManager.Instance.VideoMemory.LimitSize = 64 * 1024 * 1024;
            settings = new Settings("settings.cfg");
            CreateWindow(settings.Fullscreen);
            inputDevice = new InputDevice(!settings.Fullscreen, settings.JumpButton, settings.AttackButton, settings.StartButton);
            graphicsDevice = new GraphicsDevice(window);
            audioDevice = new AudioDevice();
            LoadStageData();

            title = new TitleScene(0, settings.ArcadeMode);
            title.AudioDevice = audioDevice;
            currentState = State.Title;
            currentStageIndex = settings.StartStageIndex;
            if (settings.StartStageIndex < settings.NumUnlockedStages)
            {
                numUnlockedStages = settings.NumUnlockedStages;
            }
            else
            {
                numUnlockedStages = settings.StartStageIndex + 1;
            }

            if (numUnlockedStages < 25)
            {
                numUnlockedStages = 25;
            }

            log = null;
            if (settings.ArcadeMode)
            {
                for (int i = 1; i <= 9999; i++)
                {
                    if (!File.Exists("log" + (10000 + i).ToString().Substring(1) + ".txt"))
                    {
                        log = new StreamWriter("log" + (10000 + i).ToString().Substring(1) + ".txt");
                        break;
                    }
                }
            }
        }

        public void LoadStageData()
        {
            stages = new StageData[NUM_STAGES];
            for (int i = 0; i < NUM_STAGES; i++)
            {
                string path;
                if (i < 9)
                {
                    path = Settings.RESOURCE_PATH + "/" + "data/stage0" + (i + 1) + ".dat";
                }
                else
                {
                    path = Settings.RESOURCE_PATH + "/" + "data/stage" + (i + 1) + ".dat";
                }
                stages[i] = new StageData(path, (i + 1) % 5 == 0);
            }
        }

        public void CreateWindow(bool fullscreen)
        {
            window = new SDLWindow();
            window.SetCaption("¬“×•¨Œê");
            window.BeginScreenTest();
            if (fullscreen)
            {
                window.TestVideoMode(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT, 32);
                window.TestVideoMode(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT, 16);
            }
            window.TestVideoMode(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT, 0);
            window.EndScreenTest();
        }

        public void Run()
        {
            timer = new FpsTimer();
            timer.Fps = 60;
            DoGC();
            while (SDLFrame.PollEvent() == YanesdkResult.NoError)
            {
                timer.WaitFrame();
                audioDevice.Update();
                inputDevice.Update();
                bool gc = Tick();
                if (currentState == State.Exit)
                {
                    break;
                }
                if (!timer.ToBeSkip)
                {
                    Draw();
                }
                if (gc)
                {
                    DoGC();
                }
            }

            settings.NumUnlockedStages = numUnlockedStages;
            settings.Save("settings.cfg");

            if (log != null)
            {
                log.Close();
            }

            SDLFrame.Quit();
        }

        public bool Tick()
        {
            switch (currentState)
            {
                case State.Title:
                    title.Tick(inputDevice.CurrentTitleInput);
                    if (title.CurrentState == TitleScene.State.StageSelect)
                    {
                        currentState = State.StageSelect;
                        stageSelect = new StageSelectScene(numUnlockedStages, currentStageIndex);
                        stageSelect.AudioDevice = audioDevice;
                        return true;
                    }
                    else if (title.CurrentState == TitleScene.State.GotoGame)
                    {
                        title = null;
                        currentState = State.Game;
                        currentStageIndex = settings.StartStageIndex;
                        currentGame = CreateGameScene(currentStageIndex);
                        currentGame.AudioDevice = audioDevice;
                        WriteLog("NewGame");
                        return true;
                    }
                    else if (title.CurrentState == TitleScene.State.Exit)
                    {
                        currentState = State.Exit;
                    }
                    break;
                case State.StageSelect:
                    stageSelect.Tick(inputDevice.CurrentStageSelectInput);
                    title.Tick(TitleInput.Empty);
                    if (stageSelect.CurrentState == StageSelectScene.State.GotoGame)
                    {
                        currentState = State.Game;
                        currentStageIndex = stageSelect.SelectedStage;
                        currentGame = CreateGameScene(currentStageIndex);
                        currentGame.AudioDevice = audioDevice;
                        WriteLog("Continue (Stage " + (currentStageIndex + 1) + ")");
                        return true;
                    }
                    else if (stageSelect.CurrentState == StageSelectScene.State.Exit)
                    {
                        stageSelect = null;
                        currentState = State.Title;
                        title.StageSelectExited();
                    }
                    break;
                case State.Game:
                    if (inputDevice.DebugKey)
                    {
                        currentGame.DebugMode = true;
                    }
                    currentGame.Tick(inputDevice.CurrentGameInput);
                    if (currentGame.CurrentState == GameScene.State.Clear)
                    {
                        if (currentStageIndex == NUM_STAGES - 1)
                        {
                            currentGame = null;
                            currentState = State.Ending;
                            ending = new EndingScene();
                            ending.AudioDevice = audioDevice;
                            return true;
                        }
                        currentStageIndex = (currentStageIndex + 1) % NUM_STAGES;
                        currentGame = CreateGameScene(currentStageIndex, currentGame.Player.State);
                        currentGame.AudioDevice = audioDevice;
                        if (numUnlockedStages < currentStageIndex + 1)
                        {
                            numUnlockedStages = currentStageIndex + 1;
                        }
                        return true;
                    }
                    else if (currentGame.CurrentState == GameScene.State.Gameover)
                    {
                        currentGame = null;
                        currentState = State.Title;
                        title = new TitleScene(1, settings.ArcadeMode);
                        title.AudioDevice = audioDevice;
                        return true;
                    }
                    break;
                case State.Ending:
                    ending.Tick(inputDevice.CurrentEndingInput);
                    if (ending.CurrentState == EndingScene.State.Exit)
                    {
                        ending = null;
                        currentState = State.Title;
                        title = new TitleScene(1, settings.ArcadeMode);
                        title.AudioDevice = audioDevice;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public void Draw()
        {
            graphicsDevice.BeginDraw();

            switch (currentState)
            {
                case State.Title:
                    title.Draw(graphicsDevice);
                    break;
                case State.StageSelect:
                    title.Draw(graphicsDevice);
                    stageSelect.Draw(graphicsDevice);
                    break;
                case State.Game:
                    currentGame.Draw(graphicsDevice);
                    break;
                case State.Ending:
                    ending.Draw(graphicsDevice);
                    break;
            }

            graphicsDevice.EndDraw();
        }

        public GameScene CreateGameScene(int index)
        {
            switch (index)
            {
                case 0:
                    return new GameScene01(stages[index]);
                case 1:
                    return new GameScene02(stages[index]);
                case 2:
                    return new GameScene03(stages[index]);
                case 3:
                    return new GameScene04(stages[index]);
                case 4:
                    return new GameScene05(stages[index]);
                case 5:
                    return new GameScene06(stages[index]);
                case 6:
                    return new GameScene07(stages[index]);
                case 7:
                    return new GameScene08(stages[index]);
                case 8:
                    return new GameScene09(stages[index]);
                case 9:
                    return new GameScene10(stages[index]);
                case 10:
                    return new GameScene11(stages[index]);
                case 11:
                    return new GameScene12(stages[index]);
                case 12:
                    return new GameScene13(stages[index]);
                case 13:
                    return new GameScene14(stages[index]);
                case 14:
                    return new GameScene15(stages[index]);
                case 15:
                    return new GameScene16(stages[index]);
                case 16:
                    return new GameScene17(stages[index]);
                case 17:
                    return new GameScene18(stages[index]);
                case 18:
                    return new GameScene19(stages[index]);
                case 19:
                    return new GameScene20(stages[index]);
                case 20:
                    return new GameScene21(stages[index]);
                case 21:
                    return new GameScene22(stages[index]);
                case 22:
                    return new GameScene23(stages[index]);
                case 23:
                    return new GameScene24(stages[index]);
                case 24:
                    return new GameScene25(stages[index]);
                default:
                    return null;
            }
        }

        public GameScene CreateGameScene(int index, PlayerState playerState)
        {
            switch (index)
            {
                case 0:
                    return new GameScene01(stages[index], playerState);
                case 1:
                    return new GameScene02(stages[index], playerState);
                case 2:
                    return new GameScene03(stages[index], playerState);
                case 3:
                    return new GameScene04(stages[index], playerState);
                case 4:
                    return new GameScene05(stages[index], playerState);
                case 5:
                    return new GameScene06(stages[index], playerState);
                case 6:
                    return new GameScene07(stages[index], playerState);
                case 7:
                    return new GameScene08(stages[index], playerState);
                case 8:
                    return new GameScene09(stages[index], playerState);
                case 9:
                    return new GameScene10(stages[index], playerState);
                case 10:
                    return new GameScene11(stages[index], playerState);
                case 11:
                    return new GameScene12(stages[index], playerState);
                case 12:
                    return new GameScene13(stages[index], playerState);
                case 13:
                    return new GameScene14(stages[index], playerState);
                case 14:
                    return new GameScene15(stages[index], playerState);
                case 15:
                    return new GameScene16(stages[index], playerState);
                case 16:
                    return new GameScene17(stages[index], playerState);
                case 17:
                    return new GameScene18(stages[index], playerState);
                case 18:
                    return new GameScene19(stages[index], playerState);
                case 19:
                    return new GameScene20(stages[index], playerState);
                case 20:
                    return new GameScene21(stages[index], playerState);
                case 21:
                    return new GameScene22(stages[index], playerState);
                case 22:
                    return new GameScene23(stages[index], playerState);
                case 23:
                    return new GameScene24(stages[index], playerState);
                case 24:
                    return new GameScene25(stages[index], playerState);
                default:
                    return null;
            }
        }

        public void Dispose()
        {
        }

        private void DoGC()
        {
            long oldTotalMemory = GC.GetTotalMemory(false);
            GC.Collect();
            // Console.WriteLine("ƒƒ‚ƒŠ‘‰Á: {0}", Convert.ToString(oldTotalMemory - GC.GetTotalMemory(true)));
            timer.Reset();
        }

        private void WriteLog(string msg)
        {
            if (log != null)
            {
                log.WriteLine(DateTime.Now.ToString("G") + " " + msg);
            }
        }
    }
}
