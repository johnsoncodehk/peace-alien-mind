using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Planet), true)]
public class UIControllerInspector : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		Planet t = this.target as Planet;
		if (GUILayout.Button("Level Up")) {
			t.LevelUp();
		}
		if (GUILayout.Button("Level Down")) {
			t.LevelDown();
		}
	}
}
#endif

public class Planet : MonoBehaviour {

	public List<GameObject> levelObjects = new List<GameObject>();

	public int level {
		get { return this.m_Level; }
		set {
			this.m_Level = Mathf.Clamp(value, 0, this.levelObjects.Count - 1);
			for (int i = 0; i < this.levelObjects.Count; i++) {
				this.levelObjects[i].SetActive(i == this.level);
			}
		}
	}

	private int m_Level;
	private Animator m_Animator;

	void Awake() {
		this.m_Animator = this.GetComponent<Animator>();

		this.level = Random.Range(0, this.levelObjects.Count);
	}

	public void LevelUp() {
		this.m_Animator.Play("planet_level_up", 0, 0);
		this.level++;
	}
	public void LevelDown() {
		this.m_Animator.Play("planet_level_down", 0, 0);
		this.level--;
	}
}
