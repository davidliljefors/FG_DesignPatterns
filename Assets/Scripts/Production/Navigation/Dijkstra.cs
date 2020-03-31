using System;
using UnityEngine;
using System.Collections.Generic;

namespace AI
{
	public class Dijkstra : IPathFinder
	{
		private HashSet<Vector2Int> accessible;

		public Dijkstra(IEnumerable<Vector2Int> accessibles)
		{
			accessible = new HashSet<Vector2Int>(accessibles);
		}

		public Dijkstra(Map map)
		{
			//accessible = map.GetWalkables();
		}


		public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
		{
			Dictionary<Vector2Int, Vector2Int?> ancestors = new Dictionary<Vector2Int, Vector2Int?>();
			Queue<Vector2Int> frontier = new Queue<Vector2Int>();
			
			Vector2Int current = start;

			ancestors.Add(current, null);
			frontier.Enqueue(current);

			// Loop through all accessible tiles
			while (frontier.Count > 0)
			{
				current = frontier.Dequeue();
				if (current == goal)
				{ break; }

				foreach (Vector2Int dir in Tools.DirectionTools.Dirs)
				{
					Vector2Int test = current + dir;
					if (accessible.Contains(test) && !ancestors.ContainsKey(test))
					{
						frontier.Enqueue(test);
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

				//We have path
				Debug.Log("Found path");
				path.Reverse();
				return path;
			}
			//We have no path
			Debug.Log("No path found");
			return accessible;
		}
	}
}
