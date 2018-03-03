using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

	public int version;
	public List<LevelData> levels = new List<LevelData>();

	public static void Save(GameData gameData) {
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "game_data.txt");
		GameData oldData = JsonUtility.FromJson<GameData>(System.IO.File.ReadAllText(filePath));
		gameData.version = oldData.version + 1;

		string json = JsonUtility.ToJson(gameData);
		Debug.Log("Save Json: " + json);
		System.IO.File.WriteAllText(filePath, json);
	}
	public static IEnumerator Load(System.Action<GameData> response) {
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "game_data.txt");

		string result;
		if (filePath.Contains("://") || filePath.Contains(":///")) {
			WWW www = new WWW(filePath);
			yield return www;
			result = www.text;
		}
		else
			result = System.IO.File.ReadAllText(filePath);

		response(JsonUtility.FromJson<GameData>(result));
	}
}

[System.Serializable]
public class LevelData {
	public List<StageData> stages = new List<StageData>();
}

[System.Serializable]
public class StageData {
	public string name;
	public List<StageObjectData> objects = new List<StageObjectData>();
}

[System.Serializable]
public class StageObjectData {
	public string name;
	public Vector2Int position;
	public Direction direction;
}