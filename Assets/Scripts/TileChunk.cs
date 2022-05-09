using System;
using UnityEngine;
using UnityEditor;

public class TileChunk
{
    LineRenderer lineRenderer;
    public Vector3 p0, p1, p2, p3;
    public Vector3 startPoint, endPoint;
    int numPoints;
    public Vector3[] positions;
    GameObject bezierCurve;

    public TileChunk(Vector3 startPoint, Vector3 endPoint)
    {
        this.startPoint = startPoint; this.endPoint = endPoint;
        numPoints = (int)Vector3.Distance(startPoint, endPoint);
        positions = new Vector3[numPoints];

        bezierCurve = new GameObject();
        bezierCurve.name = "BezierCurve preview";
        bezierCurve.tag = "Preview";
        lineRenderer = bezierCurve.AddComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.material = Resources.Load("MapBuilder/Green") as Material;
    }

    /*
     *  Delete the preview curves on cancel or on build
     */
    public void DeleteCorrespondingGameObj()
    {
        GameObject.DestroyImmediate(bezierCurve);
    }

    public void DrawLinearCurve(bool editMode)
    {
        /*
         *  If user is in edit mode
         *  draw handles and edit the construction
         *  
         *  else follow draw a line from start point(first click)
         *  to the end point (realtime mouse position)
         *  
         *  When using edit mode make sure to have "Top" enabled
         *  on the orientation tool
         */

        Handles.color = Color.yellow;
        if (editMode)
        {
            Vector3
            newPos = Handles.FreeMoveHandle(p0, Quaternion.identity, 0.5f, Vector3.zero,
                Handles.CylinderHandleCap);
            if(p0 != newPos)
            {
                p0 = newPos;
            }
            Vector3
            newPos1 = Handles.FreeMoveHandle(p1, Quaternion.identity, 0.5f, Vector3.zero,
                Handles.CylinderHandleCap);
            if (p1 != newPos1)
            {
                p1 = newPos1;
            }
            Vector3
            newPos2 = Handles.FreeMoveHandle(p2, Quaternion.identity, 0.5f, Vector3.zero,
                Handles.CylinderHandleCap);
            if (p2 != newPos2)
            {
                p2 = newPos2;
            }
            Vector3
            newPos3 = Handles.FreeMoveHandle(p3, Quaternion.identity, 0.5f, Vector3.zero,
                Handles.CylinderHandleCap);
            if (p3 != newPos3)
            {
                p3 = newPos3;
            }
        }
        else
        {
            p0 = startPoint; p3 = endPoint;
            Vector3 midPoint = (p0 + p3) / 2;
            p1 = (p0 + midPoint) / 2; p2 = (p3 + midPoint) / 2;
        }
        numPoints = (int)Vector3.Distance(startPoint, endPoint);
        positions = new Vector3[numPoints];
        lineRenderer.positionCount = numPoints;



        /*
         * Calculating the Cubic Bezier Point
         * and drawing the preview line
         */
        for (int i = 0; i < numPoints + 1; i++)
        { 
            if (i == 0)        // Including the point at t = 0.0f (0.1f isn't drawn)
            {
                float t = i;
                positions[i] = CalculateCubicBezierPoint(t, p0, p1,
                    p2, p3);
            }
            else  // All the other points that aren't t = 0.0f or 0.1f
            {
                float t = i / (float)numPoints;
                positions[i - 1] = CalculateCubicBezierPoint(t, p0, p1,
                    p2, p3);
            }
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1,
        Vector3 p2, Vector3 p3)
    {
        float t1 = 1 - t;
        Vector3 result = t1 * t1 * t1 * p0;
        result += 3 * t1 * t1 * t * p1;
        result += 3 * t1 * t * t * p2;
        result += t * t * t * p3;
        return result;
    }
}
