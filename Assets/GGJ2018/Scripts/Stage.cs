using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Stage), true)]
public class StageInspector : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		Stage stage = this.target as Stage;
		if (GUILayout.Button("Up")) {
			foreach (var t in stage.GetComponentsInChildren<GridTransform>()) {
				if (t.GetComponent<Satellite>()) continue;
				t.position += Vector2Int.up;
				EditorUtility.SetDirty(t);
				t.OnValidate();
			}
		}
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Left")) {
			foreach (var t in stage.GetComponentsInChildren<GridTransform>()) {
				if (t.GetComponent<Satellite>()) continue;
				t.position += Vector2Int.left;
				EditorUtility.SetDirty(t);
				t.OnValidate();
			}
		}
		if (GUILayout.Button("Right")) {
			foreach (var t in stage.GetComponentsInChildren<GridTransform>()) {
				if (t.GetComponent<Satellite>()) continue;
				t.position += Vector2Int.right;
				EditorUtility.SetDirty(t);
				t.OnValidate();
			}
		}
		EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Down")) {
			foreach (var t in stage.GetComponentsInChildren<GridTransform>()) {
				if (t.GetComponent<Satellite>()) continue;
				t.position += Vector2Int.down;
				EditorUtility.SetDirty(t);
				t.OnValidate();
			}
		}
	}
}
#endif

public class Stage : MonoBehaviour {

	public Tilemap tilemap;
	public List<Satellite> satelites = new List<Satellite>();

	private List<Planet> m_Planets = new List<Planet>();
	private Vector2Int size = new Vector2Int(12, 8);

	void Start() {
		this.UpdateBoardStyle(GameManager.instance.settingsPanel.boardStyle);
	}

	public void OnStartGame() {
		this.m_Planets = this.GetComponentsInChildren<Planet>().ToList();
	}
	public bool IsWin() {
		foreach (var end in this.m_Planets) {
			if (!end.isClear) return false;
		}
		return true;
	}
	public bool InBorder(Vector2Int pos) {
		Vector2 sizeF = (Vector2)this.size * 0.5f;
		return pos.x < sizeF.x && pos.x >= -sizeF.x && pos.y < sizeF.y && pos.y >= -sizeF.y;
	}
	public void UpdateBoardStyle(int style) {
		Grid grid = this.GetComponentInChildren<Grid>();
		for (int i = 0; i < grid.transform.childCount; i++) {
			grid.transform.GetChild(i).gameObject.SetActive(i == style);
		}
	}
}
