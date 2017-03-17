using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToA
{
    public class InputManager
    {
        public Dictionary<Keys, Input> keyBindingsKeyboard;
        public Dictionary<Buttons, Input> keyBindingsGamepad;

        private bool isUsingKeyboard;

        private int playerInput;
        public InputManager(bool pIsUsingKeyboard = true)
        {
            isUsingKeyboard = pIsUsingKeyboard;

            keyBindingsKeyboard = new Dictionary<Keys, Input>
            {
                {Keys.W, Input.Up },
                {Keys.A, Input.Left },
                {Keys.S, Input.Down },
                {Keys.D, Input.Right },
                {Keys.Escape, Input.Back }
            };

            keyBindingsGamepad = new Dictionary<Buttons, Input>
            {
                {Buttons.LeftThumbstickUp, Input.Up },
                {Buttons.LeftThumbstickLeft, Input.Left },
                {Buttons.LeftThumbstickDown, Input.Down },
                {Buttons.LeftThumbstickRight, Input.Right },
                {Buttons.Back, Input.Back }
            };
        }

        public void Update(PlayerIndex pPlayer = PlayerIndex.One) //PLAYER INDEX domyślnie na One
        {
            playerInput = 0;
            if(isUsingKeyboard)
            {
                Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

                foreach(Keys key in pressedKeys)
                {
                    if (keyBindingsKeyboard.ContainsKey(key))
                    {
                        playerInput |= (int)keyBindingsKeyboard[key];
                    }
                }
            }
            else
            {
                var gamepadState = GamePad.GetState(pPlayer);
                foreach(var kvp in keyBindingsGamepad)
                {
                    if (gamepadState.IsButtonDown(kvp.Key))
                    {
                        playerInput |= (int)kvp.Value;
                    }
                }
            }
        }

        public bool Pressed(params Input[] pInputs)
        {
            int n = 0;
            
            foreach(var pInput in pInputs)
            {
                n |= (int)pInput;
            }

            return playerInput == n;
        }
    }
}
