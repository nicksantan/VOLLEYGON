using UnityEngine;
using System.Collections;

public class CheckerScript : MonoBehaviour {
		
	float newX;
	// Use this for initialization
	void Start () {
		newX = 0;
	//	StartCoroutine ("MoveChecker");
	}

	// Update is called once per frame
	void Update () {

        newX += .3f * Time.deltaTime; // 0.00780201
                                                newX = newX % 1;
                                        Vector2 newOffset = new Vector2 (newX, 0f);
                                        GetComponent<TiledSpriteRenderer> ().SetTiling (GetComponent<TiledSpriteRenderer> ().size, GetComponent<TiledSpriteRenderer> ().sprite, SpriteAlignment.Center, newOffset);
    }

    IEnumerator MoveChecker()
	{
		while (true) {
            Debug.Log(Time.deltaTime);
            newX += .3f * Time.deltaTime; // 0.00780201
            newX = newX % 1;
			Vector2 newOffset = new Vector2 (newX, 0f);
			GetComponent<TiledSpriteRenderer> ().SetTiling (GetComponent<TiledSpriteRenderer> ().size, GetComponent<TiledSpriteRenderer> ().sprite, SpriteAlignment.Center, newOffset);
            yield return null;
		}
	}
}
