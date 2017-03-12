using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public Controller player;
    public float coeff;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = new Vector3(player.transform.position.x * coeff, player.transform.position.y * coeff, 0);
    }
}
