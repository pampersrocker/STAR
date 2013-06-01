using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Star.GameManagement;
using Star.Graphics;
using Star.Game;
namespace Star
{
	
	public enum EGameState
	{ 
		Intro,
		Menu,
		Game,
		Loading,
		Credits,
		Quit
	}

	public interface IExtendedDisposable: IDisposable
	{
		bool Disposed { get; }
	}

	public interface IGameState : IDisposable, IGraphicsChange, IInitializeable
	{
		EGameState Update(GameTime gameTime,Input.Inputhandler inputhandler,Options options);
		void Draw(GameTime gametime,SpriteBatch spritebatch,GraphicsDevice graphics);
		void Unload();
	}

	public class InitObjectHolder
	{
		public GraphicsDevice graphics;
		public Dictionary<EGameState,XNAParticleSystem.ParticleManager> particleManagers;
		public IServiceProvider serviceProvider;
		public DataHolder dataHolder;

		public InitObjectHolder(GraphicsDevice graphics, XNAParticleSystem.ParticleManager particleManager,IServiceProvider serviceProvider)
		{
			this.graphics         = graphics;
			this.particleManagers = new Dictionary<EGameState, XNAParticleSystem.ParticleManager>();
			foreach (EGameState curGameState in Enum.GetValues(typeof(EGameState)))
			{
				particleManagers.Add(curGameState, new XNAParticleSystem.ParticleManager());
			}
			this.serviceProvider  = serviceProvider;
			dataHolder            = new DataHolder();
		}
	}

	

}