using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Input
{
    public class KeyboardHandler : IGameController
    {
        
        KeyboardState state;
        Vector2 pos;
        KeyboardState oldstate;
		float factor;

        public KeyboardHandler(Vector2 player_pos)
        {
            state = Keyboard.GetState();
            oldstate = Keyboard.GetState();
            pos = player_pos;
        }

        public KeyboardHandler()
        {
            state = Keyboard.GetState();
            oldstate = Keyboard.GetState();
            pos = Vector2.Zero;
        }

        public Keys[] getDownKeys
        {
            get { return state.GetPressedKeys(); }
        }

        public KeyboardState GetOldState
        {
            get { return oldstate; }
        }

        public KeyboardState GetCurState
        {
            get { return state; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

		public List<MenuKeys> GetMenuKeys()
		{
			List<MenuKeys> menukeys = new List<MenuKeys>();
			Keys[] keyboardstate = getDownKeys;
			foreach (Keys key in keyboardstate)
			{
				switch (key)
				{
					case Keys.Left:

						menukeys.Add(MenuKeys.Left);
						break;
					case Keys.Right:

						menukeys.Add(MenuKeys.Right);
						break;
					case Keys.Space:
						menukeys.Add(MenuKeys.Enter);
						break;
					case Keys.Up:
						menukeys.Add(MenuKeys.Up);
						break;
					case Keys.Down:
						menukeys.Add(MenuKeys.Down);
						break;
					case Keys.Enter:
						menukeys.Add(MenuKeys.Enter);
						break;
					case Keys.Back:
					case Keys.Escape:
						menukeys.Add(MenuKeys.Back);
						break;
				}
			}
			return menukeys;
		}

        public void Update(GameTime gametime,float run_factor,Vector2 playerPos)
        {
            oldstate = state;
            state = Keyboard.GetState();
            updateposition(gametime,run_factor,playerPos);
        }

        private void updateposition(GameTime gametime,float run_factor,Vector2 playerPos)
        {
            if (!(state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Right)))
            {
				if (state.IsKeyDown(Keys.Right))
				{
					if (factor < 0)
						factor = 0;
					factor += 5 * (float)gametime.ElapsedGameTime.TotalSeconds;
					factor -= factor * (float)gametime.ElapsedGameTime.TotalSeconds;
					factor = MathHelper.Clamp(factor, -1, 1);
					pos.X = playerPos.X + factor * Inputhandler.MAX_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds * run_factor;
				}
				else if (state.IsKeyDown(Keys.Left))
				{
					if (factor > 0)
						factor = 0;
					factor -= 5 * (float)gametime.ElapsedGameTime.TotalSeconds;

					factor = MathHelper.Clamp(factor, -1, 1);

					pos.X = playerPos.X + factor * Inputhandler.MAX_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds * run_factor;
				}
				else
					pos.X = playerPos.X + factor * Inputhandler.MAX_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds * run_factor;
            }
			else
				factor -= factor * (float)gametime.ElapsedGameTime.TotalSeconds;
        }

		#region IGameController Member

		public List<InputKeys> GetInputKeys()
		{
			List<InputKeys> inputkeys = new List<InputKeys>();
			Keys[] keyboardstate = getDownKeys;
			foreach (Keys key in keyboardstate)
			{
				switch (key)
				{
					case Keys.Left:
						inputkeys.Add(InputKeys.Left);

						break;
					case Keys.Right:
						inputkeys.Add(InputKeys.Right);

						break;
					case Keys.Space:
						inputkeys.Add(InputKeys.Jump);

						break;
					case Keys.LeftControl:
						inputkeys.Add(InputKeys.Run);
						break;
				}
			}
			return inputkeys;
		}

		public void Initialize(Star.GameManagement.Options options)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
