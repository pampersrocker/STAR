using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Star.Input;

namespace Star.Game
{
    public struct DrawVector
    {
        public Vector2 vector;
        public Vector2 startpos;
        public VertexPositionColor[] vertices;
        public void setvertices(Vector2 new_vector)
        {
            vector = new_vector;
            vertices[0].Position = new Vector3(startpos, 0);
            vertices[1].Position = new Vector3(startpos + vector, 0);
            vertices[2].Position = new Vector3(((float)0.9 * vector), 0);
            vertices[3].Position = new Vector3(((float)0.9 * vector), 0);
        }
    }

    public class DebugScreen
    {
        DrawVector player_pos;
        DrawVector player_speed;
        DrawVector gravity;
        DrawVector jump;
        DrawVector gamepad_pos;
        string framerate_string;
        string collision_state_string;
        float run_factor = 1;
        string inputkeys_string;
        int fpsUpdater;
        SpriteFont font;
		DateTime starttime;

        public DebugScreen(ContentManager content,Vector2 new_player_pos)
        {
            Vector2 temp = new Vector2(1366 / 2, 768 / 2);
            font = content.Load<SpriteFont>("Stuff/Arial");
            player_pos.vector = Vector2.Zero;
            player_pos.vertices = new VertexPositionColor[4];
            player_speed.vertices = new VertexPositionColor[4];
            player_speed.startpos =temp;
            gravity.vertices = new VertexPositionColor[4];
            gravity.startpos =temp;
            jump.vertices = new VertexPositionColor[4];
            jump.startpos =temp;
            gamepad_pos.vertices = new VertexPositionColor[4];
            gamepad_pos.startpos =temp;
            for(int i=0;i<4;i++)
            {
               player_pos.vertices[i] = new VertexPositionColor(Vector3.Zero,Color.White);
               player_speed.vertices[i] = new VertexPositionColor(Vector3.Zero,Color.Blue);
               gravity.vertices[i] = new VertexPositionColor(Vector3.Zero,Color.Red);
               jump.vertices[i] = new VertexPositionColor(Vector3.Zero,Color.Green);
               gamepad_pos.vertices[i] = new VertexPositionColor(Vector3.Zero,Color.HotPink);
            }
            collision_state_string = "Collision:";
            framerate_string = "FPS:";
            inputkeys_string = "Input:";
            player_speed.vector = Vector2.Zero;
            gravity.vector = Vector2.Zero;
            jump.vector = Vector2.Zero;
            gamepad_pos.vector = Vector2.Zero;
        }

        public void update(DateTime start,Vector2 new_player_pos,Vector2 new_player_speed,Vector2 new_gravity,Vector2 new_jump,Vector2 new_gamepad_pos,List<CollisionType> new_collision,float new_run_factor,List<InputKeys> inputkeys)
        {
            run_factor = new_run_factor;
            player_pos.vector = new_player_pos;
            player_speed.setvertices( new_player_speed);
            gravity.setvertices( new_gravity);
            jump.setvertices( new_jump);
            inputkeys_string = "Input: ";
            foreach (InputKeys input in inputkeys)
            {
                inputkeys_string += input.ToString() + "; ";
            }
            collision_state_string = "Collision: ";
            foreach (CollisionType collision in new_collision)
            {
                collision_state_string += collision.ToString() + "; ";
            }
			starttime = start;
            
        }

		public void draw(SpriteBatch spritebatch, GraphicsDevice graphics, BasicEffect effect, Matrix scale)
		{
			//3_1
			//spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.SaveState);
			spritebatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
			spritebatch.DrawString(font, collision_state_string, new Vector2(10, 100), Color.Red);

			spritebatch.DrawString(font, "Player Pos: " + player_pos.vector.ToString(), new Vector2(10, 140), Color.Red);
			spritebatch.DrawString(font, "Player Speed: " + player_speed.vector.ToString(), new Vector2(10, 160), Color.Red);
			spritebatch.DrawString(font, "Run_Factor: " + run_factor.ToString(), new Vector2(10, 180), Color.Red);
			spritebatch.DrawString(font, "BackBuffer: " + graphics.PresentationParameters.BackBufferWidth.ToString() + "*" + graphics.PresentationParameters.BackBufferHeight.ToString(), new Vector2(10, 200), Color.Red);
			spritebatch.DrawString(font, inputkeys_string, new Vector2(10, 80), Color.Red);
			spritebatch.End();
			spritebatch.Begin(
				 SpriteSortMode.Texture,
				 null, null, null, null, effect);
			effect.VertexColorEnabled = true;
			//effect.Begin();
			//foreach (EffectPass pass in effect.CurrentTechnique.Passes)
			//{
			//    pass.Begin();
			//graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, gravity.vertices, 0, 1);
			//graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, player_speed.vertices, 0, 1);
			//graphics.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, jump.vertices, 0, 1);
			//    pass.End();
			//}
			//effect.End();
			spritebatch.End();
			spritebatch.Begin();
			int ms = (int)Math.Round((DateTime.Now - starttime).TotalMilliseconds);
			fpsUpdater++;
			if (fpsUpdater > 60)
			{
				fpsUpdater = 0;
				framerate_string = "FPS: " + Math.Round(1000f / ms).ToString() + " ms:" + ms.ToString();
			}
			spritebatch.DrawString(font, framerate_string, new Vector2(10, 120), Color.Red);
			spritebatch.End();

		}
    }
}
