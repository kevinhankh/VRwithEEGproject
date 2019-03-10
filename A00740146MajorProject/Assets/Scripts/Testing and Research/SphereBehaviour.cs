using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereBehaviour : MonoBehaviour {

    public GameObject EEGManager;

    private bool active;
    private bool flipSwitch;
    private bool startTimer;
    private float switchTimer;
    private Behaviour halo;

	// Use this for initialization
	void Start () {
        active = false;
        flipSwitch = false;
        EEGManager = GameObject.FindWithTag("EEGManager");
        switchTimer = 0;
        startTimer = false;
        halo = (Behaviour)gameObject.GetComponent("Halo");
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
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
    }

    public void changeColor()
    {
        flipSwitch = !flipSwitch;

        if (flipSwitch)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update () {
		if(active)
        {
            if(EEGManager.GetComponent<EEGManagerScript>().getBlink())
            {
                if (startTimer == false)
                {
                    startTimer = true;
                    changeColor();
                }
            }
        }

        if(startTimer)
        {
            switchTimer += Time.deltaTime;
            if(switchTimer > 0.5)
            {
                startTimer = false;
                switchTimer = 0;
            }
        }
	}
}
