using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour {

	public AnimationCurve sizeCurve, colorCurve;
	public Vector3 startSize, endSize;
	public Color startColor, endColor;
	public float lifeTime;
	public bool isFirst;
	[HideInInspector] public Signal child;
	[HideInInspector] public Player player;

	private float m_StartTime;
	private SpriteRenderer m_SpriteRenderer;
	private Rigidbody2D m_Rigidbody;
	private Collider2D m_Collider;
	
	void Awake() {
		this.m_StartTime = Time.time;
		this.m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
		this.m_Rigidbody = this.GetComponent<Rigidbody2D>();
		this.m_Collider = this.GetComponent<Collider2D>();

		this.transform.localScale = Vector3.zero;
	}
	void Update() {
		if (player) {
			
		}
		else {
			float currentTime = Time.time - this.m_StartTime;
			float t = currentTime / lifeTime;
			this.transform.localScale = Vector3.Lerp(this.startSize, this.endSize, this.sizeCurve.Evaluate(t));
			this.m_SpriteRenderer.color = Color.Lerp(this.startColor, this.endColor, this.colorCurve.Evaluate(t));
			this.m_Rigidbody.velocity = this.m_Rigidbody.transform.up * 1;
		}
	}
	public void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player && this.isFirst) {
			this.ToPlayer(player);
		}
		Planet planet = other.GetComponent<Planet>();
		if (planet) {
			planet.LevelUp();
		}
	}

	private void ToPlayer(Player player) {
		this.player = player;
		if (this.child) {
			this.child.ToPlayer(player);
		}
	}
}
