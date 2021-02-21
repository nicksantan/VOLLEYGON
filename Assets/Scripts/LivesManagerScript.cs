using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManagerScript : MonoBehaviour
{
    public int lives = 3;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

  
    public void UpdateLives()
    {
        if (lives >= 0)
        {
            transform.GetChild(0).GetChild(lives).gameObject.SetActive(false);
        }
       
    }

    public void OnBallDied()
    {
        Debug.Log("Ball has died according to lives manager");
        lives -= 1;

        UpdateLives();
        if (lives < 0)
        {
            // If regular game, tell game manager game is over.
            GameObject.FindWithTag("GameManager").BroadcastMessage("GameOver");
        }
    }
}
