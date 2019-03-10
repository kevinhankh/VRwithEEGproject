using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Phase 4:
 * - spawn restorative object (cylinder) and 2 enemies
 * - the 2 enemies will respawn every 10 seconds upon death
 * - 1 extra enemy spawns behind the player every 30 seconds
 * - a bonus switch to help fill the cylinder spawns every 15 seconds in a random location
 * - when the cylinder is filled completely, the player wins the game
 */
public class Phase4Script : MonoBehaviour {

    public GameObject LevelManager;
    public GameObject EnemyPrefab;
    public GameObject BonusSwitchPrefab;

    public GameObject Restorative;
    public GameObject Spawner1;
    public GameObject Spawner2;
    public GameObject Spawner3;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject BonusSwitch;
    public GameObject BonusSpawner1;
    public GameObject BonusSpawner2;
    public GameObject BonusSpawner3;
    public GameObject BonusSpawner4;
    public AudioClip BonusFillSfx;

    private AudioSource sound;

    private float timer1;
    private float timer2;
    private float timer3;
    private float bonusTimer;
    private int bonusLocation;
    private bool bonusSfx;
    
    private const int phaseId = 4;
    private const int enemyCooldown = 10;
    private const int enemyCooldown2 = 30;
    private const int bonusCooldown = 20;
    private const int bonusFill = 5;

    // initialization
    void Start () {
        LevelManager = GameObject.FindWithTag("LevelManager");
        sound = GetComponent<AudioSource>();
        bonusSfx = false;
        timer1 = 0;
        timer2 = 0;

        Enemy1 = (GameObject)Instantiate(EnemyPrefab, Spawner1.transform.position, Spawner1.transform.rotation);
        Enemy2 = (GameObject)Instantiate(EnemyPrefab, Spawner2.transform.position, Spawner2.transform.rotation);
    }

    private void exitPhase()
    {
        LevelManager.GetComponent<LevelManagerScript>().changePhase(phaseId);
    }

    // Update is called once per frame
    void Update () {
		if(Restorative.GetComponent<RestorativeScript>().checkFilled())
        {
            Destroy(Enemy1);
            Destroy(Enemy2);
            Destroy(Enemy3);
            Destroy(BonusSwitch);
            exitPhase();
        }

        if(Enemy1 == null)
        {
            timer1 += Time.deltaTime;
            if(timer1 >= enemyCooldown)
            {
                Enemy1 = (GameObject)Instantiate(EnemyPrefab, Spawner1.transform.position, Spawner1.transform.rotation);
                timer1 = 0;
            }
        }
        if(Enemy2 == null)
        {
            timer2 += Time.deltaTime;
            if (timer2 >= enemyCooldown)
            {
                Enemy2 = (GameObject)Instantiate(EnemyPrefab, Spawner2.transform.position, Spawner2.transform.rotation);
                timer2 = 0;
            }
        }
        if(Enemy3 == null)
        {
            timer3 += Time.deltaTime;
            if (timer3 >= enemyCooldown2)
            {
                Enemy3 = (GameObject)Instantiate(EnemyPrefab, Spawner3.transform.position, Spawner3.transform.rotation);
                timer3 = 0;
            }
        }

        if(BonusSwitch == null)
        {
            bonusTimer += Time.deltaTime;
            if(bonusTimer >= bonusCooldown)
            {
                bonusLocation = Random.Range(1, 3);
                switch(bonusLocation)
                {
                    case 1:
                        BonusSwitch = (GameObject)Instantiate(BonusSwitchPrefab, BonusSpawner1.transform.position, BonusSpawner1.transform.rotation);
                        break;
                    case 2:
                        BonusSwitch = (GameObject)Instantiate(BonusSwitchPrefab, BonusSpawner2.transform.position, BonusSpawner2.transform.rotation);
                        break;
                    case 3:
                        BonusSwitch = (GameObject)Instantiate(BonusSwitchPrefab, BonusSpawner3.transform.position, BonusSpawner3.transform.rotation);
                        break;
                    case 4:
                        BonusSwitch = (GameObject)Instantiate(BonusSwitchPrefab, BonusSpawner4.transform.position, BonusSpawner4.transform.rotation);
                        break;
                }
                bonusTimer = 0;
            }
        }
        else
        {
            if(BonusSwitch.GetComponent<BonusSwitchScript>().getStatus())
            {
                Restorative.GetComponent<RestorativeScript>().addFill(bonusFill);
                Destroy(BonusSwitch);
                sound.clip = BonusFillSfx;
                sound.Play();
                bonusSfx = true;
            }
        }

        if(bonusSfx)
        {
            bonusSfx = false;
        }
    }
}
