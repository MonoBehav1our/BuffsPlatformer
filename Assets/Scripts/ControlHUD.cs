using UnityEngine;

public class ControlHUD : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(DeviceInfo.IsMobile());
    }
}
