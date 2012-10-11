using System;

namespace MiswGame2007
{
    public struct GameInput
    {
        public static GameInput Empty = new GameInput(false, false, false, false, false, false, false);

        public bool Left;
        public bool Up;
        public bool Right;
        public bool Down;
        public bool Jump;
        public bool Attack;
        public bool Exit;

        public GameInput(bool left, bool up, bool right, bool down, bool jump, bool attack, bool exit)
        {
            Left = left;
            Up = up;
            Right = right;
            Down = down;
            Jump = jump;
            Attack = attack;
            Exit = exit;
        }
    }
}
