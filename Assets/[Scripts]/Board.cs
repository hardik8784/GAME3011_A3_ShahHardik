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
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }

    [SerializeField]
    private AudioClip collectSound;

    [SerializeField]
    private AudioSource audioSource;

    public Row[] rows;

    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    //private Tile _selectedTile1;
    //private Tile _selectedTile2;

    [SerializeField] private float Duration = 0.1f;

    private readonly List<Tile> _selection = new List<Tile>();

    float currentTime = 0.0f;
    float startingTime = 60.0f;

    [SerializeField]
    private TextMeshProUGUI TimeText;

    public GameObject UI;

   

    private void Awake()
    {
        Time.timeScale = 1;
        Instance = this;

        currentTime = startingTime;
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
        if(currentTime > 0)
        {
            currentTime -= 1 * Time.deltaTime;
            //Debug.Log("CurrentTime : " + currentTime);

            TimeText.SetText("Time : 00 : " + currentTime.ToString("0"));

            TimeText.color = Color.white;
        }

        //currentTime -= 1 * Time.deltaTime;
        ////Debug.Log("CurrentTime : " + currentTime);

        //TimeText.SetText("Time : " + currentTime.ToString("0"));

        if(currentTime <10 )
        {
            TimeText.color = Color.red;
        }

        if(ScoreCounter.Instance.Score >= 2000)
        {
             StartCoroutine(ExampleCoroutine());
           
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            Time.timeScale = 0;

            UI.SetActive(true);
            this.gameObject.SetActive(false);
            
        }
        //if(!Input.GetKeyDown(KeyCode.A))
        //{
        //    return;
        //}

        //foreach(var connectedTile in Tiles[0,0].GetConnectedTiles())
        //{
        //    connectedTile.icon.transform.DOScale(1.25f, Duration).Play();
        //}
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        //Time.timeScale = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    public async void Select(Tile tile)
    {
        if(!_selection.Contains(tile))
        {
            //if (_selection.Count > 0)
            //{
            //    //TODO:
            //    //Implement the behaviour not to swap if the tiles are farther
            //}
            //else
            //{
                _selection.Add(tile);
            //}
        }



        if(_selection.Count <2)
        {
            return;
        }

        Debug.Log("Selected Tiles : {" + _selection[0].x + "," + _selection[0].y + "} and {" + _selection[1].x + "," + _selection[1].y + "}");

        await Swap(_selection[0], _selection[1]);

        if(Canpop())
        {
            Pop();
        }
        else
        {
            await Swap(_selection[0], _selection[1]);
        }
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

    private bool Canpop()
    {
        for(var y =0; y< Height;y++)
        {
            for(var x=0;x<Width;x++)
            {
                if(Tiles[x,y].GetConnectedTiles().Skip(1).Count() >=2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private async void Pop()
    {
        for(var y=0; y< Height; y++)
        {
            for(var x=0; x<Width;x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if(connectedTiles.Skip(1).Count() < 2)
                {
                    continue;
                }
               
                var deafaultSequence = DOTween.Sequence();
                

                foreach(var connectedTile in connectedTiles)
                {
                    deafaultSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, Duration));

                    audioSource.PlayOneShot(collectSound);

                    ScoreCounter.Instance.Score += tile.Item.value * connectedTiles.Count;

                    await deafaultSequence.Play()
                                          .AsyncWaitForCompletion();
                }

             

                var inflateSequence = DOTween.Sequence();

                foreach(var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDataBase.Items[Random.Range(0, ItemDataBase.Items.Length)];

                    inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, Duration));
                }

                await inflateSequence.Play()
                                     .AsyncWaitForCompletion();

                x = 0;
                y = 0;
            }
        }
    }
}
