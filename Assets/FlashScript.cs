using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScript : MonoBehaviour
{

    private RectTransform rt;
    public AudioClip sfx;
    public AudioClip wooshSfx;
    public GameObject flashOut;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<Image>().GetComponent<RectTransform>();
       
    }

    public void FlashIn()
    {
        rt = GetComponent<Image>().GetComponent<RectTransform>();
        Debug.Log("flashing in");
        SoundManagerScript.instance.PlaySingle(wooshSfx);
        LeanTween.alpha(rt, 1f, .75f).setEaseInCubic().setOnComplete(FlashOut);
        LeanTween.scale(rt.gameObject, new Vector3(.125f,.125f, 1f), .75f);
    }

    public void FlashOut()
    {
        SoundManagerScript.instance.PlaySingle(sfx);
        flashOut.SetActive(true);
        LeanTween.alpha(rt, 0f, 1.5f).setEaseInCubic();
    }
}
