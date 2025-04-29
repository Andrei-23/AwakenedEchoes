using UnityEngine;
using UnityEngine.UI;

public class UICodeSymbol : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Sprite _upSprite;
    [SerializeField] private Sprite _downSprite;
    [SerializeField] private Sprite _leftSprite;
    [SerializeField] private Sprite _rightSprite;
    [SerializeField] private Sprite _defaultSprite;

    [SerializeField] private Console.Symbol _symbol;

    public void SetSymbol(Console.Symbol symbol)
    {
        if (_symbol == symbol) return;
        _symbol = symbol;
        UpdateSprite();
    }
    private void OnValidate()
    {
        UpdateSprite();
    }
    //public Console.Symbol symbol
    //{
    //    get { return symbol; }
    //    set {
    //        if(symbol == value) return;
    //        symbol = value;
    //        _image.sprite = GetSymbolSprite(value);
    //    }
    //}

    private void UpdateSprite()
    {
        if(_image == null)
        {
            _image = GetComponent<Image>();
        }
        _image.sprite = GetSymbolSprite(_symbol);
    }
    private Sprite GetSymbolSprite (Console.Symbol symbol)
    {
        switch(symbol)
        {
            case Console.Symbol.Up: return _upSprite;
            case Console.Symbol.Down: return _downSprite;
            case Console.Symbol.Left: return _leftSprite;
            case Console.Symbol.Right: return _rightSprite;
            default: return _defaultSprite;
        }
    }
}
