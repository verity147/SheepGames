using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PP_TileSetter : MonoBehaviour {

    public Tilemap tilemap;

	void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = CellCenterFromClick(Input.mousePosition);
            print("Position: " + pos.ToString() + " Tilemap:" + tilemap.GetTile(tilemap.WorldToCell(pos)));
        }
    }

    ///gives closest center of a cell in referenced tilemap from a vector3
    private Vector3 CellCenterFromClick(Vector3 mousePos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cell = tilemap.WorldToCell(worldPos);
        Vector3 cellCenter = tilemap.GetCellCenterWorld(cell);
        return cellCenter;
    }
}
