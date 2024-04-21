using UnityEngine;
using YG;

public class LanguageSetter : MonoBehaviour
{
    public void ChangeLanguage()
    {
        if (YandexGame.lang == "ru") YandexGame.SwitchLanguage("en");
        else if (YandexGame.lang == "en") YandexGame.SwitchLanguage("ru");
    }
}
