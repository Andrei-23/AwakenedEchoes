using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SpellWriteInput : MonoBehaviour
{
    [SerializeField] private Console _console;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private ConsoleRender _consoleRender;

    private InputAction _actionWriteUp;
    private InputAction _actionWriteDown;
    private InputAction _actionWriteLeft;
    private InputAction _actionWriteRight;
    private InputAction _actionComplete;
    private InputAction _actionNavigate;
    private InputAction _actionNewLine;
    private InputAction _actionErase;
    private InputAction _actionBack;

    private void Awake()
    {
        _actionWriteUp = _playerInput.actions["UpKey"];
        _actionWriteDown = _playerInput.actions["DownKey"];
        _actionWriteLeft = _playerInput.actions["LeftKey"];
        _actionWriteRight = _playerInput.actions["RightKey"];
        _actionComplete = _playerInput.actions["CompleteSpell"];
        _actionNavigate = _playerInput.actions["ConsoleNavigate"];
        _actionNewLine = _playerInput.actions["NewLine"];
        _actionErase = _playerInput.actions["Erase"];
        _actionBack = _playerInput.actions["Back"];
    }

    private void Start()
    {
        //Debug.Log(_playerInput.actions["UpKey"].enabled);
    }
    private void OnEnable()
    {
        _actionWriteUp.performed += WriteUp;
        _actionWriteDown.performed += WriteDown;
        _actionWriteLeft.performed += WriteLeft;
        _actionWriteRight.performed += WriteRight;
        _actionNavigate.performed += Navigate;
        _actionNewLine.performed += NewLine;
        _actionErase.performed += Erase;
    }
    private void OnDisable()
    {
        _actionWriteUp.performed -= WriteUp;
        _actionWriteDown.performed -= WriteDown;
        _actionWriteLeft.performed -= WriteLeft;
        _actionWriteRight.performed -= WriteRight;
        _actionNavigate.performed -= Navigate;
        _actionNewLine.performed -= NewLine;
        _actionErase.performed -= Erase;
    }

    private void UpdateCodeRender()
    {
        _consoleRender.UpdateCode();
    }
    private void WriteSymbol(Console.Symbol symbol)
    {
        Debug.Log(_playerInput.actions["UpKey"].enabled);
        _console.WriteSymbol(symbol);
        UpdateCodeRender();
    }
    private void WriteUp(InputAction.CallbackContext c) { WriteSymbol(Console.Symbol.Up); }
    private void WriteDown(InputAction.CallbackContext c) { WriteSymbol(Console.Symbol.Down); }
    private void WriteLeft(InputAction.CallbackContext c) { WriteSymbol(Console.Symbol.Left); }
    private void WriteRight(InputAction.CallbackContext c) { WriteSymbol(Console.Symbol.Right); }

    private void Navigate(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        if (dir == Vector2.zero) return;
        if(dir.x != 0 && dir.y != 0)
        {
            Debug.LogWarning("Incorrect navigation direction");
        }
        if(dir.x == 0)
        {
            if (dir.y == 1) _console.MoveUp();
            else _console.MoveDown();
        }
        else
        {
            if (dir.x == 1) _console.MoveRight();
            else _console.MoveLeft();
        }
        UpdateCodeRender();
    }

    private void NewLine(InputAction.CallbackContext c) {
        _console.CreateLine();
        UpdateCodeRender();
    }
    private void Erase(InputAction.CallbackContext c) {
        _console.EraseSymbol();
        UpdateCodeRender();
    }
}
