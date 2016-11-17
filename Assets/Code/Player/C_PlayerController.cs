using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class C_PlayerController : MonoBehaviour {

    #region variables
    private Vector2 contactNormal;
    private Rigidbody2D rBody;

    public float speed;
    public Vector2 jump;
    #endregion

    // Use this for initialization
    void Start () {
        contactNormal = Vector2.zero;
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        RotatePlayer();
        if(Input.anyKey) Jump();
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
        
    }

    void Movement()
    {
        rBody.velocity = new Vector2(contactNormal.y * speed, rBody.velocity.y);
    }

    void Jump()
    {
        rBody.AddForce(jump, ForceMode2D.Impulse);
    }

    #endregion
}
