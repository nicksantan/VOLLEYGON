using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.alphaText(GetComponent<RectTransform>(), .3f, 1.5f).setEaseInCubic().setLoopClamp();
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
