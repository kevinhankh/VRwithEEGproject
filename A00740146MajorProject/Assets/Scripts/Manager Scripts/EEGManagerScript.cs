using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * EEGManager takes the brainwave and artifact values from EEGController, and calculates it for object interactions.
 * The targeting system using the VR gaze functionality will provide the EEGManager with an active target. Then this
 * will identify the target using its tag and uses the proper data for the interaction.
 */
public class EEGManagerScript : MonoBehaviour {

    public static EEGManagerScript instance = null;

    private bool blinkValue;
    private float betaValue;
    private float gammaValue;
    private float brainValue;

    public Text debugText;
    public Text debugText2;

    public GameObject Target;

    private bool objectActive;
    private float objectTimer;

    private const float FocusedThreshold = 0.72f;
    private const float RelaxedThreshold = 0.28f;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // initialization
    void Start () {

        DontDestroyOnLoad(this.gameObject);

        blinkValue = false;
        betaValue = 0.5f;
        gammaValue = 0.5f;
        brainValue = 0;

        objectActive = false;
        objectTimer = 0;
	}

    //Blink
    public void setBlink(bool data)
    {
        blinkValue = data;
        //if (blinkValue)
        //    debugText.text = "TRUE!";
        //else
        //    debugText.text = "FALSE!";
    }

    public bool getBlink()
    {
        return blinkValue;
    }

    //Beta
    public void setBeta(float data)
    {
        betaValue = data;
        debugText.text = "Beta: " + betaValue;
    }

    public float getBeta()
    {
        return betaValue;
    }
	
    //Gamma
    public void setGamma(float data)
    {
        gammaValue = data;
        debugText2.text = "Gamma: " + gammaValue;
    }

    public float getGamma()
    {
        return gammaValue;
    }

    //Combined average score of beta and gamma waves
    public void calculateBrainValue()
    {
        brainValue = (betaValue + gammaValue) / 2;
    }

    public float getBrainwave()
    {
        return brainValue;
    }

    //Target
    public void setTarget(GameObject NewTarget)
    {
        Target = NewTarget;
    }

    public GameObject getTarget()
    {
        return Target;
    }

    public void resetTarget()
    {
        Target = null;
        objectActive = false;
        objectTimer = 0;
    }

	// Update is called once per frame
	void Update () {
        calculateBrainValue();
        //Object interactions
		if(Target != null)
        {
            switch(Target.tag)
            {
                //Ring interaction: Activates shield and healing over time when the player is in a relaxed state.
                case "Ring":
                    if(brainValue < RelaxedThreshold)
                    {
                        Target.GetComponent<RingScript>().interaction();
                        objectActive = true;
                    }
                    else
                    {
                        if(objectActive)
                        {
                            Target.GetComponent<RingScript>().resetStatus();
                            objectActive = false;
                        }
                    }
                    break;
                //Light Switch interaction: Flip on and off when the player blinks. Uses half a second cooldown to prevent accidental spam.
                case "LightSwitch":
                    if(!objectActive)
                    {
                        if(blinkValue)
                        {
                            Target.GetComponent<LightSwitchScript>().interaction();
                            objectActive = true;
                        }
                    }
                    else
                    {
                        objectTimer += Time.deltaTime;
                        if (objectTimer > 0.5)
                        {
                            objectActive = false;
                            objectTimer = 0;
                        }
                    }
                    break;
                //Big Switch interaction: Activate the switch when the player blinks.
                case "BigSwitch":
                    if (!objectActive)
                    {
                        if (blinkValue)
                        {
                            Target.GetComponent<BigSwitchScript>().interaction();
                            objectActive = true;
                        }
                    }
                    else
                    {
                        objectTimer += Time.deltaTime;
                        if (objectTimer > 0.5)
                        {
                            objectActive = false;
                            objectTimer = 0;
                        }
                    }
                    break;
                //Bonus Switch interaction: Activate when the player blinks.
                case "BonusSwitch":
                    if (!objectActive)
                    {
                        if (blinkValue)
                        {
                            Target.GetComponent<BonusSwitchScript>().interaction();
                            objectActive = true;
                        }
                    }
                    else
                    {
                        objectTimer += Time.deltaTime;
                        if (objectTimer > 1)
                        {
                            objectActive = false;
                            objectTimer = 0;
                        }
                    }
                    break;
                //Enemy interaction: Damages the target enemy over time when the player is in a focused stated.
                case "Enemy":
                    if (brainValue > FocusedThreshold)
                    {
                        Target.GetComponent<EnemyScript>().interaction();
                        objectActive = true;
                    }
                    else
                    {
                        if (objectActive)
                        {
                            Target.GetComponent<EnemyScript>().resetStatus();
                            objectActive = false;
                        }
                    }
                    break;
                //Dummy target for the tutorial. Same behaviour as the enemy object without the movement and attacks.
                case "Dummy":
                    if (brainValue > FocusedThreshold)
                    {
                        Target.GetComponent<DummyScript>().interaction();
                        objectActive = true;
                    }
                    else
                    {
                        if (objectActive)
                        {
                            Target.GetComponent<DummyScript>().resetStatus();
                            objectActive = false;
                        }
                    }
                    break;
                //Breakable cube interaction: Damages the cube overtime with the focused state.
                case "Breakable":
                    if (brainValue > FocusedThreshold)
                    {
                        Target.GetComponent<BreakableScript>().interaction();
                        objectActive = true;
                    }
                    else
                    {
                        if (objectActive)
                        {
                            Target.GetComponent<BreakableScript>().resetStatus();
                            objectActive = false;
                        }
                    }
                    break;
                //Restroative cylinder interaction: Fills up the cylinder overtime with the relaxed state.
                case "Restorative":
                    if (brainValue < RelaxedThreshold)
                    {
                        Target.GetComponent<RestorativeScript>().interaction();
                        objectActive = true;
                    }
                    else
                    {
                        if (objectActive)
                        {
                            
                            objectActive = false;
                        }
                    }
                    break;
            }
        }
    }
}
