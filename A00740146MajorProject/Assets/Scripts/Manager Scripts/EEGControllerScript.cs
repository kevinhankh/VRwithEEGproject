using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;

/*
 * EEGController handles connectivity and data parsing from the Muse EEG headband. This uses the plugin provided by Muse.
 * The connectivity codes follows an example given by Muse to test the Unity plugin. The automatic connection, data parsing,
 * calculations, and debugging tools are custom code for the development of this practicum project.
 */
public class EEGControllerScript : MonoBehaviour {

    public static EEGControllerScript instance = null;

    //Developer tools for debugging purposes. Shows data on a canvas in the game world.
    public Button startScanButton;
    public Button connectButton;
    public Button disconnectButton;
    public Dropdown museList;
    public Text dataText;
    public Text dataText2;
    public Text dataText3;
    public Text connectionText;

    public Text debugText;
    public Text debugText2;

    public float betaValue, gammaValue;
    public bool blinkValue;
    public GameObject EEGManager;

    private string userPickedMuse;
    private string dataBuffer, dataBuffer2, dataBuffer3;
    private string connectionBuffer;
    private LibmuseBridge muse;
    private float timer;
    private List<string> scores;
    private float[] scoresValue = { 0, 0, 0, 0 };

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // initialization
    void Start ()
    {
        muse = new LibmuseBridgeAndroid();
        Debug.Log("Libmuse version = " + muse.getLibmuseVersion());

        userPickedMuse = "";
        dataBuffer = "";
        dataBuffer2 = "";
        dataBuffer3 = "";
        connectionBuffer = "";
        
        timer = 0;
        blinkValue = false;
        EEGManager = GameObject.FindWithTag("EEGManager");

        //Automatically scan for a muse device when the game starts.
        registerListeners();
        registerAllData();
        startScanning();
    }

    public void startScanning()
    {
        muse.startListening();
    }

    public void userSelectedMuse()
    {
        userPickedMuse = museList.options [museList.value].text;
        //Debug.Log ("Selected muse = " + userPickedMuse);
    }

    public void connect()
    {
        //Automatically connect to first device on the list.
        if (userPickedMuse == "") {
            userPickedMuse = museList.options [0].text;
        }
        //Debug.Log ("Connecting to " + userPickedMuse);
        muse.connect (userPickedMuse);
    }

    public void disconnect()
    {
        muse.disconnect ();
    }

    //Required for connecting to a muse device and be able to receive data packets.
    void registerListeners()
    {
        muse.registerMuseListener(this.name, "receiveMuseList");
        muse.registerConnectionListener(this.name, "receiveConnectionPackets");
        muse.registerDataListener(this.name, "receiveDataPackets");
        muse.registerArtifactListener(this.name, "receiveArtifactPackets");
    }

    //This is used to listen for specific data. Currently, this game uses beta score, gamma score, and artifacts (for blinks).
    void registerAllData()
    {
        muse.listenForDataPacket("BETA_SCORE");
        muse.listenForDataPacket("GAMMA_SCORE");
        muse.listenForDataPacket("ARTIFACTS");
    }
    
    //Gets a list of Muse devices and display on the dropdown menu for debugging purposes.
    void receiveMuseList(string data)
    {
        //Debug.Log("Found list of muses = " + data);
        List<string> muses = data.Split(' ').ToList<string>();
        museList.ClearOptions ();
        museList.AddOptions (muses);
    }

    void receiveConnectionPackets(string data) {
        //Debug.Log("Unity received connection packet: " + data);
        connectionBuffer = data;
    }

    //Receives brainwave data packets in JSON strings. This will be parsed and converted to usable data for the EEGManager.
    void receiveDataPackets(string data) {   
        //Debug.Log("Unity received data packet: " + data);
        data = data.Replace("DataPacketType", "")
            .Replace("DataPacketValue", "")
            .Replace("TimeStamp", "")
            .Replace(":", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("{", "")
            .Replace("}", "")
            .Replace("\"", "")
            .Replace(" ", "");
        scores = data.Split(',').ToList<string>();
        if (scores[0] == "BETA_SCORE")
        {
            //For debugging purposes
            dataBuffer = scores[1] + " :: " + scores[2] + " :: " + scores[3] + " :: " + scores[4];

            //convert string to float
            for(int i = 0; i < 4; i++)
            {
                scoresValue[i] = float.Parse(scores[i+1]);
            }

            //Find the highest value and send data to EEGManager
            betaValue = scoresValue.Max();
            EEGManager.GetComponent<EEGManagerScript>().setBeta(betaValue);

            debugText.text = "beta " + betaValue;
            debugText2.text = "score " + scoresValue[0];
        }
        else if (scores[0] == "GAMMA_SCORE")
        {
            //For debugging purposes
            dataBuffer2 = scores[1] + " :: " + scores[2] + " :: " + scores[3] + " :: " + scores[4];

            //convert string to float
            for (int i = 0; i < 4; i++)
            {
                scoresValue[i] = float.Parse(scores[i + 1]);
            }

            //Find the highest value and send data to EEGManager
            gammaValue = scoresValue.Max();
            EEGManager.GetComponent<EEGManagerScript>().setGamma(gammaValue);
        }
    }

    //Receives artifact data packets for blinks. This is parsed and converted to usable data for the EEGManager.
    void receiveArtifactPackets(string data) {
        //Debug.Log("Unity received artifact packet: " + data);
        data = data.Replace("HeadbandOn", "")
            .Replace("Blink", "")
            .Replace(":", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("{", "")
            .Replace("}", "")
            .Replace("\"", "")
            .Replace(" ", "");
        List<string> scores = data.Split(',').ToList<string>();
        dataBuffer3 = scores[1];
        switch(dataBuffer3)
        {
            case "true":
                EEGManager.GetComponent<EEGManagerScript>().setBlink(true);
                break;
            case "false":
                EEGManager.GetComponent<EEGManagerScript>().setBlink(false);
                break;
        }
    }
    
    // Update is called once per frame
    void Update () {
        // Connection timer. Used to reconnect to device in case of a disconnection.
        timer += Time.deltaTime;
        if (timer < 30)
        {
            connect();
        }
        else if(timer > 45)
        {
            timer = 0;
        }

        // For development purposes. Debugging tool to see the data live on a canvas in the game world.
        dataText.text = dataBuffer;
        dataText2.text = dataBuffer2;
        dataText3.text = dataBuffer3;
        connectionText.text = connectionBuffer;
    }
}
