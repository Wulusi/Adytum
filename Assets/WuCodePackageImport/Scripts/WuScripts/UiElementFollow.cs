using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiElementFollow : MonoBehaviour
{
    public Canvas myCanvas;
    public Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        myCanvas = GetComponentInParent<Canvas>();
        followMouse();
    }

    public void followMouse()
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);

        transform.position = myCanvas.transform.TransformPoint(pos + offset);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
