using System;

namespace MiswGame2007
{
    public class Player : Thing
    {
        public enum Direction
        {
            Left = 1,
            Right
        }

        public enum ArmDirection
        {
            Forward = 1,
            Upper,
            Lower
        }

        public enum LandState
        {
            InAir = 1,
            OnGround
        }

        public enum Weapon
        {
            Pistol = 1,
            Machinegun,
            Shotgun,
            Rocket,
            Flamethrower
        }

        public enum EnemyDamagePart
        {
            Left = 1,
            Right,
            Top,
            Bottom
        }

        private const int INIT_HEALTH = 100;

        private const double ACCELERATION_IN_AIR = 1;
        private const double ACCELERATION_ON_GROUND = 2;
        private const double ACCELERATION_FALLING = 0.5;
        private const double MAX_MOVING_SPEED = 4;
        private const double MAX_FALLING_SPEED = 16;
        private const double JUMP_SPEED = 8;
        private const int INIT_JUMP_COUNT = 10;
        private const int INIT_FIRE_COUNT = 8;

        private const int NUM_ANIMATIONS = 16;

        private static Vector SIZE = new Vector(20, 36);
        private static Rectangle RECTANGLE = new Rectangle(new Vector(6, 28), SIZE);

        private Direction direction;
        private ArmDirection arm;
        private LandState landState;
        private Weapon weapon;
        private int ammo;
        private int jumpCount;
        private int fireCount;
        private int enemyDamageCount;
        private bool freeze;
        private int animation;
        private int armAnimation;
        private bool visible;
        private int drawHealth;

        private Vector focus;

        private int ammoHudColorCount;
        private int ammoNumColorCount;

        public Player(GameScene game, int row, int col, Direction direction)
            : base(game, RECTANGLE, new Vector(col * Settings.BLOCK_WDITH, row * Settings.BLOCK_WDITH), Vector.Zero, INIT_HEALTH)
        {
            this.direction = direction;
            arm = ArmDirection.Forward;
            landState = LandState.InAir;
            weapon = Weapon.Pistol;
            ammo = 0;
            jumpCount = -1;
            fireCount = -1;
            animation = 0;
            enemyDamageCount = 0;
            freeze = false;
            armAnimation = 0;
            visible = true;
            drawHealth = health;
            focus = Vector.Zero;
            ammoHudColorCount = 0;
            ammoNumColorCount = 0;
        }

        public override void Draw(GraphicsDevice graphics)
        {
            if (!visible) return;
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            int textureRow = 0;
            int textureCol = animation / 2;
            if (direction == Direction.Left)
            {
                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow, textureCol, drawX, drawY, this);
                if (fireCount == -1)
                {
                    graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow, textureCol + 8, drawX, drawY, this);
                }
                else
                {
                    if (armAnimation < 8)
                    {
                        switch (arm)
                        {
                            case ArmDirection.Forward:
                                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow + 1, armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Upper:
                                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow + 2, armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Lower:
                                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow + 3, armAnimation + 8, drawX, drawY, this);
                                break;
                        }
                    }
                    else
                    {
                        switch (arm)
                        {
                            case ArmDirection.Forward:
                                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow + 1, 15 - armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Upper:
                                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow + 2, 15 - armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Lower:
                                graphics.DrawImageFix(GameImage.Player, 32, 64, textureRow + 3, 15 - armAnimation + 8, drawX, drawY, this);
                                break;
                        }
                    }
                }
            }
            else
            {
                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow, textureCol, drawX, drawY, this);
                if (fireCount == -1)
                {
                    graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow, textureCol + 8, drawX, drawY, this);
                }
                else
                {
                    if (armAnimation < 8)
                    {
                        switch (arm)
                        {
                            case ArmDirection.Forward:
                                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow + 1, armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Upper:
                                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow + 2, armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Lower:
                                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow + 3, armAnimation + 8, drawX, drawY, this);
                                break;
                        }
                    }
                    else
                    {
                        switch (arm)
                        {
                            case ArmDirection.Forward:
                                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow + 1, 15 - armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Upper:
                                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow + 2, 15 - armAnimation + 8, drawX, drawY, this);
                                break;
                            case ArmDirection.Lower:
                                graphics.DrawImageFixFlip(GameImage.Player, 32, 64, textureRow + 3, 15 - armAnimation + 8, drawX, drawY, this);
                                break;
                        }
                    }
                }
            }
        }

        public override void Tick(GameInput input)
        {
            if (ammoHudColorCount > 0)
            {
                ammoHudColorCount--;
            }
            if (ammoNumColorCount > 0)
            {
                ammoNumColorCount--;
            }

            if (!freeze && visible)
            {

                #region 左右の動き

                if (input.Left == input.Right)
                {
                    if (landState == LandState.InAir)
                    {
                        if (Math.Abs(velocity.X) > ACCELERATION_IN_AIR / 8)
                        {
                            velocity.X -= Math.Sign(velocity.X) * ACCELERATION_IN_AIR / 8;
                        }
                        else
                        {
                            velocity.X = 0;
                        }
                    }
                    else if (landState == LandState.OnGround)
                    {
                        if (Math.Abs(velocity.X) > ACCELERATION_ON_GROUND)
                        {
                            velocity.X -= Math.Sign(velocity.X) * ACCELERATION_ON_GROUND;
                        }
                        else
                        {
                            velocity.X = 0;
                        }
                    }
                }
                else if (input.Left && !input.Right)
                {
                    direction = Direction.Left;
                    if (landState == LandState.InAir)
                    {
                        if (velocity.X > -MAX_MOVING_SPEED)
                        {
                            velocity.X -= ACCELERATION_IN_AIR;
                        }
                    }
                    else if (landState == LandState.OnGround)
                    {
                        if (velocity.X > -MAX_MOVING_SPEED)
                        {
                            velocity.X -= ACCELERATION_ON_GROUND;
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                }
                else if (input.Right && !input.Left)
                {
                    direction = Direction.Right;
                    if (landState == LandState.InAir)
                    {
                        if (velocity.X < MAX_MOVING_SPEED)
                        {
                            velocity.X += ACCELERATION_IN_AIR;
                        }
                    }
                    else if (landState == LandState.OnGround)
                    {
                        if (velocity.X < MAX_MOVING_SPEED)
                        {
                            velocity.X += ACCELERATION_ON_GROUND;
                        }
                        animation = (animation + 1) % NUM_ANIMATIONS;
                    }
                }

                if (Math.Abs(velocity.X) > MAX_MOVING_SPEED)
                {
                    if (Math.Abs(velocity.X) - 1 < MAX_MOVING_SPEED)
                    {
                        velocity.X = Math.Sign(velocity.X) * MAX_MOVING_SPEED;
                    }
                    else
                    {
                        velocity.X -= Math.Sign(velocity.X);
                    }
                }

                MoveBy_Horizontal(input, velocity.X);

                #endregion

                #region 上下の動き

                velocity.Y += ACCELERATION_FALLING;
                if (velocity.Y > MAX_FALLING_SPEED)
                {
                    velocity.Y = MAX_FALLING_SPEED;
                }
                if (landState == LandState.OnGround)
                {
                    if (input.Jump)
                    {
                        if (jumpCount != -1)
                        {
                            landState = LandState.InAir;
                            jumpCount = INIT_JUMP_COUNT;
                            animation = (animation + (NUM_ANIMATIONS / 4)) % NUM_ANIMATIONS;
                        }
                    }
                    else
                    {
                        jumpCount = 0;
                    }
                }
                if (landState == LandState.InAir)
                {
                    if (jumpCount > 0)
                    {
                        if (input.Jump)
                        {
                            velocity.Y = -JUMP_SPEED;
                            jumpCount--;
                        }
                        else
                        {
                            jumpCount = -1;
                        }
                    }
                    else
                    {
                        if (velocity.Y < 0)
                        {
                            jumpCount = -1;
                        }
                        else if (!input.Jump)
                        {
                            jumpCount = 0;
                        }
                    }
                }
                landState = LandState.InAir;
                MoveBy_Vertical(input, velocity.Y);

                #endregion

                #region 腕

                if (input.Up == input.Down)
                {
                    arm = ArmDirection.Forward;
                }
                else if (input.Up)
                {
                    arm = ArmDirection.Upper;
                }
                else if (input.Down && landState == LandState.InAir)
                {
                    arm = ArmDirection.Lower;
                }

                #endregion

                #region 弾丸発射

                if (input.Attack && fireCount <= 0)
                {
                    if (weapon == Weapon.Pistol)
                    {
                        fireCount = INIT_FIRE_COUNT;
                    }
                    else if (weapon == Weapon.Machinegun)
                    {
                        fireCount = INIT_FIRE_COUNT / 2;
                    }
                    else if (weapon == Weapon.Rocket)
                    {
                        fireCount = INIT_FIRE_COUNT * 2;
                    }
                    else if (weapon == Weapon.Shotgun)
                    {
                        fireCount = INIT_FIRE_COUNT * 4;
                    }
                    else if (weapon == Weapon.Flamethrower)
                    {
                        if (fireCount == -1)
                        {
                            game.PlaySound(GameSound.Flame);
                        }
                        fireCount = 1;
                    }

                    armAnimation = 16;
                    FireWeapon(weapon);

                    /*
                    if (direction == Direction.Left)
                    {
                        game.AddPlayerBullet(new PlayerBullet(game, position + new Vector(0, 32), 180));
                        // game.AddParticle(new PlayerBulletExplosion(game, position + new Vector(0, 32), velocity * 0.5));
                    }
                    else
                    {
                        game.AddPlayerBullet(new PlayerBullet(game, position + new Vector(32, 32), 0));
                        // game.AddParticle(new PlayerBulletExplosion(game, position + new Vector(32, 32), velocity * 0.5));
                    }
                    */
                }
                if (fireCount > -1)
                {
                    fireCount--;
                }
                if (armAnimation > 0)
                {
                    armAnimation--;
                }

                #endregion

                #region アイテム取得

                foreach (Thing item in game.Items)
                {
                    if (Overlappes(this, item))
                    {
                        GetItem((Item)item);
                    }
                }

                #endregion

            }

            #region カメラ位置の補正

            if (direction == Direction.Left)
            {
                focus.X -= 2;
            }
            else
            {
                focus.X += 2;
            }
            if (Math.Abs(focus.X) > Settings.SCREEN_WIDTH / 8) focus.X = Math.Sign(focus.X) * Settings.SCREEN_WIDTH / 8;
            if (Center.X + focus.X < Settings.SCREEN_WIDTH / 2)
            {
                focus.X = Settings.SCREEN_WIDTH / 2 - Center.X;
            }
            else if (Center.X + focus.X > game.Map.Width - Settings.SCREEN_WIDTH / 2)
            {
                focus.X = game.Map.Width - Settings.SCREEN_WIDTH / 2 - Center.X;
            }

            #endregion

            if (enemyDamageCount > 0)
            {
                enemyDamageCount--;
            }

            /*
            if (drawHealth < health)
            {
                drawHealth++;
            }
            else if (drawHealth > health)
            {
                drawHealth--;
            }
            */
            drawHealth = health + (drawHealth - health) * 3 / 4;

            if (game.DebugMode)
            {
                if (health < 100)
                {
                    health++;
                }
            }

            base.Tick(input);
        }

        public override void Blocked_Top(GameInput input)
        {
            base.Blocked_Top(input);
            jumpCount = -1;
        }

        public override void Blocked_Bottom(GameInput input)
        {
            base.Blocked_Top(input);
            if (input.Left == input.Right)
            {
                animation = 0;
            }
            landState = LandState.OnGround;
            if (arm == ArmDirection.Lower)
            {
                arm = ArmDirection.Forward;
            }
        }

        public void FireWeapon(Weapon weapon)
        {
            Vector posFix;
            int angle;
            switch (arm)
            {
                case ArmDirection.Forward:
                    posFix = new Vector(0, 30);
                    angle = 180;
                    break;
                case ArmDirection.Upper:
                    posFix = new Vector(28, 18);
                    angle = -90;
                    break;
                case ArmDirection.Lower:
                    posFix = new Vector(16, 64);
                    angle = 90;
                    break;
                default:
                    throw new Exception("ちょｗｗｗ");
            }
            if (direction == Direction.Left)
            {
            }
            else
            {
                posFix.X = 32 - posFix.X;
                angle = 180 - angle;
            }

            Random random = game.Random;

            switch (weapon)
            {
                case Weapon.Pistol:
                    game.AddPlayerBullet(new PlayerBullet(game, position + posFix, angle));
                    game.PlaySound(GameSound.Pistol);
                    // game.Quake(1);
                    break;
                case Weapon.Machinegun:
                    game.AddPlayerBullet(new PlayerBullet2(game, position + posFix, angle));
                    game.Quake(1);
                    game.PlaySound(GameSound.Pistol);
                    break;
                case Weapon.Rocket:
                    game.AddPlayerBullet(new PlayerRocket(game, position + posFix, angle));
                    game.PlaySound(GameSound.Rocket);
                    break;
                case Weapon.Shotgun:;
                    for (int i = 0; i < 24; i++)
                    {
                        game.AddPlayerBullet(new PlayerBullet3(game, position + posFix, angle + 15 * random.NextDouble() + 15 * random.NextDouble() - 15, 8 + 16 * random.NextDouble()));
                    }
                    game.Quake(4);
                    game.Flash(32);
                    game.PlaySound(GameSound.Shotgun);
                    break;
                case Weapon.Flamethrower:
                    game.AddPlayerBullet(new PlayerFlame(game, position + posFix, angle + 6 * random.NextDouble() + 6 * random.NextDouble() - 6));
                    break;
            }

            if (weapon != Weapon.Pistol)
            {
                ammo--;
                if (ammo == 0)
                {
                    ChangeWeapon(Weapon.Pistol, 0);
                }
            }

            ammoNumColorCount = 4;
        }

        public void ChangeWeapon(Weapon weapon, int ammo)
        {
            if (this.weapon != weapon)
            {
                this.weapon = weapon;
                this.ammo = ammo;
            }
            else
            {
                this.ammo += ammo;
            }
            if (this.ammo > 999)
            {
                this.ammo = 999;
            }

            ammoHudColorCount = 16;
            ammoNumColorCount = 0;
        }

        public void GetItem(Item item)
        {
            item.Get();
            if (item is MachinegunItem)
            {
                ChangeWeapon(Weapon.Machinegun, 200);
                game.PlaySound(GameSound.Weapon);
                game.PlaySound(GameSound.MachinegunVoice);
            }
            else if (item is RocketItem)
            {
                ChangeWeapon(Weapon.Rocket, 50);
                game.PlaySound(GameSound.Weapon);
                game.PlaySound(GameSound.RocketVoice);
            }
            else if (item is ShotgunItem)
            {
                ChangeWeapon(Weapon.Shotgun, 30);
                game.PlaySound(GameSound.Weapon);
                game.PlaySound(GameSound.ShotgunVoice);
            }
            else if (item is FlameItem)
            {
                ChangeWeapon(Weapon.Flamethrower, 500);
                game.PlaySound(GameSound.Weapon);
                game.PlaySound(GameSound.FlameVoice);
            }
            else if (item is HealthItem)
            {
                health += 50;
                if (health > 100)
                {
                    health = 100;
                }
                game.PlaySound(GameSound.Weapon);
                game.PlaySound(GameSound.OkVoice);
            }
        }

        public override void Damage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
            damageFlash = 256;
            game.PlaySound(GameSound.PlayerDamage);
        }

        public void EnemyDamage(Thing enemy)
        {
            if (enemyDamageCount == 0)
            {
                if (enemy is BossRobot || enemy is BossHouse || enemy is BossMushroom || enemy is Father || enemy is Oyaji)
                {
                    Damage(10);
                }
                else if (enemy is Mushroom)
                {
                    Damage(((Mushroom)enemy).Aggressive ? 5 : 3);
                }
                else if (enemy is Worm)
                {
                    Damage(3);
                }
                else
                {
                    Damage(5);
                }
                enemyDamageCount = 15;
            }
        }

        public void Freeze()
        {
            velocity = Vector.Zero;
            freeze = true;
        }

        public void Disappear()
        {
            visible = false;
        }

        public override void Die()
        {
            if (Removed)
            {
                return;
            }

            game.AddParticle(new BigExplosion(game, Center, Vector.Zero));
            game.Quake(4);
            game.Flash(16);
            game.PlaySound(GameSound.Explode);
            SpreadDebris(16);
            Disappear();
            Remove();
        }

        public override void MoveBy_Left(GameInput input, double d)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int leftCol = LeftCol;
            Map map = game.Map;
            for (int row = topRow; row <= bottomRow; row++)
            {
                if (map.IsObstacle(row, leftCol))
                {
                    Left = (leftCol + 1) * Settings.BLOCK_WDITH;
                    Blodked_Left(input);
                    break;
                }
            }
            foreach (Thing enemy in game.Enemies)
            {
                if (Overlappes(enemy, this) && enemy.Shootable)
                {
                    velocity.X = 8;
                    EnemyDamage(enemy);
                }
            }
        }

        public override void MoveBy_Up(GameInput input, double d)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int topRow = TopRow;
            Map map = game.Map;
            for (int col = leftCol; col <= rightCol; col++)
            {
                if (map.IsObstacle(topRow, col))
                {
                    Top = (topRow + 1) * Settings.BLOCK_WDITH;
                    Blocked_Top(input);
                    break;
                }
            }
            foreach (Thing enemy in game.Enemies)
            {
                if (Overlappes(enemy, this) && enemy.Shootable)
                {
                    if (enemy.Center.X < Center.X)
                    {
                        velocity.X = 8;
                    }
                    else if (Center.X < enemy.Center.X)
                    {
                        velocity.X = -8;
                    }
                    else
                    {
                        velocity.X = game.Random.Next(0, 2) == 0 ? 8 : -8;
                    }
                    EnemyDamage(enemy);
                }
            }
        }

        public override void MoveBy_Right(GameInput input, double d)
        {
            position.X += d;
            int topRow = TopRow;
            int bottomRow = BottomRow;
            int rightCol = RightCol;
            Map map = game.Map;
            for (int row = topRow; row <= bottomRow; row++)
            {
                if (map.IsObstacle(row, rightCol))
                {
                    Right = rightCol * Settings.BLOCK_WDITH;
                    Blocked_Right(input);
                    break;
                }
            }
            foreach (Thing enemy in game.Enemies)
            {
                if (Overlappes(enemy, this) && enemy.Shootable)
                {
                    velocity.X = -8;
                    EnemyDamage(enemy);
                }
            }
        }

        public override void MoveBy_Down(GameInput input, double d)
        {
            position.Y += d;
            int leftCol = LeftCol;
            int rightCol = RightCol;
            int bottomRow = BottomRow;
            Map map = game.Map;
            for (int col = leftCol; col <= rightCol; col++)
            {
                if (map.IsObstacle(bottomRow, col))
                {
                    Bottom = bottomRow * Settings.BLOCK_WDITH;
                    Blocked_Bottom(input);
                    break;
                }
            }
            foreach (Thing enemy in game.Enemies)
            {
                if (Overlappes(enemy, this) && enemy.Shootable)
                {
                    if (enemy.Center.X < Center.X)
                    {
                        velocity.X = 8;
                    }
                    else if (Center.X < enemy.Center.X)
                    {
                        velocity.X = -8;
                    }
                    else
                    {
                        velocity.X = game.Random.Next(0, 2) == 0 ? 8 : -8;
                    }
                    EnemyDamage(enemy);
                }
            }
        }


        public Vector Focus
        {
            get
            {
                return Center + focus;
            }
        }

        public LandState CurrentLandState
        {
            get
            {
                return landState;
            }
        }

        public PlayerState State
        {
            get
            {
                return new PlayerState(health, weapon, ammo);
            }

            set
            {
                health = drawHealth = value.Health;
                weapon = value.Weapon;
                ammo = value.Ammo;
            }
        }

        public Weapon CurrentWeapon
        {
            get
            {
                return weapon;
            }
        }

        public int Ammo
        {
            get
            {
                return ammo;
            }
        }

        public int DrawHealth
        {
            get
            {
                return drawHealth;
            }
        }

        public int AmmoHudColorCount
        {
            get
            {
                if (ammoHudColorCount >= 16)
                {
                    return 255;
                }
                else
                {
                    return 16 * ammoHudColorCount;
                }
            }
        }

        public int AmmoNumColorCount
        {
            get
            {
                if (ammoNumColorCount >= 4)
                {
                    return 255;
                }
                else
                {
                    return 64 * ammoNumColorCount;
                }
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
        }
    }
}
