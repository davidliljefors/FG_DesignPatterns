using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class Map
	{
		struct MapSetting
		{
			List<List<TileType>> tiles;
			List<int> unitCount;
		}
		public List<List<TileType>> MapArray { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }

		private Map()
		{
			MapArray = new List<List<TileType>>();
			MapArray.Add(new List<TileType>());
		}

		//public static Map ParseMap(in string filePath)
		//{
		//	if(File.Exists(filePath))
		//	{
		//		Debug.Log("File Exist");
		//		StreamReader sr = File.OpenText(filePath);
		//		string text = sr.ReadToEnd();
		//		Map map = ParseMap(Encoding.ASCII.GetBytes(text));
		//		return map;
		//	}
		//	return null;
		//}

		public static Map ParseMap(in string text)
		{
			Map map = ParseMap(Encoding.Default.GetBytes(text));
			return map;
		}

		public static Map ParseMap(byte[] data)
		{
			Map map = new Map();
			var tileTypes = TileMethods.TypeById;

			int row = 0;
			foreach (byte b in data)
			{
				if (b == '#')
				{ break; }
				if (b == '\n')
				{
					map.MapArray.Add(new List<TileType>());
					++row; 
					continue; 
				}
				map.MapArray[row].Add(tileTypes[b-'0']);
			}

			Debug.Log(map.MapArray.Count + " x " + map.MapArray[0].Count);
			
			return map;
		}

		public TileType GetTile(Vector2Int pos)
		{
			return MapArray[pos.x][pos.y];
		}

		public IEnumerable<Vector2Int> GetWalkables()
		{
			List<Vector2Int> list = new List<Vector2Int>();
			return list;
		}
	}
}