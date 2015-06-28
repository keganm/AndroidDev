using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EditRect : MonoBehaviour {
	public Rect m_Rectangle;
	public bool m_VisualizeRectangle;

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
			//OnDrawGizmos ();
	}

	void OnDrawGizmosSelected()
	{
		if (!m_VisualizeRectangle)
			return;
		
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
	
	public Vector3 VectorToScreenSpace(Vector2 value)
	{
		Vector3 vec = Camera.main.WorldToScreenPoint (new Vector3 (value.x, value.y, 0f));
		vec = new Vector3 (vec.x, vec.y, 0f);
		return vec;
	}
}
