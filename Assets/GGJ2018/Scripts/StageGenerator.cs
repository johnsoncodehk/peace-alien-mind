using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StageGenerator {

	public static Vector2Int stageSize = new Vector2Int(10, 10);

	// public static StageInfo Generate() {
	// 	StageInfo data;
	// 	// while (!TryGenerate(out data)) { }
	// 	TryGenerate(out data);
	// 	return data;
	// }
	// public static IEnumerator GenerateAsync() {
	// 	StageInfo data;
	// 	while (!TryGenerate(out data)) {
	// 		yield return new WaitForEndOfFrame();
	// 	}
	// }

	// 放置終點邏輯
	// 1. 最後接觸訊號的衛星,一定能接觸至少一個終點

	// private static bool TryGenerate(out StageInfo data) {
	// 	data = new StageInfo();
	// 	data.player = ObjectInfo.GetRandom(stageSize);

	// 	// while (data.satellites.Count < 2) {

	// 	// }
	// 	Vector2Int nextPos;
	// 	if (!TryGetRandomNextPosition(data.player.position, data.player.direction, out nextPos)) return false;

	// 	return true;
	// }
	private static bool TryGetRandomNextPosition(Vector2Int startPos, Direction dir, out Vector2Int nextPos) {
		nextPos = Vector2Int.zero;

		List<Vector2Int> allWorkPoss = GetWorkingPositions(startPos, dir);
		if (allWorkPoss.Count == 0) return false;

		nextPos = allWorkPoss[Random.Range(0, allWorkPoss.Count)];
		return true;
	}
	private static List<Vector2Int> GetWorkingPositions(Vector2Int startPos, Direction dir) {
		Vector2Int d = Vector2Int.zero;
		switch (dir) {
			case Direction.Up: d = new Vector2Int(0, 1); break;
			case Direction.UpLeft: d = new Vector2Int(-1, 1); break;
			case Direction.Left: d = new Vector2Int(-1, 0); break;
			case Direction.DownLeft: d = new Vector2Int(-1, -1); break;
			case Direction.Down: d = new Vector2Int(0, -1); break;
			case Direction.DownRight: d = new Vector2Int(1, -1); break;
			case Direction.Right: d = new Vector2Int(1, 0); break;
			case Direction.UpRight: d = new Vector2Int(1, 1); break;
		}
		List<Vector2Int> allWorkPoss = new List<Vector2Int>();
		Vector2Int currentPos = startPos + d;
		while (IsInSize(currentPos)) {
			allWorkPoss.Add(currentPos);
			currentPos += d;
		}
		return allWorkPoss;
	}
	private static bool IsInSize(Vector2Int pos) {
		return pos.x >= 0 && pos.y >= 0 && pos.x < stageSize.x && pos.y < stageSize.y;
	}
}
