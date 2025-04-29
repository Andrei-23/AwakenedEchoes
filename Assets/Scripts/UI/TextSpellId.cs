using TMPro;
using UnityEngine;

public class TextSpellId : MonoBehaviour
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        PlayerInputManager.OnSpellSelected += SelectSpell;
    }
    private void OnDisable()
    {
        PlayerInputManager.OnSpellSelected -= SelectSpell;
    }

    private void SelectSpell(PlayerInputManager.SelectData selectData)
    {
        _text.text = (selectData.id + 1).ToString();
    }
}
