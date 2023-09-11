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
            Insert(0, new GridBackground());        // 0번째 자식에 저걸 넣어줘라

            // 매뉴퓰레이터 (드래그 이벤트 -> 마우스 다운, 무브, 업 이벤트 이렇게 3개를 합쳐서 드래그 메뉴퓰레이터라고 함.)

            this.AddManipulator(new ContentZoomer());      // 줌기능
            this.AddManipulator(new ContentDragger());  // 컨탠츠 드래그 가능
            this.AddManipulator(new SelectionDragger());    // 선택해준거 움직이기
            this.AddManipulator(new RectangleSelector());   // 네모 만들어주기
        }

        public void PopulateView(BehaviourTree tree)
        {
            _tree = tree;
            graphViewChanged -= OnGraphViewChanged;     // 이전 이벤트 제거

            DeleteElements(graphElements);      // 기존에 그려졌던 애들 전부 삭제
            
            graphViewChanged += OnGraphViewChanged;     // 다시 구독

            if (_tree.rootNode == null)     // 없으면 자동으로 만들어주기
            {
                _tree.rootNode = CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(_tree);      // 에디터에서 다시 그려
                AssetDatabase.SaveAssets();
            }

           tree.nodes.ForEach(n => CreateNodeView(n));

            // 엣지 그려주는 것
            tree.nodes.ForEach(n =>
            {
                var children = tree.GetChildren(n);
                NodeView parent = FindNodeView(n);
                children.ForEach(c =>
                {
                    NodeView child = FindNodeView(c);       // 자식 가져와서
                    // 연결 시작
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
                        _tree.DeleteNode(nv.node);      // 실제 SO 에서도 삭제해라
                    }

                    var edge = elem as Edge;
                    if (edge != null)       // 연결선이 삭제된거다.
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

        private Node CreateNode(Type t)     // 노드 만들기
        {
            Node node = _tree.CreateNode(t);
            CreateNodeView(node);
            return node;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)       // 우클릭 했을 때 나오는 메뉴들
        {
            if (_tree == null)
            {
                evt.StopPropagation();      // 이벤트 전파 금지를 걸고
                return;
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();    // 상속받은 모든 타입을 가져오기
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}", (a) => { CreateNode(t); });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();    // 상속받은 모든 타입을 가져오기
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}", (a) => { CreateNode(t); });
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();    // 상속받은 모든 타입을 가져오기
                foreach (var t in types)
                {
                    evt.menu.AppendAction($"[{t.BaseType.Name}] / {t.Name}", (a) => { CreateNode(t); });
                }
            }
        }

        // 드래깅이 시작될 때 연결가능한 포트를 가져오는 함수
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(x => x.direction != startPort.direction && x.node != startPort.node).ToList();      // 방향이 다르고 나랑 똑같은게 아닌
        }
    }
}