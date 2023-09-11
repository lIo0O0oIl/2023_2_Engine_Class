using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlasticGui.LaunchDiffParameters;

namespace BTVisual
{
    [CreateAssetMenu(menuName ="BehaviourTree/Tree")]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.RUNNING;

        public List<Node> nodes = new List<Node>();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.RUNNING)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }

        public Node CreateNode(System.Type type)
        {
            var node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);     // ������� ��Ʈ�� ����Ʈ�� �ִ´�.

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            // �θ� �������� ���� �۵��� �޸� �ؾ���
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.child = child;
                return;
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.children.Add(child);
                return;
            }

            var rootNode = parent as RootNode;
            if (rootNode != null)
            {
                rootNode.child = child;
            }

            // �ٸ� ���� �ڽ��� ���⿡ ����
        }

        public void RemoveChild(Node parent, Node child)
        {
            var decorator = parent as DecoratorNode;
            if (decorator != null)
            {
                decorator.child = null;
                return;
            }

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                composite.children.Remove(child);
                return;
            }

            var rootNode = parent as RootNode;
            if (rootNode != null)
            {
                rootNode.child = null;
                return;
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            var composite = parent as CompositeNode;
            if (composite != null)
            {
                return composite.children;
            }

            var decorator = parent as DecoratorNode;
            if (decorator != null && decorator.child != null)
            {
                children.Add(decorator.child);
            }

            var rootNode = parent as RootNode;
            if (rootNode != null && rootNode.child != null)
            {
                children.Add(rootNode.child);
            }

            return children;
        }
    }
}
