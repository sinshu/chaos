using System;
using Yanesdk.Ytl;
using Yanesdk.System;
using Yanesdk.Draw;
using Yanesdk.Input;

namespace MiswGame2007
{
    public class InputDevice
    {
        private KeyBoardInput keyBoard;
        private JoyStick joyStick;
        private MouseInput mouse;
        private int jumpButton;
        private int attackButton;
        private int startButton;

        public InputDevice(bool showCursor, int jumpButton, int attackButton, int startButton)
        {
            keyBoard = new KeyBoardInput();
            joyStick = new JoyStick(0);
            mouse = new MouseInput();
            if (showCursor)
            {
                mouse.Show();
            }
            else
            {
                mouse.Hide();
            }
            this.jumpButton = jumpButton + 4;
            this.attackButton = attackButton + 4;
            this.startButton = startButton + 4;
        }

        public void Update()
        {
            keyBoard.Update();
            joyStick.Update();
            mouse.Update();
        }

        public TitleInput CurrentTitleInput
        {
            get
            {
                if (SDLFrame.IsActive)
                {
                    return new TitleInput(CurrentUp2,
                                          CurrentDown2,
                                          CurrentAttack2 || CurrentStart2,
                                          CurrentExit2);
                }
                else
                {
                    return TitleInput.Empty;
                }
            }
        }

        public GameInput CurrentGameInput
        {
            get
            {
                if (SDLFrame.IsActive)
                {
                    return new GameInput(CurrentLeft,
                                         CurrentUp,
                                         CurrentRight,
                                         CurrentDown,
                                         CurrentJump,
                                         CurrentAttack,
                                         CurrentExit2);
                }
                else
                {
                    return GameInput.Empty;
                }
            }
        }

        public StageSelectInput CurrentStageSelectInput
        {
            get
            {
                if (SDLFrame.IsActive)
                {
                    return new StageSelectInput(CurrentLeft2,
                                                CurrentRight2,
                                                CurrentAttack2 || CurrentStart2,
                                                CurrentJump2 || CurrentExit2);
                }
                else
                {
                    return StageSelectInput.Empty;
                }
            }
        }

        public EndingInput CurrentEndingInput
        {
            get
            {
                if (SDLFrame.IsActive)
                {
                    return new EndingInput(CurrentExit2);
                }
                else
                {
                    return EndingInput.Empty;
                }
            }
        }

        private bool CurrentLeft
        {
            get
            {
                return keyBoard.IsPress(KeyCode.LEFT) || joyStick.IsPress(2);
            }
        }

        private bool CurrentUp
        {
            get
            {
                return keyBoard.IsPress(KeyCode.UP) || joyStick.IsPress(0);
            }
        }

        private bool CurrentRight
        {
            get
            {
                return keyBoard.IsPress(KeyCode.RIGHT) || joyStick.IsPress(3);
            }
        }

        private bool CurrentDown
        {
            get
            {
                return keyBoard.IsPress(KeyCode.DOWN) || joyStick.IsPress(1);
            }
        }

        private bool CurrentJump
        {
            get
            {
                return keyBoard.IsPress(KeyCode.LSHIFT) || keyBoard.IsPress(KeyCode.RSHIFT) || keyBoard.IsPress(KeyCode.SPACE) || keyBoard.IsPress(KeyCode.x) || keyBoard.IsPress(KeyCode.s) || joyStick.IsPress(jumpButton);
            }
        }

        private bool CurrentAttack
        {
            get
            {
                return keyBoard.IsPress(KeyCode.LCTRL) || keyBoard.IsPress(KeyCode.RCTRL) || keyBoard.IsPress(KeyCode.RETURN) || keyBoard.IsPress(KeyCode.z) || keyBoard.IsPress(KeyCode.a) || joyStick.IsPress(attackButton);
            }
        }

        private bool CurrentExit
        {
            get
            {
                return keyBoard.IsPress(KeyCode.ESCAPE);
            }
        }

        private bool CurrentStart
        {
            get
            {
                return joyStick.IsPress(startButton);
            }
        }

        private bool CurrentLeft2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.LEFT) || joyStick.IsPush(2);
            }
        }

        private bool CurrentUp2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.UP) || joyStick.IsPush(0);
            }
        }

        private bool CurrentRight2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.RIGHT) || joyStick.IsPush(3);
            }
        }

        private bool CurrentDown2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.DOWN) || joyStick.IsPush(1);
            }
        }

        private bool CurrentJump2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.LSHIFT) || keyBoard.IsPush(KeyCode.RSHIFT) || keyBoard.IsPush(KeyCode.SPACE) || keyBoard.IsPush(KeyCode.x) || keyBoard.IsPush(KeyCode.s) || joyStick.IsPush(jumpButton);
            }
        }

        private bool CurrentAttack2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.LCTRL) || keyBoard.IsPush(KeyCode.RCTRL) || keyBoard.IsPush(KeyCode.RETURN) || keyBoard.IsPush(KeyCode.z) || keyBoard.IsPush(KeyCode.a) || joyStick.IsPush(attackButton);
            }
        }

        private bool CurrentExit2
        {
            get
            {
                return keyBoard.IsPush(KeyCode.ESCAPE);
            }
        }

        private bool CurrentStart2
        {
            get
            {
                return joyStick.IsPush(startButton);
            }
        }

        public bool DebugKey
        {
            get
            {
                return keyBoard.IsPress(KeyCode.m) && keyBoard.IsPress(KeyCode.w);
            }
        }
    }
}
