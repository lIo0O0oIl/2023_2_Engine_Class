using Cinemachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraControlTrigger))]
public class CameraControlEditor : Editor
{
    private CameraControlTrigger _cameraControlTrigger;

    private void OnEnable()
    {
        _cameraControlTrigger = (CameraControlTrigger)target; //Ÿ�� ��������
    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();      // �̰Ŷ� �Ʒ��� ����.
        DrawDefaultInspector();

        CustomInspectorObjects inspectorObj = _cameraControlTrigger.customInspectorObject;
        if (inspectorObj.swapCameras)
        {
            //https://docs.unity3d.com/ScriptReference/EditorGUILayout.ObjectField.html
            // allowSceneObject => ���� �ִ� �ֵ� �巡�׾ص�� �����ϵ��� �Ұų�.
            inspectorObj.cameraOnLeft
                = EditorGUILayout.ObjectField("Camera on Left",
                            inspectorObj.cameraOnLeft,
                            typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            inspectorObj.cameraOnRight
                = EditorGUILayout.ObjectField("Camera on Right",
                            inspectorObj.cameraOnRight,
                            typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if (inspectorObj.panCameraOnContact)
        {
            inspectorObj.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",
                inspectorObj.panDirection);

            inspectorObj.panDistance = EditorGUILayout.FloatField("Pan Distance", inspectorObj.panDistance);
            inspectorObj.panTime = EditorGUILayout.FloatField("Pan Time", inspectorObj.panTime);
        }

        //GUI�󿡼� ������ �����Ǿ��ٸ� ���� �׸��� ���.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_cameraControlTrigger);      // ���� �޶�����. ���������� �����ϰ� �آa!
        }
    }

}