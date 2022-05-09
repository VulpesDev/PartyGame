using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Tile : MonoBehaviour
{
#if (UNITY_EDITOR)
    public int tileID = 0;
    public Vector3 direction;

    private void Start()
    {
        if (EditorApplication.isPlaying) return;
        if (direction != Vector3.zero)
        transform.forward = direction;
        
    }
    private void Update()
    {
        direction = transform.forward;
        gameObject.name = "Tile " + tileID;
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }
    //private void OnValidate()
    //{
    //    Refresh();
    //}
    //public void Refresh()
    //{
    //    gameObject.name = "Tile " + tileID;
    //}
#endif
}
