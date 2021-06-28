using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CapsuleCollider2D capCollider2D;
    // public BoxCollider2D boxCollider2D;
    private Rigidbody2D rb2D;
    
    public float start_x, start_y;


    public float crouchOffSetx, crouchOffSety;
    public float crouchSizex, crouchSizey;
    public float offsetx, offsety;
    public float sizex, sizey;
    private bool gameOver;
    public float speed;
    public float jumpForce;
    public float downForce;
    public float ConstdownForce;
    private bool onGround;
    private int jumpCount = 0;
    [SerializeField]
    private int livesRemain = 1;
    public Image life01;
    public Image life02;
    public Image life03;
    public Text highScoreText;
    public Button gameOverButton;
    public ScoreController scoreController;
    public string restartScene;
    private int scoreValue = 10;

    //awake is used to intialize any variable or game state before game starts
    //awake is always called before satrt function
    private void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    public void PickUpKey()
    {
        scoreController.increaseScore(scoreValue);
    }

    //killPlayer will reduce health by one and reload player to start position
    //after three chance game over button will pop up
    public void KillPlayer()
    {
        livesRemain--;
        transform.position = new Vector3(start_x, start_y, 0);
        transform.localScale = new Vector3(2, 2, 2);
        updateLifeUI();
        if (gameOver == true)
        {
            GameOverButtonText();
        }
    }

    //this text func helps in showing final score
    public void GameOverButtonText()
    {
        gameOverButton.gameObject.SetActive(true);
        highScoreText.text = "Total Score: " + scoreController.score.ToString();
    }

    //this update fun will deactivate heart compoenent
    private void updateLifeUI()
    {
        if (livesRemain == 3)
        {
            life01.gameObject.SetActive(true);
            life02.gameObject.SetActive(true);
            life03.gameObject.SetActive(true);
        }
        if (livesRemain == 2)
        {
            life01.gameObject.SetActive(true);
            life02.gameObject.SetActive(true);
            life03.gameObject.SetActive(false);
        }
        if (livesRemain == 1)
        {

            life01.gameObject.SetActive(true);
            life02.gameObject.SetActive(false);
            life03.gameObject.SetActive(false);
        }
        if (livesRemain == 0)
        {

            life01.gameObject.SetActive(false);
            life02.gameObject.SetActive(false);
            life03.gameObject.SetActive(false);
            gameOver = true;
        }
    }

    //this func help inn reloading scene with the help of game over button
    public void ReloadScene()
    {
        Debug.Log("reload this scene");
        SceneManager.LoadScene(restartScene);
    }
    float horizantal;
    float vertical;
    bool crouch;
    private void Update()
    {
        horizantal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Jump");      //use "Jump" or "Vertical" both are same
        crouch = Input.GetKey("left ctrl");
        MoveCharacter(horizantal, vertical);
        PlayMovementAniamation(horizantal, vertical, crouch);

    }

    //checking Player on ground or not and setting bool onGround
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = true;

        }
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            Debug.Log("123456");
            onGround = true;
            transform.parent = collision.transform;
            rb2D.gravityScale=0;


        }



    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = false;
        }
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            onGround = false;
            transform.parent = null;
            rb2D.gravityScale=1;


        }
    }

    private void MoveCharacter(float horizantal, float vertical)
    {
        RunChar(horizantal);
        JumpChar(vertical);
    }

    void RunChar(float horizantal)
    {
        Vector3 pos = transform.position;
        pos.x += horizantal * speed * Time.deltaTime;
        transform.position = pos;
    }

    void JumpChar(float vertical)
    {
        if ((vertical > 0) && (onGround == true))
        {
            rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            // rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            // Debug.Log("jump on");
            onGround = false;
        }
    }
    private void PlayMovementAniamation(float horizantal, float vertical, bool crouch)
    {
        RunAnim(horizantal);
        JumpAnim(vertical);
        CrouchAnim(crouch);
    }

    //RunAnim fun for run animation
    void RunAnim(float horizantal)
    {
        animator.SetFloat("Speed", Mathf.Abs(horizantal));
        Vector3 scale = transform.localScale;
        if (horizantal < 0)
        {
            scale.x = -1 * Mathf.Abs(scale.x);
        }
        else if (horizantal > 0)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }
    void JumpAnim(float vertical)
    {
        if (vertical > 0 && rb2D.velocity.y > 0)
        {
            if (jumpCount == 0 && onGround == false)
            {
                animator.SetBool("IsJump", true);
                jumpCount = 1;
            }
            animator.SetBool("JumpFall", false);
        }
        else
        {
            animator.SetBool("JumpFall", true);
            animator.SetBool("IsJump", false);
            rb2D.velocity = new Vector2(0.0f, downForce);

            if (onGround == true)
            {
                jumpCount = 0;
                animator.SetBool("JumpFall", false);
                rb2D.velocity = new Vector2(0.0f, ConstdownForce);

            }
        }
        if ((rb2D.velocity.y == 0) && (onGround == true))
        {
            animator.SetBool("JumpFall", false);
        }

        if (onGround == false)
        {
            animator.SetBool("JumpFall", true);
        }
    }

    //CrouchAnim fun for crouch animation
    void CrouchAnim(bool crouch)
    {
        if (crouch == true)
        {
            animator.SetBool("IsCrouch", crouch);
            capCollider2D.offset = new Vector2(crouchOffSetx, crouchOffSety);
            capCollider2D.size = new Vector2(crouchSizex, crouchSizey);
            // boxCollider2D.offset = new Vector2(-0.004810318f, 0.6084107f);
            // boxCollider2D.size = new Vector2(0.4740263f, 1.351288f);

        }
        else
        {
            animator.SetBool("IsCrouch", crouch);
            capCollider2D.offset = new Vector2(offsetx, offsety);
            capCollider2D.size = new Vector2(sizex, sizey);
            // boxCollider2D.offset = new Vector2(-0.004810318f, 0.9641527f);
            // boxCollider2D.size = new Vector2(0.4740263f, 2.012844f);
        }
    }

}
