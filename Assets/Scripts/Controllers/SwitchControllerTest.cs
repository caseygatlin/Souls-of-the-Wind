using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControllerTest : MonoBehaviour {

    public bool checkAxis = false;
    private int numJoysticks = 0;
    private string walkInput = "WalkXbox";
    private string jumpInput = "JumpXbox";
    private string windHInput = "WindHXbox";
    private string windVInput = "WindVXbox";
    private float controllerCorrection = 1f;
    private bool xboxControls = false;
    private bool switchControls = false;

    private float LXMax = 0.0f;
    private float RXMax = 0.0f;
    private float RYMax = 0.0f;
    private float LXMin = 0.0f;
    private float RXMin = 0.0f;
    private float RYMin = 0.0f;


	// Use this for initialization
	void Start () {
        numJoysticks = Input.GetJoystickNames().Length;
        Debug.Log("There are " + numJoysticks + " joysticks connected.");
        for (int i = 0; i < numJoysticks; i++)
        {
            Debug.Log("Joystick " + i + " is: " + Input.GetJoystickNames()[i]);
        }

        // Joystick configurations
        if ((Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 16) ||
            (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 0 && Input.GetJoystickNames()[1].Length == 16) ||
            (Input.GetJoystickNames().Length == 1 && Input.GetJoystickNames()[0].Length == 16))
            switchControls = true;

        else if ((Input.GetJoystickNames().Length == 1 && Input.GetJoystickNames()[0].Length == 33) ||
            (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 33))
            xboxControls = true;

        if (switchControls)
        {
            Debug.Log("Using Switch Controls");
            walkInput = "WalkSwitch";
            jumpInput = "JumpSwitch";
            windHInput = "WindHSwitch";
            windVInput = "WindVSwitch";
            controllerCorrection = 1.5f;
        }
        else if (xboxControls)
        {
            Debug.Log("Using Xbox Controls");
            walkInput = "WalkXbox";
            jumpInput = "JumpXbox";
            windHInput = "WindHXbox";
            windVInput = "WindVXbox";
            controllerCorrection = 1f;
        }

        LXMax = 0f;
        LXMin = 0f;
        RXMax = 0f;
        RXMin = 0f;
        RYMax = 0f;
        RYMin = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (checkAxis)
        {
            if (Input.GetAxisRaw(walkInput) > 0 || Input.GetAxisRaw(walkInput) < 0)
            {
                if (Input.GetAxisRaw(walkInput) > LXMax)
                    LXMax = Input.GetAxisRaw(walkInput);
                if (Input.GetAxisRaw(walkInput) < LXMin)
                    LXMin = Input.GetAxisRaw(walkInput);
            }
            if (Input.GetAxisRaw(windHInput) > 0 || Input.GetAxisRaw(windHInput) < 0)
            {
                if (Input.GetAxisRaw(windHInput) > RXMax)
                    RXMax = Input.GetAxisRaw(windHInput);
                if (Input.GetAxisRaw(windHInput) < RXMin)
                    RXMin = Input.GetAxisRaw(windHInput);
            }
            if (Input.GetAxisRaw(windVInput) > 0 || Input.GetAxisRaw(windVInput) < 0)
            {
                if (Input.GetAxisRaw(windVInput) > RYMax)
                    RYMax = Input.GetAxisRaw(windVInput);
                if (Input.GetAxisRaw(windVInput) < RYMin)
                    RYMin = Input.GetAxisRaw(windVInput);
            }


            if (!(Input.GetAxisRaw(walkInput) > 0 || Input.GetAxisRaw(walkInput) < 0) && !(Input.GetAxisRaw(windHInput) > 0 || Input.GetAxisRaw(windHInput) < 0) && !(Input.GetAxisRaw(windVInput) > 0 || Input.GetAxisRaw(windVInput) < 0))
            {
                Debug.Log("Right Stick X Max: " + RXMax);
                Debug.Log("Right Stick X Min: " + RXMin);
                Debug.Log("Right Stick Y Max: " + RYMax);
                Debug.Log("Right Stick Y Min: " + RYMin);
                Debug.Log("Left Stick X Max: " + LXMax);
                Debug.Log("Left Stick X Min: " + LXMin);
            }
        }

        if (Input.GetButton("JumpSwitchP2"))
            Debug.Log("Jump Switch P2 Activated");
        if (Input.GetButton("JumpXboxP2"))
            Debug.Log("Jump Xbox P2 Activated");
        if (Input.GetButton("JumpSwitchP1"))
            Debug.Log("Jump Switch P1 Activated");
        if (Input.GetButton("JumpXboxP1"))
            Debug.Log("Jump Xbox P1 Activated");

    }
}
