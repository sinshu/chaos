using System;

namespace MiswGame2007
{
    public class Particle
    {
        protected GameScene game;
        protected Vector position;
        protected Vector velocity;

        private bool removed;

        public Particle(GameScene game, Vector position, Vector velocity)
        {
            this.game = game;
            this.position = position;
            this.velocity = velocity;
            removed = false;
        }

        public virtual void Remove()
        {
            removed = true;
        }

        public virtual void Tick()
        {
            position += velocity;
        }

        public virtual void Draw(GraphicsDevice graphics)
        {
        }

        public bool Removed
        {
            get
            {
                return removed;
            }
        }
    }
}
