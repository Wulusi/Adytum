using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildAreaManager : MonoBehaviour
{
    public List<GameObject> buildableTiles = new List<GameObject>();
    public List<GameObject> removedTiles = new List<GameObject>();
    public List<GameObject> UIElements = new List<GameObject>();
    public GameObject selectedTile;
    public Material highlightMaterial;
    public string BuildableTileMask;
    public bool noSelection;

    ObjectPooler objectPooler;

    private void Start()
    {
        DisableSprites();
        StartCoroutine(_detectRay());
        GetUIElements();
        objectPooler = ObjectPooler.Instance;
    }

    //Disables mesh rendered of the buildable areas so that it appears transparent and invisible until the mouse
    //is hovered over the tiles
    private void DisableSprites()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buildableTiles.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).gameObject.GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.inactive;
            noSelection = true;
        }
    }

    //Main functional loop for this script, runs a if statement that detects a ray fired from the mouse's position
    private IEnumerator _detectRay()
    {
        while (true)
        {
            if (noSelection)
            {
                detectRay();
            }
            ResetSelection();
            yield return null;
        }
    }

    //Tile selection function used by tile Disabler
    public void SelectedTile(GameObject _selectedTile)
    {
        selectedTile = _selectedTile;
        if (selectedTile != null)
        {
            selectedTile.GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }

    //Main function that is used to detect mouse selection input and hover over tiles to change their states
    private void detectRay()
    {
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        LayerMask layerMask = (1 << LayerMask.NameToLayer(BuildableTileMask) | ~1 << LayerMask.NameToLayer("Towers"));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //print("ray hit " + hit.collider.name);
            //If ray hits a tile containing the component TileDisabler, then turn the selected tile into an activated state
            if (hit.collider.GetComponent<TileDisabler>())
            {
                hit.collider.gameObject.GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.active;
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
        }
        else
        {
            disableOverlay();
        }
    }

    private void disableOverlay()
    {
        for (int i = 0; i < buildableTiles.Count; i++)
        {
            buildableTiles[i].GetComponent<TileDisabler>().hoverstate = TileDisabler.Hoverstate.inactive;
        }
    }

    public void ResetSelection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            DisableSprites();
            DisableUI();
            SelectedTile(null);
        }
    }

    public void ManualReset()
    {
        DisableSprites();
        DisableUI();
        SelectedTile(null);
    }

    private void GetUIElements()
    {
        var UIElementFromArray = GameObject.FindObjectsOfType<UiElementFollow>();

        Debug.Log("UI Found are: " + UIElementFromArray);

        foreach (UiElementFollow uiElement in UIElementFromArray)
        {
            UIElements.Add(uiElement.gameObject);
        }
    }

    public void ActivateUI()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            UIElements[i].GetComponent<UiElementFollow>().gameObject.SetActive(true);
            UIElements[i].GetComponent<UiElementFollow>().followMouse();
        }
    }

    public void DisableUI()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            UIElements[i].GetComponent<UiElementFollow>().gameObject.SetActive(false);
            //UIElements[i].GetComponent<UiElementFollow>().followMouse();
        }
    }

    public void BuildAtLocation(GameObject tower)
    {
        Debug.Log(tower.name + " Built!");
        objectPooler.SpawnFromPool(tower.name, selectedTile.transform.position, Quaternion.identity);
        DisableUI();
        RemoveFromList(selectedTile.gameObject);

        //TO DO: countdown timer for building towers
        ManualReset();
    }

    public void RemoveFromList(GameObject objToRemove)
    {
        if (buildableTiles.Contains(objToRemove))
        {
            Debug.Log("Removing " + objToRemove.name);
            objToRemove.SetActive(false);
            buildableTiles.Remove(objToRemove);
            removedTiles.Add(objToRemove);
        }
    }
}
