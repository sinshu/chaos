using System;

namespace MiswGame2007
{
    public class Map
    {
        private const string BLOCK_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private const int BLOCK_ENEMYS = -666;

        GameScene game;
        int numRows, numCols;
        int[,] blocks;

        public Map(GameScene game, int numRows, int numCols)
        {
            this.game = game;
            this.numRows = numRows;
            this.numCols = numCols;
            blocks = new int[numRows, numCols];
            Random random = new Random();
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    blocks[row, col] = 1;
                }
            }
        }

        public Map(GameScene game, int numRows, int numCols, string[] data)
        {
            this.game = game;
            this.numRows = numRows;
            this.numCols = numCols;
            blocks = new int[numRows, numCols];
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    int index = BLOCK_CHARACTERS.IndexOf(data[row][col]);
                    if (index == -1)
                    {
                        if (data[row][col] == '!')
                        {
                            blocks[row, col] = BLOCK_ENEMYS;
                        }
                        else
                        {
                            blocks[row, col] = 0;
                        }
                    }
                    else
                    {
                        blocks[row, col] = index;
                    }
                }
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            int topRow = game.IntCameraY / Settings.BLOCK_WDITH;
            int bottomRow = (game.IntCameraY + Settings.SCREEN_HEIGHT) / Settings.BLOCK_WDITH;
            int leftCol = game.IntCameraX / Settings.BLOCK_WDITH;
            int rightCol = (game.IntCameraX + Settings.SCREEN_WIDTH) / Settings.BLOCK_WDITH;

            for (int row = topRow; row <= bottomRow; row++)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    int block = this[row, col];
                    if (block > 0)
                    {
                        int drawX = col * Settings.BLOCK_WDITH - game.IntCameraX;
                        int drawY = row * Settings.BLOCK_WDITH - game.IntCameraY;
                        int textureRow = block / 8;
                        int textureCol = block % 8;
                        if (block != 16 && block != 24)
                        {
                            graphics.DrawImage(GameImage.Block, Settings.BLOCK_WDITH, Settings.BLOCK_WDITH, textureRow, textureCol, drawX, drawY);
                        }
                        else
                        {
                            if (block == 16)
                            {
                                graphics.DrawImageAlpha(GameImage.Block, Settings.BLOCK_WDITH, Settings.BLOCK_WDITH, textureRow, textureCol, drawX, drawY, 128);
                            }
                            else
                            {
                                graphics.DrawImageAdd(GameImage.Block, Settings.BLOCK_WDITH, Settings.BLOCK_WDITH, textureRow, textureCol, drawX, drawY, 64);
                            }
                        }
                    }
                }
            }
        }

        public void Draw(GraphicsDevice graphics, int r, int g, int b)
        {
            int topRow = game.IntCameraY / Settings.BLOCK_WDITH;
            int bottomRow = (game.IntCameraY + Settings.SCREEN_HEIGHT) / Settings.BLOCK_WDITH;
            int leftCol = game.IntCameraX / Settings.BLOCK_WDITH;
            int rightCol = (game.IntCameraX + Settings.SCREEN_WIDTH) / Settings.BLOCK_WDITH;

            for (int row = topRow; row <= bottomRow; row++)
            {
                for (int col = leftCol; col <= rightCol; col++)
                {
                    int block = this[row, col];
                    if (block > 0)
                    {
                        int drawX = col * Settings.BLOCK_WDITH - game.IntCameraX;
                        int drawY = row * Settings.BLOCK_WDITH - game.IntCameraY;
                        int textureRow = block / 8;
                        int textureCol = block % 8;
                        if (block != 16)
                        {
                            graphics.DrawImage(GameImage.Block, Settings.BLOCK_WDITH, Settings.BLOCK_WDITH, textureRow, textureCol, drawX, drawY, r, g, b);
                        }
                        else
                        {
                            graphics.DrawImageAlpha(GameImage.Block, Settings.BLOCK_WDITH, Settings.BLOCK_WDITH, textureRow, textureCol, drawX, drawY, 128, r, g, b);
                        }
                    }
                }
            }
        }

        public bool IsObstacle(int row, int col)
        {
            if (this[row, col] > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsObstacleForEnemy(int row, int col, Enemy enemy)
        {
            if (!enemy.IgnoreEnemyBlock)
            {
                if (this[row, col] != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return IsObstacle(row, col);
            }
        }

        public int this[int row, int col]
        {
            get
            {
                if (row < 0 || numRows <= row || col < 0 || numCols <= col)
                {
                    return 0;
                }
                else
                {
                    return blocks[row, col];
                }
            }

            set
            {
                if (row < 0 || numRows <= row || col < 0 || numCols <= col)
                {
                    return;
                }
                else
                {
                    blocks[row, col] = value;
                }
            }
        }

        public int Width
        {
            get
            {
                return Settings.BLOCK_WDITH * numCols;
            }
        }

        public int Height
        {
            get
            {
                return Settings.BLOCK_WDITH * numRows;
            }
        }
    }
}
