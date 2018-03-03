using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public List<Button> levelButtons = new List<Button>();

	private Vector3 startPosition;

	void Awake() {
		for (int i = 0; i < this.levelButtons.Count; i++) {
			int level = i + 1;
			this.levelButtons[i].onClick.AddListener(() => {
				AudioManager.instance.PlayClickButton();
				GameManager.instance.PlayLevel(level);
			});
		}
		if (this.startPosition == Vector3.zero) {
			this.startPosition = GameManager.instance.player.transform.position;
		}
	}
	void OnEnable() {
		GameManager.instance.player.character.targetPosition = this.startPosition;
	}

	public void OnPointerEnterButton(int index) {
		GameManager.instance.player.control = Player.Control.Program;
		GameManager.instance.player.character.targetPosition = this.levelButtons[index].transform.TransformPoint(Vector3.zero) - new Vector3(0, 1f, 0);
	}
}
