using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private int HealAmount = 1;

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
        //Heal player
        Collider.OverlapCollider(CFilter, Cols);
        if(Cols.Count > 0)
        {
            Cols[0].GetComponent<Health>().UpdateHealth(HealAmount);
            Destroy(gameObject);
        }
    }
}
