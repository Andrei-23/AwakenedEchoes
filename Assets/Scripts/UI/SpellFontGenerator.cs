using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class SpellFontGenerator : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private List<UICodeSymbol> _symbolObjects;
    [SerializeField] private string _code;
    private void OnValidate()
    {
        UpdateCode();
    }
    private void Start()
    {
        UpdateCode();
    }
    public void SetCode(string code)
    {
        _code = code;
        UpdateCode();
    }

    private void UpdateCode()
    {
        List<Console.Symbol> symbols = new List<Console.Symbol>();
        foreach (char c in _code)
        {
            Console.Symbol s = Console.CharToSymbol(c);
            if (s != Console.Symbol.Space)
            {
                symbols.Add(s);
            }
        }

        for (int i = 0; i < _symbolObjects.Count; i++)
        {
            bool active = i < symbols.Count;
            _symbolObjects[i].gameObject.SetActive(active);
            if (active)
            {
                _symbolObjects[i].SetSymbol(symbols[i]);
            }
        }
    }
}
