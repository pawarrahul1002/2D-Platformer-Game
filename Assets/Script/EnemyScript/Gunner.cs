using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Elle2D
{
    //this class is control boss enemy, its movement damage and health and also shoot
    public class Gunner : MonoBehaviour
    {
        [SerializeField] public LayerMask playerLayer;
        [SerializeField] public Transform leftCollison;
        [SerializeField] public GameObject bombPrefab;
        public EnemyController enemy;

        public HealthBar healthBar;
        private float timeuntillFire;
        [SerializeField] public float fireRate;

        private AudioSource audioSource;
        [SerializeField] AudioClip[] gunnerSounds;

        private void Awake()
        {

            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {

            int enemyMaxHealth = enemy.enemyHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
        }

        void Update()
        {
            checkCollision();
            CheckGunnerhealth();
        }

        /*this fun checks collision with player and if collision h
        appen using raycast then shoot fun is called, also control sound of shooting */
        void checkCollision()
        {
            RaycastHit2D leftHit = Physics2D.Raycast(leftCollison.position, Vector2.left, 15f, playerLayer);
            // Debug.DrawRay(leftCollison.position, Vector2.left * 15f);
            if (leftHit)
            {
                if (timeuntillFire < Time.time)
                {
                    Shoot();
                    audioSource.PlayOneShot(gunnerSounds[0], 1f);
                    timeuntillFire = Time.time + fireRate;
                }

            }
        }

        void CheckGunnerhealth()
        {
            healthBar.SetHealth(enemy.enemyHealth);
        }

        /*this fun is foe instantiating bullet from enemy*/
        void Shoot()
        {
            float angle = enemy.moveRight ? 0f : 180f;
            Instantiate(bombPrefab, leftCollison.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        }
    }


}// class