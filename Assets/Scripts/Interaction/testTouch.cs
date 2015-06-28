using UnityEngine;
using System.Collections;

public class testTouch : MonoBehaviour {
	public float raiseZ = -1.2f;
	float dropZ;
	public float velocityFade = 0.05f;
	public bool killVelocity = true;

	Vector3 targetPosition;

	int currentFingerID = -1;
	bool isTouched = false;
	// Use this for initialization
	void Start () {
		dropZ = this.transform.position.z;
		targetPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!isTouched) {
			currentFingerID = TestForTouch ();
			if (currentFingerID >= 0)
				isTouched = true;
			else
				isTouched = false;
		}

		if (isTouched)
			isTouched = TestTouchRelease ();
		else
			targetPosition = new Vector3(targetPosition.x, targetPosition.y, dropZ);

		ReactToTouch ();

		this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, 0.1f);
		
		if(killVelocity)
		targetPosition = Vector3.Lerp (targetPosition, this.transform.position, 0.1f);
	}

	void ReactToTouch()
	{
		if (isTouched)
			this.GetComponent<Renderer> ().material.color = Color.green;
		else
			this.GetComponent<Renderer> ().material.color = Color.red;

		if (!isTouched)
			return;

		Touch touch = Input.GetTouch (currentFingerID);

		
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x,touch.position.y, 0));
		RaycastHit hit;
		int layerMask = 1 << LayerMask.NameToLayer("TouchTracking");
		if(Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask)){
			targetPosition = hit.point + new Vector3(0,0,raiseZ);
			return;
		}
//
	}

	int TestForTouch()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Interactable");
		foreach (Touch touch in Input.touches) {
			if( touch.phase == TouchPhase.Began ) {
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(touch.position.x,touch.position.y, 0f));
				RaycastHit hit;
				
				if(Physics.Raycast(ray,out hit,Mathf.Infinity,layerMask)){
					if(hit.collider == this.GetComponent<Collider>())
					return touch.fingerId;
				}
			}
		}
		return -1;
	}

	/// <summary>
	/// Tests the touch release.
	/// </summary>
	/// <returns><c>true</c>, still touching, <c>false</c> otherwise.</returns>
	bool TestTouchRelease()
	{
		if (Input.touches.Length <= 0 || currentFingerID < 0 || currentFingerID > Input.touches.Length) {
			currentFingerID = -1;
			return false;
		}
		Touch touch = Input.GetTouch (currentFingerID);
		if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
			targetPosition = new Vector3(targetPosition.x, targetPosition.y, dropZ);
			this.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity * velocityFade;

			currentFingerID = -1;
			return false;
		}
		return true;
	}
}
