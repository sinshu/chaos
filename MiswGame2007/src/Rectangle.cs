using System;

namespace MiswGame2007
{
    public struct Rectangle
    {
        public Vector Position;
        public Vector Size;

        public Rectangle(Vector position, Vector size)
        {
            Position = position;
            Size = size;
        }

        public double Left
        {
            get
            {
                return Position.X;
            }
        }

        public double Right
        {
            get
            {
                return Position.X + Size.X;
            }
        }

        public double Top
        {
            get
            {
                return Position.Y;
            }
        }

        public double Bottom
        {
            get
            {
                return Position.Y + Size.Y;
            }
        }

        public Vector LeftTop
        {
            get
            {
                return Position;
            }
        }

        public Vector RightTop
        {
            get
            {
                return new Vector(Position.X + Size.X, Position.Y);
            }
        }

        public Vector LeftBottom
        {
            get
            {
                return new Vector(Position.X, Position.Y + Size.Y);
            }
        }

        public Vector RightBottom
        {
            get
            {
                return Position + Size;
            }
        }

        public Vector Center
        {
            get
            {
                return Position + (0.5 * Size);
            }
        }
    }
}
