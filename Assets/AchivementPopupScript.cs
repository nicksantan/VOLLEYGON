using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementPopupScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SlideIn", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SlideIn()
    {
        float currentY = gameObject.GetComponent<RectTransform>().localPosition.y;
        Debug.Log("current Y is");
        Debug.Log(currentY);
        LeanTween.moveY(gameObject.GetComponent<RectTransform>(), currentY + 200f, .250f).setEase(LeanTweenType.easeInCubic).setDelay(3f).setLoopPingPong();
    }
}
