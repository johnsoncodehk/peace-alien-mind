using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Player : MonoBehaviour {

	public enum Control {
		None,
		Program,
		Mouse,
		Keybroad,
	}

	public Control control = Control.None;
	public Signal signal;
	[HideInInspector] public Character character;

	void Awake() {
		this.character = this.GetComponent<Character>();
	}
	void Update() {
		if (this.control == Control.Mouse) {
			this.character.enabledMove = true;
			this.character.targetPosition = GameManager.instance.ScreenToWorldPoint(this.transform.position.z);
		}
		else if (this.control == Control.Keybroad) {
			this.character.enabledMove = true;
			Vector3 input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
			this.character.targetPosition = this.transform.position + input * 10;
		}
		else if (this.control == Control.Program) {
			this.character.enabledMove = true;
		}
	}
	void OnMouseDown() {
		GameManager.instance.step++;
		StartCoroutine(Signal.ShootAsync(this.signal, this.transform.position, Direction.Up, this.transform, GameManager.instance.step));
	}
}
