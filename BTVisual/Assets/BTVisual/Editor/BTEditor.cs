using BTVisual;
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class BTEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private BehaviourTreeView _treeView;
    private InspectorView _inspectorView;

    [MenuItem("Window/BTEditor")]
    public static void OpenWindow()
    {
        BTEditor wnd = GetWindow<BTEditor>();
        wnd.titleContent = new GUIContent("BTEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement template = m_VisualTreeAsset.Instantiate();
        template.style.flexGrow = 1;
        root.Add(template);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BTVisual/Editor/BTEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviourTreeView>("tree-view");
        _inspectorView = root.Q<InspectorView>("inspector-view");

        _treeView.OnNodeSelected += OnSelectionNodeChanged;

        OnSelectionChange();        // ������ ȣ���ؼ�
    }

    private void OnSelectionNodeChanged(NodeView nv)
    {
        _inspectorView.UpdateSelection(nv);
    }

    private void OnSelectionChange()        // ���� �������� ��
    {
        var tree = Selection.activeObject as BehaviourTree;

        if (tree != null && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()) )
        {
            _treeView.PopulateView(tree);
        }
    }
}
