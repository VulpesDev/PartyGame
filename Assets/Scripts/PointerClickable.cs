using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerClickable : MonoBehaviour
{
    private void OnMouseDown()
    {
        gameObject.GetComponentInParent<BranchTile>().PointerClicked();
    }
}
