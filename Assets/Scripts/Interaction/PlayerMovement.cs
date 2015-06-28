using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

	private Rigidbody2D body;
	public int activeTouchID = -1;
	public bool m_moved = false;
	public Vector3 m_targetPosition;
	public EditRect m_editRect;
	public Rect m_targetBorders = new Rect (-1.0f, -1.0f, 1.0f, 1.0f);
	public bool m_useScreenSize = false;
	public bool m_useBoxCollider = false;
	public float m_deltaVelocity = 1.0f;
	public float m_deltaThreshold = 2.0f;
	public float m_targetSmoothness = 0.2f;
	public float m_snapThreshold = 0.2f;
	public int m_heightCount = 15;
	public int m_widthCount = 5;
	public float m_heightOffset;
	public float m_widthOffset;
	public bool isAsleep = false;
	public float sleepTime = 0.2f;
	private float sleepTimer = 0f;

	// Use this for initialization
	void Start ()
	{
		
		m_editRect = GameObject.Find("BoundingBox").GetComponent<EditRect> ();

		body = this.GetComponent<Rigidbody2D> ();

		Reset ();
	}

	void Update ()
	{
		Vector2 tp = new Vector2 (m_targetPosition.x, m_targetPosition.y);
		Vector2 cp = new Vector2 (this.transform.localPosition.x, this.transform.localPosition.y);
		body.velocity = (tp - cp) * m_targetSmoothness;
	}

	public void Reset ()
	{
		this.transform.localPosition = new Vector3 (0f, 0f, this.transform.localPosition.z);
		m_targetPosition = this.transform.localPosition;
		body.velocity *= 0f;
		CalculateGridResolution ();
		Vector3 sp = ( new Vector2 (m_targetBorders.center.x, m_targetBorders.center.y));
		this.transform.localPosition = new Vector3 (sp.x, sp.y, 0f);
		m_targetPosition = this.transform.localPosition;
	}

	void LateUpdate ()
	{
		CalculateGridResolution ();
		CheckSleep ();
	}
		
	// Update is called once per frame
	void OnGUI ()
	{

		if (isAsleep) {
//				Vector3 jitter = new Vector3(0.025f,0.025f,0f);
//				jitter.Scale(Random.insideUnitSphere);
//				this.transform.localPosition += jitter;
			return;
		}
		
		if (Vector3.Distance (this.transform.localPosition, m_targetPosition) < m_snapThreshold) {
			SnapToTargetPosition ();
		}
		for (int i = 0; i < Input.touches.Length; i++) {
			if (m_moved && Input.touches [i].phase == TouchPhase.Began) {
				if (Vector3.Distance (this.transform.localPosition, m_targetPosition) < m_snapThreshold) {
					SnapToTargetPosition ();
				}
			}

			if (Input.touches [i].deltaPosition.sqrMagnitude > m_deltaThreshold && !m_moved) {
				UpdateTargetPosition (Input.touches [i].deltaPosition);
				activeTouchID = Input.touches [i].fingerId;
				m_moved = true;
			}
		}
	}

	void SnapToTargetPosition ()
	{

		body.position = m_targetPosition;
		body.velocity = Vector3.zero;
		activeTouchID = -1;
		m_moved = false;
	}

	void UpdateTargetPosition (Vector2 touchDelta)
	{
		//if(textDebug.debugIsOn)
		//	textDebug.Log ("touchDelta" + touchDelta.ToString());
		/*
			 * /TODO: find a more consistent/satisfying approach to delta tracking
			if (touchDelta.magnitude > m_deltaThreshold * 2)
				m_deltaVelocity = 4.0f;
			else
				m_deltaVelocity = 1.0f;
				*/

		Vector2 direction = Vector2.zero;
		if (Mathf.Abs (touchDelta.x) > Mathf.Abs (touchDelta.y)) {
			if (touchDelta.x < 0.0f) {
				direction = Vector2.left;
			} else {
				direction = Vector2.right;
			}
		} else {
			if (touchDelta.y < 0.0f) {
				direction = Vector2.down;
			} else {
				direction = Vector2.up;
			}
		}


		Vector3 tp = new Vector3 (m_targetPosition.x + (direction.x * m_widthOffset), m_targetPosition.y + (direction.y * m_heightOffset), 0f);
		//tp = Camera.main.WorldToScreenPoint (tp);

		if(m_targetBorders.Contains(tp))

	//	if (tp.x > m_targetBorders.x && tp.x < m_targetBorders.width + m_targetBorders.x && tp.y > m_targetBorders.y && tp.y < m_targetBorders.height + m_targetBorders.y)
			m_targetPosition = tp;
	}

	void CalculateGridResolution ()
	{
		if (m_useScreenSize) {
			m_targetBorders.width = Screen.width - m_targetBorders.x * 2;
			m_targetBorders.height = Screen.height - m_targetBorders.y * 2;
		}

		if (m_useBoxCollider && m_editRect != null) {

			m_targetBorders = new Rect(m_editRect.xMin,m_editRect.yMin,m_editRect.width,m_editRect.height);
			                        

		}


		m_widthOffset = m_targetBorders.width / (float)m_widthCount;
		m_heightOffset = m_targetBorders.height / (float)m_heightCount;

//		m_widthOffset = tb.x / ((float)m_widthCount * 0.5f);
//		m_heightOffset = tb.y / ((float)m_heightCount * 0.5f);
	}


	public void StartSleep ()
	{
		isAsleep = true;
		sleepTimer = Time.time;
		this.GetComponentInChildren<MeshRenderer> ().material.color = Color.red;
	}

	public void CheckSleep ()
	{
		if (!isAsleep)
			return;
		if (Time.time - sleepTimer > sleepTime) {
				
			this.GetComponentInChildren<MeshRenderer> ().material.color = Color.white;
			isAsleep = false;
		}

	}
}
