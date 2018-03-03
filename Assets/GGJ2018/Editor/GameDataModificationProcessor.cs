using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MyAssetModificationProcessor : AssetModificationProcessor {

	public static string[] OnWillSaveAssets(string[] paths) {
		foreach (string path in paths) {
			if (path.Contains(".unity") && path.Contains("Level_")) {
				SaveGameData();
			}
		}
		return paths;
	}

	public static void SaveGameData() {
		GameData gameData = new GameData();

		for (int i = 1; i <= 3; i++) {
			var level = EditorSceneManager.OpenScene("Assets/GGJ2018/Scenes/Levels/Level_" + i + ".unity", OpenSceneMode.Additive);
			LevelData levelData = new LevelData();

			foreach (GameObject go in level.GetRootGameObjects()) {
				Stage stage = go.GetComponent<Stage>();
				StageData stageData = new StageData();

				stageData.name = go.name;
				stageData.objects = new List<StageObjectData>();
				foreach (GridTransform tile in stage.GetComponentsInChildren<GridTransform>()) {
					StageObjectData stageObjectData = new StageObjectData();

					stageObjectData.name = PrefabUtility.GetPrefabParent(tile).name;
					stageObjectData.position = tile.position;
					stageObjectData.direction = tile.direction;
					stageData.objects.Add(stageObjectData);
				}
				levelData.stages.Add(stageData);
			}
			gameData.levels.Add(levelData);
			EditorSceneManager.CloseScene(level, true);
		}
		GameData.Save(gameData);
	}
}