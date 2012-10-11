using System;
using System.IO;
using Yanesdk.Ytl;
using Yanesdk.System;

namespace MiswGame2007
{
    public class Settings
    {
        public const int SCREEN_WIDTH = 640;
        public const int SCREEN_HEIGHT = 480;
        public const int BLOCK_WDITH = 32;

        public const string RESOURCE_PATH = "chaos";

        public static string[] UNLOCK_KEYWORD =
            {
                "yin",
                "asdf",
                "qwer",
                "shana",
                "hoge",
                "dtb",
                "test",
                "btw",
                "kiko",
                "dotnet",
                "basic",
                "yanesdk",
                "sharp",
                "opengl",
                "doom",
                "lisp",
                "sdl",
                "sam",
                "god",
                "quake",
                "java",
                "opera",
                "dummy",
                "csharp",
                "misw"
            };

        private bool fullscreen;
        private int startStageIndex;
        private int attackButton;
        private int jumpButton;
        private int startButton;
        private int numUnlockedStages;
        private bool arcade;

        private bool saveStartStage;

        public Settings(string path)
        {
            fullscreen = false;
            startStageIndex = 0;
            attackButton = 0;
            jumpButton = 1;
            startButton = 2;
            numUnlockedStages = 0;
            arcade = false;
            saveStartStage = false;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(path);
                while (!reader.EndOfStream)
                {
                    string[] data = reader.ReadLine().Split(' ');
                    try
                    {
                        switch (data[0])
                        {
                            case "fullscreen":
                                fullscreen = data[1] == "1";
                                break;
                            case "startstage":
                                startStageIndex = int.Parse(data[1]) - 1;
                                saveStartStage = true;
                                break;
                            case "attackbutton":
                                {
                                    int button = int.Parse(data[1]) - 1;
                                    if (0 <= button)
                                    {
                                        attackButton = button;
                                    }
                                }
                                break;
                            case "jumpbutton":
                                {
                                    int button = int.Parse(data[1]) - 1;
                                    if (0 <= button)
                                    {
                                        jumpButton = button;
                                    }
                                }
                                break;
                            case "startbutton":
                                {
                                    int button = int.Parse(data[1]) - 1;
                                    if (0 <= button)
                                    {
                                        startButton = button;
                                    }
                                }
                                break;
                            case "unlock":
                                {
                                    for (int i = 0; i < UNLOCK_KEYWORD.Length; i++)
                                    {
                                        if (data[1] == UNLOCK_KEYWORD[i])
                                        {
                                            numUnlockedStages = (i + 1);
                                            break;
                                        }
                                    }
                                }
                                break;
                            case "arcade":
                                arcade = data[1] == "1";
                                break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                fullscreen = System.Windows.Forms.MessageBox.Show("フルスクリーンで起動しますか？", "確認", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            // Console.WriteLine("Settings : " + (fullscreen ? "true" : "false") + ", " + startStageIndex + ", " + attackButton + ", " + jumpButton);
        }

        public void Save(string path)
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path);
                writer.WriteLine("fullscreen " + (fullscreen ? "1" : "0"));
                if (saveStartStage) writer.WriteLine("startstage " + (startStageIndex + 1));
                writer.WriteLine("attackbutton " + (attackButton + 1));
                writer.WriteLine("jumpbutton " + (jumpButton + 1));
                writer.WriteLine("startbutton " + (startButton + 1));
                if (numUnlockedStages > 0)
                {
                    writer.WriteLine("unlock " + UNLOCK_KEYWORD[numUnlockedStages - 1]);
                }
                writer.WriteLine("arcade " + (arcade ? "1" : "0"));
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public bool Fullscreen
        {
            get
            {
                return fullscreen;
            }
        }

        public int StartStageIndex
        {
            get
            {
                return startStageIndex;
            }
        }

        public int AttackButton
        {
            get
            {
                return attackButton;
            }
        }

        public int JumpButton
        {
            get
            {
                return jumpButton;
            }
        }

        public int StartButton
        {
            get
            {
                return startButton;
            }
        }

        public int NumUnlockedStages
        {
            get
            {
                return numUnlockedStages;
            }

            set
            {
                numUnlockedStages = value;
            }
        }

        public bool ArcadeMode
        {
            get
            {
                return arcade;
            }
        }
    }
}
