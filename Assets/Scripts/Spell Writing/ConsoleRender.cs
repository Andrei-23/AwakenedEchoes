using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConsoleRender : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;
    [SerializeField] private Console _console;
    [SerializeField] private Cursor _cursor;
    [SerializeField] private GameObject _cellPrefab;
    private List<List<ConsoleCell>> _cells;

    private int _cursorX = 0;
    private int _cursorY = 0;
    private RectTransform _cursorRect;

    [Inject] private DiContainer _diContainer;

    public static ConsoleRender Instance;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;

        _cursorRect = _cursor.GetComponent<RectTransform>();
        _cells = new List<List<ConsoleCell>>();
        for (int i = 0; i < _console.height; i++)
        {
            _cells.Add(new List<ConsoleCell>());
            for (int j = 0; j < _console.width; j++)
            {
                GameObject go = _diContainer.InstantiatePrefab(_cellPrefab, _grid.transform);
                _cells[i].Add(go.GetComponent<ConsoleCell>());
            }
        }

        _grid.constraintCount = _console.width;
    }

    private void OnEnable()
    {
        UpdateCode();
        UpdateCursorPosition(false);
    }
    private void Start()
    {
        UpdateCode();
        UpdateCursorPosition(false);
    }
    public void InsertCommand(string code)
    {
        Console.Instance.InsertCommand(code);
        UpdateCode();
    }
    public void UpdateCode(/*List<List<Console.Symbol>> code, int _cursorX, int _cursorY*/)
    {
        _console.OpenUpdate();

        for(int i = 0; i < _console.height; i++)
        {
            for (int j = 0; j < _console.width; j++)
            {
                Console.Symbol s = Console.Symbol.Space;
                if(i < _console.code.Count && j < _console.code[i].Count)
                {
                    s = _console.code[i][j];
                }
                _cells[i][j].Set(s);
            }
        }

        UpdateCursorPosition();
    }
    private void UpdateCursorPosition(bool checkChanged = true)
    {
        if (checkChanged && _cursorX == _console.cursorX && _cursorY == _console.cursorY)
        {
            return;
        }
        
        _cursorX = _console.cursorX;
        _cursorY = _console.cursorY;

        Vector2 pos = Vector2.zero;
        pos.x += (_grid.cellSize.x + _grid.spacing.x) * (_cursorX - _console.width / 2f);
        pos.y -= (_grid.cellSize.y + _grid.spacing.y) * (_cursorY - _console.height / 2f + 0.5f);
        _cursorRect.anchoredPosition = pos;
    }

}
