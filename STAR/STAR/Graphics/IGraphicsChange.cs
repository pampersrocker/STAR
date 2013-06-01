using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Star.GameManagement;
using Microsoft.Xna.Framework.Graphics;

namespace Star.Graphics
{
	public interface IGraphicsChange
	{
		void GraphicsChanged(GraphicsDevice device,Options options);
		void UnloadGraphicsChanged();
	}
}
