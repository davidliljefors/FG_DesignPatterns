using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MapTools
{
	public static class MapParser
	{
		private static readonly char splitCharacter = '#';
		public static MapInfo Parse(string text)
		{
			// Split string into necessary parts
			string mapData = text.Split(splitCharacter)[0];

			string enemyData = text.Split(splitCharacter)[1];

			TileType[,] tiles;

			//Loop over the map layout
			{
				string[] rows = mapData.Split(new char[] { '\n', (char)13 }, System.StringSplitOptions.RemoveEmptyEntries);
				Assert.IsNotNull(rows);
				Assert.IsNotNull(rows[0]);

				tiles = new TileType[rows.Length, rows[0].Length];

				for (int i = 0; i < rows.Length; ++i)
				{
					for (int j = 0; j < rows[i].Length; ++j)
					{
						if(TileMethods.TypeByChar.TryGetValue(rows[i][j], out TileType tile))
						{
							tiles[i, j] = tile;
						}
						else
						{
							// Question to Ederic, thorw Exception or Debug.LogError?
							Debug.LogError("Unexpected Map format!");
							throw new System.InvalidOperationException("Cannot parse map data");
						}
					}
				}
			}

			List<UnitWave> unitWaves = new List<UnitWave>();
			// Loop over the enemy counts
			{
				string[] rows = enemyData.Split(new char[] { '\n', (char)13 }, System.StringSplitOptions.RemoveEmptyEntries);

				// One Wave per line
				for (int i = 0; i < rows.Length; ++i)
				{
					string[] unitCounts = rows[i].Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
					UnitWave wave = new UnitWave();

					int typeID = 0;
					foreach (string unitCount in unitCounts)
					{
						if (int.TryParse(unitCount, out int result))
						{
							wave.Units.Add(UnitMethods.TypeById[typeID], int.Parse(unitCount));
						}
						else
						{
							// Question to Ederic, thorw Exception or Debug.LogError?
							Debug.LogError("Unexpected Map format!");
							throw new System.InvalidOperationException("Cannot parse map data");
						}
						typeID++;
					}
					unitWaves.Add(wave);
				}
			}

			// Todo check if map settings works
			MapInfo info = new MapInfo(tiles, unitWaves);

			return info;
		}
	}
}