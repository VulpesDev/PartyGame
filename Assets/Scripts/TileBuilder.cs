using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileBuilder : EditorWindow
{
    [MenuItem("Window/Custom Tools/Enable_TileBuilder")]
    public static void Enable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    [MenuItem("Window/Custom Tools/Disable_TileBuilder")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    static bool started = false;
    public static GameObject prefab, prefabPreview;
    static void Start()
    {


        /*
         *  Find the Tiles parent GameObj
         *  if there isn't any, create one.
         */
        if (GameObject.Find("Tiles") != null)
            parentTiles = GameObject.Find("Tiles");
        else
        {
            Debug.Log("Created 'Tiles' gameObj");

            parentTiles = new GameObject();
            parentTiles.name = "Tiles";
            parentTiles.AddComponent<Tiles>();
        }

        /*
         * Load Tile prefab
         */
        prefabPreview = Resources.Load("MapBuilder/TilePreview") as GameObject;
        started = true;
    }

    static bool preview = false, inverse = false;
    static int layerMask = LayerMask.GetMask("Ground");
    static GameObject parentTiles;
    static TileChunk tc;
    static List<TileChunk> tcs = new List<TileChunk>();
    static int toolbarInt = 0;
    static string[] toolbarStrings = { "Build lines", "Edit lines" };

    private static void OnSceneGUI(SceneView sceneview)
    {

        if (!started) Start();

        /*
         *  Draw the UI
         */

        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, 10, 200, 200));
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        if(GUILayout.Button("Submit Selection"))
        {
            try
            {
                foreach (var chunk in tcs)
                {
                    CreateConstruction(chunk);
                }

                //Delete all preview lines
                foreach (var tc in tcs)
                {
                    tc.DeleteCorrespondingGameObj();
                }
                tcs.Clear();
            }
            catch (InvalidOperationException)
            {

            }
        }
        if(GUILayout.Button("Cancel Selection"))
        {
            RemovePreviewTiles();
            preview = false;
            //Delete all preview lines
            foreach (var tc in tcs)
            {
                tc.DeleteCorrespondingGameObj();
            }
            tcs.Clear();
        }

        if (toolbarInt == 1)
        {
            spacing = EditorGUILayout.IntField("Spacing: ", spacing);
            inverse = EditorGUILayout.Toggle("Inverse: ", inverse);
        }
        if(GUILayout.Button("Reset"))
        {
            started = false;
        }

        GUILayout.EndArea();
        Handles.EndGUI();

        /*
         *  Manage preview, edit, create mode
         */
        Event e = Event.current;
        if (toolbarInt == 0)
        {
            Vector3 mousePos = Calculations.GetMousePos(e, sceneview);
            
            Ray ray = sceneview.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
            {
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    Tools.current = Tool.View;
                    if (!preview)
                    {
                        preview = true;
                        tc = new TileChunk(new Vector3(hit.point.x, hit.point.y + 0.1f,
                            hit.point.z),
                            new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z));
                        tcs.Add(tc);
                    }
                    else
                    {
                        preview = false;
                    }
                }

                if (preview)
                {
                    try { tc.DrawLinearCurve(false); } catch (IndexOutOfRangeException) { }
                    tc.endPoint = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
                    
                    RemovePreviewTiles();
                    foreach (var chunk in tcs)
                    {
                        GameObject parentCurve = new GameObject("CurveParent");
                        parentCurve.tag = "ParentCurvePreview";
                        parentCurve.transform.parent = parentTiles.transform;
                        CreateConstructionPreview(chunk, parentCurve.transform);
                    }
                }
            }
        }
        else if (toolbarInt == 1)
        {
            RemovePreviewTiles();
            foreach (var chunk in tcs)
            {
                try { chunk.DrawLinearCurve(true); } catch (IndexOutOfRangeException) { }
                GameObject parentCurve = new GameObject("CurveParent");
                parentCurve.tag = "ParentCurvePreview";
                parentCurve.transform.parent = parentTiles.transform;
                CreateConstructionPreview(chunk, parentCurve.transform);
            }
        }

    }

    /*
     *  Button function that places
     *  tiles on the preview line on every
     *  second point of the curve
     */
    static int spacing = 3;
    static void CreateConstruction(TileChunk linePreview)
    {
        GameObject[] previewTiles = GameObject.FindGameObjectsWithTag("PreviewTile");
        foreach (var item in previewTiles)
        {
            item.tag = "Tile";
            //Give another Material Later!
            //item.GetComponent<Renderer>().material = 
        }
        GameObject[] previewParents = GameObject.FindGameObjectsWithTag("ParentCurvePreview");
        foreach (var item in previewParents)
        {
            item.tag = "ParentCurve";
        }
    }
    static void CreateConstructionPreview(TileChunk linePreview, Transform parentCurve)
    {

        /*
        * Make tiles spawn as a Preview
        */
        List<GameObject> tiles =  new List<GameObject>();
        int counter = 0;
        for (int i = 0; i < linePreview.positions.Length; i++)
        {

            if (counter > spacing)
            {
               GameObject inst = Instantiate(prefabPreview, linePreview.positions[i], Quaternion.identity,
                    parentCurve);
                inst.tag = "PreviewTile";
                inst.GetComponent<Tile>().tileID = tiles.Count;
                tiles.Add(inst);
                counter = 0;
            }
            else
            {
                counter++;
            }
        }

        /*
         * Calculate the direction
         */
        if(inverse)
        {
            for (int i = 0; i < tiles.Count - 1; i++)
            {
                    tiles[i].GetComponent<Tile>().direction =
                        (tiles[i + 1].transform.position - tiles[i].transform.position).normalized;
            }
        }
        else
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (i == 0)
                {
                    tiles[i].GetComponent<Tile>().direction =
                        (linePreview.positions[0] - tiles[i].transform.position).normalized;
                }
                else
                {
                    tiles[i].GetComponent<Tile>().direction =
                        (tiles[i - 1].transform.position - tiles[i].transform.position).normalized;
                }
            }
        }
        
    }
    static void RemovePreviewTiles()
    {
        GameObject[] previewTiles = GameObject.FindGameObjectsWithTag("PreviewTile");
        foreach (var item in previewTiles)
        {
            GameObject.DestroyImmediate(item);
        }

        GameObject[] previewParents = GameObject.FindGameObjectsWithTag("ParentCurvePreview");
        foreach (var item in previewParents)
        {
            GameObject.DestroyImmediate(item);
        }
    }
}