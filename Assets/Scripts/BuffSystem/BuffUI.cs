using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private Image[] pictures;

    private void Start()
    {
        pictures = GetComponentsInChildren<Image>();
        BuffManager.Instance.BuffUpdated += UpdateUI;
    }
    private void OnDisable() => BuffManager.Instance.BuffUpdated -= UpdateUI;

    public void UpdateUI(BuffInfo[] buffsInfo)
    {
        for (int i = 0; i < pictures.Length; i++) 
        {
            if (i < buffsInfo.Length)
            {
                pictures[i].color = Color.white;
                pictures[i].sprite = buffsInfo[i].Sprite;
            }
            else
            {
                pictures[i].color = Color.clear;
                pictures[i].sprite = null;
            }
        }
    }
}