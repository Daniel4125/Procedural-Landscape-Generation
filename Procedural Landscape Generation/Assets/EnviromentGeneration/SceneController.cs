using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

    public MapGenerator mapGen;
    //MapGenerator mapGen = GameObject.Find("LandscapeController").GetComponent<MapGenerator>();

    void Start () {
        mapGen.GenerateMap();
	}
}
