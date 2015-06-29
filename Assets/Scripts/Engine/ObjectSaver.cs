using UnityEngine;
using System.Collections;
using System;

public class ObjectSaver : MonoBehaviour {

	[SerializeField]
	public GameObject[] initialHolds;
	void Awake () {
		for (int i = 0; i < initialHolds.Length; i++) {
			GameObject.DontDestroyOnLoad(initialHolds[i]);
		}
	}

	public void AddProtected(GameObject toprotect)
	{
		Array.Resize<GameObject> (ref initialHolds, initialHolds.Length + 1);
		initialHolds [initialHolds.Length - 1] = toprotect;
		GameObject.DontDestroyOnLoad (toprotect);
	}

	void OnDestroy()
	{
		for (int i = 0; i < initialHolds.Length; ++i) {
			GameObject.Destroy(initialHolds[i]);
		}
	}
}
