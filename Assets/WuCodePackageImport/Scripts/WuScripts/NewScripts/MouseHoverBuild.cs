using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverBuild : MonoBehaviour
{
    [SerializeField]
    Color m_OriginalColor;

    [SerializeField]
    Color m_HoverOverColor;

    [SerializeField]
    SpriteRenderer m_spriteRenderer;

    ObjectPooler objectPooler;
    
    //TODO: create logic for enabling multiple tower selection
    [SerializeField]
    private GameObject TowerBase;

    private bool canBuild, canHover;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<SpriteRenderer>() != null)
        {
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_OriginalColor = m_spriteRenderer.color;
        }

        canHover = true;
        objectPooler = ObjectPooler.Instance;
        StartCoroutine(CheckForMouseClick());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CheckForMouseClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0) && canBuild)
            {
                objectPooler.SpawnFromPool
                    (TowerBase.name, 
                    this.transform.position, 
                    transform.rotation);
                m_spriteRenderer.enabled = false;
                canBuild = false;
                canHover = false;
                yield return null;
            }
            yield return null;
        }
    }

    private void OnMouseEnter()
    {
        if (canHover)
        {
            m_spriteRenderer.color = m_HoverOverColor;
            canBuild = true;
        }
    }

    private void OnMouseExit()
    {
        if (canHover)
        {
            m_spriteRenderer.color = m_OriginalColor;
            canBuild = false;
        }
    }
}
