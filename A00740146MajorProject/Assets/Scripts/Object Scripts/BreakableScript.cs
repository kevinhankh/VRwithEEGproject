using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableScript : MonoBehaviour {

    public GameObject EEGManager;
    public Material DefaultMaterial;
    public Material DamagedMaterial;
    public AudioClip BreakableHitSfx;

    private AudioSource sound;
    private Behaviour halo;
    private Renderer rend;

    private int health;
    private float damageTimer;
    private bool damageCooldown;
    private bool beginRotation;
    private float xMove;
    private float zMove;
    
    private const int healthPoints = 10;

    // initialization
    void Start()
    {
        EEGManager = GameObject.FindWithTag("EEGManager");
        sound = GetComponent<AudioSource>();
        sound.clip = BreakableHitSfx;
        halo = (Behaviour)gameObject.GetComponent("Halo");
        rend = GetComponent<Renderer>();
        health = healthPoints;
        damageTimer = 0;
        damageCooldown = false;
        beginRotation = false;
    }

    //Gazed behaviour
    public void activate()
    {
        EEGManager.GetComponent<EEGManagerScript>().setTarget(this.gameObject);
        halo.enabled = true;
    }

    //No longer gazed
    public void ignore()
    {
        EEGManager.GetComponent<EEGManagerScript>().resetTarget();
        halo.enabled = false;
        resetStatus();
    }

    //Takes damage every second and rotates faster as it gets more damaged
    public void interaction()
    {
        rend.material = DamagedMaterial;
        if (!damageCooldown && (health > 0))
        {
            health -= 2;
            damageCooldown = true;
            beginRotation = true;
            sound.Play();
        }
    }

    public void resetStatus()
    {
        rend.material = DefaultMaterial;
    }

    //Rotation behaviour
    public void movement()
    {
        if (health > 8)
        {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        }
        else if (health > 6)
        {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 50);
        }
        else if (health > 4)
        {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 100);
        }
        else if (health > 2)
        {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 200);
        }
        else
        {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 400);
        }
    }

    public void death()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if (health == 0)
        {
            death();
        }
        if (damageCooldown)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > 1.5f)
            {
                damageCooldown = false;
                damageTimer = 0;
            }
        }
        
        if (beginRotation)
            movement();
    }
}
