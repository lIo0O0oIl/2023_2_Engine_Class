using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace BTVisual
{
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
        public new class UxmlTraits : GraphView.UxmlTraits { }

        public BehaviourTreeView() 
        {
            Insert(0, new GridBackground());        // 0��° �ڽĿ� ���� �־����

            // �Ŵ�ǽ������ (�巡�� �̺�Ʈ -> ���콺 �ٿ�, ����, �� �̺�Ʈ �̷��� 3���� ���ļ� �巡�� �޴�ǽ�����Ͷ�� ��.)

            this.AddManipulator(new ContentZoomer());      // �ܱ��
            this.AddManipulator(new ContentDragger());  // ������ �巡�� ����
            this.AddManipulator(new SelectionDragger());    // �������ذ� �����̱�
            this.AddManipulator(new RectangleSelector());   // �׸� ������ֱ�
        }
    }
}