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

        // 바인딩 데이터 불러오기 불러오면서 애들 바꿔주기도 해야함
        if (PlayerPrefs.HasKey("bindInfo"))
        {
            Debug.Log("데이터 불러옴");
            _playerAction.InputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString("bindInfo"));
        }
    }

    private void OnEnable()
    {
        menu = _uiDocument.rootVisualElement.Q<VisualElement>("MenuBox");
        Debug.Log(menu);

        /*menu.RegisterCallback<ClickEvent>(evt =>
        {
            // 이벤트의 Target 은 해당 이벤트를 발생시킨 얘
            // CurrentTarget 은 해당 이벤트를 처리하고 있는 얘
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
                quene = quene.WithControlsExcluding("Mouse");       // 키가 지금 파이어가 아닐 경우엔 마우스 컨트롤을 제외한 것만 받아온다. 마우스 입력이 되어도 캔슬은 되지 않음
            }
            quene.WithTargetBinding(label.IndexData)
                .WithCancelingThrough("<keyboard>/escape")      // esc 누르면 캔슬
                .OnComplete(op =>       // 바인딩 성공
                {
                    label.text = op.selectedControl.name;
                    Debug.Log($"{label}, {op.selectedControl.name}");

                    op.Dispose();
                    _playerAction.InputAction.Enable();
                })
                .OnCancel(op =>     // 실패
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
        Debug.Log("오픈");
    }

    private void CloseWindow()
    {
        //menu.RemoveFromClassList("open");
        menu.visible = false;
        menu.style.opacity = 0;
        _playerAction?.InputAction.Player.Enable();
        Debug.Log("닫음");

        // 바인딩 하기 전으로 돌아가게 만들어야 함
        _playerAction.InputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString("bindInfo"));
    }

    private void KeySave()
    {
        Debug.Log("제이슨 저장, PlayerPrefs 사용해서!");
        var rebindInfo = _playerAction.InputAction.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("bindInfo", rebindInfo);
        //Debug.Log(PlayerPrefs.GetString("bindInfo"));
        //Debug.Log(rebindInfo);
       // _playerAction.InputAction.LoadBindingOverridesFromJson(rebindInfo);
        _playerAction.InputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString("bindInfo"));
    }
}
