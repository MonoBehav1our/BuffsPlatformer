using UnityEngine;
using YG;

public class VolumeSetter : MonoBehaviour
{
    private LangYGAdditionalText text;

    void Start()
    {
        text = GetComponentInChildren<LangYGAdditionalText>();
    }

    private void Update()
    {
        SetText();
    }

    private void SetText()
    {
        if (AudioListener.volume == 1f)
        {
            if (YandexGame.lang == "ru")
                text.additionalText = "вкл";
            else if (YandexGame.lang == "en")
                text.additionalText = "on";
        }
        else if (AudioListener.volume == 0f)
        {
            if (YandexGame.lang == "ru")
                text.additionalText = "выкл";
            else if (YandexGame.lang == "en")
                text.additionalText = "off";
        }
    }

    public void ChangeVolume()
    {
        if (AudioListener.volume == 1f) AudioListener.volume = 0f;
        else if (AudioListener.volume == 0f) AudioListener.volume = 1f;
    }
}
