using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class ParticleList
    {
        List<Particle> particles;

        public ParticleList()
        {
            particles = new List<Particle>();
        }

        public List<Particle>.Enumerator GetEnumerator()
        {
            return particles.GetEnumerator();
        }

        public void ForEach(Action<Particle> action)
        {
            particles.ForEach(action);
        }

        public void Tick()
        {
            foreach (Particle particle in particles)
            {
                particle.Tick();
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(graphics);
            }
        }

        public void AddParticle(Particle particle)
        {
            particles.Add(particle);
        }

        public void SweepRemovedParticles()
        {
            particles.RemoveAll(IsRemoved);
        }

        public void Clear()
        {
            particles.Clear();
        }

        private bool IsRemoved(Particle particle)
        {
            return particle.Removed;
        }
    }
}
