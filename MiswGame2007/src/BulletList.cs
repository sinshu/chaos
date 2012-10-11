using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class BulletList
    {
        List<Bullet> bullets;

        public BulletList()
        {
            bullets = new List<Bullet>();
        }

        public void Tick(ThingList targetThings)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Tick(targetThings);
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(graphics);
            }
        }

        public void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet);
        }

        public void BreakAll()
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.Hit();
            }
        }

        public void SweepRemovedBullets()
        {
            bullets.RemoveAll(IsRemoved);
        }

        private bool IsRemoved(Bullet bullet)
        {
            return bullet.Removed;
        }
    }
}
