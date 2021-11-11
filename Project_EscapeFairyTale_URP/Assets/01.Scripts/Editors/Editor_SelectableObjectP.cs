using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(SelectableObject_Parent))]
public class Editor_SelectableObjectP : Editor
{
    SelectableObject_Parent _editor;

    void OnEnable()
    {
        // target은 위의 CustomEditor() 애트리뷰트에서 설정해 준 타입의 객체에 대한 레퍼런스
        // object형이므로 실제 사용할 타입으로 캐스팅 해 준다.
        _editor = target as SelectableObject_Parent;
    }

    public override void OnInspectorGUI()  //Editor상속, 커스텀에디터 구현 함수 재 정의.  
    {
        _editor.selectText = EditorGUILayout.TextField(new GUIContent("커서 텍스트", "커서 텍스트"), _editor.selectText);
        _editor.ignoreRaycast = EditorGUILayout.Toggle(new GUIContent("Normal 상태일때 무시됨 여부", "Normal 상태일때 무시됨 여부"), _editor.ignoreRaycast);
        _editor.ignoreRaycast_inSubCam = EditorGUILayout.Toggle(new GUIContent("Detail 상태일때 무시됨 여부", "Detail 상태일때 무시됨 여부"), _editor.ignoreRaycast_inSubCam);
        UseProperty("selectableObjects");   //아래 함수로 만들어 둔 부분 참조
        EditorGUILayout.Space();
        _editor.isSubCameraMove = EditorGUILayout.Toggle(new GUIContent("서브 카메라 이동", "체크되면 클릭되었을때 포지션, 로테이션 값으로 서브 카메라 전환"), _editor.isSubCameraMove);
        if (_editor.isSubCameraMove)
        {
            _editor.movePos = EditorGUILayout.Vector3Field(new GUIContent("이동 포지션", "서브 카메라가 이동할 포지션"), _editor.movePos);
            _editor.moveRot = EditorGUILayout.Vector3Field(new GUIContent("회전값", "서브 카메라가 회전할 값(Euler)"), _editor.moveRot);
        }

        if (GUI.changed)    //변경이 있을 시 적용된다. 이 코드가 없으면 인스펙터 창에서 변화는 있지만 적용은 되지 않는다.
            EditorUtility.SetDirty(target);
    }

    void UseProperty(string propertyName)   //해당 변수를 원래의 pubilc 형태로 사용
    {   //배열의 경우 이곳으로 불러오는 기능을 자체적으로 지원하지 않는다. 여러 방법이 있겠지만 이 방법을 쓰면 원래 쓰던 그대로를 가져올 수 있다.
        SerializedProperty tps = serializedObject.FindProperty(propertyName);   //변수명을 입력해서 찾는다.
        EditorGUI.BeginChangeCheck();   //Begin과 End로 값이 바뀌는 것을 검사한다.
        EditorGUILayout.PropertyField(tps, true);   //변수에 맞는 필드 생성. 인자의 true부분은 includeChildren로써 자식에 해당하는 부분까지 모두 가져온다는 뜻이다.
                                                    //만약 여기서 false를 하면 변수명 자체는 인스펙터 창에 뜨지만 배열항목이 아예 뜨지 않아 이름뿐인 항목이 된다.
        if (EditorGUI.EndChangeCheck()) //여기까지 검사해서 필드에 변화가 있으면
            serializedObject.ApplyModifiedProperties(); //원래 변수에 적용시킨다.

        //툴팁의 경우 원래 스크립트의 있는 것을 가져온다.
    }
}
#endif