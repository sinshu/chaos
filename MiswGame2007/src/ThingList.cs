using System;
using System.Collections.Generic;

namespace MiswGame2007
{
    public class ThingList
    {
        List<Thing> things;

        public ThingList()
        {
            things = new List<Thing>();
        }

        public List<Thing>.Enumerator GetEnumerator()
        {
            return things.GetEnumerator();
        }

        public void ForEach(Action<Thing> action)
        {
            things.ForEach(action);
        }

        public void Tick(GameInput input)
        {
            foreach (Thing thing in things)
            {
                thing.Tick(input);
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
            foreach (Thing thing in things)
            {
                thing.Draw(graphics);
            }
        }

        public void AddThing(Thing thing)
        {
            things.Add(thing);
        }

        public void SweepRemovedThings()
        {
            things.RemoveAll(IsRemoved);
        }

        public void Clear()
        {
            things.Clear();
        }

        public void KillAll()
        {
            foreach (Thing thing in things)
            {
                thing.Die();
            }
        }

        private bool IsRemoved(Thing thing)
        {
            return thing.Removed;
        }

        public int Count
        {
            get
            {
                return things.Count;
            }
        }
    }
}
