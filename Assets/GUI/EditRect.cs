using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditRect : MonoBehaviour {
	enum RectSide{
		None,
		Left,
		Top,
		Right,
		Bottom
	}


	public Rect m_Rectangle;
	public bool m_VisualizeRectangle;

	private bool m_isDragging;
	private RectSide m_draggingSide;

	public Vector2 Min {
		get{ return m_Rectangle.min;}
	}
	public float xMin {
		get{ return m_Rectangle.xMin; }
	}
	public float yMin {
		get{ return m_Rectangle.yMin; }
	}
	public Vector2 Max {
		get{ return m_Rectangle.max;}
	}
	public float xMax {
		get{ return m_Rectangle.xMax;}
	}
	public float yMax {
		get{ return m_Rectangle.yMax;}
	}
	public float width {
		get{ return m_Rectangle.width;}
	}
	public float height {
		get { return m_Rectangle.height;}
	}

	void OnEnabled () {
	}

	public void SetRectangle(float minX, float minY, float maxX, float maxY)
	{
		m_Rectangle.xMin = minX;
		m_Rectangle.yMin = minY;
		m_Rectangle.xMax = maxX;
		m_Rectangle.yMax = maxY;
	}

	public Vector3 VectorToScreenSpace(Vector2 value)
	{
		Vector3 vec = Camera.main.WorldToScreenPoint (new Vector3 (value.x, value.y, 0f));
		vec = new Vector3 (vec.x, vec.y, 0f);
		return vec;
	}
	public Vector3 VectorToToWorldPoint(Vector3 value)
	{
		Vector3 vec = Camera.current.ScreenToWorldPoint (new Vector3 (value.x, value.y, 0f));
		vec = new Vector3 (vec.x, vec.y, 0f);
		return vec;
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(EditRect))] 
class RectEditorHandle : Editor {

	void OnSceneGUI()
	{
		EditRect editTarget = (EditRect)target;
		if (!editTarget.m_VisualizeRectangle)
			return;

		//draw outline
		Vector3[] rectanglePoints = new Vector3[4];
		
		rectanglePoints [0] = new Vector3 (editTarget.m_Rectangle.x, editTarget.m_Rectangle.y);
		rectanglePoints [1] = new Vector3 (editTarget.m_Rectangle.x + editTarget.m_Rectangle.width, editTarget.m_Rectangle.y);
		rectanglePoints [2] = new Vector3 (editTarget.m_Rectangle.x + editTarget.m_Rectangle.width, editTarget.m_Rectangle.y + editTarget.m_Rectangle.height);
		rectanglePoints [3] = new Vector3 (editTarget.m_Rectangle.x, editTarget.m_Rectangle.y + editTarget.m_Rectangle.height);
		
		for(int i = 0; i < rectanglePoints.Length-1; i++)
		{
			Handles.color = Color.green;
			Handles.DrawLine(rectanglePoints[i], rectanglePoints[i+1]);
		}
		Handles.DrawLine (rectanglePoints [rectanglePoints.Length - 1], rectanglePoints [0]);



		//do handles
		Vector3 min = Handles.DoPositionHandle (new Vector3 (editTarget.m_Rectangle.min.x, editTarget.m_Rectangle.min.y, 0f),Quaternion.identity);
		Vector3 max = Handles.DoPositionHandle(new Vector3(editTarget.m_Rectangle.max.x,editTarget.m_Rectangle.max.y,0f),Quaternion.identity);
		editTarget.SetRectangle (min.x, min.y, max.x, max.y);
	}

}
#endif
