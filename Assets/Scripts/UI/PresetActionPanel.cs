using UnityEngine;

public class PresetActionPanel : MonoBehaviour
{
    //private bool _visible = true;
    //[SerializeField] private GameObject _panel;
    [SerializeField] private SpellFontGenerator _codeText;
    [SerializeField] private bool _autoUpdateSpellText;

    [Header("")]
    [SerializeField] private string _code;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        if (_autoUpdateSpellText)
        {
            _codeText.SetCode(_code);
        }
    }
    //private void SetVisibility(bool visible)
    //{
    //    _visible = visible;
    //    _panel.SetActive(visible);
    //}
    //public void EnableCommand()
    //{
    //    SetVisibility(true);
    //}
    public void EnterCode()
    {
        ConsoleRender.Instance.InsertCommand(_code);
    }

}
