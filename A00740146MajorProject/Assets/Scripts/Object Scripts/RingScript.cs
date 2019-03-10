using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingScript : MonoBehaviour {

    public GameObject EEGManager;
    public GameObject Player;
    public Material DefaultMaterial;
    public Material GazedMaterial;
    public Material ActiveMaterial;
    public AudioClip HealSfx;

    private AudioSource sound;
    private Renderer rend;

    private bool healCooldown;
    private float healTimer;
    private bool gazed;

    private const int healAmount = 8;

    // Use this for initialization
    void Start()
    {
        EEGManager = GameObject.FindWithTag("EEGManager");
        Player = GameObject.FindWithTag("Player");
        sound = GetComponent<AudioSource>();
        sound.clip = HealSfx;
        rend = GetComponent<Renderer>();
        healCooldown = false;
        healTimer = 0;
        gazed = false;
    }

    //Gazed behaviour
    public void activate()
    {
        EEGManager.GetComponent<EEGManagerScript>().setTarget(this.gameObject);
        rend.material = GazedMaterial;
        gazed = true;
    }

    //No longer gazed
    public void ignore()
    {
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
        rend.material = DefaultMaterial;
        gazed = false;
        Player.GetComponent<PlayerScript>().setShield(false);
    }

    //Turns the ring green when player is in a relaxed state. Heals the player every second it is activated.
    public void interaction()
    {
        rend.material = ActiveMaterial;
        if(!healCooldown)
        {
            Player.GetComponent<PlayerScript>().heal(healAmount);
            healCooldown = true;
        }
        if(!sound.isPlaying)
        {
            sound.Play();
        }
        Player.GetComponent<PlayerScript>().setShield(true);
    }
    
    public void resetStatus()
    {
        Player.GetComponent<PlayerScript>().setShield(false);

        if (gazed)
            rend.material = GazedMaterial;
        else
            rend.material = DefaultMaterial;
    }
	
	// Update is called once per frame
	void Update () {
		if(healCooldown)
        {
            healTimer += Time.deltaTime;
            if(healTimer > 1)
            {
                healCooldown = false;
                healTimer = 0;
            }
        }
    }
}
