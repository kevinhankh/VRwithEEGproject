using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSwitchScript : MonoBehaviour {

    public GameObject EEGManager;

    private bool switchStatus;
    private Behaviour halo;
    private Light lit;

    // Use this for initialization
    void Start()
    {
        EEGManager = GameObject.FindWithTag("EEGManager");
        halo = (Behaviour)gameObject.GetComponent("Halo");
        lit = GetComponent<Light>();
        switchStatus = false;
    }

    //Gazed behaviour
    public void activate()
    {
        halo.enabled = true;
        EEGManager.GetComponent<EEGManagerScript>().setTarget(this.gameObject);
    }

    //No longer gazed
    public void ignore()
    {
        halo.enabled = false;
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
    }

    //Activates the switch and flag the status to true.
    public void interaction()
    {
        switchStatus = true;

        if (switchStatus)
        {
            lit.range = 3;
        }
    }

    public bool getStatus()
    {
        return switchStatus;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
