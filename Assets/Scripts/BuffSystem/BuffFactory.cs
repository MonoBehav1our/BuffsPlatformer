using UnityEngine;

public class BuffFactory : MonoBehaviour
{
    public static BuffFactory Instance;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GravityController _gravityController;
    [SerializeField] private LasersController _lasersController;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void AddBuff(BuffConfig config)
    {
        IAction action = null;
        switch (config.Type) 
        {
            case BuffType.Jump:
                action = new JumpBuff(config, _playerController);
                break;
            case BuffType.Dash:
                action = new DashBuff(config, _playerController);
                break;
            case BuffType.UpDash:
                action = new UpDashBuff(config, _playerController);
                break;
            case BuffType.ReverseGravity:
                action = new ReverseGravityBuff(config, _gravityController);
                break;
            case BuffType.Lasers:
                action = new LasersBuff(config, _lasersController);
                break;
        }

        if (action != null) BuffManager.Instance.EnqueueBuff(action);
    }    
}
