using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScript : MonoBehaviour
{

    private RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<Image>().GetComponent<RectTransform>();
       // FlashIn();
    }

    public void FlashIn()
    {
        rt = GetComponent<Image>().GetComponent<RectTransform>();
        Debug.Log("flashing in");
        LeanTween.alpha(rt, 1f, .1f).setEaseInCubic().setOnComplete(FlashOut);
    }

    public void FlashOut()
    {
        LeanTween.alpha(rt, 0f, 3f).setEaseInCubic();
    }
}
