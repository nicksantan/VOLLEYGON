using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rotateShapeScript : MonoBehaviour
{
    public float amount = .3f;
    public float speed = 50f;
    public List<Sprite> shapes;
    public int currentShape = 0;

    public void Update()
    {
        //transform.Rotate(new Vector3(0f, 0f, amount) * Time.deltaTime * speed);
    }

    public void IncrementShape()
    {
        currentShape++;
        currentShape = currentShape % shapes.Count;
        ChooseNewShape(currentShape);
    }

    public void ChooseNewShape(int whichShape)
    {
        gameObject.GetComponent<Image>().sprite = shapes[whichShape];
    }
}
       
