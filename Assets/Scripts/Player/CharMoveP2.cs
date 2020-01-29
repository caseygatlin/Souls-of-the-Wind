using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMoveP2 : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMove player1;
    public Animator animator;
    public SpriteRenderer sr;
    public Collider2D playerCollider1;
    public Collider2D playerCollider2;
    public Collider2D playerCollider3;
    public bool isGrounded = false;
    public float speed = 10.0f;
    public float jump = 10.0f;
    public float windSpeed = 10.0f;
    public GameObject dustObject;
    public GameObject deathBurstObject;
    public bool isHit = false;
    public bool isHitTimerOn = false;
    public GameObject mainCamera;
    public float magnifier = 29f;
    public float lrWindModifier = .7f;
    public float dWindModifier = .7f;

    public float windH;
    public float windV;




    private Rigidbody2D rb;

    private float horizontalSpeed;
    private float verticalSpeed;
    private bool isControlling = false;
    private bool isFlipped = false;
    private Vector3 charStartPos;
    private Vector3 charHitPos;
    public float timer = 0f;
    private float timerMax = 2f;
    private bool playerCanControl;
    private List<Vector3> respawnLocations = new List<Vector3>();
    private GameObject cameraBackground;

    // Input variables
    private bool switchControls = false;
    private bool xboxControls = false;
    private string walkInput = "WalkXboxP2";
    private string jumpInput = "JumpXboxP2";
    private string windHInput = "WindHXboxP2";
    private string windVInput = "WindVXboxP2";

    // Controller corrections for Switch axes
    private float switchCorrectLXMin = 1.24f;
    private float switchCorrectLXMax = 1.49f;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        charStartPos = transform.position;
        charHitPos = transform.position;
        playerCanControl = true;
        cameraBackground = mainCamera.GetComponent<CameraFollow>().background;
        respawnLocations.Add(transform.position);

        // Joystick configurations
        if ((Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[1].Length == 16) ||
            (Input.GetJoystickNames().Length == 3 && Input.GetJoystickNames()[1].Length == 0 && Input.GetJoystickNames()[2].Length == 16) ||
            (Input.GetJoystickNames().Length == 3 && Input.GetJoystickNames()[1].Length == 16))
            switchControls = true;

        else if ((Input.GetJoystickNames().Length == 3 && Input.GetJoystickNames()[1].Length == 33) ||
            (Input.GetJoystickNames().Length == 2 && Input.GetJoystickNames()[1].Length == 33))
            xboxControls = true;

        if (switchControls)
        {
            Debug.Log("Using Switch Controls");
            walkInput = "WalkSwitchP2";
            jumpInput = "JumpSwitchP2";
            windHInput = "WindHSwitchP2";
            windVInput = "WindVSwitchP2";
        }
        else if (xboxControls)
        {
            Debug.Log("Using Xbox Controls");
            walkInput = "WalkXboxP2";
            jumpInput = "JumpXboxP2";
            windHInput = "WindHXboxP2";
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
    void FixedUpdate()
    {
        // Fixes bug: Rigidbody falls asleep sometimes when player's head is touching the ceiling
        if (playerCollider3.IsTouchingLayers(LayerMask.GetMask("Ground")) && rb.IsSleeping())
            rb.WakeUp();

        // Management of jump dust -------------------------------------------------------------------------------------
        GameObject dummy = GameObject.Find("DustAnimator(Clone)");
        if (dummy != null)
        {
            Animator dummyAnim = dummy.GetComponent<Animator>();
            AnimatorStateInfo dummyAnimStateInf = dummyAnim.GetCurrentAnimatorStateInfo(0);
            if (dummyAnimStateInf.IsName("Done"))
                Destroy(dummy);
        }

        GameObject dummyDeathObj = GameObject.Find("DeathParticleBurst(Clone)");
        if (dummyDeathObj != null)
        {
            ParticleSystem dummyPS = dummyDeathObj.GetComponent<ParticleSystem>();
            if (!dummyPS.isPlaying)
                Destroy(dummyDeathObj);
        }
        // ------------------------------------------------------------------------------------------------------

        // Management of being hit and death sequence --------------------------------------------------------------------
        isHit |= (playerCollider1.IsTouchingLayers(LayerMask.GetMask("Hazards")) ||
                      playerCollider2.IsTouchingLayers(LayerMask.GetMask("Hazards")) ||
                      playerCollider3.IsTouchingLayers(LayerMask.GetMask("Hazards")));
        isHit &= (playerCollider1.IsTouchingLayers(LayerMask.GetMask("Hazards")) ||
                  playerCollider2.IsTouchingLayers(LayerMask.GetMask("Hazards")) ||
                  playerCollider3.IsTouchingLayers(LayerMask.GetMask("Hazards")));

        if (isHitTimerOn)
        {
            isHit = true;
            transform.position = charHitPos;
        }

        if (isHit)
        {
            sr.enabled = false;
            isHitTimerOn = true;
            playerCanControl = false;
            charHitPos = transform.position;
            GameObject deathBurstObjClone;
            if (dummyDeathObj == null && timer < (0 + Time.deltaTime))
                deathBurstObjClone = (GameObject)Instantiate(deathBurstObject, transform.position, transform.rotation);
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                //Vector3 cameraSteady = mainCamera.transform.position;
                int respawnIndex = 0;
                for (int index = respawnLocations.Count - 1; 0 <= index; index--)
                {
                    transform.position = respawnLocations[index];
                    //mainCamera.transform.position = cameraSteady;
                    if (!isHit)
                    {
                        respawnIndex = index;
                        break;
                    }
                }
                float newCameraPosY = respawnLocations[respawnIndex].y;
                if (respawnLocations[respawnIndex].y > 4)
                    newCameraPosY = 4f;
                if (respawnLocations[respawnIndex].y < 0)
                    newCameraPosY = 0f;
                cameraBackground.transform.position = new Vector3(respawnLocations[respawnIndex].x, cameraBackground.transform.position.y, cameraBackground.transform.position.z);
                mainCamera.transform.position = new Vector3(respawnLocations[respawnIndex].x, newCameraPosY, mainCamera.transform.position.z);
                transform.position = respawnLocations[respawnIndex];
                playerCanControl = true;
                isHitTimerOn = false;
                isHit = false;
                timer = 0f;
                sr.enabled = true;
            }

        }
        // --------------------------------------------------------------------------------------------------------------

        // Management of player movement -------------------------------------------------------------------------------------
        if (sr.enabled)
        {

            // Wind controls
            windH = Input.GetAxis(windHInput) * windSpeed;
            windV = Input.GetAxis(windVInput) * windSpeed;


            if (switchControls && Input.GetAxis(windHInput) < 0)
                windH = Input.GetAxis(windHInput) * windSpeed * switchCorrectRXMin;
            if (switchControls && Input.GetAxis(windHInput) > 0)
                windH = Input.GetAxis(windHInput) * windSpeed * switchCorrectRXMax;
            if (switchControls && Input.GetAxis(windVInput) < 0)
                windV = Input.GetAxis(windVInput) * windSpeed * switchCorrectRYMin;
            if (switchControls && Input.GetAxis(windVInput) > 0)
                windV = Input.GetAxis(windVInput) * windSpeed * switchCorrectRYMax;

            if (Input.GetKey(KeyCode.RightArrow))
                windH = windSpeed * lrWindModifier;
            if (Input.GetKey(KeyCode.LeftArrow))
                windH = windSpeed * -1 * lrWindModifier;
            if (Input.GetKey(KeyCode.UpArrow))
                windV = windSpeed;
            if (Input.GetKey(KeyCode.DownArrow))
                windV = windSpeed * -1 * dWindModifier;


            // Walking controls
            var move = Input.GetAxis(walkInput) * speed;
            if (switchControls && Input.GetAxis(walkInput) < 0)
                move = Input.GetAxis(walkInput) * speed * switchCorrectLXMin;
            else if (switchControls && Input.GetAxis(walkInput) > 0)
                move = Input.GetAxis(walkInput) * speed * switchCorrectLXMax;
            if (Input.GetKey(KeyCode.J))
                move = speed * -1;
            if (Input.GetKey(KeyCode.L))
                move = speed;


            // Setting up variables for animation and controlling boundaries
            horizontalSpeed = rb.velocity.x;
            verticalSpeed = rb.velocity.y;
            animator.SetFloat("Speed", Mathf.Abs(horizontalSpeed));
            animator.SetFloat("verticalSpeed", verticalSpeed);
            isControlling = (Input.GetAxis(walkInput) > 0.5 || Input.GetAxis(walkInput) < -0.5 || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.I) || Input.GetButton(jumpInput));
            animator.SetBool("isControlling", isControlling);
            isGrounded = (playerCollider1.IsTouchingLayers(LayerMask.GetMask("Ground")) || playerCollider2.IsTouchingLayers(LayerMask.GetMask("Ground")));


            // Manages respawn locations
            if (isHit)
                isGrounded = false;
            if (isGrounded && !isHit)
            {
                if (respawnLocations.Count == 15)
                {
                    respawnLocations.RemoveAt(0);
                    respawnLocations.Add(transform.position);
                }
                else
                    respawnLocations.Add(transform.position);
            }



            // Manages landing animations
            if (verticalSpeed < -5f)
                animator.SetBool("Land", true);
            AnimatorStateInfo stateInfChar = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfChar.IsName("Char_Land"))
                animator.SetBool("Land", false);
            animator.SetBool("isGrounded", isGrounded);

            if (playerCanControl)
            {

                // Walking right
                if (move > 0)
                {
                    sr.flipX = false;
                    isFlipped = false;
                    rb.AddRelativeForce(transform.right * move);
                }

                // Walking left
                if (move < 0)
                {
                    sr.flipX = true;
                    isFlipped = true;
                    rb.AddRelativeForce(transform.right * move);
                }

                // Jumping
                if ((Input.GetButton(jumpInput) || Input.GetKey(KeyCode.I)) && isGrounded)
                {
                    Vector3 playerPos = new Vector3(transform.position.x - .39f, transform.position.y - .374f, transform.position.z);
                    Vector3 playerPosFlip = new Vector3(transform.position.x + .39f, transform.position.y - .374f, transform.position.z);
                    GameObject dustObjectClone;
                    if (dummy == null && Mathf.Abs(horizontalSpeed) > 2f)
                    {
                        if (isFlipped)
                        {
                            dustObjectClone = (GameObject)Instantiate(dustObject, playerPosFlip, transform.rotation);
                            dustObjectClone.GetComponent<SpriteRenderer>().flipX = true;
                        }
                        else
                            dustObjectClone = (GameObject)Instantiate(dustObject, playerPos, transform.rotation);
                        AnimatorStateInfo stateInf = dustObjectClone.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    }


                    rb.AddForce(transform.up * jump, ForceMode2D.Impulse);
                    isGrounded = false;
                }
                if (!gameManager.twoPlayers)
                {
                    // East-West Wind
                    if (windH > 0 && westWind)
                        rb.AddRelativeForce(transform.right * windH);
                    if (windH < 0 && eastWind)
                        rb.AddRelativeForce(transform.right * windH);


                    // North-South Wind
                    if (windV > 0 && southWind)
                        rb.AddRelativeForce(transform.up * windV);
                    if (windV < 0 && northWind)
                        rb.AddRelativeForce(transform.up * windV);
                }
                else
                {
                    // North-South Wind
                    if (windV > 0 && southWind)
                        rb.AddRelativeForce(transform.up * windV);
                    if (windV < 0 && northWind)
                        rb.AddRelativeForce(transform.up * windV);

                    // East-West Wind
                    if (player1.windH > 0 && westWind)
                        rb.AddRelativeForce(transform.right * player1.windH);
                    if (player1.windH < 0 && eastWind)
                        rb.AddRelativeForce(transform.right * player1.windH);
                }





                /*if (windH > 0 || windH < 0)
                {
                    rb.AddRelativeForce(transform.right * windH);
                }

                if (windV > 0 || windV < 0)
                {
                    rb.AddRelativeForce(transform.up * windV);
                }
                */
            }
        }
        // -----------------------------------------------------------------------------------------------------------------------

    }
}
