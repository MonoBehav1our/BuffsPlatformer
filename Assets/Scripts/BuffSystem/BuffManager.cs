using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    public static event Action<IAction[]> BuffUpdated;
    public static event Action<IAction> BuffUsed;
    public static event Action VoidBuffUsed;

    [Header("Buff Settings")]
    [SerializeField] private Queue<IAction> _buffs = new Queue<IAction>();
    [SerializeField] private BuffConfig[] _startBuffsType;
    [SerializeField] private float _rechargingTime;
    private bool _recharging = true;

    public void EnqueueBuff(IAction buff)
    {
        _buffs.Enqueue(buff);
        BuffUpdated?.Invoke(_buffs.ToArray());
    }

    public void DisableBlock()
    {
        _recharging = false;
    }

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < _startBuffsType.Length; i++)
            BuffFactory.Instance.AddBuff(_startBuffsType[i]);

        BuffUpdated?.Invoke(_buffs.ToArray());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !PauseHandler.Instance.IsPausing) UseBuff();
    }

    public void UseBuff()
    {
        if (_recharging) return;
        if (_buffs.Count == 0)
        {
            VoidBuffUsed?.Invoke();
            return;
        }      

        if (_buffs.Peek().UseBuff())
        {
            BuffUsed?.Invoke(_buffs.Dequeue());
            BuffUpdated?.Invoke(_buffs.ToArray());
            StartCoroutine(BuffCD());
        }
    }

    private IEnumerator BuffCD()
    {
        _recharging = true;
        yield return new WaitForSeconds(_rechargingTime);
        _recharging = false;
    }
}
