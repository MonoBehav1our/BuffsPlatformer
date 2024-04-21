using UnityEngine;

public class GravityController : MonoBehaviour, IService
{
    private void Start() => Physics2D.gravity = new Vector2(0, -Mathf.Abs(Physics2D.gravity.y));

    public bool ReverseGravity()
    {
        Physics2D.gravity *= -1;
        return true;
    }
}
