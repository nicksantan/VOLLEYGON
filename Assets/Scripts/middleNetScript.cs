using UnityEngine;
using System.Collections;

public class middleNetScript : MonoBehaviour {
	private Rigidbody2D rb;
	public float speed = 1000.0f;
	public float borderPosition;
	public float directionModifier;

	// Use this for initialization
	void Start () {

        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = new Vector2(0, directionModifier * speed);
        }
	}

	// Update is called once per frame
	void Update () {
		if (transform.localPosition.y >= borderPosition) {
            if (rb != null) rb.velocity = new Vector2 (0, speed * -1);
		}

		if (transform.localPosition.y <= -1 * borderPosition) {
            if (rb != null) rb.velocity = new Vector2 (0, speed * 1);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Wall") {
			float tempVel = rb.velocity.y;
			tempVel *= -1;
			rb.velocity = new Vector2 (0, tempVel);

		}
	}
}
