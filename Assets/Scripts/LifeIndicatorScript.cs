using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIndicatorScript : MonoBehaviour
{

    public GameObject targetPlayer;
    public PlayerController pc;
    public int playerType = -1;
    private GameObject innershape;

    void Start()
    {
        // Identify target player by tag.
        Debug.Log("Life indicator looking for player");
        targetPlayer = GameObject.FindWithTag("Player");
      
        if (targetPlayer != null)
        {
            Debug.Log("Target player found");
            Debug.Log(targetPlayer);
            pc = targetPlayer.GetComponent<PlayerController>();
            playerType = targetPlayer.GetComponent<PlayerController>().playerType;
            Debug.Log("player type is " + playerType);
        }
        else
        {
            Debug.Log("Player not found");
        }

        if (playerType != -1)
        {
            Debug.Log("Setting child to active");
            Debug.Log(transform.GetChild(playerType).gameObject);
            transform.GetChild(playerType).gameObject.SetActive(true);
            transform.GetChild(playerType).gameObject.active = true;

            innershape = transform.GetChild(playerType).Find("innershape").gameObject;
        }
    }


    void Update()
    {
        // Update the rotation to match the player's rotation
        transform.localEulerAngles = targetPlayer.transform.localEulerAngles;
        if (Mathf.Sign(pc.rb.gravityScale) == -1)
        {
            innershape.SetActive(true);
        } else
        {
            innershape.SetActive(false);
        }
    }
}
