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

    private float power;
    public float maxPower;
    public float powerRegenSpeed;

    private Animator myAnimator;
    private Animator myAnimatorN;
    #endregion

    IEnumerator CooldownWaiter()
    {
        power = -1;
        yield return new WaitForSeconds(2);
        power = powerRegenSpeed * Time.deltaTime;
    }
    // Use this for initialization
    void Start () {
        power = maxPower;
        startPos = transform.position.x;
        distanceSinceStart = 0;
        jumpCount = 1;
        currentJumpCount = jumpCount;
        contactNormal = Vector2.zero;
        rBody = GetComponent<Rigidbody2D>();

        myAnimatorN = GameObject.FindGameObjectWithTag("Night").gameObject.GetComponent<Animator>();
        myAnimator = GameObject.FindGameObjectWithTag("Day").gameObject.GetComponent<Animator>();

        myAnimator.SetBool("Grounded", true);
        myAnimatorN.SetBool("Grounded", true);
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
                    myAnimator.SetTrigger("Moving");
                    myAnimatorN.SetTrigger("Moving");

                    RotateFromYSpeed();

                    Movement();
                    ClampSpeed();
                    RotatePlayer();

                    UpdateDistance();

                    if(power > 0)power += powerRegenSpeed * Time.deltaTime;
                    power = Mathf.Clamp(power, 0, maxPower);

                    if(power == 0)
                    {
                        StartCoroutine(CooldownWaiter());
                    }

                    Debug.Log("Power! - "+power);

                    if (transform.position.y <= -10) GameCore.Instance.gameState = GameState.GAMEOVER;
                }
                break;
            case GameState.GAMEOVER:
                {

                }break;
            default:
                break;
        }

    }

    #region events

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentJumpCount = jumpCount;
        myAnimator.SetBool("Grounded", true);
        myAnimatorN.SetBool("Grounded", true);

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y <= 0) Die();
            Debug.Log(contact.normal);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        foreach (ContactPoint2D contact in other.contacts)
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

    public void UsePower()
    {
        power -= Time.deltaTime;
    }

    public float GetCurrentPower()
    {
        return power;
    }

    void RotateFromYSpeed()//Changes player's rotation by raycasting towards the -y vector and detecting floor's normal.
    {
        
    }

    public void Jump()
    {
        if (currentJumpCount > 0)
        {
            myAnimator.SetBool("Grounded", false);
            myAnimatorN.SetBool("Grounded", false);
            currentJumpCount -= 1;
            rBody.AddForce(jump, ForceMode2D.Impulse);
            contactNormal.x = 0;
        }

    }

    private void Die()
    {
        GameCore.Instance.gameState = GameState.GAMEOVER;
        Debug.Log("Die!");
    }

    #endregion
}
