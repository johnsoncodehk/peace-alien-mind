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

	public enum State {
		None,
		Playing,
		GameOver,
	}

	public Tilemap tilemap;
	public LineRenderer line;
	public List<Satellite> satelites = new List<Satellite>();
	public State state = State.None;

	private List<Planet> m_Planets = new List<Planet>();
	private Vector2Int size = new Vector2Int(12, 8);
	private List<LineRenderer> m_Lines = new List<LineRenderer>();
	private float m_LineSpeed;

	void Start() {
		this.UpdateBoardStyle(GameManager.instance.settingsPanel.boardStyle);
		this.line.gameObject.SetActive(false);
	}
	void Update() {
		if (this.m_Lines.Count > 0) {
			float w = Mathf.SmoothDamp(this.m_Lines[0].widthMultiplier, this.line.widthMultiplier, ref this.m_LineSpeed, 0.2f);
			foreach (LineRenderer line in this.m_Lines) {
				line.widthMultiplier = w;
			}
		}
		if (this.state == State.Playing) {
			if (this.IsWin()) {
				this.state = State.GameOver;
				StartCoroutine(this.WinAction());
			}
			else if (this.IsLose()) {
				this.state = State.GameOver;
				StartCoroutine(this.LoseAction());
			}
		}
	}

	public void OnStartGame() {
		this.m_Planets = this.GetComponentsInChildren<Planet>().ToList();
		this.state = State.Playing;
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

	private bool IsLose() {
		return GameManager.instance.player.energys.remain == 0
			&& !FindObjectOfType<Signal>()
			&& !this.GetComponentsInChildren<Satellite>().ToList().Any(s => s.hasSignal);
	}
	private IEnumerator LoseAction() {
		yield return new WaitForSeconds(2);
		GameManager.instance.NextStage(0);
	}
	private bool IsWin() {
		foreach (var end in this.m_Planets) {
			if (!end.isClear) return false;
		}
		return true;
	}
	private IEnumerator WinAction() {
		float score = Mathf.Clamp(5.5f + GameManager.instance.player.energys.remain * 0.5f, 0, 10);
		foreach (Planet planet in this.m_Planets) {
			var newLine = Instantiate(this.line);
			newLine.transform.SetParent(this.transform, false);
			newLine.gameObject.SetActive(true);
			newLine.positionCount = planet.receiverSignalPositions.Count;
			newLine.widthMultiplier = 0;
			newLine.SetPositions(planet.receiverSignalPositions.Select(p => new Vector3(p.x, p.y, 0)).ToArray());
			this.m_Lines.Add(newLine);
		}

		yield return new WaitForSeconds(2);
		GameManager.instance.NextStage(score);
		if (GameManager.instance.gameConfig.debugMode) {
			GameManager.instance.gameMessage.ShowMessage("Add Score: " + score);
		}
	}
}
