using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Console : MonoBehaviour
{
    [Serializable]
    public enum Symbol
    {
        Up, Down, Left, Right, Space,
    }

    public static Console Instance { get; private set; }

    public int width;
    public int height;

    [HideInInspector] public int cursorX = 0;
    [HideInInspector] public int cursorY = 0;

    private int curID = 0;
    private List<List<List<Symbol>>> _savedCode;
    [HideInInspector] public List<List<Symbol>> code;

    [Inject] private PlayerInputManager _playerInputManager;
    //[SerializeField] private ConsoleRender _consoleRender;

    private bool _initialized = false;

    private List<Symbol> _line => code[cursorY];

    private void Awake()
    {
        Instance = this;
        InitializeCheck();
    }
    public void OpenUpdate()
    {
        InitializeCheck();
        SelectSpellCode(_playerInputManager.GetSlotId());
    }
    private void InitializeCheck()
    {
        if (_initialized) return;
        _initialized = true;

        _savedCode = new List<List<List<Symbol>>>();
        for (int i = 0; i < PlayerSpellManager.spellCount; i++)
        {
            _savedCode.Add(new List<List<Symbol>>
            {
                new List<Symbol>()
            });
        }
        ResetCode();
    }
    private void OnEnable()
    {
        PlayerInputManager.OnSpellSelected += OnSpellSelected;
        SelectSpellCode(_playerInputManager.GetSlotId());
    }
    private void OnDisable()
    {
        PlayerInputManager.OnSpellSelected -= OnSpellSelected;
        SaveSpellCode();
    }
    /// <summary>
    /// Updates symbols in game visual code
    /// </summary>
    //private void UpdateCodeVisual()
    //{
    //    PrintCodeDebug();
    //}
    //private void UpdateCursorVisual()
    //{
    //    PrintCodeDebug();
    //}
    private void PrintCodeDebug()
    {
        string res = "";
        for (int i = 0; i < code.Count; i++)
        {
            for (int j = 0; j < code[i].Count; j++)
            {
                if (cursorX == j && cursorY == i) res += '|';
                res += SymbolToChar(code[i][j]);
            }
            if (cursorX == code[i].Count && cursorY == i) res += '|';
            res += '\n';
            //if(i + 1 != _code.Count)
            //{
            //    res += "\n";
            //}
        }
        res += "Cursor at " + cursorY.ToString() + ":" + cursorX.ToString();
        Debug.Log(res);
    }
    public static char SymbolToChar(Symbol s)
    {
        switch (s)
        {
            case Symbol.Up: return 'W';
            case Symbol.Down: return 'S';
            case Symbol.Left: return 'A';
            case Symbol.Right: return 'D';
            default: return ' ';
        }
    }
    public static Symbol CharToSymbol (char c)
    {
        switch (c)
        {
            case 'W': return Symbol.Up;
            case 'A': return Symbol.Left;
            case 'S': return Symbol.Down;
            case 'D': return Symbol.Right;
            default: return Symbol.Space;
        }
    }

    private void ResetCode()
    {
        cursorX = cursorY = 0;
        code = new List<List<Symbol>>
        {
            new List<Symbol>()
        };
    }
    //private void CreateLineAt(int row)
    //{
    //    if (code.Count >= height) return;
    //    code.Insert(row, new List<Symbol>());
    //}

    private void SaveSpellCode()
    {
        _savedCode[curID] = code;
    }
    private void SelectSpellCode(int id)
    {
        if (id == curID) return;
        SaveSpellCode();
        code = _savedCode[id];
        curID = id;
        cursorY = code.Count - 1;
        cursorX = _line.Count;
    }
    private void OnSpellSelected(PlayerInputManager.SelectData data)
    {
        SelectSpellCode(data.id);
    }

    /// <summary>
    /// Creates line under the cursor, moves cursor to this line.
    /// </summary>
    public void CreateLine()
    {
        if (code.Count >= height) return;
        code.Insert(cursorY + 1, new List<Symbol>());
        cursorY += 1;
        cursorX = 0;
    }
    public void WriteSymbol(Symbol symbol)
    {
        if (cursorX >= code[cursorY].Count)
        {
            if (cursorX >= width)
            {
                // go to next line
                if (code.Count < height)
                {
                    CreateLine();

                    code[cursorY].Add(symbol);
                    cursorX = code[cursorY].Count;
                }
            }
            else
            {
                code[cursorY].Add(symbol);
                cursorX = code[cursorY].Count;
            }
            
        }
        else if (_line.Count < width)
        {
            _line.Insert(cursorX, symbol);
            cursorX += 1;
        }
    }
    public void EraseSymbol()
    {
        if(cursorX == 0)
        {
            if(_line.Count > 0)
            {
                _line.RemoveAt(0);
            }
            else if (cursorY >= 1)
            {
                MoveLeft();
                code.RemoveAt(cursorY + 1);
            }
        }
        else
        {
            _line.RemoveAt(cursorX - 1);
            cursorX -= 1;
        }
    }
    public void EraseLine()
    {
        // TODO
    }

    public void InsertCommand(string command)
    {
        if (cursorX == 0 && _line.Count == 0)
        {
            // stay there
        }
        else if(code.Count < height)
        {
            CreateLine();
        }
        else
        {
            return; // cant fit
        }

        foreach(char c in command) {
            if (char.IsWhiteSpace(c)) continue;
            Symbol s = CharToSymbol(c);
            if (s == Symbol.Space) continue;

            WriteSymbol(s);
        }
    }
    public void MoveLeft()
    {
        if(cursorX == 0)
        {
            if (cursorY == 0) return;
            cursorY -= 1;
            cursorX = code[cursorY].Count;
        }
        else
        {
            cursorX--;
        }
    }
    public void MoveRight()
    {
        if (cursorX == code[cursorY].Count)
        {
            if (cursorY + 1 == code.Count) return;
            cursorY += 1;
            cursorX = 0;
        }
        else
        {
            cursorX++;
        }
    }
    private void MoveVertical(int dy)
    {
        cursorY = Mathf.Clamp(cursorY + dy, 0, code.Count - 1);
        cursorX = Mathf.Min(code[cursorY].Count, cursorX);
    }
    public void MoveUp()
    {
        MoveVertical(-1);
    }
    public void MoveDown()
    {
        MoveVertical(1);
    }
    //private void UpdateRenderedCode()
    //{
    //    _consoleRender.UpdateCode();
    //}
}