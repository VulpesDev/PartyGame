using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Calculations
{
    public static Vector3 GetMousePos(Event e, SceneView sceneview)
    {
        Vector3 mousePos = e.mousePosition;
        float ppp = EditorGUIUtility.pixelsPerPoint;
        mousePos.y = sceneview.camera.pixelHeight - mousePos.y * ppp;
        mousePos.x *= ppp;
        return mousePos;
    }
}
