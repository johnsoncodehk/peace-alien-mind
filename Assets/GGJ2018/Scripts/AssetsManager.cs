using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour {

	public static AssetsManager instance {
		get {
			if (!AssetsManager.m_Instance) {
				AssetsManager.m_Instance = FindObjectOfType<AssetsManager>();
			}
			return AssetsManager.m_Instance;
		}
	}
	private static AssetsManager m_Instance;

	public Stage stage;
	public List<GridTransform> stageObjects = new List<GridTransform>();

}
