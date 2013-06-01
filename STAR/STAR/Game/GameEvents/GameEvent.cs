using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Level;
using Star.GameManagement;
using Star.GameManagement.Gamestates;

namespace Star.Game.GameEvents
{
    public enum EventAction
    { 
        Nothing,
        BlockInput,
		BlockUpdate,
        Exit
    }

    public enum DrawPosition
    { 
        BehindLevel,
        InFrontOfLevel,
        Post,
    }

    public abstract class GameEvent : IDisposable
    {
        #region Fields

        bool enabled = false;
        bool initialized = false;
        ContentManager content;
        DrawPosition drawposition;

        #endregion

        #region Properties

        public ContentManager Content
        {
            get { return content; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public DrawPosition DrawPosition
        {
            get { return drawposition; }
        }

        #endregion

        #region Private Members

        private void FinishInitialization()
        {
            initialized = true;
        }

        #endregion

        #region Public Members

        public void Initialize(IServiceProvider serviceProvider,GraphicsDevice device,LevelVariables levelVariables,Options options)
        {
            content = new ContentManager(serviceProvider, "Data");
            drawposition = InitializeEvent(serviceProvider,device, levelVariables,options);
            FinishInitialization();
        }

		public void CheckActivation(GameTime gameTime, Rectangle playerBoundingBox, int timeLeft, Quadtree quadtree, SGame stateGame)
        {
            enabled = CheckActvationEvent(gameTime, playerBoundingBox, timeLeft,quadtree,stateGame);
        }

        public EventAction Update(GameTime gameTime,List<Enemy.Enemy> enemies)
        {
            EventAction action = EventAction.Nothing;
            if (initialized && enabled)
            {
                action = UpdateEvent(gameTime,enemies);
            }
            return action;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix)
        {
            if (initialized && enabled)
            {
                DrawEvent(gameTime, spriteBatch, matrix);
            }
        }

        #endregion

        #region Abstract Members

        protected abstract bool CheckActvationEvent(GameTime gameTime, Rectangle playerBoundingBox, int timeLeft,Quadtree quadtree,SGame stateGame);
        protected abstract DrawPosition InitializeEvent(IServiceProvider serviceProvider,GraphicsDevice device,LevelVariables levelVariables,Options options);
        protected abstract void DrawEvent(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix);
        protected abstract EventAction UpdateEvent(GameTime gameTime,List<Enemy.Enemy> enemies);
        public abstract void Start();
        #endregion

		#region IDisposable Member

		public abstract void Dispose();

		#endregion
	}
}
