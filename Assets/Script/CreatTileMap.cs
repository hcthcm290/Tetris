using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatTileMap : MonoBehaviour
{
    [SerializeField] GameObject tile;
    GameObject[,] map;
    void Start()
    {
        map = new GameObject[Block.width, Block.height];
        for(int x=0; x<Block.width; x++)
        {
            for(int y=0; y<Block.height; y++)
            {
                map[x, y] = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                map[x, y].transform.parent = this.transform;
            }
        }
    }
}
