using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BranchTileP))]
public class BranchTilePeditor : Editor
{
    public override void OnInspectorGUI()
    {
        BranchTileP myScript = (BranchTileP)target;
        int childCount = myScript.gameObject.transform.childCount;
        if (GUILayout.Button("+"))
        {
            Instantiate(Resources.Load<GameObject>("MapBuilder/TileBranch"),
            myScript.gameObject.transform.position,
            Quaternion.identity, myScript.gameObject.transform);
        }
        if (GUILayout.Button("-"))
        {
            if (childCount >= 1)
                DestroyImmediate(myScript.gameObject.transform
                    .GetChild(childCount - 1).gameObject);
        }

        GUILayout.Label($"Branches: {childCount}");

        if(GUILayout.Button("Map Branches"))
        {
            myScript.checkForMouse = true;
        }
    }
}
