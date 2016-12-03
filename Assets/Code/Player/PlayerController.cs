using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour {

    #region variables
    private Vector2 contactNormal;
    public Rigidbody2D rBody { get; private set; }
    private Quaternion rotation;
    private int jumpCount, currentJumpCount;
    private float startPos;

    public int distanceSinceStart;
    public float speed;
    public float maxSpeed;
    public float minSpeed;
    public float rotationLerpTime;
    public Vector2 jump;
    #endregion

    // Use this for initialization
    void Start () {
        startPos = transform.position.x;
        distanceSinceStart = 0;
        jumpCount = 1;
        currentJumpCount = jumpCount;
        contactNormal = Vector2.zero;
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (GameCore.Instance.gameState)
        {
            case GameState.AWAKE:
                {

                }
                break;
            case GameState.PAUSE:
                {

                }
                break;
            case GameState.PLAY:
                {
                    Movement();
                    ClampSpeed();
                    RotatePlayer();

                    UpdateDistance();
                }
                break;
            default:
                break;
        }

    }

    #region events

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentJumpCount = jumpCount; 
    }

    void OnCollisionStay2D(Collision2D other)
    {
        foreach(ContactPoint2D contact in other.contacts)
        {
            contactNormal += contact.normal;
        }
        contactNormal = contactNormal.normalized;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    #endregion

    #region functions

    void UpdateDistance()
    {
        distanceSinceStart = (int)(transform.position.x - startPos);
    }

    void ClampSpeed()
    {
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        rBody.velocity = new Vector2(Mathf.Clamp(rBody.velocity.x, minSpeed, maxSpeed), rBody.velocity.y);
    }

    void RotatePlayer()
    {
        rotation = Quaternion.Euler(0, 0, -Mathf.Sin(contactNormal.x) * 180);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, rotationLerpTime * Time.deltaTime);
    }

    void Movement()
    {
        rBody.velocity = new Vector2(contactNormal.y * speed, rBody.velocity.y);

    }

    public void Jump()
    {
        if (currentJumpCount > 0)
        {
            currentJumpCount -= 1;
            rBody.AddForce(jump, ForceMode2D.Impulse);
            contactNormal.x = 0;
        }
    }

    #endregion
}
