using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarPathFinding
{
	public enum Walkable
	{
		Walkable,
		Blocked,
		Platform,
		NotReachable
	}

	public enum NodeState
	{ 
		Unknown,
		Known,
		Closed
	}
}
