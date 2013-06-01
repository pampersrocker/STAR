using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Star.GameManagement;

namespace Star.Input
{
    class Gamepadhandler : IGameController
    {
        GamePadState gamepadstate;
        Vector2 pos;
        bool is_active=true;
		readonly float MenuTimeThreshold = 0.1f;
		float factor = 0;
		float thumbStickMenuTimeElapsedX, thumbStickMenuTimeElapsedY;
		bool leftRightMenu;
		bool upDownMenu;

        public Gamepadhandler(Vector2 player_pos)
        {
            is_active = GamePad.GetState(PlayerIndex.One).IsConnected;
            pos = player_pos;
            if (is_active == true)
            {
                gamepadstate = GamePad.GetState(PlayerIndex.One);
            }
        }

        public GamePadState GetState
        {
            get { return gamepadstate; }
        }

        public Vector2 Pos 
        {
            get { return pos; }
            set { pos = value; }
        }

		public void Initialize(Options options)
		{
			throw new NotImplementedException();
		}

		public List<MenuKeys> GetMenuKeys()
		{
			List<MenuKeys> menukeys = new List<MenuKeys>();

			GamePadButtons gamepadstate = GetState.Buttons;
			if (gamepadstate.A == ButtonState.Pressed)
			{
				menukeys.Add(MenuKeys.Enter);
			}
			if (GetState.ThumbSticks.Left.X > 0 && leftRightMenu)
			{
				menukeys.Add(MenuKeys.Right);
			}
			if (GetState.ThumbSticks.Left.X < 0 && leftRightMenu)
			{
				menukeys.Add(MenuKeys.Left);
			}
			if (gamepadstate.Start == ButtonState.Pressed)
			{
				menukeys.Add(MenuKeys.Enter);
			}
			if (GetState.DPad.Down == ButtonState.Pressed || GetState.ThumbSticks.Left.Y < -0.5 && upDownMenu)
			{
				menukeys.Add(MenuKeys.Down);
			}
			if (GetState.DPad.Up == ButtonState.Pressed || GetState.ThumbSticks.Left.Y > 0.5 && upDownMenu)
			{
				menukeys.Add(MenuKeys.Up);
			}
			if (GetState.DPad.Left == ButtonState.Pressed)
			{
				menukeys.Add(MenuKeys.Left);
			}
			if (GetState.DPad.Right == ButtonState.Pressed)
			{
				menukeys.Add(MenuKeys.Right);
			}
			if (GetState.Buttons.B == ButtonState.Pressed || GetState.Buttons.Back == ButtonState.Pressed)
			{
				menukeys.Add(MenuKeys.Back);
			}

			return menukeys;
		}

		public List<InputKeys> GetInputKeys()
		{
			List<InputKeys> inputkeys = new List<InputKeys>();
			if (GetState.Buttons.A == ButtonState.Pressed)
			{
				inputkeys.Add(InputKeys.Jump);

			}
			if (GetState.ThumbSticks.Left.X > 0)
			{
				inputkeys.Add(InputKeys.Right);

			}
			if (GetState.ThumbSticks.Left.X < 0)
			{
				inputkeys.Add(InputKeys.Left);

			}
			if (GetState.Buttons.LeftShoulder == ButtonState.Pressed)
			{
				inputkeys.Add(InputKeys.Run);
			}

			return inputkeys;
		}

        public void Update(GameTime gametime,float run_factor,Vector2 playerPos)
        {
            gamepadstate = GamePad.GetState(PlayerIndex.One);
			if (gamepadstate.ThumbSticks.Left.X != 0)
			{
				if (gamepadstate.ThumbSticks.Left.X < 0 && factor > 0)
					factor = 0;
				if (gamepadstate.ThumbSticks.Left.X > 0 && factor < 0)
					factor = 0;
				factor += 5 * (float)gametime.ElapsedGameTime.TotalSeconds * gamepadstate.ThumbSticks.Left.X;
			}
			if (!(gamepadstate.DPad.Right == ButtonState.Pressed && gamepadstate.DPad.Left == ButtonState.Pressed))
			{
				if (gamepadstate.DPad.Left == ButtonState.Pressed)
				{
					if (factor > 0)
						factor = 0;
					factor -= 5 * (float)gametime.ElapsedGameTime.TotalSeconds;
				}
				if (gamepadstate.DPad.Right == ButtonState.Pressed)
				{
					if (factor < 0)
						factor = 0;
					factor += 5 * (float)gametime.ElapsedGameTime.TotalSeconds;
				}
			}
			if (gamepadstate.ThumbSticks.Left.X == 0)
				factor -= factor * (float)gametime.ElapsedGameTime.TotalSeconds;
			else if (gamepadstate.ThumbSticks.Left.X > 0.5 || gamepadstate.ThumbSticks.Left.X <= -0.5)
				thumbStickMenuTimeElapsedX += (float)gametime.ElapsedGameTime.TotalSeconds;
			else
				thumbStickMenuTimeElapsedX = MenuTimeThreshold;

			if (thumbStickMenuTimeElapsedX > MenuTimeThreshold)
			{
				thumbStickMenuTimeElapsedX = 0;
				leftRightMenu = true;
			}
			else
				leftRightMenu = false;

			if (gamepadstate.ThumbSticks.Left.Y >= -0.2 && gamepadstate.ThumbSticks.Left.Y <= 0.2)
				thumbStickMenuTimeElapsedY = MenuTimeThreshold;

			thumbStickMenuTimeElapsedY += (float)gametime.ElapsedGameTime.TotalSeconds * MathHelper.Clamp(gamepadstate.ThumbSticks.Left.Y * (gamepadstate.ThumbSticks.Left.Y < 0 ? -1 : 1), 0, 0.7f);
				if (thumbStickMenuTimeElapsedY > MenuTimeThreshold)
			{
				thumbStickMenuTimeElapsedY = 0;
				upDownMenu = true;
			}
			else
				upDownMenu = false;
			factor = MathHelper.Clamp(factor,-1,1);
            pos.X = playerPos.X + factor * Inputhandler.MAX_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds*run_factor;
            //pos.Y -= gamepadstate.ThumbSticks.Left.Y * MAX_SPEED * (float)gametime.ElapsedGameTime.TotalSeconds;
        }
    }
}
