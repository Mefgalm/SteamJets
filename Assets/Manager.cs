using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public Camera camera;
    public Controller player;

    public float cameraDistOffset;

    // Use this for initialization
    void Start()
    {
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -cameraDistOffset);
    }

    // Update is called once per frame
    void Update()
    {        
        transform.rotation = Quaternion.Euler(Vector3.zero);
        //camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -cameraDistOffset);
    }
}
