using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;   // Секунд между выстрелами
    public float health = 10;
    public int score = 100;         // Очки за уничтожение корабля

    protected BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    private void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown)
        {  // Корабль за нижней границей, поэтому его можно уничтожить
           Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGo = collision.gameObject;
        if (otherGo.tag == "ProjectileHero")
        {
            Destroy(otherGo);                       // Уничтожить снаряд
            Destroy(gameObject);                    // Уничтожить Enemy
        }
        else
        {

        }
    }
}
