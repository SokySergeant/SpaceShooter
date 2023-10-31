using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 5f;

    [SerializeField] private LayerMask EnemyLayer;
    private BoxCollider2D Collider;
    private ContactFilter2D CFilter;
    List<Collider2D> Cols;

    void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        CFilter = new ContactFilter2D();
        CFilter.SetLayerMask(EnemyLayer);
        Cols = new List<Collider2D>();
    }

    void Update()
    {
        //Move
        transform.position += transform.up * Speed * Time.deltaTime;

        //Kill overlapping enemy
        Collider.OverlapCollider(CFilter, Cols);
        if(Cols.Count > 0)
        {
            Destroy(Cols[0].gameObject);
            Destroy(gameObject);
        }
    }
}
