using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSelection : MonoBehaviour
{
    [SerializeField]
    Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    [SerializeField]
    private List<GameObject> selectedObjs = new List<GameObject>();

    [SerializeField]
    private GameObject selectionCursorPrefab;

    RaycastHit hit;

    bool dragSelect = false;

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1, p2;

    Vector2[] corners;
    Vector3[] verts;
    Vector3[] vecs;

    // Start is called before the first frame update
    void Start()
    {
        //selectedTable = GameHub.GameManager.selectedObjects;
        dragSelect = false;
    }

    // Update is called once per frame
    void Update()
    {
        //For movement selection
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameHub.GameManager.setMousePositionVector(hit.point);
                GameHub.ObjectPooler.SpawnFromPool(selectionCursorPrefab.name, hit.point, Quaternion.identity);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            p1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragSelect == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out hit, 5000f))
                {
                    int id = hit.transform.GetInstanceID();
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        addSelected(hit.transform.gameObject);
                        //selectedTable.Add(id, hit.transform.gameObject);
                    }
                    else
                    {
                        deselectAll();
                        addSelected(hit.transform.gameObject);
                        //selectedTable.Add(id, hit.transform.gameObject);
                    }
                }
                else
                {
                    int id = hit.transform.GetInstanceID();
                    if (Input.GetKey(KeyCode.LeftShift))
                    {

                    }
                    else
                    {
                        deselectAll();
                    }
                }
            }
            else //marquee select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, (1 << 7)))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    deselectAll();
                }

                Destroy(selectionBox, 0.02f);

            }//end marquee select

            dragSelect = false;
        }
    }

    public void deselectAll()
    {
        foreach (GameObject selected in selectedObjs)
        {
            Unit selectedUnit = selected.GetComponentInParent<Unit>();

            if (selectedUnit != null)
            {
                selectedUnit.setSelectionCursor(false);
            }
            //Get rid of selection graphic of some type etc
        }

        selectedTable.Clear();
        selectedObjs.Clear();
    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x) //if p1 is to the left of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else //if p1 is below p2
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else //if p1 is to the right of p2
        {
            if (p1.y > p2.y) // if p1 is above p2
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else //if p1 is below p2
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }

        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }

    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }
    private void OnTriggerEnter(Collider other)
    {
        //int id = other.transform.gameObject.GetInstanceID();
        Debug.Log("Triggered mesh collider " + other.name);
        addSelected(other.gameObject);
    }

    public void addSelected(GameObject gObj)
    {
        int id = gObj.GetInstanceID();

        if (!selectedTable.ContainsKey(id))
        {
            selectedTable.Add(id, gObj);
        }

        Unit selectedUnit = gObj.GetComponentInParent<Unit>();

        if (selectedUnit != null && !selectedObjs.Contains(selectedUnit.gameObject))
        {
            selectedObjs.Add(selectedUnit.gameObject);
            selectedUnit.setSelectionCursor(true);
        }
    }
}
