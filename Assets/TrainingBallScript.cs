using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBallScript : MonoBehaviour
{
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("trying to launch ball");
        IEnumerator coroutine_1 = ball.GetComponent<BallScript>().CustomLaunchBallWithDelay(.1f, 15f, -.5f);
        StartCoroutine(coroutine_1);
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
