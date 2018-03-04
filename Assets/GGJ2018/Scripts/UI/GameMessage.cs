using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage : UIController {

	public Text text;
	public float lifeTime;

	private List<string> nextTexts = new List<string>();

	void Awake() {
		this.text.text = "";
	}
	void Update() {
		this.lifeTime -= Time.deltaTime;
		if (!this.isPlaying) {
			if (this.isShow) {
				if (this.lifeTime <= 0 || this.nextTexts.Count > 0) {
					this.Hide();
				}
			}
			else {
				if (this.nextTexts.Count > 0) {
					this.text.text = this.nextTexts[0];
					this.nextTexts.RemoveAt(0);
					this.lifeTime = 3;
					this.Show();
				}
			}
		}
	}

	public void ShowMessage(string message) {
		this.nextTexts.Add(message);
	}
}
