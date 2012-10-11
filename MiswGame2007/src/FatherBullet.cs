using System;

namespace MiswGame2007
{
    public class FatherBullet : Bullet
    {
        private const double RADIUS = 4;
        private const int DAMAGE = 8;

        private Vector waitPos;
        private bool facePlayer;
        private int timer;
        private int stateCount;
        private double direction;
        private int color;

        public FatherBullet(GameScene game, Vector position, Vector waitPos, bool facePlayer, int timer)
            : base(game, RADIUS, position, Vector.Zero, DAMAGE)
        {
            this.waitPos = waitPos;
            this.facePlayer = facePlayer;
            this.timer = timer;
            stateCount = 0;
            direction = 0;
            color = 2 * game.Random.Next(0, 3);
            if (game.Random.Next(0, 8) == 0) color++;
            velocity = 0.015625 * (waitPos - position);
        }

        public override void Tick(ThingList targetThings)
        {
            stateCount++;
            if (stateCount < timer)
            {
                velocity = 0.0625 * (waitPos - position);
            }
            if (stateCount == timer)
            {
                if (facePlayer)
                {
                    direction = Math.Atan2(game.Player.Center.Y - position.Y, game.Player.Center.X - position.X);
                    velocity = 8 * new Vector(Math.Cos(direction), Math.Sin(direction));
                }
                else
                {
                    direction = Math.PI / 2;
                    velocity = new Vector(0, 8);
                }
                base.Tick(targetThings);
                base.Tick(targetThings);
            }

            base.Tick(targetThings);
            base.Tick(targetThings);
            base.Tick(targetThings);
            base.Tick(targetThings);
        }

        public override void Draw(GraphicsDevice graphics)
        {
            int drawX = (int)Math.Round(position.X) - game.IntCameraX;
            int drawY = (int)Math.Round(position.Y) - game.IntCameraY;
            int r = (color <= 1 || color == 5) ? 255 : 0;
            int g = (color >= 3 && color <= 5) ? 255 : 0;
            int b = (color >= 1 && color <= 3) ? 255 : 0;

            if (stateCount < timer)
            {
                graphics.DrawImageAdd(GameImage.EnemyBullet, 32, 32, 2, stateCount / 2 % 8, drawX - 16, drawY - 16, 255, r, g, b);
            }
            else
            {
                graphics.DrawImageRotateAdd(GameImage.EnemyBullet, 64, 32, 0, 3, drawX, drawY, 48, 16, (int)Math.Round(direction * 180 / Math.PI), 255, r, g, b);
            }
        }

        public override void Hit()
        {
            if (Removed)
            {
                return;
            }
            game.AddParticle(new BigExplosion(game, position, Vector.Zero));
            game.Flash(4);
            game.Quake(4);
            game.PlaySound(GameSound.Shotgun);
            Remove();
        }
    }
}
