using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMessage : UIController {

	public Text text;
	public float lifeTime;
	public bool canSkip;

	private List<string> nextTexts = new List<string>();
	private List<bool> canSkips = new List<bool>();

	void Awake() {
		this.text.text = "";
	}
	void Update() {
		this.lifeTime -= Time.deltaTime;
		if (!this.isPlaying) {
			if (this.isShow) {
				if (this.lifeTime <= 0 || (this.nextTexts.Count > 0 && this.canSkip)) {
					this.Hide();
				}
			}
			else {
				if (this.nextTexts.Count > 0) {
					this.text.text = this.nextTexts[0];
					this.canSkip = this.canSkips[0];
					this.nextTexts.RemoveAt(0);
					this.canSkips.RemoveAt(0);
					this.lifeTime = 3;
					this.Show();
				}
			}
		}
	}

	public void ShowMessage(string message, bool canSkip = false) {
		this.nextTexts.Add(message);
		this.canSkips.Add(canSkip);
	}
}
