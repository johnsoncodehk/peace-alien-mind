using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, ISignalReceiverHandler {

	public int levelCount = 4;
	public List<GameObject> levelObjects = new List<GameObject>();
	public Transform holder, ball;
	public float rotationSpeed;
	public float zPosSize = 0.2f;
	public int saveSignal;
	public bool isMenuPlanet;
	public int clearAt;
	public List<Vector2Int> receiverSignalPositions = new List<Vector2Int>();

	public int level {
		get { return this.m_Level; }
		set { this.m_Level = Mathf.Clamp(value, 0, this.levelCount - 1); }
	}
	public bool isClear {
		get { return this.m_Level == 0; }
	}

	private int m_Level;
	private Animator m_Animator;
	private Vector3 v;

	void Awake() {
		this.m_Animator = this.GetComponent<Animator>();
		this.ball.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

		this.level = this.isMenuPlanet ? 0 : this.levelCount - 1;
		for (int i = 0; i < this.levelObjects.Count; i++) {
			var levelObj = this.levelObjects[i];
			for (int j = 0; j < levelObj.transform.childCount; j++) {
				levelObj.transform.GetChild(j).gameObject.SetActive(j == this.level);
			}
		}

		foreach (Tower tower in this.GetComponentsInChildren<Tower>()) {
			tower.fromPlanet = this;
		}
	}
	void Start() {
		if (this.holder) {
			this.transform.position = this.holder.transform.position;
		}
	}
	void Update() {
		if (this.holder) {
			this.transform.position = this.holder.transform.position;
		}
		this.ball.Rotate(new Vector3(0, 0, this.rotationSpeed * Time.deltaTime));
		Stage stage = this.GetComponentInParent<Stage>();
		if (this.isClear && this.clearAt != GameManager.instance.step && stage && stage.state != Stage.State.GameOver) {
			this.ResetLevel();
		}
	}

	public void OnSignalReceiver(Signal signal) {
		if (signal.isLast) {
			GridTransform gridTran = this.GetComponent<GridTransform>();
			if (gridTran)
				signal.sendPositions.Add(gridTran.position);
			this.receiverSignalPositions = signal.sendPositions;
			this.m_Animator.Play("planet_level_up", 1, 0);
			this.SetLevel(0);
			this.clearAt = signal.shootAt;
		}
		Destroy(signal.gameObject);
		AudioManager.instance.PlaySignal2();
	}
	// public void LevelUp() {
	// 	this.m_Animator.Play("planet_level_up", 1, 0);
	// 	this.SetLevel(this.level - 1);
	// }
	// public void LevelDown() {
	// 	this.m_Animator.Play("planet_level_down", 1, 0);
	// 	this.SetLevel(this.level + 1);
	// }

	private void ResetLevel() {
		this.m_Animator.Play("planet_level_down", 1, 0);
		this.SetLevel(this.levelCount - 1);
	}
	private void SetLevel(int newLevel) {
		int oldLevel = this.level;
		if (oldLevel != newLevel) {
			this.level = newLevel;
			for (int i = 0; i < this.levelObjects.Count; i++) {
				var levelObj = this.levelObjects[i];
				StartCoroutine(this.ChangeLevelObjectColor(levelObj.transform.GetChild(oldLevel), levelObj.transform.GetChild(this.level)));
			}
		}
	}
	private IEnumerator ChangeLevelObjectColor(Transform oldObj, Transform newObj) {
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
				oldObj.gameObject.SetActive(true);
				newObj.gameObject.SetActive(true);
				yield return new WaitForEndOfFrame();
			}
		}

		oldObj.gameObject.SetActive(false);
		newObj.gameObject.SetActive(true);
	}
	private void OnRandomSpeed() {
		if (Random.value > 0.5f) {
			this.rotationSpeed *= -1;
		}
	}
}
