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
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    public Row[] rows;

    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    //private Tile _selectedTile1;
    //private Tile _selectedTile2;

    [SerializeField] private float Duration = 0.1f;

    private readonly List<Tile> _selection = new List<Tile>();

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
    private void Update()
    {
        if(!Input.GetKeyDown(KeyCode.A))
        {
            return;
        }

        foreach(var connectedTile in Tiles[0,0].GetConnectedTiles())
        {
            connectedTile.icon.transform.DOScale(1.25f, Duration).Play();
        }
    }

    public async void Select(Tile tile)
    {
        if(!_selection.Contains(tile))
        {
            _selection.Add(tile);
        }

        if(_selection.Count <2)
        {
            return;
        }

        Debug.Log("Selected Tiles : {" + _selection[0].x + "," + _selection[0].y + "} and {" + _selection[1].x + "," + _selection[1].y + "}");

        await Swap(_selection[0], _selection[1]);

        _selection.Clear();

    }

    public async Task Swap(Tile tile1, Tile tile2)
    {
        var icon1 = tile1.icon;
        var icon1Transform = icon1.transform;
        var icon2 = tile2.icon;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1.transform.DOMove(icon2Transform.position, Duration))
                .Join(icon2Transform.DOMove(icon1Transform.position, Duration));

        await sequence.Play()
                          .AsyncWaitForCompletion();

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2;
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item;
        tile2.Item = tile1Item;
    }

    private void Canpop()
    {
        return;
    }

    private void Pop()
    {
        return;
    }
}
