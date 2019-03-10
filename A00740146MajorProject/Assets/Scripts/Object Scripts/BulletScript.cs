using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    private const int bulletDamage = 5;

	// initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Special collision behaviour against player object. Player takes damage and bullet is destroyed.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(bulletDamage);
        }
        Destroy(this.gameObject);
    }
}
