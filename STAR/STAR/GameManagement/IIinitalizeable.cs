using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Star.Game.Level;

namespace Star.GameManagement
{
	public interface IIsInitialized
	{
		bool Initialized { get; }
	}

	public interface IInitializeable:IIsInitialized
	{
		void Initialize(Options options);
	}

	public interface ILevelContent: IIsInitialized
	{
		void Initialize(Options options, LevelVariables levelVariables);
	}
}
