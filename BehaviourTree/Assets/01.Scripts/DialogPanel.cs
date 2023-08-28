using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogPanel
{
    private Label _label;
    public string Text
    {
        get => _label.text;
        set => _label.text = value;
    }

    public DialogPanel(VisualElement root, string msg)
    {
        _label = root.Q<Label>("MessageLabel");
    }

    public void Show(bool value)
    {

    }
}
