using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWallScan : MonoBehaviour
{
    public int scanMapSizeX = 20;
    public int scanMapSizeY = 20;

    private Collider2D[]    col;
    public  List<Vector2>   wallVectorList;

    void Start()
    {
        for (int y = 0; y < scanMapSizeY; y++)
        {
            for (int x = 0; x < scanMapSizeX; x++)
            {
                col = Physics2D.OverlapCircleAll(new Vector2(x, y), 0.4f);

                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(x, y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
                    {
                        Vector2 wallVector = new Vector2(x, y);
                        wallVectorList.Add(wallVector);
                    }
            }
        }
    }
}
