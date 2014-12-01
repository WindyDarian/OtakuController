using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace MagicXboxOneController
{
    class GamePadManager
    {


        public GamePadManager()
        {
            if (GamePad.GetState(currentController).IsConnected)
            {
                Console.WriteLine("Controller Connected!");
            }

            Thread padThread = new Thread(ThreadRun);
            padThread.Start();
        }

        public void Destroy()
        {
            exiting = true;
        }


        void Update(float elapsedTime)
        {

            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            #region Finding Controller
            if (!GamePad.GetState(currentController).IsConnected)
            {
                //Todo: alert users 
                return;
            }
            
            #endregion

            #region movement and scroll
            Vector2 currentMousePos = MouseOperations.GetCursorPosition().ToVector2();
            Vector2 delta = currentGamePadState.ThumbSticks.Left;
            if (delta != Vector2.Zero)
            {
                delta = delta * elapsedTime * rate;

                if (!invertY)
                    delta.Y *= -1;

                //Vector2 mpos = currentMousePos + delta;
                MouseOperations.MouseMove(delta);
            }


            Vector2 scroll = currentGamePadState.ThumbSticks.Right;
            if (scroll != Vector2.Zero)
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.Wheel, (int)(scrollrate * elapsedTime * scroll.Y), 0);


            #endregion

            #region mousebutton
            if (IsPadButtonJustDown(Buttons.A) || IsPadButtonJustDown(Buttons.RightTrigger)) //Left button: A and RT
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            if (IsPadButtonJustUp(Buttons.A) || IsPadButtonJustUp(Buttons.RightTrigger)) //Left button: A and RT
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
            if (IsPadButtonJustDown(Buttons.B) || IsPadButtonJustDown(Buttons.LeftTrigger)) //Left button: B and LT
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightDown);
            if (IsPadButtonJustUp(Buttons.B) || IsPadButtonJustUp(Buttons.LeftTrigger)) //Left button: B and LT
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.RightUp);
            if (IsPadButtonJustDown(Buttons.Y) || IsPadButtonJustDown(Buttons.RightStick)) //Middle button: Y and RS
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.MiddleDown);
            if (IsPadButtonJustUp(Buttons.Y) || IsPadButtonJustUp(Buttons.RightStick)) //Middle button: Y and RS
                MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.MiddleUp);




            #endregion

            previousGamePadState = currentGamePadState;
        }

        private void ThreadRun()
        {
            while (!exiting)
            {
                var elapsedTime = (float)(DateTime.Now - lastUpdate).TotalSeconds;
                if (elapsedTime > updateTimeSpaninSecond)
                {
                    Update(elapsedTime);
                    lastUpdate = DateTime.Now;
                }
            }
        }

        private bool IsPadButtonJustUp(Buttons btn)
        {
            if (previousGamePadState == null || currentGamePadState == null)
            {
                return false;
            }

            return !currentGamePadState.IsButtonDown(btn) && previousGamePadState.IsButtonDown(btn);
        }

        private bool IsPadButtonJustDown(Buttons btn)
        {
            if (previousGamePadState == null || currentGamePadState == null)
            {
                return false;
            }

            return currentGamePadState.IsButtonDown(btn) && !previousGamePadState.IsButtonDown(btn);
        }


        private bool IsPadButtonDown(Buttons btn)
        {
            if (!GamePad.GetState(currentController).IsConnected)
            {
                //Todo: alert users 
                return false;
            }

            return currentGamePadState.IsButtonDown(btn);
        }



        public float Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public float Scrollrate
        {
            get { return scrollrate; }
            set { scrollrate = value; }
        }

        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }


        float updateTimeSpaninSecond = 0.0167f;      // 1/60 second
        float rate = 2048f;
        float scrollrate = 2048f;
        bool invertY = false;


        bool exiting = false;
        PlayerIndex currentController = PlayerIndex.One;

        DateTime lastUpdate = DateTime.Now;
        GamePadState previousGamePadState;
        GamePadState currentGamePadState;

        
    }

}
