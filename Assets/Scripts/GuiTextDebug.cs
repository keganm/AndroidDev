

using UnityEngine;
using System.Collections;

public class GuiTextDebug : MonoBehaviour {
	
	private float windowPosition = 10.0f;
	private static string windowText = "";
	private Vector2 scrollViewVector = Vector2.zero;
	private GUIStyle debugBoxStyle;
	
	private float leftSide = 0.0f;
	private float debugWidth = 600.0f;
	
	public bool debugIsOn = false;
	
	public void Log(string newString)
	{
		windowText = Time.captureFramerate.ToString() + " ~ " + newString + "\n" + windowText;
		UnityEngine.Debug.Log(newString);
	}
	
	void Start () {
		debugBoxStyle = new GUIStyle();
		debugBoxStyle.alignment = TextAnchor.UpperLeft;
		debugBoxStyle.normal.textColor = Color.white;
		leftSide = 203; //Screen.width - debugWidth - 3;
	}
	
	
	void OnGUI () {
		
		if (debugIsOn) {
			
			GUI.depth = 0;

			GUI.BeginGroup (new Rect(windowPosition, 10.0f, leftSide, 200.0f));
			
			scrollViewVector = GUI.BeginScrollView (new Rect (0, 0.0f, debugWidth, 200.0f), scrollViewVector, new Rect (0.0f, 0.0f, 400.0f, 2000.0f));


			GUI.Box (new Rect (0, 0.0f, debugWidth - 20.0f, 2000.0f), windowText, debugBoxStyle);
			GUI.EndScrollView();
			
			GUI.EndGroup ();

		}
	}
	
	
}
