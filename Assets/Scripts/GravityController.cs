using UnityEngine;

public class GravityController : MonoBehaviour
{
    public bool ReverseGravity()
    {
        Physics2D.gravity *= -1;
        return true;
    }
}
