using System;
using Yanesdk.Ytl;
using Yanesdk.Draw;

namespace MiswGame2007
{
    public class GraphicsDevice : IDisposable
    {
        private enum DrawMode
        {
            Unknown = 1,
            Alpha,
            Add
        }

        private static string[] TEXTURE_PATH = {
            "images/test.png",
            "images/block.png",
            "images/player.png",
            "images/pbullet.png",
            "images/pbexp.png",
            "images/back1.png",
            "images/back2.png",
            "images/back3.png",
            "images/back4.png",
            "images/back5.png",
            "images/item.png",
            "images/hud.png",
            "images/bigexp.png",
            "images/house.png",
            "images/baaka.png",
            "images/ebullet.png",
            "images/babo.png",
            "images/kyoro.png",
            "images/nurunuru.png",
            "images/byaa.png",
            "images/debris.png",
            "images/itemexp.png",
            "images/aurora.png",
            "images/norio.png",
            "images/hboss.png",
            "images/title.png",
            "images/ienemy.png",
            "images/hanabi1.png",
            "images/hanabi2.png",
            "images/mrboss1.png",
            "images/mrboss2.png",
            "images/mrboss3.png",
            "images/mr.png",
            "images/emachine.png",
            "images/oyaji.png",
            "images/robot.png",
            "images/thunder.png",
            "images/msg.png",
            "images/number.png",
            "images/father.png",
            "images/staff.png",
            "images/skater.png",
            "images/rain.png",
            "images/black.png",
            "images/worm.png",
            "images/starman.png",
            "images/penguin.png",
            "images/mafia.png"
        };

        private SDLWindow window;
        private IScreen screen;
        private GlTexture[] textures;
        private DrawMode currentDrawMode;

        public GraphicsDevice(SDLWindow window)
        {
            this.window = window;
            screen = window.Screen;

            BeginDraw();
            FillScreen(0, 0, 0, 255);
            EndDraw();

            LoadTextures();
            currentDrawMode = DrawMode.Unknown;
        }

        public void Dispose()
        {
        }

        private void LoadTextures()
        {
            textures = new GlTexture[TEXTURE_PATH.Length];
            screen.Select();
            for (int i = 1; i < TEXTURE_PATH.Length; i++)
            {
                textures[i] = LoadTextureByPath(Settings.RESOURCE_PATH + "/" + TEXTURE_PATH[i]);
            }
            screen.Unselect();
        }

        private GlTexture LoadTextureByPath(string path)
        {
            GlTexture texture = new GlTexture();
            texture.LocalOption.Smooth = false;
            YanesdkResult result = texture.Load(path);
            if (result == YanesdkResult.NoError)
            {
                return texture;
            }
            else
            {
                throw new Exception("‰æ‘œu" + path + "v‚ð“Ç‚Ýž‚ß‚Ü‚¹‚ñ„ƒ");
            }
        }

        public void BeginDraw()
        {
            screen.Select();
            screen.Blend = true;
            currentDrawMode = DrawMode.Unknown;
        }

        public void EndDraw()
        {
            screen.Update();
        }

        public void FillScreen(int r, int g, int b, int a)
        {
            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }
            screen.SetColor(r, g, b, a);
            screen.DrawPolygon(0, 0, Settings.SCREEN_WIDTH, 0, Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT, 0, Settings.SCREEN_HEIGHT);
        }

        public void DrawRect(int x, int y, int width, int height, int r, int g, int b, int a)
        {
            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }
            screen.SetColor(r, g, b, a);
            screen.DrawPolygon(x, y, x + width, y, x + width, y + height, x, y + height);
        }

        public void DrawImage(GameImage image, int width, int height, int x, int y)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(255, 255, 255, 255);
            screen.Blt(textures[(int)image], x, y);
        }

        public void DrawImage(GameImage image, int width, int height, int x, int y, int r, int g, int b)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(r, g, b, 255);
            screen.Blt(textures[(int)image], x, y);
        }
        
        public void DrawImage(GameImage image, int width, int height, int row, int col, int x, int y)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(255, 255, 255, 255);
            screen.Blt(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)));
        }

        public void DrawImage(GameImage image, int width, int height, int row, int col, int x, int y, int r, int g, int b)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(r, g, b, 255);
            screen.Blt(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageFix(GameImage image, int width, int height, int row, int col, int x, int y, Thing thing)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }
            screen.SetColor(255, thing.DamageColorGreen, thing.DamageColorBlue, 255);
            screen.Blt(textures[(int)image], x, y + 1, new Rect(width * col, height * row + 1, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageFixFlip(GameImage image, int width, int height, int row, int col, int x, int y, Thing thing)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }
            screen.SetColor(255, thing.DamageColorGreen, thing.DamageColorBlue, 255);
            screen.Blt(textures[(int)image], x, y + 1, new Rect(width * (col + 1), height * row + 1, width * col, height * (row + 1)));
        }

        public void DrawImageAlphaFix(GameImage image, int width, int height, int row, int col, int x, int y, Thing thing, int alpha)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }
            screen.SetColor(255, thing.DamageColorGreen, thing.DamageColorBlue, alpha);
            screen.Blt(textures[(int)image], x, y + 1, new Rect(width * col, height * row + 1, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageAlphaFixFlip(GameImage image, int width, int height, int row, int col, int x, int y, Thing thing, int alpha)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }
            screen.SetColor(255, thing.DamageColorGreen, thing.DamageColorBlue, alpha);
            screen.Blt(textures[(int)image], x, y + 1, new Rect(width * (col + 1), height * row + 1, width * col, height * (row + 1)));
        }

        public void DrawImageAddFlipRotate90(GameImage image, int width, int height, int row, int col, int x, int y, int flipRotate, int r, int g, int b)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Add)
            {
                screen.BlendAddColorAlpha();
                currentDrawMode = DrawMode.Add;
            }
            screen.SetColor(r, g, b);
            if (flipRotate % 2 == 0)
            {
                screen.BltRotate(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)), flipRotate / 2 * 128, 1.0f, width / 2, height / 2);
            }
            else
            {
                switch (flipRotate / 2)
                {
                    case 0:
                        screen.BltRotate(textures[(int)image], x + width, y, new Rect(width * (col + 1), height * row, width * col, height * (row + 1)), flipRotate / 2 * 128, 1.0f, -width / 2, -height / 2);
                        break;
                    case 1:
                        screen.BltRotate(textures[(int)image], x + 2 * width, y + height, new Rect(width * (col + 1), height * row, width * col, height * (row + 1)), flipRotate / 2 * 128, 1.0f, -width / 2, -height / 2);
                        break;
                    case 2:
                        screen.BltRotate(textures[(int)image], x + width, y + 2 * height, new Rect(width * (col + 1), height * row, width * col, height * (row + 1)), flipRotate / 2 * 128, 1.0f, -width / 2, -height / 2);
                        break;
                    case 3:
                        screen.BltRotate(textures[(int)image], x, y + height, new Rect(width * (col + 1), height * row, width * col, height * (row + 1)), flipRotate / 2 * 128, 1.0f, -width / 2, -height / 2);
                        break;
                }
            }
        }

        public void DrawImage2(GameImage image, int srcX, int srcY, int width, int height, int x, int y)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(255, 255, 255, 255);
            screen.Blt(textures[(int)image], x, y, new Rect(srcX, srcY, srcX + width, srcY + height));
        }

        public void DrawImageAlpha(GameImage image, int width, int height, int row, int col, int x, int y, int alpha)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(255, 255, 255, alpha);
            screen.Blt(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageAlpha(GameImage image, int width, int height, int row, int col, int x, int y, int alpha, int r, int g, int b)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(r, g, b, alpha);
            screen.Blt(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageAdd(GameImage image, int width, int height, int row, int col, int x, int y, int alpha)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Add)
            {
                screen.BlendAddColorAlpha();
                currentDrawMode = DrawMode.Add;
            }

            screen.SetColor(255, 255, 255, alpha);
            screen.Blt(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageAdd(GameImage image, int width, int height, int row, int col, int x, int y, int alpha, int r, int g, int b)
        {
            if (x + width < 0) return;
            if (x - width > Settings.SCREEN_WIDTH) return;
            if (y + height < 0) return;
            if (y - height > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Add)
            {
                screen.BlendAddColorAlpha();
                currentDrawMode = DrawMode.Add;
            }

            screen.SetColor(r, g, b, alpha);
            screen.Blt(textures[(int)image], x, y, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)));
        }

        public void DrawImageRotate(GameImage image, int width, int height, int row, int col, int x, int y, int centerX, int centerY, int angle)
        {
            int range = (int)Math.Ceiling(Math.Sqrt(width * width + height * height));
            if (x + range < 0) return;
            if (x - range > Settings.SCREEN_WIDTH) return;
            if (y + range < 0) return;
            if (y - range > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(255, 255, 255, 255);
            screen.BltRotate(textures[(int)image], x - centerX, y - centerY, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)), (int)Math.Round((double)angle / 360.0 * 512.0), 1, centerX, centerY);
        }

        public void DrawImageRotateAlpha(GameImage image, int width, int height, int row, int col, int x, int y, int centerX, int centerY, int angle, int alpha)
        {
            int range = (int)Math.Ceiling(Math.Sqrt(width * width + height * height));
            if (x + range < 0) return;
            if (x - range > Settings.SCREEN_WIDTH) return;
            if (y + range < 0) return;
            if (y - range > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Alpha)
            {
                screen.BlendSrcAlpha();
                currentDrawMode = DrawMode.Alpha;
            }

            screen.SetColor(255, 255, 255, alpha);
            screen.BltRotate(textures[(int)image], x - centerX, y - centerY, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)), (int)Math.Round((double)angle / 360.0 * 512.0), 1, centerX, centerY);
        }

        public void DrawImageRotateAdd(GameImage image, int width, int height, int row, int col, int x, int y, int centerX, int centerY, int angle, int alpha)
        {
            int range = (int)Math.Ceiling(Math.Sqrt(width * width + height * height));
            if (x + range < 0) return;
            if (x - range > Settings.SCREEN_WIDTH) return;
            if (y + range < 0) return;
            if (y - range > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Add)
            {
                screen.BlendAddColorAlpha();
                currentDrawMode = DrawMode.Add;
            }

            screen.SetColor(255, 255, 255, alpha);
            screen.BltRotate(textures[(int)image], x - centerX, y - centerY, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)), (int)Math.Round((double)angle / 360.0 * 512.0), 1, centerX, centerY);
        }

        public void DrawImageRotateAdd(GameImage image, int width, int height, int row, int col, int x, int y, int centerX, int centerY, int angle, int alpha, int r, int g, int b)
        {
            int range = (int)Math.Ceiling(Math.Sqrt(width * width + height * height));
            if (x + range < 0) return;
            if (x - range > Settings.SCREEN_WIDTH) return;
            if (y + range < 0) return;
            if (y - range > Settings.SCREEN_HEIGHT) return;

            if (currentDrawMode != DrawMode.Add)
            {
                screen.BlendAddColorAlpha();
                currentDrawMode = DrawMode.Add;
            }

            screen.SetColor(r, g, b, alpha);
            screen.BltRotate(textures[(int)image], x - centerX, y - centerY, new Rect(width * col, height * row, width * (col + 1), height * (row + 1)), (int)Math.Round((double)angle / 360.0 * 512.0), 1, centerX, centerY);
        }
    }
}
