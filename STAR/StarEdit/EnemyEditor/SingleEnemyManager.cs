using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Star.Game.Enemy;
using Star.GameManagement;

namespace StarEdit.EnemyEditor
{
    public class SingleEnemyManager
    {
        ContentManager content;
        Dictionary<string, Dictionary<string, Texture2D>> textures;
		Enemy enemy;

		public Enemy Enemy
		{
			get { return enemy; }
		}
        Vector2 pos;

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

		public string Type
		{
			get 
			{
				if (enemy != null)
					return enemy.Type;
				else
					return "";
			}
		}

		public Star.Game.Enemy.Enemy.StandardDirection StandardDirection
		{
			get 
			{
				if (enemy != null)
					return enemy.RunDirection;
				else
					return Enemy.StandardDirection.Left;
			}
		}

        public SingleEnemyManager(IServiceProvider serviceProvider)
        {
            content = new ContentManager(serviceProvider, "Data");
            textures = new Dictionary<string, Dictionary<string, Texture2D>>();
            pos = new Vector2();
        }

        public void LoadEnemy(string type)
        {
            enemy = new Enemy();
            enemy.Initialize(type,content.ServiceProvider,0,new Options());
            if (!textures.ContainsKey(type))
            {
                LoadTextures(enemy.Variables, type);
            }
        }

        public void Update(Vector2 pos)
        {
            this.pos = pos;
            if(enemy!= null)
                enemy.Pos = pos;
        }

        private void LoadTextures(Dictionary<Star.Game.Enemy.Enemy.EnemyVariables, string> dict, string name)
        {
            textures.Add(name, new Dictionary<string, Texture2D>());
            string path = GameConstants.EnemiesPath + name + "/";
            string[] rectangles;
            rectangles = dict[Enemy.EnemyVariables.AnimRectangles].Split(',');

            for (int i = 0; i < rectangles.Length; i++)
            {
                rectangles[i] = rectangles[i].Trim();
            }
            textures[name] = new Dictionary<string, Texture2D>();
            foreach (string rect in rectangles)
            {
                try
                {
                    textures[name].Add(rect, content.Load<Texture2D>(path + rect));
                }
                catch
                {
                    textures[name].Add(rect, content.Load<Texture2D>("Stuff/Error"));
                }
            }
        }

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix matrix)
		{
			if (enemy != null)
				//3_1
				//enemy.Draw(gameTime, spriteBatch, matrix, textures[enemy.Type],null,null,null);
				enemy.Draw(gameTime, spriteBatch, matrix, textures[enemy.Type], null, null);
		}

    }
}
