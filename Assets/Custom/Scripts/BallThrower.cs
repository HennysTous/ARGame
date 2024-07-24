using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
	[SerializeField]
	float throwForce = 2f; // to control throw force in Z direction

	GameObject ball;

	public bool isPicked = false;
	Rigidbody rb;

	// Update is called once per frame
	void Update()
	{
		if (isPicked) ball.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, Camera.main.nearClipPlane * 7.5f));

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{

			if(isPicked) Throw(); else Pick();

		}
		

	}

	void Throw(){

			isPicked = false;
			rb.isKinematic = false;
			rb.useGravity = true;
			rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
	}

	void Pick(){

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit _hit;

			if (Physics.Raycast(ray, out _hit, 100f))
			{
				if (_hit.transform.tag == "Throwable")
				{
					isPicked = true;
					ball = _hit.transform.gameObject;
					rb = ball.GetComponent<Rigidbody>();
					ball.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, Camera.main.nearClipPlane * 7.5f));

					rb.isKinematic = true;
					rb.useGravity = false;

				}
			}

	}

}
