﻿using Microsoft.Xna.Framework;
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
        public bool right;
        public bool left;
        public bool up;
        public bool back = false;

        public void Update(PlayerIndex pPlayer = PlayerIndex.One) //PLAYER INDEX domyślnie na One
        {
            KeyboardState state = Keyboard.GetState();
            var gamePadState = GamePad.GetState(pPlayer);
            if (state.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed || gamePadState.ThumbSticks.Left.X < 0.0f)
                right = true;
            else
                right = false;
            if (state.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed || gamePadState.ThumbSticks.Left.X > 0.0f)
                left = true;
            else
                left = false;
            if (state.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed || gamePadState.ThumbSticks.Left.Y > 0.5f)
                up = true;
            else
                up = false;
            if (state.IsKeyDown(Keys.Escape))
                back = true;
        }
    }
}
