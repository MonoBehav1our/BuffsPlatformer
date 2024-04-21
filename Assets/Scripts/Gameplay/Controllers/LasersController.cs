using UnityEngine;

public class LasersController : MonoBehaviour, IService
{
    [SerializeField] private Laser[] lasers;

    public bool ChangeState()
    {
        foreach (Laser laser in lasers)
        {
            laser.ChangeState();
        }
        return true;
    }
}
