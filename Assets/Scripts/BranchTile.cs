using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchTile : TileG
{
    BranchTileManager btm;
    GameObject pointer;
    private void Awake()
    {
        pointer = transform.GetChild(0).gameObject;
        btm = gameObject.GetComponentInParent<BranchTileManager>();
        layNum = layerMask.value;
        CheckForNextTileAndNotFinishedCircle();
    }
    private void FixedUpdate()
    {
        pointer.SetActive(btm.playerIsHere);
    }
    public void PointerClicked()
    {
        btm.nextTile = nextTile;
        StartCoroutine(btm.NullNextTile());
    }
}
