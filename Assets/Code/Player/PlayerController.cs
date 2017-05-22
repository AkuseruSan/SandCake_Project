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
    public float invTimeInspector;
    private float invTime = 1.5f;
    private bool invulnerable = false;
    private Color dayColor = new Color(0.79f, 0.79f, 0.79f, 1), nightColor = new Color(0.12f, 0.14f, 0.23f, 1);
    private int enemyKillScore = 50;

    //Giant Sun spawn bool
    public bool spawnGiantSun = false;

    [HideInInspector]
    public bool onCoolDown;

    private float power;
    public float maxPower;
    public float powerRegenSpeed;

    [HideInInspector]
    public float barrier;

    public float maxBarrier;

    private Animator myAnimator;
    private Animator myAnimatorN;
    #endregion

    // Use this for initialization
    void Start () {
        onCoolDown = false;
        power = maxPower;
        barrier = maxBarrier;
        startPos = transform.position.x;
        distanceSinceStart = 0;

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
                    if (GameCore.Instance.doubleJump)
                    {
                        jumpCount = 2;
                    }
                    else { jumpCount = 1; }

                    currentJumpCount = jumpCount;
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

                    InvulnerabilityCount();

                    UpdateDistance();

                    if (!onCoolDown && GameCore.Instance.staminaBoost) power -= powerRegenSpeed / 2 * Time.deltaTime;
                    else if (!onCoolDown) power -= powerRegenSpeed * Time.deltaTime;
                    power = Mathf.Clamp(power, 0, maxPower);

                    Debug.Log(currentJumpCount);

                    if (transform.position.y <= -10 || (power <= 0 && !GameCore.Instance.revive) ) GameCore.Instance.gameState = GameState.GAMEOVER;
                    if (power <= 0 && GameCore.Instance.revive)
                    {
                        power = maxPower;
                        spawnGiantSun = true;
                        Time.timeScale = 0.001f;
                        invTime = invTimeInspector;
                        invulnerable = true;
                        GameCore.Instance.reviveFirstFrame = true;
                        GameCore.Instance.revive = false;
                    }
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
        if (collision.tag == "Enemy" && invulnerable == false && !GameCore.Instance.barrier)
        {
            DecreaseStamina(5);
            invTime = invTimeInspector;
            invulnerable = true;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Enemy" && GameCore.Instance.barrier)
        {
            DecreaseBarrierValue(5);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "KillEnemy")
        {
            GameCore.Instance.finalScore += enemyKillScore;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Death" && !invulnerable && !GameCore.Instance.barrier)
        {
            Die();
        }
        else if(collision.gameObject.tag == "Death" && GameCore.Instance.barrier)
        {
            DecreaseBarrierValue(2);
        }

        //Debug.Log(invulnerable);
        //Debug.Log("The wolf has been TRIGGERED with: " + collision.name);
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

    public void DecreaseBarrierValue(float percentOnTotal)
    {
        barrier -= AuxLib.Map(Mathf.Clamp(percentOnTotal, 0, 100), 0, 100, 0, maxBarrier);
    }

    public float GetCurrentPower()
    {
        return power;
    }

    public float GetCurrentBarrier()
    {
        return barrier;
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

    private void InvulnerabilityCount()
    {
        if (invulnerable)
        {
            invTime -= Time.deltaTime;

            GameObject Child1 = transform.GetChild(0).gameObject;
            GameObject Child2 = transform.GetChild(1).gameObject;

            Component[] SMP = Child1.GetComponentsInChildren<Anima2D.SpriteMeshInstance>();
            Component[] SMP2 = Child2.GetComponentsInChildren<Anima2D.SpriteMeshInstance>();


            foreach (Anima2D.SpriteMeshInstance x in SMP)
            {
                x.color = new Color(Mathf.Lerp(Mathf.Abs(Mathf.Sin(invTime * 10)), nightColor.r, 0.7f), Mathf.Lerp(0, nightColor.g, 0.5f), Mathf.Lerp(0, nightColor.b, 0.5f), 1);
            }


            foreach (Anima2D.SpriteMeshInstance x in SMP2)
            {
                x.color = new Color(Mathf.Lerp(Mathf.Abs(Mathf.Sin(invTime * 10)), dayColor.r, 0.7f), Mathf.Lerp(0, dayColor.g, 0.5f), Mathf.Lerp(0, dayColor.b, 0.5f), 1);
            }


        }

        if(invTime <= 0 && invulnerable == true)
        {

            invulnerable = false;

            GameObject Child1 = transform.GetChild(0).gameObject;
            GameObject Child2 = transform.GetChild(1).gameObject;

            Component[] SMP = Child1.GetComponentsInChildren<Anima2D.SpriteMeshInstance>();
            Component[] SMP2 = Child2.GetComponentsInChildren<Anima2D.SpriteMeshInstance>();

            if (SMP[0].GetComponent<Anima2D.SpriteMeshInstance>().color != nightColor)
            {
                foreach (Anima2D.SpriteMeshInstance x in SMP)
                {
                    x.color = nightColor;
                }
            }

            if (SMP2[0].GetComponent<Anima2D.SpriteMeshInstance>().color != dayColor)
            {
                foreach (Anima2D.SpriteMeshInstance x in SMP2)
                {
                    x.color = dayColor;
                }
            }

        
        }

    }

    private void Die()
    {
        GameCore.Instance.gameState = GameState.GAMEOVER;
        Debug.Log("Die!");
    }

    #endregion
}
