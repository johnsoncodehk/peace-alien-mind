using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameConfig {

	[System.Serializable]
	public class GameDataUpdate {
		public bool enabled;
		public string url;
	}

	public GameDataUpdate gameDataUpdate;
	public bool debugMode;

	public static string fileName = "game_config.json";

	public static IEnumerator Load(System.Action<GameConfig> response) {
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
		string result;
		if (filePath.Contains("://") || filePath.Contains(":///")) {
			WWW www = new WWW(filePath);
			yield return www;
			result = www.text;
		}
		else
			result = System.IO.File.ReadAllText(filePath);

		GameConfig config = JsonUtility.FromJson<GameConfig>(result);
#if UNITY_EDITOR
		config.debugMode = true;
#endif
		response(config);
	}
}
