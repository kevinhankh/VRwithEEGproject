using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keep all objects relevant to the Google Daydream VR functionalities to be persistent with no duplications.
public class GvrManagerScript : MonoBehaviour {

    public static GvrManagerScript instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
