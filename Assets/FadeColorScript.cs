using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeColorScript : MonoBehaviour
{
    public float FadeRate;
    private Image image;
    private float targetAlpha;
    private bool fadingOut = true;
    private float originalAlpha;
    // Use this for initialization
    void Start()
    {
        this.image = this.GetComponent<Image>();
        if (this.image == null)
        {
            Debug.LogError("Error: No image on " + this.name);
        }
        originalAlpha = this.image.color.a;
        targetAlpha = 0f;

        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), .25f, 2f).setEase(LeanTweenType.easeInCubic).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        //Color curColor = this.image.color;
        //float alphaDiff = Mathf.Abs(curColor.a - this.targetAlpha);
        //if (alphaDiff > 0.0001f)
        //{
        //    curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.FadeRate * Time.deltaTime);
        //    this.image.color = curColor;
          
        //} else
        //{
        //    if (fadingOut)
        //    {
        //        targetAlpha = originalAlpha;
        //        fadingOut = false;
        //    } else
        //    {
        //        targetAlpha = 0f;
        //        fadingOut = true;
        //    }
        //}
    }

    public void FadeOut()
    {
        this.targetAlpha = 0.0f;
    }

    public void FadeIn()
    {
        this.targetAlpha = 1.0f;
    }
}
