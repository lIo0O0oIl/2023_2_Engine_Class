using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTVisual
{
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
        public new class UxmlTraits : GraphView.UxmlTraits { }

        private BehaviourTree _tree;

        public Action<NodeView> OnNodeSelected;

        public BehaviourTreeView() 
        {
            Insert(0, new GridBackground());        // 0��° �ڽĿ� ���� �־����

            // �Ŵ�ǽ������ (�巡�� �̺�Ʈ -> ���콺 �ٿ�, ����, �� �̺�Ʈ �̷��� 3���� ���ļ� �巡�� �޴�ǽ�����Ͷ�� ��.)

            this.AddManipulator(new ContentZoomer());      // �ܱ��
            this.AddManipulator(new ContentDragger());  // ������ �巡�� ����
            this.AddManipulator(new SelectionDragger());    // �������ذ� �����̱�
            this.AddManipulator(new RectangleSelector());   // �׸� ������ֱ�
        }

        public void PopulateView(BehaviourTree tree)
        {
            _tree = tree;
            graphViewChanged -= OnGraphViewChanged;     // ���� �̺�Ʈ ����

            DeleteElements(graphElements);      // ������ �׷����� �ֵ� ���� ����
            
            graphViewChanged += OnGraphViewChanged;     // �ٽ� ����

            if (_tree.rootNode == null)     // ������ �ڵ����� ������ֱ�
            {
                _tree.rootNode = CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(_tree);      // �����Ϳ��� �ٽ� �׷�
                AssetDatabase.SaveAssets();
            }

           tree.nodes.ForEach(n => CreateNodeView(n));

            // ���� �׷��ִ� ��
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                NodeView parent = FindNodeView(n);
                children.ForEach(c =>
                {
                    NodeView child = FindNodeView(c);       // �ڽ� �����ͼ�
                    // ���� ����
                    Edge edge = parent.output.ConnectTo(child.input);
                    AddElement(edge);
                });
            });
        }

        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    var nv = elem as NodeView;
                    if (nv != null)
                    {
                        _tree.DeleteNode(nv.node);      // ���� SO ������ �����ض�
                    }

                    var edge = elem as Edge;
                    if (edge != null)       // ���ἱ�� �����ȰŴ�.
                    {
                        NodeView parent = edge.output.node as NodeView;
                        NodeView child = edge.input.node as NodeView;

                        _tree.RemoveChild(parent.node, child.node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parent = edge.output.node as NodeView;
                    NodeView child = edge.input.node as NodeView;

                    _tree.AddChild(parent.node, child.node);
                });
            }

            return graphViewChange;
        }

        private void CreateNodeView(Node n)
        {
            NodeView nv = new NodeView(n);
            nv.OnNodeSelected = OnNodeSelected;
            AddElement(nv);
        }

        private Node CreateNode(Type t)     // ��� �����
        {
            Node node = _tree.CreateNode(t);
            CreateNodeView(node);
            return node;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)       // ��Ŭ�� ���� �� ������ �޴���
        {
            if (_tree == null)
            {
                evt.StopPropagation();      // �̺�Ʈ ���� ������ �ɰ�
                return;
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();    // ��ӹ��� ��� Ÿ���� ��������
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}", (a) => { CreateNode(t); });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();    // ��ӹ��� ��� Ÿ���� ��������
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}", (a) => { CreateNode(t); });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();    // ��ӹ��� ��� Ÿ���� ��������
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}", (a) => { CreateNode(t); });
                }
            }
        }

        // �巡���� ���۵� �� ���ᰡ���� ��Ʈ�� �������� �Լ�
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(x => x.direction != startPort.direction && x.node != startPort.node).ToList();      // ������ �ٸ��� ���� �Ȱ����� �ƴ�
        }
    }
}