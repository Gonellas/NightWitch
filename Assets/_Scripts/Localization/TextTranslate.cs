using UnityEngine;
using UnityEngine.UI;

public class TextTranslate : MonoBehaviour
{
    [SerializeField] private string _id;

    [SerializeField] private Localization _localization;

    [SerializeField] private Text _myText;

    private void Awake()
    {
        _localization.OnUpdate += ChangeLang;
    }

    void ChangeLang()
    {
        _myText.text = _localization.GetTranslate(_id);
    }

    private void OnDestroy()
    {
        _localization.OnUpdate -= ChangeLang;
    }
}
