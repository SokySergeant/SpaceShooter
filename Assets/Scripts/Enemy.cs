using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float Speed = 2f;
    [SerializeField] private float TimeBetweenShots = 1f;

    [HideInInspector] public Transform PlayerTrans;
    [SerializeField] private GameObject Projectile;

    void Start()
    {
        StartCoroutine(Shoot());
    }

    void Update()
    {
        //Rotate and move towards player
        if(PlayerTrans)
        {
            Vector2 Dir = PlayerTrans.position - transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, Dir);
            transform.position += transform.up * Speed * Time.deltaTime;
        }
    }

    private IEnumerator Shoot()
    {
        while(true)
        {
            if(PlayerTrans)
            {
                Instantiate(Projectile, transform.position, transform.rotation);
            }
            yield return new WaitForSeconds(TimeBetweenShots);
        }
    }
}
