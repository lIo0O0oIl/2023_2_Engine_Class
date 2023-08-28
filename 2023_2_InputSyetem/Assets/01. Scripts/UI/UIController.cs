using System;
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

        //OpenWindow();

        // ���ε� ������ �ҷ����� �ҷ����鼭 �ֵ� �ٲ��ֱ⵵ �ؾ���
        if (PlayerPrefs.HasKey("bindInfo"))
        {
            Debug.Log("������ �ҷ���");
            _playerAction.InputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString("bindInfo"));
        }
    }

    private void OnEnable()
    {
        menu = _uiDocument.rootVisualElement.Q<VisualElement>("MenuBox");
        Debug.Log(menu);

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

        _uiDocument.rootVisualElement.Q<Button>("BtnSave").RegisterCallback<ClickEvent>(evt =>
        {
            KeySave();
        });
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
                quene = quene.WithControlsExcluding("Mouse");       // Ű�� ���� ���̾ �ƴ� ��쿣 ���콺 ��Ʈ���� ������ �͸� �޾ƿ´�. ���콺 �Է��� �Ǿ ĵ���� ���� ����
            }
            quene.WithTargetBinding(label.IndexData)
                .WithCancelingThrough("<keyboard>/escape")      // esc ������ ĵ��
                .OnComplete(op =>       // ���ε� ����
                {
                    label.text = op.selectedControl.name;
                    Debug.Log($"{label}, {op.selectedControl.name}");

                    op.Dispose();
                    _playerAction.InputAction.Enable();
                })
                .OnCancel(op =>     // ����
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
        //menu.AddToClassList("Open");
        menu.visible = true;
        menu.style.opacity = 100;
        _playerAction.InputAction.Player.Disable();
        Debug.Log("����");
    }

    private void CloseWindow()
    {
        //menu.RemoveFromClassList("open");
        menu.visible = false;
        menu.style.opacity = 0;
        _playerAction?.InputAction.Player.Enable();
        Debug.Log("����");

        // ���ε� �ϱ� ������ ���ư��� ������ ��
        _playerAction.InputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString("bindInfo"));
    }

    private void KeySave()
    {
        Debug.Log("���̽� ����, PlayerPrefs ����ؼ�!");
        var rebindInfo = _playerAction.InputAction.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("bindInfo", rebindInfo);
        //Debug.Log(PlayerPrefs.GetString("bindInfo"));
        //Debug.Log(rebindInfo);
       // _playerAction.InputAction.LoadBindingOverridesFromJson(rebindInfo);
        _playerAction.InputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString("bindInfo"));
    }
}
