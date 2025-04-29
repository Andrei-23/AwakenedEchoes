using TMPro;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    [SerializeField] private RectTransform _bar;
    [SerializeField] private TextMeshProUGUI _text;

    private float _curSize = 1f;

    public void SetValue(float value, float maxValue)
    {
        _text.text = ((int)value).ToString() + " / " + ((int)maxValue).ToString();
        float size = Mathf.Clamp01(value / maxValue);
        _curSize = size;
        Vector3 scale = _bar.localScale;
        scale.x = size;
        _bar.localScale = scale;
    }
}
