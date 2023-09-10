using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    private Transform target;
    [SerializeField] private Enemy targetEnemy;
    private Transform protectPoint;

    [Header("General")]
    public float range = 20f;

    [Header("User Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool userLaser = false;
    public int damageOverTime = 30;
    public float slowPercent;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint; //Thay vi ban tu trung tam, ta se tao ra mot diem phu hop hon de ban ra vien dan




    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    // Update is called once per frame
    void Update()
    {
        if(target==null)
        {
            if(userLaser)
            {
                if (lineRenderer.enabled)
                {
                    impactEffect.Stop();
                    lineRenderer.enabled = false;
                    impactLight.enabled = false;
                }
            }
            return;
        }
        
        LockOnTarget();
        UpdateTarget();
        if (userLaser)
        {
            Laser();
        }else
        {
            if(fireCountdown <=0f)
            {
                Shoot();
                fireCountdown = 1f/fireRate;
            }
            fireCountdown -=Time.deltaTime;
        }
        
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if(nearestEnemy!=null && shortestDistance<=range)
            {
                target = nearestEnemy.transform;
                targetEnemy = nearestEnemy.GetComponent<Enemy>(); 
            }
            else
                target = null;
        }
    }

    //Bắn kẻ địch gần Protect Point
    void NewUpdateTarget()
    {
        GameObject [] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestDistanceToProtectPoint = null;
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(protectPoint.transform.position, enemy.transform.position);
            if(distanceToEnemy<shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestDistanceToProtectPoint = enemy;
            }
            if(nearestDistanceToProtectPoint!=null && shortestDistance<range)
            {
                target = nearestDistanceToProtectPoint.transform;
                targetEnemy = nearestDistanceToProtectPoint.GetComponent<Enemy>(); 
            }
            else
            {
                target = null;
            }
        }
    }

    //Bắn kẻ địch đầu tiên tiến vào range 
    void NewUpdateTarget1()
    {
        
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet!=null)
        {
            bullet.Seek(target);
        }
        Debug.Log("SHOOT!");
    }


    private void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime*Time.deltaTime);
        targetEnemy.Slow(slowPercent);

        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.position = target.position + dir.normalized;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void LockOnTarget()
    {
        //TargetLock
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    //Nếu chọn thì sẽ hiện range, còn ko thì bỏ chữ Selected ở hàm đi
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
