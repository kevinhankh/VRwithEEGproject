using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Phase 3: (logic puzzle)
 * - spawns 4 small lights and one big light
 * - player must find the right combination of small light states
 * - the big light is used to check the combination
 * - for each small light that is incorrect, an enemy will spawn
 * - when the correct combination has been confirmed, the level will change to the next phase
 */
public class Phase3Script : MonoBehaviour {

    public GameObject LevelManager;
    public GameObject EnemyPrefab;

    public GameObject Switch1;
    public GameObject Switch2;
    public GameObject Switch3;
    public GameObject Switch4;
    public GameObject BigSwitch;
    public GameObject EnemySpawner;
    private bool confirmation;
    private float confirmTimer;
    private bool checkSwitches;
    public List<bool> switchValues;
    private int randomSwitch;
    private float endTimer;
    private bool ending;
    
    private const int phaseId = 3;

    // initialization
    void Start () {
        LevelManager = GameObject.FindWithTag("LevelManager");
        for (int i = 0; i < 4; i++)
        {
            switchValues.Add(Random.value > 0.5f);
            Debug.Log("values: " + switchValues[i]);
        }
        if (switchValues[0] == switchValues[1] == switchValues[2] == switchValues[3] == false)
        {
            randomSwitch = Random.Range(0, 3);
            switchValues[randomSwitch] = true;
        }
        confirmation = false;
        checkSwitches = false;
        endTimer = 0;
        ending = false;
    }

    private void spawnEnemy()
    {
        var newEnemy = (GameObject)Instantiate(EnemyPrefab, EnemySpawner.transform.position, EnemySpawner.transform.rotation);
    }

    private void exitPhase()
    {
        LevelManager.GetComponent<LevelManagerScript>().changePhase(phaseId);
    }

    // Update is called once per frame
    void Update () {
        //check if the combination is correct when the big switch is activated
        if (BigSwitch.GetComponent<BigSwitchScript>().getConfirm())
        {
            if (!confirmation)
            {
                confirmation = true;
                checkSwitches = true;
                if (Switch1.GetComponent<LightSwitchScript>().getStatus() != switchValues[0])
                {
                    checkSwitches = false;
                    spawnEnemy();
                }
                if (Switch2.GetComponent<LightSwitchScript>().getStatus() != switchValues[1])
                {
                    checkSwitches = false;
                    spawnEnemy();
                }
                if (Switch3.GetComponent<LightSwitchScript>().getStatus() != switchValues[2])
                {
                    checkSwitches = false;
                    spawnEnemy();
                }
                if (Switch4.GetComponent<LightSwitchScript>().getStatus() != switchValues[3])
                {
                    checkSwitches = false;
                    spawnEnemy();
                }
                BigSwitch.GetComponent<BigSwitchScript>().confirmCheck(checkSwitches);
                if (checkSwitches)
                {
                    ending = true;
                }
            }
        }

        if (confirmation)
        {
            confirmTimer += Time.deltaTime;
            if (confirmTimer > 5)
            {
                confirmation = !confirmation;
                confirmTimer = 0;
                BigSwitch.GetComponent<BigSwitchScript>().resetStatus();
            }
        }

        if(ending)
        {
            endTimer += Time.deltaTime;
            if (endTimer > 1)
                exitPhase();
        }
    }
}
