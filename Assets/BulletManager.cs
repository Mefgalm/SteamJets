using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.Networking;

public class BulletManager : NetworkBehaviour
{
    public GameObject bulletObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
 	}
    
    void OnCollisionEnter2D(Collision2D collision)
    {       
        Health health = collision.gameObject.GetComponent<Health>();
        Rigidbody2D rigidBody2D = collision.gameObject.GetComponent<Rigidbody2D>();

        if(rigidBody2D != null)
        {
            rigidBody2D.velocity = Vector3.zero;
            rigidBody2D.freezeRotation = true;
        }
        if (health != null)
        {
            health.TakeDamage(10);
        }

        Destroy(bulletObject);
    }
}
