using UnityEngine;

public class StartGuide : MonoBehaviour
{
    [SerializeField] private GameObject _pcGuide;
    [SerializeField] private GameObject _mobileGuide;

    [SerializeField] private GameObject _mobileHand;

    void Start()
    {
        if (DeviceInfo.IsMobile())
        {
            _mobileGuide.SetActive(true);
            _mobileHand.SetActive(true);
        }
        else _pcGuide.SetActive(true);
    }
}
