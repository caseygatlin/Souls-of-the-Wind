using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool twoPlayers = false;
    public bool westWind = true;
    public bool eastWind = true;
    public bool northWind = true;
    public bool southWind = true;


	// Use this for initialization
	void Start () {
        if (GameObject.Find("Char") && GameObject.Find("2ndPlayer"))
            twoPlayers = true;
        else
            twoPlayers = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
