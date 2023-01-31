using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashOutScript : MonoBehaviour
{

    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<Image>().GetComponent<RectTransform>();
        FlashOut();
    }

    public void FlashOut()
    {
        LeanTween.alpha(rt, 0f, .5f).setEaseOutCubic();
        LeanTween.scale(rt.gameObject, new Vector3(15f,15f, 1f), .5f).setEaseOutCubic();
    }
}
