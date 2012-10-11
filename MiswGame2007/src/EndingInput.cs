using System;

namespace MiswGame2007
{
    public struct EndingInput
    {
        public static EndingInput Empty = new EndingInput(false);

        public bool Exit;

        public EndingInput(bool exit)
        {
            Exit = exit;
        }
    }
}
