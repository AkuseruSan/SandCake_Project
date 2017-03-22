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

    [HideInInspector]
    public bool onCoolDown;

    private float power;
    public float maxPower;
    public float powerRegenSpeed;

    private Animator myAnimator;
    private Animator myAnimatorN;
    #endregion

    // Use this for initialization
    void Start () {
        onCoolDown = false;
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

        myAnimator.SetBool("Dead", false);
        myAnimatorN.SetBool("Dead", false);

        myAnimator.SetBool("Sitting", true);
        myAnimatorN.SetBool("Sitting", true);
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
                    myAnimator.SetBool("Sitting", false);
                    myAnimatorN.SetBool("Sitting", false);

                    RotateFromYSpeed();

                    Movement();
                    ClampSpeed();
                    RotatePlayer();

                    UpdateDistance();

                    

                    if(!onCoolDown) power -= powerRegenSpeed * Time.deltaTime;
                    power = Mathf.Clamp(power, 0, maxPower);

                    //Debug.Log("Power! - "+power);

                    if (transform.position.y <= -10 || power <= 0) GameCore.Instance.gameState = GameState.GAMEOVER;
                }
                break;
            case GameState.GAMEOVER:
                {
                    myAnimator.SetBool("Dead", true);
                    myAnimatorN.SetBool("Dead", true);
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
        myAnimator.SetBool("Grounded", true);
        myAnimatorN.SetBool("Grounded", true);

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal == new Vector2( -1, 0)) Die();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stamina") RecoverStamina(20);
        else if (collision.tag == "Enemy") collision.gameObject.GetComponent<BaseEnemyBehaviour>().Attack(GetComponent<PlayerController>());
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

    public void UsePower(float spd)
    {
        power -= spd*Time.deltaTime;
    }

    public void RecoverStamina(float percentOnTotal)
    {
        
        power += AuxLib.Map(Mathf.Clamp(percentOnTotal, 0, 100), 0, 100, 0, maxPower);
    }

    public void DecreaseStamina(float percentOnTotal)
    {
        power -= AuxLib.Map(Mathf.Clamp(percentOnTotal, 0, 100), 0, 100, 0, maxPower);
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
