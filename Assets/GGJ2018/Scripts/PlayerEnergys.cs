using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergys : MonoBehaviour {

	public int remain;
	public Transform holder;
	public TransformHook sprite;
	public bool show;

	private List<Transform> m_Holders = new List<Transform>();
	private List<TransformHook> m_Hooks = new List<TransformHook>();
	private float sizeV;

	void Start() {
		this.holder.gameObject.SetActive(false);
		this.sprite.gameObject.SetActive(false);
	}
	void Update() {
		bool refush = false;
		while (this.remain >= 0 && this.m_Holders.Count > this.remain) {
			int index = Random.Range(0, this.m_Holders.Count);
			Destroy(this.m_Holders[index].gameObject);
			Destroy(this.m_Hooks[index].gameObject);
			this.m_Holders.RemoveAt(index);
			this.m_Hooks.RemoveAt(index);
			// refush = true;
		}
		while (this.m_Holders.Count < this.remain) {
			Transform newHolder = Instantiate(this.holder);
			newHolder.SetParent(this.holder.parent, false);
			newHolder.gameObject.SetActive(true);
			this.m_Holders.Add(newHolder);
			TransformHook newsprite = Instantiate(this.sprite);
			newsprite.transform.SetParent(this.sprite.transform.parent, false);
			newsprite.gameObject.SetActive(true);
			this.m_Hooks.Add(newsprite);
			newsprite.target = newHolder.GetChild(0);
			refush = true;
		}
		if (refush) {
			this.Refush();
		}
		this.transform.localScale = Vector3.one * Mathf.SmoothDamp(this.transform.localScale.x, this.show ? 1 : 0, ref this.sizeV, 0.5f);
	}

	private void Refush() {
		for (int i = 0; i < this.m_Holders.Count; i++) {
			float angle = 360 / this.m_Holders.Count;
			this.m_Holders[i].localEulerAngles = new Vector3(0, angle * i, 0);
			this.m_Holders[i].name = (angle * i).ToString();
		}
	}
}
