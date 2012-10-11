using System;

namespace MiswGame2007
{
    public class TitleScene
    {
        public enum State
        {
            None = 1,
            GotoGame,
            StageSelect,
            Exit
        }

        private static int[,] TITLE_MAP = {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0},
            {0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0},
            {0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0},
            {0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 0},
            {0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0},
            {0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };

        private static int[] C_ROW = {2, 2, 2, 2, 3, 4, 5, 6, 6, 6, 6};
        private static int[] C_COL = {1, 2, 3, 4, 1, 1, 1, 1, 2, 3, 4};
        private static int[] H_ROW = {2, 2, 3, 3, 4, 4, 4, 4, 5, 5, 6, 6};
        private static int[] H_COL = {6, 9, 6, 9, 6, 7, 8, 9, 6, 9, 6, 9};
        private static int[] A_ROW = {2, 2, 2, 2, 3, 3, 4, 4, 4, 4, 5, 5, 6, 6};
        private static int[] A_COL = {11, 12, 13, 14, 11, 14, 11, 12, 13, 14, 11, 14, 11, 14};
        private static int[] O_ROW = {2, 2, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 6, 6};
        private static int[] O_COL = {16, 17, 18, 19, 16, 19, 16, 19, 16, 19, 16, 17, 18, 19};
        private static int[] S_ROW = {9, 9, 9, 9, 10, 11, 11, 11, 11, 12, 13, 13, 13, 13};
        private static int[] S_COL = {1, 2, 3, 4, 1, 1, 2, 3, 4, 4, 1, 2, 3, 4};
        private static int[] L_ROW = {9, 10, 11, 12, 13, 13, 13, 13};
        private static int[] L_COL = {6, 6, 6, 6, 6, 7, 8, 9};
        private static int[] U_ROW = {9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 13, 13};
        private static int[] U_COL = {11, 14, 11, 14, 11, 14, 11, 14, 11, 12, 13, 14};
        private static int[] G_ROW = {9, 9, 9, 9, 10, 11, 11, 11, 12, 12, 13, 13, 13, 13};
        private static int[] G_COL = {16, 17, 18, 19, 16, 16, 18, 19, 16, 19, 16, 17, 18, 19};

        private State currentState;
        private bool gotoGame;
        private int gotoGameCount;
        private bool exit;
        private int exitCount;
        private int numTicks;
        private int[,] titleMap;
        private GameSceneForTitleScene game;
        private int currentSelectIndex;
        private bool arcadeMode;

        private AudioDevice audio;

        public TitleScene(int selectIndex, bool arcadeMode)
        {
            currentState = State.None;
            gotoGame = false;
            gotoGameCount = 0;
            exit = false;
            exitCount = 0;
            numTicks = 0;
            titleMap = new int[16, 21];
            for (int row = 0; row < 16; row++)
            {
                for (int col = 0; col < 21; col++)
                {
                    titleMap[row, col] = 0;
                }
            }
            game = new GameSceneForTitleScene(16, 21);
            game.Camera = new Vector(16, 16);
            currentSelectIndex = selectIndex;
            this.arcadeMode = arcadeMode;
            audio = null;
        }

        public void Tick(TitleInput input)
        {
            if (numTicks == 0)
            {
                audio.StopMusic();
            }

            if (input.Exit)
            {
                exit = true;
            }

            numTicks++;

            switch (numTicks)
            {
                case 30:
                    FireC();
                    break;
                case 45:
                    FireH();
                    break;
                case 60:
                    FireA();
                    break;
                case 75:
                    FireO();
                    break;
                case 90:
                    FireS();
                    break;
                case 105:
                    FireL();
                    break;
                case 120:
                    FireU();
                    break;
                case 135:
                    FireG();
                    break;
                case 165:
                    game.Quake(16);
                    game.Flash(128);
                    game.ShowTitle();
                    game.PlaySound(GameSound.Explode);
                    game.PlaySound(GameSound.TitleVoice);
                    break;
                case 240:
                    audio.PlayMusic(GameMusic.Title);
                    break;
            }

            game.Tick(GameInput.Empty);

            if (!gotoGame && !exit)
            {
                if (input.Up)
                {
                    currentSelectIndex--;
                    if (!arcadeMode)
                    {
                        if (currentSelectIndex < 0)
                        {
                            currentSelectIndex += 3;
                        }
                    }
                    else
                    {
                        if (currentSelectIndex < 0)
                        {
                            currentSelectIndex += 2;
                        }
                    }
                }
                else if (input.Down)
                {
                    if (!arcadeMode)
                    {
                        currentSelectIndex = (currentSelectIndex + 1) % 3;
                    }
                    else
                    {
                        currentSelectIndex = (currentSelectIndex + 1) % 2;
                    }
                }

                if (input.Start)
                {
                    if (currentSelectIndex == 0)
                    {
                        gotoGame = true;
                    }
                    else if (currentSelectIndex == 1)
                    {
                        currentState = State.StageSelect;
                    }
                    else if (currentSelectIndex == 2)
                    {
                        exit = true;
                    }
                }
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
            else if (exit)
            {
                if (exitCount < 16)
                {
                    if (exitCount == 0)
                    {
                        audio.StopMusic();
                    }
                    exitCount++;
                }
                else
                {
                    currentState = State.Exit;
                }
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            game.Draw(graphics);

            if (numTicks < 16)
            {
                graphics.FillScreen(0, 0, 0, 255 - numTicks * 16);
            }

            if (numTicks == 165)
            {
                graphics.FillScreen(255, 255, 255, 255);
            }

            if (numTicks > 180)
            {
                for (int i = 0; i < 3; i++)
                {
                    graphics.DrawImageAdd(GameImage.Message, 256, 32, i, 0, (Settings.SCREEN_WIDTH - 256) / 2, Settings.SCREEN_HEIGHT / 2 + 112 + 32 * i, (i <= 1 || !arcadeMode) ? 255 : 32);
                }
                graphics.DrawImage(GameImage.Hud, 32, 32, 0, 4, (Settings.SCREEN_WIDTH - 256) / 2 - 32 + 48, Settings.SCREEN_HEIGHT / 2 + 112 + currentSelectIndex * 32);
                graphics.DrawImage(GameImage.Hud, 32, 32, 0, 0, (Settings.SCREEN_WIDTH - 256) / 2 + 256 - 48, Settings.SCREEN_HEIGHT / 2 + 112 + currentSelectIndex * 32);
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
            
            if (exitCount > 0)
            {
                if (exitCount < 16)
                {
                    graphics.FillScreen(0, 0, 0, 16 * exitCount);
                }
                else
                {
                    graphics.FillScreen(0, 0, 0, 255);
                }
            }
        }

        public void StageSelectExited()
        {
            currentState = State.None;
            gotoGame = false;
            gotoGameCount = 0;
            exit = false;
            exitCount = 0;
        }

        private void FireMap(int row, int col)
        {
            game.Map[row, col] = 9;
            Vector pos = new Vector(Settings.BLOCK_WDITH * (col + 0.5), Settings.BLOCK_WDITH * (row + 0.5));
            game.AddParticle(new BigExplosion(game, pos, Vector.Zero));
            game.AddParticle(new BlackDebris(game, pos + new Vector(-8, -8), new Vector(-4 - 4 * game.Random.NextDouble(), -4 - 4 * game.Random.NextDouble()), game.Random.Next(0, 4)));
            game.AddParticle(new BlackDebris(game, pos + new Vector(8, -8), new Vector(4 + 4 * game.Random.NextDouble(), -4 - 4 * game.Random.NextDouble()), game.Random.Next(0, 4)));
            game.AddParticle(new BlackDebris(game, pos + new Vector(-8, 8), new Vector(-4 - 4 * game.Random.NextDouble(), 4 + 4 * game.Random.NextDouble()), game.Random.Next(0, 4)));
            game.AddParticle(new BlackDebris(game, pos + new Vector(8, 8), new Vector(4 + 4 * game.Random.NextDouble(), 4 + 4 * game.Random.NextDouble()), game.Random.Next(0, 4)));
            game.Flash(16);
            game.Quake(2);
        }

        private void FireC()
        {
            for (int i = 0; i < C_ROW.Length; i++)
            {
                FireMap(C_ROW[i], C_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireH()
        {
            for (int i = 0; i < H_ROW.Length; i++)
            {
                FireMap(H_ROW[i], H_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireA()
        {
            for (int i = 0; i < A_ROW.Length; i++)
            {
                FireMap(A_ROW[i], A_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireO()
        {
            for (int i = 0; i < O_ROW.Length; i++)
            {
                FireMap(O_ROW[i], O_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireS()
        {
            for (int i = 0; i < S_ROW.Length; i++)
            {
                FireMap(S_ROW[i], S_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireL()
        {
            for (int i = 0; i < L_ROW.Length; i++)
            {
                FireMap(L_ROW[i], L_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireU()
        {
            for (int i = 0; i < U_ROW.Length; i++)
            {
                FireMap(U_ROW[i], U_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
        }

        private void FireG()
        {
            for (int i = 0; i < G_ROW.Length; i++)
            {
                FireMap(G_ROW[i], G_COL[i]);
            }
            game.PlaySound(GameSound.Shotgun);
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
                game.AudioDevice = value;
            }
        }
    }
}
