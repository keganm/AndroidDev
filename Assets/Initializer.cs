using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class Initializer : MonoBehaviour {

	void Awake()
	{
		if (Advertisement.isSupported) {
			Advertisement.Initialize ("48833", true);
		} else {
			Debug.Log ("Platform not supported");
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		if (GUI.Button (new Rect (10, 10, 150, 50), Advertisement.IsReady () ? "Show Ad" : "Waiting...")) {
			Advertisement.Show(null,new ShowOptions {
				resultCallback = result => {
					Debug.Log(result.ToString());
				}
			});
		}
	}
}
