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
        public bool right;
        public bool left;
        public bool up;
        public bool menuDown;
        public bool menuUp;
        public bool menuLeft;
        public bool menuRight;
        public bool menuChoose;
        public bool back = false;
        public bool changeGravity;
        public bool pause = false;
        public bool action;
        public bool enter = false;

        KeyboardState previousstate = Keyboard.GetState();
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
            if ((state.IsKeyDown(Keys.Q) && previousstate.IsKeyUp(Keys.Q)) || gamePadState.Buttons.A == ButtonState.Pressed)
                changeGravity = true;
            else
                changeGravity = false;
            if (state.IsKeyDown(Keys.Escape))
                back = true;
            if (state.IsKeyDown(Keys.Enter))
                enter = true;
            else
                enter = false;
            if (state.IsKeyDown(Keys.P) && previousstate.IsKeyUp(Keys.P))
                pause = !pause;
            if (state.IsKeyDown(Keys.E) || gamePadState.Buttons.B == ButtonState.Pressed)
                action = true;
            else
                action = false;
            if (state.IsKeyDown(Keys.Up) && previousstate.IsKeyUp(Keys.Up))
                menuUp = true;
            else
                menuUp = false;
            if (state.IsKeyDown(Keys.Down) && previousstate.IsKeyUp(Keys.Down))
                menuDown = true;
            else
                menuDown = false;
            if (state.IsKeyDown(Keys.Left) && previousstate.IsKeyUp(Keys.Left))
                menuLeft = true;
            else
                menuLeft = false;
            if (state.IsKeyDown(Keys.Right) && previousstate.IsKeyUp(Keys.Right))
                menuRight = true;
            else
                menuRight = false;
            if (state.IsKeyDown(Keys.Enter) && previousstate.IsKeyUp(Keys.Enter))
                menuChoose = true;
            else
                menuChoose = false;
            previousstate = state;

        }
    }
}
