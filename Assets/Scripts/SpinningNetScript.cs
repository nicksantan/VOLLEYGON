using UnityEngine;
using System.Collections;

public class SpinningNetScript : MonoBehaviour {
	private Rigidbody2D rb;
	public float acceleration;
	public float accelerationFactor;
	public float vel = 20f;
	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody2D>();

	//	rb.velocity = new Vector2 (0, speed);
		acceleration = accelerationFactor;
		rb.angularVelocity = vel;
	}

	// Update is called once per frame
	void Update () {
		vel += acceleration * Time.deltaTime;
		rb.angularVelocity = vel;

		if (vel > 500) {
			acceleration = -1 * accelerationFactor;
		}

		if (vel < -500) {
			acceleration = 1 * accelerationFactor;
		}
		
	}
}
