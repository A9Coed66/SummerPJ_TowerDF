using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector] public float speed;
    public float health = 100;
    public int worth = 40;
    public GameObject deathEffect;

    private void Start() {
        speed = startSpeed;
    }

    public void TakeDamage(float amount)
    {
        health-=amount;
        if (health<=0)
        {
            Debug.Log("Die");
            Die();
        }
    }

    public void Slow(float percent)
    {
        speed = startSpeed*(1f-percent);
    }

    private void Die()
    {
        GameObject effect = Instantiate(deathEffect,transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        PlayerStats.Money+=worth;
        Destroy(gameObject);
    }

}
