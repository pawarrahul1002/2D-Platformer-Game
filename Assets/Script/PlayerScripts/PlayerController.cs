using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Elle2D
{
    //this class controll player health , damage, different aniamations and all movements
    public class PlayerController : MonoBehaviour
    {
        private Animator animator;
        private CapsuleCollider2D capCollider2D;
        private Rigidbody2D rb2D;

        [Header("Collider position")]
        [SerializeField] Vector2 colliderOffset;
        [SerializeField] Vector2 crouchColliderOffset;
        [SerializeField] Vector2 colliderSize;
        [SerializeField] Vector2 crouchColliderSize;

        [Header("Movement Setting")]
        [SerializeField] float speed;
        [SerializeField] float jumpForce;
        [SerializeField] float downForce;
        private bool onGround;
        private bool isJump;

        [Header("Health Setting")]
        [SerializeField] int livesRemain = 1;
        [SerializeField] Image[] heart;
        [SerializeField] Text scoreText;
        [SerializeField] Image gameOverButtonImage;
        public Image nextSceneButtonImage;
        [SerializeField] ScoreController scoreController;
        private int scoreValue = 10;
        [HideInInspector] public bool isFacingRight = true;
        [HideInInspector] public bool withGun = false;
        private AudioSource audioSource;
        [SerializeField] AudioClip[] PlayerSounds;
        private float volume = 0.3f;
        private bool gameOver;
        private float horizantal;
        private float vertical;
        private float timeForHurt = 1f;
        private bool isPaused;
        private string movingPlatformTag = "MovingPlatform";
        private string staticPlatformTag = "Platform";

        [HideInInspector] public bool crouch;


        private enum Sounds
        {
            key,
            playerDied,
            jump,
            fire
        }

        //<summry>
        // awake is used to intialize any variable or game state before game starts
        // awake is always called before satrt function

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            capCollider2D = GetComponent<CapsuleCollider2D>();
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
        }

        void start()
        {
            isFacingRight = true;
            Debug.Log(onGround);
        }

        public void PickUpKey()
        {
            audioSource.PlayOneShot(PlayerSounds[(int)(Sounds.key)], volume);
            scoreController.increaseScore(scoreValue);

        }//PickUpKey

        //<summry>
        // killPlayer will reduce health by one and reload player to start position
        // after three chance game over button will pop up
        public void KillPlayer()
        {

            audioSource.PlayOneShot(PlayerSounds[(int)(Sounds.playerDied)], volume);
            livesRemain--;
            capCollider2D.enabled = false;

            animator.Play("PlayerHurt");
            StartCoroutine(PlayHurtAnimation());
            updateLifeUI();
            if (gameOver == true)
            {
                GameOverButtonText();
            }

        }//KillPlayer

        IEnumerator PlayHurtAnimation()
        {
            yield return new WaitForSeconds(timeForHurt);


            transform.position = Vector2.zero;
            Vector3 startScale = transform.localScale;
            startScale.x = Mathf.Abs(startScale.x);
            transform.localScale = startScale;
            capCollider2D.enabled = true;
            animator.Play("Ellen_Idle");

        }//PlayHurtAnimation

        //GameOverButtonText func helps in showing final score
        public void GameOverButtonText()
        {
            gameOverButtonImage.gameObject.SetActive(true);
            scoreText.text = "Total Score: " + scoreController.score.ToString();
        }

        //this update function will deactivate heart compoenent
        private void updateLifeUI()
        {
            heart[livesRemain].gameObject.SetActive(false);

            if (livesRemain == 0)
            {
                heart[livesRemain].gameObject.SetActive(false);
                gameOver = true;
            }

        }//updateLifeUI

        ///from here script is for  movement
        private void Update()
        {
            horizantal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Jump");      //use "Jump" or "Vertical" both are same
            crouch = Input.GetKey(KeyCode.DownArrow);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = !isPaused;
                PauseGame();
            }

        }//Update

        void PauseGame()
        {
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

        }//PauseGame

        private void FixedUpdate()
        {
            MoveCharacter(horizantal, vertical);
            PlayMovementAniamation(horizantal, vertical, crouch);

        }//FixedUpdate

        // checking Player on ground or not and setting bool onGround
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(staticPlatformTag))
            {
                onGround = true;
            }
            if (collision.gameObject.CompareTag(movingPlatformTag))
            {
                onGround = true;
                transform.parent = collision.transform;
                rb2D.gravityScale = 0;
            }

        }//OnCollisionEnter2D

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(staticPlatformTag))
            {
                onGround = false;
            }
            if (collision.gameObject.CompareTag(movingPlatformTag))
            {
                onGround = false;
                transform.parent = null;
                rb2D.gravityScale = 1;
            }

        }//OnCollisionExit2D

        private void MoveCharacter(float horizantal, float vertical)
        {
            RunChar(horizantal);
            JumpChar(vertical);

        }//MoveCharacter

        void RunChar(float horizantal)
        {

            Vector3 pos = transform.position;
            pos.x += horizantal * speed * Time.deltaTime;
            transform.position = pos;

        }//RunChar

        void JumpChar(float vertical)
        {
            if ((vertical > 0) && (onGround == true))
            {
                rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                onGround = false;
            }

        }//JumpChar
        private void PlayMovementAniamation(float horizantal, float vertical, bool crouch)
        {
            RunAnim(horizantal);
            JumpAnim(vertical);
            CrouchAnim(crouch);
            IdleWithGun();

        }//PlayMovementAniamation

        private void IdleWithGun()
        {
            if (Input.GetButton("Fire1") && onGround)
            {
                audioSource.PlayOneShot(PlayerSounds[(int)Sounds.fire], volume);
                withGun = true;
                animator.SetBool("WithGun", true);
            }
            else
            {
                withGun = false;
                animator.SetBool("WithGun", false);
            }

        }//IdleWithGun

        //RunAnim fun for run animation
        void RunAnim(float horizantal)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizantal));
            Vector3 scale = transform.localScale;
            if (horizantal < 0)
            {
                isFacingRight = false;
                scale.x = -1 * Mathf.Abs(scale.x);
            }
            else if (horizantal > 0)
            {
                isFacingRight = true;
                scale.x = Mathf.Abs(scale.x);
            }
            transform.localScale = scale;

        }//RunAnim

        void JumpAnim(float vertical)
        {
            if (vertical > 0 && rb2D.velocity.y > 0)
            {
                if (isJump == false && onGround == false)
                {

                    audioSource.PlayOneShot(PlayerSounds[(int)Sounds.jump], volume);
                    animator.SetBool("IsJump", true);
                    isJump = true;
                }
                animator.SetBool("JumpFall", false);
            }
            else
            {
                animator.SetBool("JumpFall", true);
                animator.SetBool("IsJump", false);
                rb2D.velocity = new Vector2(0, downForce);

                if (onGround == true)
                {
                    isJump = false;
                    animator.SetBool("JumpFall", false);
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
        }//JumpAnim

        void CrouchAnim(bool crouch)
        {
            if (crouch == true)
            {
                animator.SetBool("IsCrouch", crouch);
                capCollider2D.offset = new Vector2(crouchColliderOffset.x, crouchColliderOffset.y);
                capCollider2D.size = new Vector2(crouchColliderSize.x, crouchColliderSize.y);

            }
            else
            {
                animator.SetBool("IsCrouch", crouch);
                capCollider2D.offset = new Vector2(colliderOffset.x, colliderOffset.y);
                capCollider2D.size = new Vector2(colliderSize.x, colliderSize.y);
            }

        }//CrouchAnim
        
    } // class

}

