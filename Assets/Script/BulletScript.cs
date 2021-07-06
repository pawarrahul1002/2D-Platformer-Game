﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] int damage = 10;
    public Rigidbody2D rb;
    public PlayerController player;

    // Update is called once per frame
    void Start()
    {
        rb.velocity = transform.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyController enemy = collider.GetComponent<EnemyController>();
        // Debug.Log(collider.gameObject.name);
        // if (collider.gameObject.tag == "Enemy")
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
