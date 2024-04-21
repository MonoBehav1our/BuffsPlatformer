using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [SerializeField] private Image[] pictures;

    private void Start()
    {
        pictures = GetComponentsInChildren<Image>();
    }

    private void OnEnable() => BuffManager.BuffUpdated += UpdateUI;
    private void OnDisable() => BuffManager.BuffUpdated -= UpdateUI;

    public void UpdateUI(IAction[] buffs)
    {
        for (int i = 0; i < pictures.Length; i++) 
        {
            if (i < buffs.Length)
            {
                pictures[i].color = Color.white;
                pictures[i].sprite = buffs[i].GetConfig().Sprite;
            }
            else
            {
                pictures[i].color = Color.clear;
                pictures[i].sprite = null;
            }
        }
    }
}