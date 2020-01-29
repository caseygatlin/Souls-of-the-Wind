using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class CameraFollow : MonoBehaviour {

    public GameManager gameManager;

    public GameObject player;
    public float edgeBuffer = 10f;
    public float edgeBufferY = 2f;
    public float edgeBufferBottom = 10f;
    public float cameraSpeed;
    public GameObject background;

    private Rigidbody2D rb;
    private Rigidbody2D rbPlayer;
    private float playerPosX;
    private float playerPosY;
    private float cameraPosX;
    private float cameraPosY;
    private float bgPosX;
    private float bgPosY;
    private float bgPosZ;
    //private LineRenderer lineRenderer;
    private bool mouseOn = false;
    private bool belowMidLevel = false;
    private bool snapCameraToZero = true;

    //variables for player 2
    public GameObject player2;
    private float p2PosX;
    private float p2PosY;
    private Rigidbody2D rbPlayer2;

    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 5.0f;
    public float maxOrtho = 20.0f;


    // Use this for initialization
    void Start () {

        // Camera zoom
        targetOrtho = Camera.main.orthographicSize;

        // Set starting values for position variables.
        bgPosX = background.transform.position.x;
        bgPosY = background.transform.position.y;
        bgPosZ = background.transform.position.z;
        playerPosX = player.transform.position.x;
        playerPosY = player.transform.position.y;         
        cameraPosX = transform.position.x;
        cameraPosY = transform.position.y;

        if (player2)
        {
            p2PosX = player2.transform.position.x;
            p2PosY = player2.transform.position.x;
            rbPlayer2 = player2.GetComponent<Rigidbody2D>();
        }

        // Initialize rigidbody variables
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rbPlayer = player.GetComponent<Rigidbody2D>();


        //lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!gameManager.twoPlayers)
        {
            belowMidLevel = playerPosY < (-4f);
            if (cameraPosY >= 0)
                snapCameraToZero = true;

            if (player.GetComponent<Player>().timer > 0f && (player.GetComponent<Player>().timer < 0.01f + Time.deltaTime))
                CameraShaker.Instance.ShakeOnce(4f, 1f, .1f, 2f);

            /*
            if (Input.GetKey(KeyCode.P))
            {
                if (lineRenderer.enabled)
                    lineRenderer.enabled = false;
                else
                    lineRenderer.enabled = true;
            }

            mouseOn = Input.GetButton("MouseClick");

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0f;
            Vector3 zZeroCamera = new Vector3(transform.position.x, transform.position.y, 0f);

            lineRenderer.SetPosition(1, zZeroCamera);
            lineRenderer.SetPosition(0, mousePos);
            */




            // Set position variables to current position for all objects.
            playerPosX = player.transform.position.x;
            playerPosY = player.transform.position.y;
            cameraPosX = transform.position.x;
            cameraPosY = transform.position.y;
            bgPosX = background.transform.position.x;
            bgPosY = background.transform.position.y;
            bgPosZ = background.transform.position.z;

            // Move the background with the camera in the x direction.
            background.transform.position = new Vector3(cameraPosX, bgPosY, bgPosZ);

            // Move the camera right if the player is closer to the right.
            if ((playerPosX > (cameraPosX + edgeBuffer)) &&
                (rb.velocity.x < rbPlayer.velocity.x))
            {
                rb.AddForce(transform.right * rbPlayer.velocity.x * cameraSpeed);
            }

            // Move the camera left if the player is closer to the left.
            if ((playerPosX < (cameraPosX - edgeBuffer)) &&
                (rb.velocity.x > rbPlayer.velocity.x))
            {
                rb.AddForce(transform.right * rbPlayer.velocity.x * cameraSpeed);
            }



            if (!belowMidLevel)
            {
                // Move the camera up if the player is closer to the top
                if (playerPosY > (cameraPosY + edgeBufferY) &&
                (cameraPosY < (30 - edgeBufferY)) &&
                (rb.velocity.y < rbPlayer.velocity.y))
                {
                    rb.AddForce(transform.up * rbPlayer.velocity.y * cameraSpeed);
                }

                // Move the camera down if the player is closer to the bottom, only if the cameraPosY is greater than zero.
                if (playerPosY < (cameraPosY - edgeBufferY) &&
                    (cameraPosY > 0) &&
                    (rb.velocity.y > rbPlayer.velocity.y))
                {
                    rb.AddForce(transform.up * rbPlayer.velocity.y * cameraSpeed);
                }

                // Correct for any forces that may push the cameraPosY below zero.
                if (snapCameraToZero && cameraPosY < 0)
                    transform.position = new Vector3(cameraPosX, 0, transform.position.z);

            }
            else
            {
                snapCameraToZero = false;
                if (playerPosY > (cameraPosY + edgeBufferY) &&
                (rb.velocity.y < rbPlayer.velocity.y))
                {
                    rb.AddForce(transform.up * rbPlayer.velocity.y * cameraSpeed);
                }

                if (playerPosY < (cameraPosY - edgeBufferY) &&
                (cameraPosY > -30) &&
                (rb.velocity.y > rbPlayer.velocity.y))
                {
                    rb.AddForce(transform.up * rbPlayer.velocity.y * cameraSpeed);
                }

                if (cameraPosY < -30)
                    transform.position = new Vector3(cameraPosX, -30, transform.position.z);

            }



            /*

            if (cameraPosY < 0)
            {
                transform.position = new Vector3(cameraPosX, 0, transform.position.z);
            }
            */



            /*
            if ((playerPosX - edgeBuffer) < cameraPosX)
            {


                if (rb.velocity.x > rbPlayer.velocity.x)
                {
                    rb.AddForce(transform.right * (-cameraSpeed));
                }
                if (rb.velocity.x < rbPlayer.velocity.x)
                {
                    rb.velocity = new Vector2(rbPlayer.velocity.x, rb.velocity.y);
                }

            }
            */
        }
        else
        {

            edgeBuffer = 2 * Camera.main.orthographicSize - 4;
            if (player.GetComponent<Player>().timer > 0f && (player.GetComponent<Player>().timer < 0.01f + Time.deltaTime))
                CameraShaker.Instance.ShakeOnce(4f, 1f, .1f, 2f);
            if (player2.GetComponent<CharMoveP2>().timer > 0f && (player2.GetComponent<CharMoveP2>().timer < 0.01f + Time.deltaTime))
                CameraShaker.Instance.ShakeOnce(4f, 1f, .1f, 2f);

            playerPosX = player.transform.position.x;
            playerPosY = player.transform.position.y;
            p2PosX = player2.transform.position.x;
            p2PosY = player2.transform.position.y;
            cameraPosX = transform.position.x;
            cameraPosY = transform.position.y;
            bgPosX = background.transform.position.x;
            bgPosY = background.transform.position.y;
            bgPosZ = background.transform.position.z;

            // Move the background with the camera in the x direction.
            background.transform.position = new Vector3(cameraPosX, bgPosY, bgPosZ);

            // Move the camera right if the player is closer to the right.
            if ((playerPosX > (cameraPosX + edgeBuffer)) &&
                (p2PosX > (cameraPosX + edgeBuffer)) &&
                (rb.velocity.x < rbPlayer.velocity.x) &&
                (rb.velocity.x < rbPlayer2.velocity.x))
            {
                float maxVelocity = rbPlayer2.velocity.x;
                if (rbPlayer.velocity.x > rbPlayer2.velocity.x)
                    maxVelocity = rbPlayer.velocity.x;
                rb.AddForce(transform.right * maxVelocity * cameraSpeed);
            }

            else if (((playerPosX > (cameraPosX + edgeBuffer)) && (rb.velocity.x < rbPlayer.velocity.x)) ||
                ((p2PosX > (cameraPosX + edgeBuffer)) && (rb.velocity.x < rbPlayer2.velocity.x)))
            {
                if (((playerPosX > (cameraPosX + edgeBuffer)) && (p2PosX < (cameraPosX - edgeBuffer))) ||
                    (p2PosX > (cameraPosX + edgeBuffer) && playerPosX < (cameraPosX - edgeBuffer)))
                {
                    targetOrtho += .02f;
                    targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
                    smoothSpeed = 1f;
                }
                else
                {
                    float maxVelocity = rbPlayer2.velocity.x;
                    if (rbPlayer.velocity.x > rbPlayer2.velocity.x)
                        maxVelocity = rbPlayer.velocity.x;
                    rb.AddForce(transform.right * maxVelocity * cameraSpeed);
                }
                //rb.AddForce(transform.right * rbPlayer.velocity.x * cameraSpeed);

            }
            else if (((playerPosX < (cameraPosX + edgeBuffer)) && (p2PosX > (cameraPosX - edgeBuffer))) &&
                    (p2PosX < (cameraPosX + edgeBuffer) && playerPosX > (cameraPosX - edgeBuffer)))
            {
                if (targetOrtho > 5f)
                    targetOrtho -= 0.02f;
                smoothSpeed = .1f;
            }

            // Move the camera left if the player is closer to the left.
            if ((playerPosX < (cameraPosX - edgeBuffer)) &&
                (p2PosX < (cameraPosX - edgeBuffer)) &&
                (rb.velocity.x > rbPlayer.velocity.x) &&
                (rb.velocity.x > rbPlayer2.velocity.x))
            {
                float minVelocity = rbPlayer2.velocity.x;
                if (rbPlayer.velocity.x < rbPlayer2.velocity.x)
                    minVelocity = rbPlayer.velocity.x;
                rb.AddForce(transform.right * minVelocity * cameraSpeed);
            }

            else if (((playerPosX < (cameraPosX - edgeBuffer)) && (rb.velocity.x > rbPlayer.velocity.x)) ||
                ((p2PosX < (cameraPosX - edgeBuffer)) && (rb.velocity.x > rbPlayer2.velocity.x)))
            {
                if (((playerPosX < (cameraPosX - edgeBuffer)) && (p2PosX > (cameraPosX + edgeBuffer))) ||
                    (p2PosX < (cameraPosX - edgeBuffer) && playerPosX > (cameraPosX + edgeBuffer)))
                {
                    targetOrtho += .02f;
                    targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
                    smoothSpeed = 1f;
                }
                else
                {
                    float minVelocity = rbPlayer2.velocity.x;
                    if (rbPlayer.velocity.x < rbPlayer2.velocity.x)
                        minVelocity = rbPlayer.velocity.x;
                    rb.AddForce(transform.right * minVelocity * cameraSpeed);
                }
                //rb.AddForce(transform.right * rbPlayer.velocity.x * cameraSpeed);

                
            }
            else if (((playerPosX < (cameraPosX + edgeBuffer)) && (p2PosX > (cameraPosX - edgeBuffer))) &&
                    (p2PosX < (cameraPosX + edgeBuffer) && playerPosX > (cameraPosX - edgeBuffer)))
            {
                if (targetOrtho > 5f)
                    targetOrtho -= 0.02f;
                smoothSpeed = .1f;
            }

            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

        }
    }
}
