using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGruntScript : MonoBehaviour
{
    public AudioClip Sound;
    public float Speed;

    private Rigidbody2D Rigidbody2D;
    private Vector2 Direction;
    private int Damage;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * Speed;
    }

    public void SetDirection(Vector2 direction)
    {
        Direction = direction;
    }

    public void SetDamage(int damage)
    {
        Damage = damage;
    }

    public void DestroyBulletGrunt()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        JohnMovement john = collision.GetComponent<JohnMovement>();
        if (john != null)
        {
            john.Hit(Damage);
        }
        DestroyBulletGrunt();
    }
}
