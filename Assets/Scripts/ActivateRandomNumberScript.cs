using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRandomNumberScript : MonoBehaviour {

	public GameObject team1Scores;
	public GameObject team2Scores;
	// Use this for initialization
	void Start () {
		int randomChildOne = Random.Range(0,10);
		int randomChildTwo = Random.Range(0,10);
		team1Scores.transform.GetChild(randomChildOne).gameObject.SetActive(true);
		team2Scores.transform.GetChild(randomChildTwo).gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
