using UnityEngine;
using System.Collections;


namespace AndroidInput{
	public class solidTouch : MonoBehaviour {

		public float m_sizeMultiplier = 1.2f;


		public int activeTouchID = -1;
		public bool m_moved = false;

		public Vector3 m_targetPosition;
		public Rect m_targetBorders = new Rect(10.0f,10.0f,100.0f,100.0f);
		public bool m_useScreenSize = false;

		public float m_deltaVelocity = 1.0f;
		public float m_deltaThreshold = 2.0f;
		public float m_targetSmoothness = 0.2f;

		public float m_snapThreshold = 0.2f;

		GuiTextDebug textDebug;

		// Use this for initialization
		void Start () 
		{
			textDebug = Camera.main.GetComponent<GuiTextDebug> ();
			m_targetPosition = this.transform.localPosition;
		}

		void Update()
		{
			Vector2 tp = new Vector2 (m_targetPosition.x, m_targetPosition.y);
			Vector2 cp = new Vector2 (this.transform.localPosition.x, this.transform.localPosition.y);
			this.GetComponent<Rigidbody2D> ().velocity = (tp - cp) * m_targetSmoothness;

			//this.GetComponent<Rigidbody2D> ().position = Vector2.Lerp (cp, tp, m_targetSmoothness * m_deltaVelocity);
			UpdateInput ();
			//this.transform.localPosition = Vector3.Lerp (this.transform.localPosition, m_targetPosition, m_targetSmoothness * m_deltaVelocity);
		}
		
		// Update is called once per frame
		void UpdateInput () {

			for (int i = 0; i < Input.touches.Length; i++) {
				if(m_moved && Input.touches[i].phase == TouchPhase.Began)
				{
					if (Vector3.Distance (this.transform.localPosition, m_targetPosition) < m_snapThreshold) {
						SnapToTargetPosition();
					}
				}

				if(Input.touches[i].deltaPosition.sqrMagnitude > m_deltaThreshold && !m_moved)
				{
					UpdateTargetPosition(Input.touches[i].deltaPosition);
					activeTouchID = Input.touches[i].fingerId;
					m_moved = true;
				}
			}
		}

		void SnapToTargetPosition()
		{
			if(textDebug.debugIsOn)
				textDebug.Log ("SnapToTargetPosition");

			this.transform.localPosition = m_targetPosition;
			activeTouchID = -1;
			m_moved = false;
		}

		void UpdateTargetPosition(Vector2 touchDelta)
		{
			if(textDebug.debugIsOn)
				textDebug.Log ("touchDelta" + touchDelta.ToString());
			/*
			 * /TODO: find a more consistent/satisfying approach to delta tracking
			if (touchDelta.magnitude > m_deltaThreshold * 2)
				m_deltaVelocity = 4.0f;
			else
				m_deltaVelocity = 1.0f;
				*/

			Vector3 direction = Vector3.zero;
			if (Mathf.Abs (touchDelta.x) > Mathf.Abs (touchDelta.y)) {
				if (touchDelta.x < 0.0f) {
					direction = Vector3.left;
				} else {
					direction = Vector3.right;
				}
			} else {
				if (touchDelta.y < 0.0f) {
					direction = Vector3.down;
				} else {
					direction = Vector3.up;
				}
			}
			Vector3 tp = Camera.main.WorldToScreenPoint( m_targetPosition + (direction * m_sizeMultiplier) );

			if (m_useScreenSize) {
				m_targetBorders.width = Screen.width - m_targetBorders.x*2;
				m_targetBorders.height = Screen.height - m_targetBorders.y*2;
			}
			if(tp.x > m_targetBorders.x && tp.x < m_targetBorders.width + m_targetBorders.x && tp.y > m_targetBorders.y && tp.y < m_targetBorders.height + m_targetBorders.y)
				m_targetPosition += direction * m_sizeMultiplier;
		}
	}
}
