///----------------------------------------------------------------------------------
///   GAME3011_A3_ShahHardik
///   Board.cs
///   Author            : Hardik Dipakbhai Shah
///   Last Modified     : 2022/03/23
///   Description       : 
///   Revision History  : 1st ed.                    

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;

    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    private void Start()
    {
        Tiles = new Tile[rows.Max(Row => Row.tiles.Length), rows.Length]; //(Row => Row.tiles.length), rows.Length];
        
        for(var y=0; y< Height; y++)
        {
            for(var x=0;x<Width;x++)
            {
                var Tile = rows[y].tiles[x];

                //Tiles[x, y] = rows[y].tiles[x];
                Tile.x = x;
                Tile.y = y;

                Tile.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];

                Tiles[x, y] = Tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
