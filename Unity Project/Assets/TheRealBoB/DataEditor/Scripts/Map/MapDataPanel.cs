using UnityEngine;
using System.Collections;

namespace GameDataUI {
	public class MapDataPanel : MonoBehaviour {

		public MapTileDataInput mtDataInput;
		public TeamMemberInput tmPlayerDataInput;
		public TeamMemberInput tmAIDataInput;

		// Use this for initialization
		public void Init(MapData mapData) {
			mtDataInput.Init(mapData.penalties);
			tmPlayerDataInput.Init(mapData.teamUnits[0]);
			tmAIDataInput.Init(mapData.teamUnits[1]);
		}

		public void Refresh()
		{
			tmPlayerDataInput.Refresh();
			tmAIDataInput.Refresh();
		}
		
		public void Save() 
		{
			MapData map = new MapData();

			map.penalties = mtDataInput.GetPenalties();
			map.width = map.penalties.Length;
			map.height = map.penalties[0].Length;
			Debug.Log("Width: " + map.width + " height: " + map.height);
			map.teamUnits = new MapData.TeamUnit[2][];
			map.teamUnits[0] = tmPlayerDataInput.GetTeamUnits();
			map.teamUnits[1] = tmAIDataInput.GetTeamUnits();
			Debug.Log("TeamSize: " + map.teamUnits[0].Length + ", " + map.teamUnits[1].Length);

			Database.SetMapData(map);
			Debug.Log("MapData saved in Database");
		}
	}
}