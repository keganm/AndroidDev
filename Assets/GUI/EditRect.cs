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

	//TODO: add touch/drag operations to this
	public void  Update () {

	}

	public void SetRectangle(float minX, float minY, float maxX, float maxY)
	{
		m_Rectangle.xMin = minX;
		m_Rectangle.yMin = minY;
		m_Rectangle.xMax = maxX;
		m_Rectangle.yMax = maxY;
	}
	/*
	//[DrawGizmo(GizmoType.NotSelected | GizmoType.Pickable)]
	void OnDrawGizmos()
	{
		if (!m_VisualizeRectangle)
			return;

		InputManipulation ();

		Vector3[] rectanglePoints = new Vector3[4];
		
		rectanglePoints [0] = new Vector3 (m_Rectangle.x, m_Rectangle.y);
		rectanglePoints [1] = new Vector3 (m_Rectangle.x + m_Rectangle.width, m_Rectangle.y);
		rectanglePoints [2] = new Vector3 (m_Rectangle.x + m_Rectangle.width, m_Rectangle.y + m_Rectangle.height);
		rectanglePoints [3] = new Vector3 (m_Rectangle.x, m_Rectangle.y + m_Rectangle.height);
		
		for(int i = 0; i < rectanglePoints.Length-1; i++)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(Vector3.Lerp(rectanglePoints[i],rectanglePoints[i+1],0.5f),new Vector3(0.2f,0.2f,0.2f));

				Gizmos.DrawLine(rectanglePoints[i], rectanglePoints[i+1]);
		}
		Debug.DrawLine (rectanglePoints [rectanglePoints.Length - 1], rectanglePoints [0]);
		Gizmos.DrawCube(Vector3.Lerp(rectanglePoints[rectanglePoints.Length - 1],rectanglePoints[0],0.5f),new Vector3(0.2f,0.2f,0.2f));
	}
	*/

	/*
	void InputManipulation()
	{
		Event e = Event.current;
		if (!m_isDragging) {
			if(e.type == EventType.MouseDown && e.button == 0) {
			
				Vector3 mousePosition = e.mousePosition;


				RectSide tmpSide = TestSide(mousePosition);
				if(tmpSide != RectSide.None)
				{
					m_isDragging = true;
					m_draggingSide = tmpSide;
				}
			}
		}

		if (m_isDragging) {
			if(e.type == EventType.MouseDrag)
			{
				Vector3 mousePosition = e.mousePosition;
				UpdateRectSide(mousePosition);
			}

			if(e.type == EventType.MouseUp)
			{
				m_isDragging = false;
				m_draggingSide = RectSide.None;
			}
		}
	}

	RectSide TestSide(Vector3 pos)
	{
		if (!GetPosInRectSpace (ref pos))
			return RectSide.None;

		if (pos.x > m_Rectangle.xMin + 0.1f && pos.x < m_Rectangle.xMax - 0.1f) {
			//test top or bottom

			if(Mathf.Abs(m_Rectangle.yMin - pos.y) < 1f)
				return RectSide.Top;
			else if(Mathf.Abs(m_Rectangle.yMax - pos.y) < 1f)
				return RectSide.Bottom;
			else 
				return RectSide.None; // didn't match

		} else if (pos.y > m_Rectangle.yMin + 0.1f && pos.y < m_Rectangle.yMax - 0.1f) {
			//test left or right
			
			if(Mathf.Abs(m_Rectangle.xMin - pos.x) < 1f)
				return RectSide.Left;
			else if(Mathf.Abs(m_Rectangle.xMax - pos.x) < 1f)
				return RectSide.Right;
			else 
				return RectSide.None; // didn't match
		}

		Debug.Log ("Didn't match a side");
		return RectSide.None;
	}

	void UpdateRectSide(Vector3 pos)
	{
		GetPosInRectSpace (ref pos);

		switch (m_draggingSide) {
		case RectSide.Left:
			m_Rectangle.xMin = pos.x;
			break;
		case RectSide.Top:
			m_Rectangle.yMin = pos.y;
			break;
		case RectSide.Right:
			m_Rectangle.xMax = pos.x;
			break;
		case RectSide.Bottom:
			m_Rectangle.yMax = pos.y;
			break;
		default:
			break;
		}
	}

	bool GetPosInRectSpace(ref Vector3 pos)
	{
		BoxCollider colliderTest = this.gameObject.AddComponent<BoxCollider>();

		colliderTest.center = new Vector3(m_Rectangle.center.x,m_Rectangle.center.y,0f);
		colliderTest.size = new Vector3(m_Rectangle.size.x*2f,m_Rectangle.size.y*2f,0f);
		Ray ray = HandleUtility.GUIPointToWorldRay (pos);
		RaycastHit hit;

		if (colliderTest.Raycast (ray, out hit, Camera.current.farClipPlane)) {
			Debug.Log("Contact made at: " + hit.point.ToString());

			DestroyImmediate (colliderTest);
			pos = hit.point;
			return true;
		}
		return false;
	}
	*/
	
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
