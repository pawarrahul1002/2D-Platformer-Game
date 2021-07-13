using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elle2D
{
    //this class controlls movement, health, damage of enemies  
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float speed;
        public bool moveRight;
        public int enemyHealth = 40;
        [SerializeField] bool idleEnemy = false;
        public Animator anim;

        void Update()
        {
            if (!idleEnemy)
            {
                MoveEnemy();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                playerController.KillPlayer();
            }
        }

        private void MoveEnemy()
        {

            Vector3 scale = transform.localScale;
            if (moveRight)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
                scale.x = 1 * Mathf.Abs(scale.x);
            }
            else
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                scale.x = -1 * Mathf.Abs(scale.x);
            }

            transform.localScale = scale;
        }

        private void OnTriggerEnter2D(Collider2D trig)
        {
            if (trig.gameObject.CompareTag("Turn"))
            {
                if (moveRight)
                {
                    moveRight = false;
                }
                else
                {
                    moveRight = true;
                }
            }
        }

        public void TakeDamage(int damage)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0)
            {
                StartCoroutine(PlayDeadEnemy());
            }
        }

        IEnumerator PlayDeadEnemy()
        {
            anim.SetBool("Dead", true);

            idleEnemy = true;
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }



    }
}