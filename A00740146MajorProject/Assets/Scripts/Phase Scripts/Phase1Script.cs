using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Phase 1:
 * - 3 light switch spawns
 * - one is randomly picked to be the correct switch
 * - activating incorrect switches will spawn an enemy
 * - activating the correct switch will change the level to the next phase
 */
public class Phase1Script : MonoBehaviour {

    public GameObject LevelManager;
    public GameObject EnemyPrefab;
    
    public GameObject Switch1;
    public GameObject Switch2;
    public GameObject Switch3;
    public int correctSwitch;
    private bool switch1State;
    private bool switch2State;
    private bool switch3State;

    private const int phaseId = 1;

    // initialization
    void Start () {
        LevelManager = GameObject.FindWithTag("LevelManager");
        correctSwitch = Random.Range(1, 4);
        if (correctSwitch > 3)
            correctSwitch = 3;
        switch1State = false;
        switch2State = false;
        switch3State = false;
    }

    private void spawnEnemy(Transform spawnLocation)
    {
        Vector3 spawnPos = new Vector3(spawnLocation.position.x + 2, spawnLocation.position.y + 3, spawnLocation.position.z + 2);
        var newEnemy = (GameObject)Instantiate(EnemyPrefab, spawnPos, spawnLocation.rotation);
    }

    private void exitPhase()
    {
        LevelManager.GetComponent<LevelManagerScript>().changePhase(phaseId);
    }

    // Update is called once per frame
    void Update () {
        //checks for the correct light switch
        if (!switch1State)
        {
            if (Switch1.GetComponent<LightSwitchScript>().getStatus())
            {
                switch1State = true;
                if (correctSwitch != 1)
                    spawnEnemy(Switch1.transform);
                else
                    exitPhase();
            }
        }
        if (!switch2State)
        {
            if (Switch2.GetComponent<LightSwitchScript>().getStatus())
            {
                switch2State = true;
                if (correctSwitch != 2)
                    spawnEnemy(Switch2.transform);
                else
                    exitPhase();
            }
        }
        if (!switch3State)
        {
            if (Switch3.GetComponent<LightSwitchScript>().getStatus())
            {
                switch3State = true;
                if (correctSwitch != 3)
                    spawnEnemy(Switch3.transform);
                else
                    exitPhase();
            }
        }
    }
}
