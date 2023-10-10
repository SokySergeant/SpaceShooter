using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D Rb;
    [SerializeField] private float Speed = 1f;
    [SerializeField] private GameObject Projectile;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext Ctx)
    {
        Rb.velocity = Ctx.ReadValue<Vector2>() * Speed;
    }

    public void OnLook(InputAction.CallbackContext Ctx)
    {
        Vector2 InputLoc = Ctx.ReadValue<Vector2>();
        Vector2 Dir = new Vector3(InputLoc.x, InputLoc.y, 0) - Camera.main.WorldToScreenPoint(transform.position);
        transform.rotation = Quaternion.LookRotation(transform.forward, Dir);
    }

    public void OnFire(InputAction.CallbackContext Ctx)
    {
        if(Ctx.started)
        {
            Instantiate(Projectile, transform.position, transform.rotation);
        }
    }
}
