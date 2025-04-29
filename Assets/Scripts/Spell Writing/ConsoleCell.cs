using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConsoleCell : MonoBehaviour
{
    [SerializeField] private Image _image;
    private Console.Symbol _currentSymbol;
    private RectTransform _rectTransform;

    [Inject] private ConsoleCellSprites _cellSprites;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Set(Console.Symbol symbol)
    {
        if (symbol == _currentSymbol) return;
        _currentSymbol = symbol;
        // animation?
        _image.sprite = GetSprite(symbol);
    }

    private Sprite GetSprite(Console.Symbol symbol)
    {
        switch (symbol)
        {
            case Console.Symbol.Up: return _cellSprites.upSymbol;
            case Console.Symbol.Down: return _cellSprites.downSymbol;
            case Console.Symbol.Left: return _cellSprites.leftSymbol;
            case Console.Symbol.Right: return _cellSprites.rightSymbol;

            case Console.Symbol.Space:
            default:
                return _cellSprites.spaceSymbol;
        }
    }
    public RectTransform GetRect()
    {
        return _rectTransform;
    }
}
