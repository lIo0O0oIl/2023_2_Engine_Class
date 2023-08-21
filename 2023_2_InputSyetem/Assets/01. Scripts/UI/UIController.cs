using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private UIDocument _uiDocument;

    [SerializeField] private TestScenePlayerInput _playerAction;
    private Dictionary<string, InputAction> _inputMap;

    private VisualElement menu;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void Start()
    {
        _inputMap = new Dictionary<string, InputAction>();
        _inputMap.Add("Fire", _playerAction.InputAction.Player.Fire);
        _inputMap.Add("Jump", _playerAction.InputAction.Player.Jump);
        _inputMap.Add("Movement", _playerAction.InputAction.Player.Movement);

        OpenWindow();
    }

    private void OnEnable()
    {
        menu = _uiDocument.rootVisualElement.Q("MenuBox");

        /*menu.RegisterCallback<ClickEvent>(evt =>
        {
            // �̺�Ʈ�� Target �� �ش� �̺�Ʈ�� �߻���Ų ��
            // CurrentTarget �� �ش� �̺�Ʈ�� ó���ϰ� �ִ� ��
            //Debug.Log(evt.target);

            var label = evt.target as UILabelWithData;
            if (label != null)
            {
                Debug.Log(label.KeyData);
                Debug.Log(label.IndexData);
                label.text = "Listening...";
                _playerAction.InputAction.Player.Disable();
                _inputMap[label.KeyData].PerformInteractiveRebinding()
                .OnComplete(op =>
                {
                    Debug.Log(op.selectedControl.name);
                    //label.text = op.selectedControl.name;
                });
            }
        });*/

        menu.RegisterCallback<ClickEvent>(HandleKeyBindClick);


    _uiDocument.rootVisualElement.Q<Button>("BtnCancel").RegisterCallback<ClickEvent>(evt =>
    {
        CloseWindow();
    });

        //CloseWindow();
    }

    private void HandleKeyBindClick(ClickEvent evt)
    {
        var label = evt.target as UILabelWithData;
        if (label == null) return;

        var oldText = label.text;
        label.text = "Listening...";
        if (_inputMap.TryGetValue(label.KeyData, out InputAction action))
        {
            var quene = action.PerformInteractiveRebinding();
            if (label.KeyData != "Fire")
            {
                quene = quene.WithControlsExcluding("Mouse");
            }
            quene.WithTargetBinding(label.IndexData)
                .WithCancelingThrough("<keyboard>/escape")
                .OnComplete(op =>
                {
                    label.text = op.selectedControl.name;
                    op.Dispose();
                     Debug.Log("��");
                    _playerAction.InputAction.Enable();
                })
                .OnCancel(op =>
                {
                    label.text = oldText;
                    op.Dispose();
                })
            .Start();
        }
        else
        {
            label.text = oldText;
        }
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasReleasedThisFrame)
        {
           // _playerAction.InputAction.Dispose();
           OpenWindow();
        }

        /*if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            _inputAction.Disable();
            if (_uiMove == false)
            {
                _uiDocument.SetActive(true);
                _inputAction.UI.Enable();
            }
            else
            {
                _uiDocument.SetActive(false);
                _inputAction.Player.Enable();
            }

            _uiMove = !_uiMove;
        }*/
    }

    private void OpenWindow()
    {
        menu.AddToClassList("Open");
        _playerAction.InputAction.Player.Disable();
        Debug.Log("����");
    }

    private void CloseWindow()
    {
        menu.RemoveFromClassList("open");
        _playerAction?.InputAction.Player.Enable();
        Debug.Log("����");
    }
}
