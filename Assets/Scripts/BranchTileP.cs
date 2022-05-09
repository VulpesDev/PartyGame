using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BranchTileP : MonoBehaviour
{
#if UNITY_EDITOR
    public int counter = 0;
    public bool checkForMouse;
    public LayerMask layerMask;
    int layerMaskNum;
    Transform[] children;
    private void OnValidate()
    {
        layerMaskNum = layerMask.value;
        SceneView.duringSceneGui += CustomUpdate;
    }
    void CustomUpdate(SceneView sceneview)
    {
        try
        {
            children = new Transform[transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = transform.GetChild(i);
                children[i].GetComponent<Tile>().tileID = i;
            }

        }
        catch (MissingReferenceException) { }


        if (!checkForMouse) return;
        if (counter >= children.Length)
        {
            counter = 0;
            Tools.current = Tool.Move;
            checkForMouse = false;
        }
        if (counter < children.Length)
        {
            Tools.current = Tool.View;
        }
        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector3 mousePos = Calculations.GetMousePos(e, sceneview);
            Ray ray = sceneview.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, float.MaxValue, layerMaskNum))
            {
                children[counter].rotation = Quaternion.LookRotation(
                    (hit.collider.transform.position - transform.position).normalized,
                    Vector3.up);
                counter++;
            }
        }
    }
#endif
}
