using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchTileManager : MonoBehaviour
{
    public GameObject nextTile;
    public bool playerIsHere = false;
    [SerializeField] LayerMask layerMask;
    int layerNum;
    private void Start()
    {
        nextTile = gameObject;
        layerNum = layerMask.value;
    }
    public IEnumerator NullNextTile()
    {
        yield return new WaitForSeconds(1);
        nextTile = gameObject;
    }
}
