using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour {

    public GameObject EEGManager;
    public bool active;
    public Behaviour halo;

    void relaxed()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    void focused()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    void neutral()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void activate()
    {
        active = true;
        halo.enabled = true;
        EEGManager.GetComponent<EEGManagerScript>().setTarget(this.gameObject);
    }

    public void ignore()
    {
        active = false;
        halo.enabled = false;
        neutral();
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
    }

	// Use this for initialization
	void Start () {
        EEGManager = GameObject.FindWithTag("EEGManager");
        active = false;
        halo = (Behaviour)gameObject.GetComponent("Halo");
    }
	
	// Update is called once per frame
	void Update () {
		if(active)
        {
            if((EEGManager.GetComponent<EEGManagerScript>().getBeta() > 0.7) && (EEGManager.GetComponent<EEGManagerScript>().getGamma() > 0.7))
            {
                focused();
            }
            else if((EEGManager.GetComponent<EEGManagerScript>().getBeta() < 0.28) && (EEGManager.GetComponent<EEGManagerScript>().getGamma() < 0.28))
            {
                relaxed();
            }
            else
            {
                neutral();
            }
        }
	}
}
