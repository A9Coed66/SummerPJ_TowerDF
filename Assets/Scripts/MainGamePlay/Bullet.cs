using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public  Transform protectPoint;
    private Transform target;
    public float speed = 70f;
    public int damage = 50;
    public float explosionRadius = 0f;
    public GameObject impactEffect;


    public void Seek(Transform _target  )
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        if(target==null)
        {
            Destroy(gameObject);
            return;            
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        Debug.Log("Hit target");
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if(explosionRadius>0f)
        {
            Explode();
        }else
        {
            Damage(target);
        }

        Destroy(gameObject);  
        // Destroy(target.gameObject); 
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider colider in colliders)
        {
            if(colider.tag=="Enemy")
            {
                Damage(colider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e =  enemy.GetComponent<Enemy>();

        if (e!=null)
        {
            e.TakeDamage(damage);
        }        
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;    
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
