using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCam : MonoBehaviour {

    public float vel = 10f;
    public GameObject background;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector2(vel, 0f);
        background.transform.position = new Vector3(transform.position.x, background.transform.position.y, background.transform.position.z);
	}
}
