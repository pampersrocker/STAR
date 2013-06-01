using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Star.Game;
using Star.Game.Level;
using Star.GameManagement;

namespace Star.Input
{
    public enum Controller
    { 
        Keyboard,
        Xbox_360_Controller
    }

    public enum RunDirection
    {
        Right,
        Left,
        None
    }

    public enum InputKeys
    { 
        Left = 0,
        Right,
        Jump,
        Run
    }

    public enum MenuKeys
    { 
        Left,
        Right,
        Up,
        Down,
        Back,
        Enter
    }

    public class Inputhandler
    {
        RunDirection rundirection = RunDirection.Right;
        public const float MAX_SPEED = 3000.0f;
        Vector2 pos;
        Vector2 speed;
        Controller controller = Controller.Keyboard;
        Gamepadhandler gamepadhandler;
        KeyboardHandler keyboardhandler;
        List<InputKeys> inputkeys;
        List<MenuKeys> menukeys;
        List<MenuKeys> oldmenukeys;
        List<InputKeys> oldinputkeys;
		Dictionary<MenuKeys, float> newPressDelay;
		float NewPressThreshold = 0.2f;
		float newPressSpeedDivider= 1;
        GamePadButtons gamepadstate;
        Keys[] keyboardstate;
        float run_factor = GameParameters.minRunFactor;
        float max_run_factor = 1.5f;

        public float RunFactor
        {
            get { return run_factor; }
            set{ run_factor = value; }
        }

        public RunDirection RunDirection
        {
            get { return rundirection; }
        }

        public Vector2 Pos
        {
            get { return pos; }
            set 
            {
                pos = value;
                gamepadhandler.Pos = value;
                keyboardhandler.Pos = value;
            }
        }

        public List<InputKeys> GetInputKeys
        {
            get { return inputkeys; }
        }

        public List<InputKeys> GetOldInputkeys
        {
            get { return oldinputkeys; }
        }

        public List<MenuKeys> GetMenuKeys
        {
            get { return menukeys; }
        }

        public List<MenuKeys> GetOldMenuKeys
        {
            get { return oldmenukeys; }
        }

        public List<MenuKeys> GetNewPressedMenuKeys
        {
            get
            {
                List<MenuKeys> keys = new List<MenuKeys>();
                foreach (MenuKeys key in menukeys)
                {
                    if (!oldmenukeys.Contains(key))
                    {
                        keys.Add(key);
                    }
					if (newPressDelay.ContainsKey(key))
					{
						if (newPressDelay[key] > NewPressThreshold/newPressSpeedDivider)
						{
							keys.Add(key);
						}
					}
                }
				//List<MenuKeys> collection = new List<MenuKeys>();
				//collection.AddRange(newPressDelay.Keys.ToList());
				//foreach (MenuKeys key in collection)
				//{
				//    if (newPressDelay[key] > NewPressThreshold)
				//    {
				//        keys.Add(key);
				//        newPressDelay[key] = 0;
				//    }
				//}
                return keys;
            } 
        }

        public Inputhandler(Vector2 player_pos,Options options)
        {
			newPressDelay = new Dictionary<MenuKeys, float>();
			options.ControllerChanged += new ControllerChangedEventHandler(options_ControllerChanged);
			controller = options.Controller;
            pos = player_pos;
            speed = Vector2.Zero;
            gamepadhandler = new Gamepadhandler(player_pos);
            keyboardhandler = new KeyboardHandler(player_pos);
            menukeys = new List<MenuKeys>();
            oldmenukeys = new List<MenuKeys>();
            inputkeys = new List<InputKeys>();
            oldinputkeys = new List<InputKeys>();
        }

		void options_ControllerChanged(Options options, Controller controller)
		{
			this.controller = controller;	
		}

        private void SetOldState()
        {
            oldinputkeys = inputkeys.ToList<InputKeys>();
            oldmenukeys = menukeys.ToList<MenuKeys>();
        }

        public void Update(GameTime gametime,Vector2 playerPos)
        {
            SetOldState();
            inputkeys.Clear();
            menukeys.Clear();
			gamepadhandler.Update(gametime, run_factor, playerPos);
			keyboardhandler.Update(gametime, run_factor, playerPos);
			keyboardstate = keyboardhandler.getDownKeys;
			gamepadstate = gamepadhandler.GetState.Buttons;
            switch (controller)
            {
                case(Controller.Keyboard):
                    pos = keyboardhandler.Pos;
					inputkeys.AddRange(keyboardhandler.GetInputKeys()); 
                    break;
                case(Controller.Xbox_360_Controller):
                    pos = gamepadhandler.Pos;
					inputkeys.AddRange(gamepadhandler.GetInputKeys());
                    break;
            }
            if (inputkeys.Contains(InputKeys.Right) && inputkeys.Contains(InputKeys.Left))
            {
				while (inputkeys.Contains(InputKeys.Left))
					inputkeys.Remove(InputKeys.Left);
				while (inputkeys.Contains(InputKeys.Right))
					inputkeys.Remove(InputKeys.Right);
            }
			CheckMenuKeys(gametime.GetElapsedTotalSecondsFloat());
        }

		private void CheckMenuKeys(float elapsedGameTime)
		{
			menukeys.AddRange(gamepadhandler.GetMenuKeys());
			menukeys.AddRange(keyboardhandler.GetMenuKeys());
			List<MenuKeys> collection = new List<MenuKeys>();
			collection.AddRange(newPressDelay.Keys.ToList());
			foreach (MenuKeys key in collection)
			{
				if (!menukeys.Contains(key))
				{
					newPressDelay.Remove(key);
				}
				else if (newPressDelay[key] > NewPressThreshold/newPressSpeedDivider)
				{
					newPressDelay[key] = 0;
					newPressSpeedDivider+=0.05f;
					newPressSpeedDivider = MathHelper.Clamp(newPressSpeedDivider, 1, 5f);
				}
			}
			foreach (MenuKeys key in menukeys)
			{
				if (oldmenukeys.Contains(key))
				{
					if (newPressDelay.ContainsKey(key))
					{
						newPressDelay[key] += elapsedGameTime;
					}
					else
					{
						newPressDelay.Add(key, elapsedGameTime);
					}
				}
			}
			if (newPressDelay.Count == 0)
				newPressSpeedDivider = 1;


		}

        public void UpdateRunFactor(GameTime gametime,List<CollisionType> collision)
        {
            if (collision.Contains(CollisionType.WalksAgainstIt) || collision.Contains(CollisionType.JumpsAgainstIt))
            {
                //run_factor = GameParameters.minRunFactor;
            }
            else
            {
                if (inputkeys.Contains(InputKeys.Run))
                {
                    max_run_factor = 2.25f;
                }
                else
                {
                    max_run_factor = 1.5f;
                }
                if (rundirection == RunDirection.Right &&
                    inputkeys.Contains(InputKeys.Right))
                {
					run_factor += ((float)gametime.ElapsedGameTime.TotalSeconds / GameParameters.RunFactorCoefficient) * (max_run_factor / 1.5f);
					run_factor = MathHelper.Clamp(run_factor, GameParameters.minRunFactor, max_run_factor);
                }
                else if (rundirection == RunDirection.Left &&
                    inputkeys.Contains(InputKeys.Right))
                {
					run_factor = GameParameters.minRunFactor;
                    rundirection = RunDirection.Right;
                }
                else if (rundirection == RunDirection.Right &&
                    inputkeys.Contains(InputKeys.Left))
                {
					run_factor = GameParameters.minRunFactor;
                    rundirection = RunDirection.Left;
                }
                else if (rundirection == RunDirection.Left &&
                    inputkeys.Contains(InputKeys.Left))
                {
                    run_factor += ((float)gametime.ElapsedGameTime.TotalSeconds / GameParameters.RunFactorCoefficient) * (max_run_factor / 1.5f);
					run_factor = MathHelper.Clamp(run_factor, GameParameters.minRunFactor, max_run_factor);
                }
                else if (!inputkeys.Contains(InputKeys.Left) && !inputkeys.Contains(InputKeys.Right))
                {
					run_factor -= ((float)gametime.ElapsedGameTime.TotalSeconds / GameParameters.RunFactorCoefficient);
					run_factor = MathHelper.Clamp(run_factor, GameParameters.minRunFactor, max_run_factor);
                }
                else
                {
					//run_factor = GameParameters.minRunFactor;
                }
            }
        }

    }
}
