using System;
using UnityEngine;
using System.Collections.Generic;

namespace MapTools
{
	public class Dijkstra : IPathFinder
	{
		private HashSet<Vector2Int> accessible;

		public Dijkstra(IEnumerable<Vector2Int> accessibles)
		{
			accessible = new HashSet<Vector2Int>(accessibles);
		}

		public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
		{
			Dictionary<Vector2Int, Vector2Int?> ancestors = new Dictionary<Vector2Int, Vector2Int?>();
			Queue<Vector2Int> positionsToEvaluate = new Queue<Vector2Int>();
			
			Vector2Int current = start;

			ancestors.Add(current, null);
			positionsToEvaluate.Enqueue(current);

			// Loop through all accessible tiles
			while (positionsToEvaluate.Count > 0)
			{
				current = positionsToEvaluate.Dequeue();
				if (current == goal)
				{ break; }

				foreach (Vector2Int dir in Tools.DirectionTools.Dirs)
				{
					Vector2Int test = current + dir;
					if (accessible.Contains(test) && !ancestors.ContainsKey(test))
					{
						positionsToEvaluate.Enqueue(test);
						ancestors.Add(test, current);
					}
				}
			}

			if (ancestors.ContainsKey(goal))
			{
				List<Vector2Int> path = new List<Vector2Int>();
				for(Vector2Int? step = ancestors[goal]; step != null; step = ancestors[step.Value])
				{
					path.Add(step.Value);
				}
				path.Reverse();
				return path;
			}
			return accessible;
		}
	}
}
