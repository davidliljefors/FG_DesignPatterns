using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{

    AI.Map map;

    void Start()
    {
        TextAsset txt = Resources.Load("MapSettings/map_3") as TextAsset;
        map = AI.Map.ParseMap(txt.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
