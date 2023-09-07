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
            Insert(0, new GridBackground());        // 0번째 자식에 저걸 넣어줘라

            // 매뉴퓰레이터 (드래그 이벤트 -> 마우스 다운, 무브, 업 이벤트 이렇게 3개를 합쳐서 드래그 메뉴퓰레이터라고 함.)

            this.AddManipulator(new ContentZoomer());      // 줌기능
            this.AddManipulator(new ContentDragger());  // 컨탠츠 드래그 가능
            this.AddManipulator(new SelectionDragger());    // 선택해준거 움직이기
            this.AddManipulator(new RectangleSelector());   // 네모 만들어주기
        }
    }
}