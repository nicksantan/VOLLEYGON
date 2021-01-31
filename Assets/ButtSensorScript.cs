using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtSensorScript : MonoBehaviour
{

    private GameObject aim;
    private ManualAIScript aimc;
    // Start is called before the first frame update
    void Start()
    {
        aim = transform.parent.gameObject;
        aimc = aim.GetComponent<ManualAIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball") {
            Debug.Log("BUTT SENSOR!");
            // report this to parent ai manager
            aimc.buttSensorEntered();
    
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("BUTT SENSOR!");
            // report this to parent ai manager
            aimc.buttSensorExited();

        }
    }
}
