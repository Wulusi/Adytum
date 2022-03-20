using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSelectionAnimator : MonoBehaviour
{
    [SerializeField]
    float startSize, speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animateCursorSelection();
    }

    void animateCursorSelection()
    {
        if(this.transform != null)
        {
            this.transform.localScale -= Vector3.one * Time.deltaTime * speed;
        }

        if(this.transform.localScale.x <= 0.1f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        this.transform.localScale = Vector3.one * startSize;
    }
}
