///----------------------------------------------------------------------------------
///   GAME3011_A3_ShahHardik
///   ItemDataBase.cs
///   Author            : Hardik Dipakbhai Shah
///   Last Modified     : 2022/03/23
///   Description       : 
///   Revision History  : 1st ed.                    


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase 
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => Items = Resources.LoadAll<Item>("Items/");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
