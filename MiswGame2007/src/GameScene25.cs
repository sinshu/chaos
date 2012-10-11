using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class GameScene25 : GameScene
    {
        private Oyaji oyaji;

        public GameScene25(StageData data)
            : base(data)
        {
            Init();
        }

        public GameScene25(StageData data, PlayerState playerState)
            : base(data, playerState)
        {
            Init();
        }

        private void Init()
        {
            EggMachine left = new EggMachine(this, 10, 1, EggMachine.Direction.Right);
            EggMachine right = new EggMachine(this, 10, 27, EggMachine.Direction.Left);
            oyaji = new Oyaji(this, 6, 14, left, right);
            Enemies.AddThing(oyaji);
            Enemies.AddThing(left);
            Enemies.AddThing(right);
        }

        public override void Tick(GameInput input)
        {
            if (Ticks == 0)
            {
                StopMusic();
            }
            else if (Ticks == 60)
            {
                PlayMusic(GameMusic.Boss2);
            }

            base.Tick(input);

            if (Items.Count == 0 && Ticks % 180 == 90 && (Player.CurrentWeapon == Player.Weapon.Pistol || (Player.CurrentWeapon == Player.Weapon.Rocket && Player.Ammo <= 25) || (Player.CurrentWeapon == Player.Weapon.Machinegun && Player.Ammo <= 100) || (oyaji.CurrentState == Oyaji.State.Pattern2_5 && Player.CurrentWeapon == Player.Weapon.Rocket)))
            {
                if (oyaji.CurrentState == Oyaji.State.Pattern2_5 || Random.Next(0, 2) == 0)
                {
                    Items.AddThing(new MachinegunItem(this, new Vector(160 + Random.NextDouble() * (Map.Width - 320 - 32), 32), Vector.Zero));
                }
                else
                {
                    Items.AddThing(new RocketItem(this, new Vector(160 + Random.NextDouble() * (Map.Width - 320 - 32), 32), Vector.Zero));
                }
            }
        }

        public override void Draw(GraphicsDevice graphics)
        {
            base.Draw(graphics);
        }

        public override void DrawBackground(GraphicsDevice graphics)
        {
            graphics.DrawImage(GameImage.Background5, 1024, 512, IntBackgroundX, IntBackgroundY);
        }
    }
}
