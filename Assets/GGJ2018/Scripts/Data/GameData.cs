using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {

	public int version;
	public List<LevelData> levels = new List<LevelData>();

	public static string fileName = "game_data.json";

	public static void Save(GameData gameData) {
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
		GameData oldData = JsonUtility.FromJson<GameData>(System.IO.File.ReadAllText(filePath));
		gameData.version = oldData.version + 1;

		string json = JsonUtility.ToJson(gameData);
		Debug.Log("Save Json: " + json);
		System.IO.File.WriteAllText(filePath, json);
	}
	public static IEnumerator Load(GameConfig.GameDataUpdate updateConfig, System.Action<GameData, int> response) {
		int error = 0; // 成功
#if UNITY_EDITOR
		error = -1; // Editor模式不下載
#else
		if (updateConfig.enabled) {
			WWW download;
			if (GetWWW(updateConfig.url, out download)) {
				yield return download;
				if (download.error == null) {
					try {
						GameData data = JsonUtility.FromJson<GameData>(download.text);
						if (data.version > 0 && data.levels.Count != 0) {
							response(data, error);
							yield break;
						}
						else {
							error = 4; // 不是有效的GameData
						}
					}
					catch {
						error = 3; // Json parse fail
					}
				}
				else {
					error = 2; // 下載失敗
				}
			}
			else {
				error = 1; // Url無效
			}
		}
#endif

		// 後備
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
		string result;
		if (filePath.Contains("://") || filePath.Contains(":///")) {
			WWW www = new WWW(filePath);
			yield return www;
			result = www.text;
		}
		else
			result = System.IO.File.ReadAllText(filePath);

		response(JsonUtility.FromJson<GameData>(result), error);
	}
	public static bool GetWWW(string downUrl, out WWW www) {
		try {
			www = new WWW(downUrl);
			return true;
		}
		catch {
			www = null;
			return false;
		}
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