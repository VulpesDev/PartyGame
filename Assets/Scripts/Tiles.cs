using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public static Tile[] tiles;
    private void OnValidate()
    {
        Refresh();
    }
    public void Refresh()
    {
        tiles = GetComponentsInChildren<Tile>();
    }
}
