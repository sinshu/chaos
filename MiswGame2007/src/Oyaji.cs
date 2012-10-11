using System;

namespace MiswGame2007
{
    public class Oyaji : Enemy
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum State
        {
            Wait1 = 1,
            Pattern1,
            Wait2,
            Pattern2,
            Pattern2_5,
            Wait3,
            Pattern3,
            Wait4,
            Pattern4,
            Wait5,
            Pattern5
        }

        private const int INIT_HEALTH = 7000;

        private static Vector SIZE = new Vector(80, 96);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(24, 16), SIZE);

        private EggMachine leftEggMachine;
        private EggMachine rightEggMachine;
        private Byaa byaa;
        private Nurunuru nuru;
        private Norio norio1;
        private Norio norio2;

        private State currentState;
        private int stateCount;

        private int animation;

        private int numDeathTicks;

        public Oyaji(GameScene game, int row, int col, EggMachine left, EggMachine right)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            leftEggMachine = left;
            rightEggMachine = right;
            byaa = null;
            nuru = null;
            norio1 = null;
            norio2 = null;
            currentState = State.Wait1;
            stateCount = 0;
            animation = 0;
            numDeathTicks = 0;
        }

        public override void Tick(GameInput input)
        {
            if (health <= 0)
            {
                DeathTick(input);
                base.Tick(input);
                return;
            }

            if (4000 < health && health <= 5500 && currentState != State.Wait2 && currentState != State.Pattern2)
            {
                currentState = State.Wait2;
                stateCount = 0;
            }
            else if (2500 < health && health <= 4000 && currentState != State.Pattern2_5)
            {
                currentState = State.Pattern2_5;
                stateCount = 0;
            }
            else if (1000 < health && health <= 2500 && currentState != State.Wait3 && currentState != State.Pattern3)
            {
                currentState = State.Wait3;
                stateCount = 0;
            }
            else if (500 < health && health <= 1000 && currentState != State.Wait4 && currentState != State.Pattern4)
            {
                currentState = State.Wait4;
                stateCount = 0;
            }
            else if (0 < health && health <= 500 && currentState != State.Wait5 && currentState != State.Pattern5)
            {
                currentState = State.Wait5;
                stateCount = 0;
            }

            switch (currentState)
            {
                case State.Wait1:
                    if (stateCount < 120)
                    {
                        stateCount++;
                    }
                    else
                    {
                        currentState = State.Pattern1;
                        stateCount = 0;
                    }
                    break;
                case State.Pattern1:
                    if (stateCount < 64)
                    {
                        if (stateCount == 0)
                        {
                            game.PlaySound(GameSound.Shuhu);
                        }
                        animation = stateCount / 4 % 2;
                        leftEggMachine.SetAnimation(stateCount / 8);
                        rightEggMachine.SetAnimation(stateCount / 8);
                    }
                    else
                    {
                        if (stateCount % 8 == 0)
                        {
                            leftEggMachine.FireBullet();
                            rightEggMachine.FireBullet();
                        }
                        animation = 0;
                    }
                    stateCount = (stateCount + 1) % 512;
                    break;
                case State.Wait2:
                    if (stateCount < 120)
                    {
                        if (stateCount == 0)
                        {
                            leftEggMachine.BeginIdle();
                            rightEggMachine.BeginIdle();
                        }
                        stateCount++;
                    }
                    else
                    {
                        currentState = State.Pattern2;
                        stateCount = 0;
                    }
                    break;
                case State.Pattern2:
                    if (stateCount < 64)
                    {
                        if (stateCount == 0)
                        {
                            game.PlaySound(GameSound.Duely);
                        }
                        animation = stateCount / 4 % 2;
                    }
                    else
                    {
                        if (stateCount % 12 == 0)
                        {
                            game.AddParticle(new OyajiThunder(game, 176 + 672 * game.Random.NextDouble()));
                        }
                        animation = 0;
                    }
                    stateCount = (stateCount + 1) % 256;
                    break;
                case State.Pattern2_5:
                    if (stateCount < 64)
                    {
                        if (stateCount == 0)
                        {
                            game.PlaySound(GameSound.Shuhu);
                        }
                        animation = stateCount / 4 % 2;
                        leftEggMachine.SetAnimation(stateCount / 8);
                        rightEggMachine.SetAnimation(stateCount / 8);
                    }
                    else
                    {
                        if (stateCount % 32 == 8)
                        {
                            leftEggMachine.FireWormEgg();
                        }
                        if (stateCount % 32 == 24)
                        {
                            rightEggMachine.FireWormEgg();
                        }
                        animation = 0;
                    }
                    stateCount = (stateCount + 1) % 512;
                    break;
                case State.Wait3:
                    foreach (Thing enemy in game.Enemies)
                    {
                        if (enemy is Worm)
                        {
                            enemy.Die();
                        }
                    }
                    if (stateCount < 64)
                    {
                        leftEggMachine.SetAnimation(stateCount / 8);
                        rightEggMachine.SetAnimation(stateCount / 8);
                    }
                    if (stateCount < 120)
                    {
                        stateCount++;
                    }
                    else
                    {
                        byaa = new Byaa(game, new Vector(32, 32), Byaa.Direction.Right, true);
                        nuru = new Nurunuru(game, new Vector(game.Map.Width - 128 - 32, 32), Nurunuru.Direction.Left, true);
                        game.AddEnemy(byaa);
                        game.AddEnemy(nuru);
                        currentState = State.Pattern3;
                        stateCount = 0;
                    }
                    break;
                case State.Pattern3:
                    foreach (Thing enemy in game.Enemies)
                    {
                        if (enemy is Worm)
                        {
                            enemy.Die();
                        }
                    }
                    if (stateCount < 64)
                    {
                        if (stateCount == 0)
                        {
                            game.PlaySound(GameSound.Horay);
                        }
                        animation = stateCount / 4 % 2;
                    }
                    else
                    {
                        animation = 0;
                    }
                    if (stateCount % 64 == 16)
                    {
                        leftEggMachine.FireBullet();
                    }
                    if (stateCount % 64 == 48)
                    {
                        rightEggMachine.FireBullet();
                    }
                    /*
                    if (stateCount % 128 == 64)
                    {
                        double x = game.Player.Center.X + 128 * game.Random.NextDouble() - 64;
                        if (x < 176) x = 176;
                        if (x > 848) x = 848;
                        game.AddParticle(new OyajiThunder(game, x));
                    }
                    */
                    stateCount = (stateCount + 1) % 512;
                    break;
                case State.Wait4:
                    if (stateCount == 0)
                    {
                        if (byaa != null)
                        {
                            byaa.Die();
                        }
                        if (nuru != null)
                        {
                            nuru.Die();
                        }
                    }
                    if (stateCount < 32)
                    {
                        leftEggMachine.SetAnimation(stateCount / 4);
                        rightEggMachine.SetAnimation(stateCount / 4);
                    }
                    if (stateCount < 60)
                    {
                        stateCount++;
                    }
                    else
                    {
                        norio1 = new Norio(game, 1, 1, Norio.Direction.Right, true);
                        norio2 = new Norio(game, 1, (game.Map.Width / Settings.BLOCK_WDITH) - 5, Norio.Direction.Left, true);
                        game.AddEnemy(norio1);
                        game.AddEnemy(norio2);
                        currentState = State.Pattern4;
                        stateCount = 0;
                    }
                    break;
                case State.Pattern4:
                    if (stateCount % 64 == 16)
                    {
                        leftEggMachine.FireBullet();
                    }
                    if (stateCount % 64 == 48)
                    {
                        rightEggMachine.FireBullet();
                    }
                    stateCount = (stateCount + 1) % 64;
                    break;
                case State.Wait5:
                    if (stateCount < 60)
                    {
                        if (stateCount == 0)
                        {
                            if (norio1 != null)
                            {
                                norio1.Die();
                            }
                            if (norio2 != null)
                            {
                                norio2.Die();
                            }
                            leftEggMachine.BeginIdle();
                            rightEggMachine.BeginIdle();
                        }
                        stateCount++;
                    }
                    else
                    {
                        currentState = State.Pattern5;
                        stateCount = 0;
                    }
                    break;
                case State.Pattern5:
                    if (stateCount < 64)
                    {
                        leftEggMachine.SetAnimation(stateCount / 8);
                        rightEggMachine.SetAnimation(stateCount / 8);
                    }
                    else
                    {
                        if (stateCount % 8 == 0)
                        {
                            leftEggMachine.FireBullet();
                            rightEggMachine.FireBullet();
                        }
                        if (stateCount % 16 == 0)
                        {
                            game.AddParticle(new OyajiThunder(game, 176 + 672 * game.Random.NextDouble()));
                        }
                    }
                    stateCount = (stateCount + 1) % 512;
                    break;
            }

            velocity.X = 0.0078125 * (game.Player.Center.X - Center.X);
            if (Math.Abs(velocity.X) > 16)
            {
                velocity.X = Math.Sign(velocity.X) * 16;
            }
            MoveBy(input, velocity);

            base.Tick(input);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            graphics.DrawImageFix(GameImage.Oyaji, 128, 128, 0, animation, drawX, drawY, this);
        }

        public void DeathTick(GameInput input)
        {
            if (numDeathTicks < 256)
            {
                if (numDeathTicks == 0)
                {
                    game.EnemyBullets.BreakAll();
                    game.StopMusic();
                }
                if (numDeathTicks % 8 == 0)
                {
                    DoSomeExplode();
                    leftEggMachine.DoSomeExplode();
                    rightEggMachine.DoSomeExplode();
                    game.Quake(4);
                    game.PlaySound(GameSound.Shotgun);
                    damageFlash = 256;
                }
                if (numDeathTicks % 16 == 0)
                {
                    game.Flash(16);
                }
                numDeathTicks++;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X - 48, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top)), Vector.Zero));
                }
                for (int i = 0; i < 5; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X, Top + 0.25 * i * (Bottom - Top)), Vector.Zero));
                }
                for (int i = 0; i < 4; i++)
                {
                    game.AddParticle(new BigExplosion(game, new Vector(Center.X + 48, Top + 0.125 * (Bottom - Top) + 0.25 * i * (Bottom - Top)), Vector.Zero));
                }
                leftEggMachine.Die();
                rightEggMachine.Die();
                game.Quake(16);
                game.Flash(128);
                game.PlaySound(GameSound.Explode);
                SpreadDebris(64);
                Remove();
            }
        }

        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }
    }
}
