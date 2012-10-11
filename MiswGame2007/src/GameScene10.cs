using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene10 : GameScene
    {
        private static int[] HANABI_COLOR_INDEX = {0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 3, 4, 4, 4, 4, 4, 4, 4, 5};

        private BossHouse boss;

        public GameScene10(StageData data)
            : base(data)
        {
            Init();
        }

        public GameScene10(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            Init();
        }

        private void Init()
        {
            boss = new BossHouse(this, 11, 23);
            Enemies.AddThing(boss);
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                StopMusic();
            }
            else if (Ticks == 60)
            {
                PlayMusic(GameMusic.Boss1);
            }

            base.Tick(input);
            if (!boss.Removed)
            {
                if (boss.BossHealth > 1000)
                {
                    if (Ticks % 30 == 0)
                    {
                        AddBackgroundParticle(new Hanabi(this, new Vector(1024 * Random.NextDouble(), 256 * Random.NextDouble()), Random.Next(0, 4) == 0, HANABI_COLOR_INDEX[Random.Next(0, HANABI_COLOR_INDEX.Length)]));
                    }
                }
                else
                {
                    if (Ticks % 10 == 0)
                    {
                        AddBackgroundParticle(new Hanabi(this, new Vector(1024 * Random.NextDouble(), 256 * Random.NextDouble()), Random.Next(0, 4) == 0, HANABI_COLOR_INDEX[Random.Next(0, HANABI_COLOR_INDEX.Length)]));
                    }
                }
            }
            else
            {
                if (Ticks % 120 == 0)
                {
                    AddBackgroundParticle(new Hanabi(this, new Vector(1024 * Random.NextDouble(), 256 * Random.NextDouble()), Random.Next(0, 4) == 0, HANABI_COLOR_INDEX[Random.Next(0, HANABI_COLOR_INDEX.Length)]));
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background2, 1024, 512, IntBackgroundX, IntBackgroundY, 64, 64, 64);
        }

        public override void DrawMap(GraphicsDevice graphics)
        {
            Map.Draw(graphics, 128, 128, 128);
        }
    }
}
