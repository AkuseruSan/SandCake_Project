using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class C_PlayerController : MonoBehaviour {

    #region variables
    private Vector2 contactNormal;
    private Rigidbody2D rBody;
    private Quaternion rotation;

    public float speed;
    public float maxSpeed { get; private set; }
    public float minSpeed { get; private set; }
    public float rotationLerpTime;
    public Vector2 jump;
    #endregion

    // Use this for initialization
    void Start () {
        minSpeed = 1;
        maxSpeed = 10;
        contactNormal = Vector2.zero;
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
        RotatePlayer();
    }

    #region events

    void OnCollisionStay2D(Collision2D other)
    {
        foreach(ContactPoint2D contact in other.contacts)
        {
            contactNormal += contact.normal;
        }
        contactNormal = contactNormal.normalized;
    }

    #endregion

    #region functions

    void RotatePlayer()
    {
        rotation = new Quaternion(0, 0, -Mathf.Sin(contactNormal.x) * 180 * Time.deltaTime * rotationLerpTime, 0);
        //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, rotationLerpTime * Time.deltaTime);
    }

    void Movement()
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        rBody.velocity = new Vector2(contactNormal.y * speed, rBody.velocity.y);
    }

    public void Jump()
    {
        rBody.AddForce(jump, ForceMode2D.Impulse);
        rBody.rotation = 0;
    }

    #endregion
}
