

using UnityEngine;
using System.Collections;

public class GuiTextDebug : MonoBehaviour {
	
	private float windowPosition = 10.0f;
	private static string windowText = "";
	private Vector2 scrollViewVector = Vector2.zero;
	private GUIStyle debugBoxStyle;

	private float debugWidth = 600.0f;
	private float debugHeight = 210.0f;
	
	public bool debugIsOn = false;
	
	public void Log(string newString)
	{
		windowText = Time.time.ToString("0.0") + " ~ " + newString + "\n" + windowText;
		UnityEngine.Debug.Log(newString);
	}
	
	void Start () {
		debugBoxStyle = new GUIStyle();
		debugBoxStyle.alignment = TextAnchor.UpperLeft;
		debugBoxStyle.normal.textColor = Color.white;
		debugBoxStyle.fontSize = 20;
	}
	
	
	void OnGUI () {
		
		if (debugIsOn) {
			
			GUI.depth = 0;

			debugWidth = Screen.width - windowPosition  * 2;
			GUI.BeginGroup (new Rect(windowPosition, windowPosition, debugWidth, debugHeight));
			
			scrollViewVector = GUI.BeginScrollView (new Rect (0, 0.0f, debugWidth, debugHeight), scrollViewVector, new Rect (0.0f, 0.0f, debugWidth-windowPosition  * 2, debugHeight));


			GUI.Box (new Rect (0, 0.0f, debugWidth, 2000.0f), windowText, debugBoxStyle);
			GUI.EndScrollView();
			
			GUI.EndGroup ();

		}
	}
	
	
}
