using System;
namespace Star.Input
{
	interface IGameController
	{
		System.Collections.Generic.List<InputKeys> GetInputKeys();
		System.Collections.Generic.List<MenuKeys> GetMenuKeys();
		void Initialize(Star.GameManagement.Options options);
		Microsoft.Xna.Framework.Vector2 Pos { get; set; }
		void Update(Microsoft.Xna.Framework.GameTime gametime, float run_factor, Microsoft.Xna.Framework.Vector2 playerPos);
	}
}
