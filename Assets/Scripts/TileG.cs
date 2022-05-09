using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileG : MonoBehaviour
{
    public GameObject nextTile;
    public LayerMask layerMask;
    protected int layNum;
    bool errorOnce = false;
    public static List<GameObject> emptyObjsList = new List<GameObject>();
    public static GameObject[] emptyObjs;
    void Start()
    {
        layNum = layerMask.value;
        CheckForNextTileAndNotFinishedCircle();
    }
    void CheckForNotFinishedCicle()
    {
        if (nextTile == null)
        {
            Debug.LogError("The cicle of tiles is not finished!\n"
                + "TileID: " + GetComponent<Tile>().tileID);
            emptyObjsList.Add(this.gameObject);
            errorOnce = true;
        }
        if (emptyObjsList.Count > 0)
        {
            emptyObjs = emptyObjsList.ToArray();
            Selection.objects = emptyObjs;
        }
    }
    protected void CheckForNextTileAndNotFinishedCircle()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, float.MaxValue,
            layNum))
        {
            nextTile = hit.collider.gameObject;
        }
        if (!errorOnce)
        {
            CheckForNotFinishedCicle();
            errorOnce = true;
        }
    }
}
