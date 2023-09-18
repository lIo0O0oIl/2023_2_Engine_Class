using System.Collections.Generic;
using UnityEngine;

namespace BTVisual
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> children = new List<Node>();        // 저거 하이드 빼서 정렬하는거 볼 수 있음

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}
