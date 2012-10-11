using System;

namespace MiswGame2007
{
    public struct StageSelectInput
    {
        public static StageSelectInput Empty = new StageSelectInput(false, false, false, false);

        public bool Left;
        public bool Right;
        public bool Start;
        public bool Exit;

        public StageSelectInput(bool left, bool right, bool start, bool exit)
        {
            Left = left;
            Right = right;
            Start = start;
            Exit = exit;
        }
    }
}
