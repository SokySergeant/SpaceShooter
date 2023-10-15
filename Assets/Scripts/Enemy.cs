using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float Speed = 2f;
    [SerializeField] private int Damage = 1;

    [HideInInspector] public Transform PlayerTrans;

    [SerializeField] private LayerMask PlayerLayer;
    private BoxCollider2D Collider;
    private ContactFilter2D CFilter;
    List<Collider2D> Cols;

    void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        CFilter = new ContactFilter2D();
        CFilter.SetLayerMask(PlayerLayer);
        Cols = new List<Collider2D>();
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

        //Damage player
        Collider.OverlapCollider(CFilter, Cols);
        if(Cols.Count > 0)
        {
            Cols[0].GetComponent<Health>().UpdateHealth(-Damage);
            Destroy(gameObject);
        }
    }
}
