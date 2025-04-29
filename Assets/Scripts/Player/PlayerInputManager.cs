using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerInputManager : MonoBehaviour
{
    public class SelectData
    {
        public int id;
        public Spell spell;
        // image?
        public SelectData(int id, Spell spell)
        {
            this.id = id;
            this.spell = spell;
        }
    }

    [Inject] private PlayerCaster _player;
    [Inject] private PlayerMovement _playerMovement;
    [Inject] private PlayerSpellManager _playerSpellManager;

    [SerializeField] private PlayerInput _playerInput;
    private InputAction _actionMove;
    private InputAction _actionRoll;
    private InputAction _actionSpell;
    private InputAction _actionLook;

    private InputAction _actionScrollSpell;
    private InputAction _actionSwitchSpell;
    private List<InputAction> _actionSelectSpell;


    private Vector2 _lookInput;
    private Vector2 _prevLookDir;
    //private bool _isMovedMouse = false;

    [SerializeField] private float _gamepadMaxLookDistance;
    [SerializeField] private float _lookVectorMinDist;

    [HideInInspector] public static event Action<int> OnSpellCasted; // index of selected spell
    [HideInInspector] public static event Action<SelectData> OnSpellSelected; // index of selected spell

    private int _currentSpellId = 0;
    private int _spellSlotCount => PlayerSpellManager.spellCount;

    private void Awake()
    {
        _prevLookDir = Vector2.right * _lookVectorMinDist;

        _actionMove = _playerInput.actions["Move"];
        _actionRoll = _playerInput.actions["Roll"];
        _actionSpell = _playerInput.actions["Fire"];
        _actionLook = _playerInput.actions["Look"];

        _actionScrollSpell = _playerInput.actions["ScrollSpell"];
        _actionSelectSpell = new List<InputAction>();
        for(int i = 0; i < _spellSlotCount; i++)
        {
            _actionSelectSpell.Add(_playerInput.actions["SelectSpell" + (i+1).ToString()]);
        }
    }

    private void OnEnable()
    {
        //_actionMove.Enable();

        //_actionSpell.Enable();
        _actionSpell.performed += Spell;

        //_actionLook.Enable();
        _actionLook.performed += OnLookUpdate;
        _actionRoll.performed += DoRoll;
        _actionLook.canceled += ctx => _lookInput = Vector2.zero;

        _actionScrollSpell.performed += CheckSpellScroll;
        _actionSelectSpell[0].performed += SelectSpell1;
        _actionSelectSpell[1].performed += SelectSpell2;
        _actionSelectSpell[2].performed += SelectSpell3;
    }
    private void OnDisable()
    {
        //_actionMove.Disable();
        //_actionSpell.Disable();
        //_actionLook.Disable();
        _actionSpell.performed -= Spell;
        _actionLook.performed -= OnLookUpdate;
        _actionRoll.performed -= DoRoll;
        _actionLook.canceled -= ctx => _lookInput = Vector2.zero;

        _actionScrollSpell.performed -= CheckSpellScroll;
        _actionSelectSpell[0].performed -= SelectSpell1;
        _actionSelectSpell[1].performed -= SelectSpell2;
        _actionSelectSpell[2].performed -= SelectSpell3;
    }

    void Start()
    {
        _playerMovement = _player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector2 moveDirection = _actionMove.ReadValue<Vector2>();
        _playerMovement.setMoveDirection(moveDirection);

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Vector2 direction = Vector2.zero;

        // Gamepad
        if (Gamepad.current != null)
        {
            //Debug.Log("Gpad");
            direction = _lookInput * _gamepadMaxLookDistance;
        }
        // Mouse (and keyboard)
        else if (Mouse.current != null)
        {
            //Debug.Log("Mous");
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 from = _player.transform.position;
            Vector2 to = Camera.main.ScreenToWorldPoint(mousePos);
            direction = to - from;
        }
        else
        {
            Debug.LogWarning("No input schemes?");
        }

        if (direction.magnitude < _lookVectorMinDist)
        {
            direction = _prevLookDir;
        }
        _prevLookDir = direction;
        _playerMovement.SetLookDirection(direction);
    }
    private void Spell(InputAction.CallbackContext context)
    {
        OnSpellCasted?.Invoke(_currentSpellId);
    }
    private void OnLookUpdate(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }
    
    private void CheckSpellScroll(InputAction.CallbackContext context)
    {
        float val = context.ReadValue<Vector2>().y;
        if (val > 0)
        {
            SelectSpellId((_currentSpellId + 1) % _spellSlotCount);
        }
        if(val < 0)
        {
            SelectSpellId((_currentSpellId + _spellSlotCount - 1) % _spellSlotCount);
        }
    }
    private void SelectSpell1(InputAction.CallbackContext context) { SelectSpellId(0); }
    private void SelectSpell2(InputAction.CallbackContext context) { SelectSpellId(1); }
    private void SelectSpell3(InputAction.CallbackContext context) { SelectSpellId(2); }
    private void SelectSpellId(int id)
    {
        _currentSpellId = id;
        Debug.Log("Selected speel " + id.ToString());
        OnSpellSelected?.Invoke(new SelectData(id, _playerSpellManager.spells[id]));
    }

    private void DoRoll(InputAction.CallbackContext context)
    {
        _playerMovement.Roll();
    }
    public int GetSlotId()
    {
        return _currentSpellId;
    }
}
