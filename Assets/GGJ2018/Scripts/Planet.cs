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

	public int levelCount = 4;
	public List<GameObject> levelObjects = new List<GameObject>();
	public Transform holder, ball;
	public float rotationSpeed;
	public float zPosSize = 0.2f;
	public int saveSignal;

	public int level {
		get { return this.m_Level; }
		set { this.m_Level = Mathf.Clamp(value, 0, this.levelCount - 1); }
	}

	private int m_Level;
	private Animator m_Animator;

	void Awake() {
		this.m_Animator = this.GetComponent<Animator>();

		this.level = this.levelCount - 1;
		for (int i = 0; i < this.levelObjects.Count; i++) {
			var levelObj = this.levelObjects[i];
			for (int j = 0; j < levelObj.transform.childCount; j++) {
				levelObj.transform.GetChild(j).gameObject.SetActive(j == this.level);
			}
		}
	}
	void Update() {
		if (this.holder) {
			Vector3 pos = this.holder.position;
			pos.z = (pos.z - this.holder.parent.position.z) * zPosSize + this.holder.parent.position.z;
			this.transform.position = pos;
		}
		this.ball.Rotate(new Vector3(0, 0, this.rotationSpeed * Time.deltaTime));
	}

	public void LevelUp() {
		this.m_Animator.Play("planet_level_up", 1, 0);
		int oldLevel = this.level;
		this.level--;
		if (oldLevel != this.level) {
			for (int i = 0; i < this.levelObjects.Count; i++) {
				var levelObj = this.levelObjects[i];
				StartCoroutine(this.ChangeLevelObjectColor(levelObj.transform.GetChild(oldLevel), levelObj.transform.GetChild(this.level)));
			}
		}
	}
	public void LevelDown() {
		this.m_Animator.Play("planet_level_down", 1, 0);
		int oldLevel = this.level;
		this.level++;
		if (oldLevel != this.level) {
			for (int i = 0; i < this.levelObjects.Count; i++) {
				var levelObj = this.levelObjects[i];
				StartCoroutine(this.ChangeLevelObjectColor(levelObj.transform.GetChild(oldLevel), levelObj.transform.GetChild(this.level)));
			}
		}
	}
	
	private IEnumerator ChangeLevelObjectColor(Transform oldObj, Transform newObj) {
		oldObj.gameObject.SetActive(true);
		newObj.gameObject.SetActive(true);
		
		SpriteRenderer oldSpr = oldObj.GetComponent<SpriteRenderer>();
		SpriteRenderer newSpr = newObj.GetComponent<SpriteRenderer>();

		Color color1 = new Color(1, 1, 1, 1);
		Color color2 = new Color(1, 1, 1, 0);

		if (oldSpr && newSpr) {
			float time = 0.5f;
			float currentTime = 0;
			while (currentTime < time) {
				currentTime += Time.deltaTime;
				float t = currentTime / time;
				oldSpr.color = Color.Lerp(color1, color2, t);
				newSpr.color = Color.Lerp(color2, color1, t);
				yield return new WaitForEndOfFrame();
			}
		}

		oldObj.gameObject.SetActive(false);
	}
}
