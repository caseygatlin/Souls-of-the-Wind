using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWindMovement : MonoBehaviour {

    public GameManager gameManager;
    public int particleMax = 60;
    public float dampen = .1f;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    public float windSpeed = 10f;
    public GameObject mainCamera;

    // Controller input variables
    private bool switchControlsP1 = false;
    private bool xboxControlsP1 = false;
    private bool switchControlsP2 = false;
    private bool xboxControlsP2 = false;
    private string windHInput = "WindHXboxP1";
    private string windVInput = "WindVXboxP1";

    // Controller corrections for Switch axes
    private float switchCorrectRXMin = 1.36f;
    private float switchCorrectRXMax = 1.31f;
    private float switchCorrectRYMin = 1.17f;
    private float switchCorrectRYMax = 1.57f;

    // Wind restrictions
    private bool westWind = true;
    private bool eastWind = true;
    private bool northWind = true;
    private bool southWind = true;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();

        if (!gameManager.twoPlayers)
        {
            // Joystick configurations
            if ((Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 16) ||
                (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 0 && Input.GetJoystickNames()[1].Length == 16) ||
                (Input.GetJoystickNames().Length == 1 && Input.GetJoystickNames()[0].Length == 16))
                switchControlsP1 = true;

            else if ((Input.GetJoystickNames().Length == 1 && Input.GetJoystickNames()[0].Length == 33) ||
                (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 33))
                xboxControlsP1 = true;

            if (switchControlsP1)
            {
                windHInput = "WindHSwitchP1";
                windVInput = "WindVSwitchP1";
            }
            else if (xboxControlsP1)
            {
                windHInput = "WindHXboxP1";
                windVInput = "WindVXboxP1";
            }
        }

        if (gameManager.twoPlayers)
        {
            // Joystick configurations
            if ((Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 16) ||
                (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 0 && Input.GetJoystickNames()[1].Length == 16) ||
                (Input.GetJoystickNames().Length == 1 && Input.GetJoystickNames()[0].Length == 16))
                switchControlsP1 = true;

            else if ((Input.GetJoystickNames().Length == 1 && Input.GetJoystickNames()[0].Length == 33) ||
                (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[0].Length == 33))
                xboxControlsP1 = true;

            if ((Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[1].Length == 16) ||
                (Input.GetJoystickNames().Length == 3 && Input.GetJoystickNames()[1].Length == 0 && Input.GetJoystickNames()[2].Length == 16) ||
                (Input.GetJoystickNames().Length == 3 && Input.GetJoystickNames()[1].Length == 16))
                switchControlsP2 = true;

            else if ((Input.GetJoystickNames().Length == 3 && Input.GetJoystickNames()[1].Length == 33) ||
                (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[1].Length == 33))
                xboxControlsP2 = true;

            if (switchControlsP1)
                windHInput = "WindHSwitchP1";
            else if (xboxControlsP1)
                windHInput = "WindHXboxP1";

            if (switchControlsP2)
                windVInput = "WindVSwitchP2";
            else if (xboxControlsP2)
                windVInput = "WindVXboxP2";

        }

        // Gets wind restrictions from gameManager
        if (gameManager)
        {
            westWind = gameManager.westWind;
            eastWind = gameManager.eastWind;
            northWind = gameManager.northWind;
            southWind = gameManager.southWind;
        }

        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        InitializeIfNeeded();

        int numParticlesAlive = ps.GetParticles(particles);

        var windH = Input.GetAxis(windHInput) * (windSpeed);
        var windV = Input.GetAxis(windVInput) * (windSpeed);


        if (!gameManager.twoPlayers)
        {
            if (switchControlsP1 && Input.GetAxis(windHInput) < 0)
                windH = Input.GetAxis(windHInput) * windSpeed * switchCorrectRXMin;
            if (switchControlsP1 && Input.GetAxis(windHInput) > 0)
                windH = Input.GetAxis(windHInput) * windSpeed * switchCorrectRXMax;
            if (switchControlsP1 && Input.GetAxis(windVInput) < 0)
                windV = Input.GetAxis(windVInput) * windSpeed * switchCorrectRYMin;
            if (switchControlsP1 && Input.GetAxis(windVInput) > 0)
                windV = Input.GetAxis(windVInput) * windSpeed * switchCorrectRYMax;
        }
        else
        {
            if (switchControlsP1 && xboxControlsP2 && Input.GetAxis(windHInput) < 0)
                windH = Input.GetAxis(windHInput) * windSpeed * switchCorrectRXMin;
            if (switchControlsP1 && xboxControlsP2 && Input.GetAxis(windHInput) > 0)
                windH = Input.GetAxis(windHInput) * windSpeed * switchCorrectRXMax;
            if (switchControlsP2 && xboxControlsP1 && Input.GetAxis(windVInput) < 0)
                windV = Input.GetAxis(windVInput) * windSpeed * switchCorrectRYMin;
            if (switchControlsP2 && xboxControlsP1 && Input.GetAxis(windVInput) > 0)
                windV = Input.GetAxis(windVInput) * windSpeed * switchCorrectRYMax;
        }


        if (Input.GetKey(KeyCode.RightArrow))
            windH = windSpeed;
        if (Input.GetKey(KeyCode.LeftArrow))
            windH = windSpeed * -1;
        if (Input.GetKey(KeyCode.UpArrow))
            windV = windSpeed;
        if (Input.GetKey(KeyCode.DownArrow))
            windV = windSpeed * -1;


        //mouseOn = Input.GetButton("MouseClick");

        /*
        if (mouseOn)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 center = mainCamera.transform.position;
            float mousePosX = mousePos.x - mainCamera.transform.position.x;
            float mousePosY = mousePos.y - mainCamera.transform.position.y;
            for (int l = 0; l < numParticlesAlive; l++)
            {
                particles[l].velocity += Vector3.right * mousePosX * dampen;
                particles[l].velocity += Vector3.up * mousePosY * dampen;
            }
            ps.SetParticles(particles, numParticlesAlive);

        }
        */

        // East-West Wind
        if (windH > 0 && westWind)
        {
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].velocity += Vector3.right * windH;
            }
            ps.SetParticles(particles, numParticlesAlive);
        }
        if (windH < 0 && eastWind)
        {
            for (int i = 0; i < numParticlesAlive; i++)
            {
                particles[i].velocity += Vector3.right * windH;
            }
            ps.SetParticles(particles, numParticlesAlive);
        }


        // North-South Wind
        if (windV > 0 && southWind)
        {
            for (int j = 0; j < numParticlesAlive; j++)
            {
                particles[j].velocity += Vector3.up * windV;

            }
            ps.SetParticles(particles, numParticlesAlive);
        }
        if (windV < 0 && northWind)
        {
            for (int j = 0; j < numParticlesAlive; j++)
            {
                particles[j].velocity += Vector3.up * windV;

            }
            ps.SetParticles(particles, numParticlesAlive);
        }

        // When wind is not controlled
        if (!(windV > 0 || windV < 0) && !(windH > 0 || windH < 0))
        {
            for (int k = 0; k < numParticlesAlive; k++)
            {
                particles[k].velocity = new Vector3(particles[k].velocity.x, particles[k].velocity.y, particles[k].velocity.z);
            }
            ps.SetParticles(particles, numParticlesAlive);
        }

    }

    void InitializeIfNeeded()
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        if (particles == null || particles.Length < particleMax)
            particles = new ParticleSystem.Particle[particleMax];
    }
}
