using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    public Action<BuffInfo[]> BuffUpdated;

    [Header("Buff Settings")]
    [SerializeField] private Queue<BuffInfo> _buffs = new Queue<BuffInfo>();
    [SerializeField] private BuffInfo[] _startBuffsType;
    [SerializeField] private float _rechargingTime;
    private bool _recharging = false;

    [Space]

    [Header("BuffUser Components")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GravityController _gravityController;
    [SerializeField] private LasersController _lasersController;

    public void EnqueueBuff(BuffInfo buff)
    {
        _buffs.Enqueue(buff);
        BuffUpdated?.Invoke(_buffs.ToArray());
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);

        for (int i = 0; i < _startBuffsType.Length; i++)
            _buffs.Enqueue(_startBuffsType[i]);

    }

    private void Start()
    {
        BuffUpdated?.Invoke(_buffs.ToArray());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) UseBuff();
    }

    private void UseBuff()
    {
        if (_buffs.Count == 0) return;
        if (_recharging) return;

        bool actionEnd = false;
        bool needCD = false;

        switch (_buffs.Peek().Type) //buff type
        {
            case BuffType.Jump:
                actionEnd = _playerController.Jump();
                needCD = actionEnd;
                break;
            case BuffType.Dash:
                actionEnd = _playerController.Dash();
                needCD = actionEnd;
                break;
            case BuffType.ReverseGravity: 
                actionEnd = _gravityController.ReverseGravity();
                needCD = actionEnd;
                break;
            case BuffType.UpDash:
                actionEnd = _playerController.UpDash();
                break;
            case BuffType.Lasers:
                actionEnd = _lasersController.ChangeState();
                break;
        }

        if (actionEnd)
        {
            _buffs.Dequeue();
            BuffUpdated?.Invoke(_buffs.ToArray());
        }
        if (needCD) StartCoroutine(BuffCD());
    }

    private IEnumerator BuffCD()
    {
        _recharging = true;
        yield return new WaitForSeconds(_rechargingTime);
        _recharging = false;
    }
}
