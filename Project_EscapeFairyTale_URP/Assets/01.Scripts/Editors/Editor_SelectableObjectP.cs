using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelectableObject_Parent))]
public class Editor_SelectableObjectP : Editor
{
    SelectableObject_Parent _editor;

    void OnEnable()
    {
        // target�� ���� CustomEditor() ��Ʈ����Ʈ���� ������ �� Ÿ���� ��ü�� ���� ���۷���
        // object���̹Ƿ� ���� ����� Ÿ������ ĳ���� �� �ش�.
        _editor = target as SelectableObject_Parent;
    }

    public override void OnInspectorGUI()  //Editor���, Ŀ���ҿ����� ���� �Լ� �� ����.  
    {
        _editor.selectText = EditorGUILayout.TextField(new GUIContent("Ŀ�� �ؽ�Ʈ", "Ŀ�� �ؽ�Ʈ"), _editor.selectText);
        _editor.ignoreRaycast = EditorGUILayout.Toggle(new GUIContent("Normal �����϶� ���õ� ����", "Normal �����϶� ���õ� ����"), _editor.ignoreRaycast);
        _editor.ignoreRaycast_inSubCam = EditorGUILayout.Toggle(new GUIContent("Detail �����϶� ���õ� ����", "Detail �����϶� ���õ� ����"), _editor.ignoreRaycast_inSubCam);
        UseProperty("selectableObjects");   //�Ʒ� �Լ��� ����� �� �κ� ����
        EditorGUILayout.Space();
        _editor.pickable = EditorGUILayout.Toggle(new GUIContent("Pickable", "�ش� ������Ʈ�� �ֿ� �� �ִ°�?"), _editor.pickable);
        if (_editor.pickable) //���ǿ� ���� ǥ���ϴ� ������ �ٸ���.
        {
            _editor.itemId = EditorGUILayout.IntField(new GUIContent("Item Id", "�ֿ� �� ������ ���̵�"), _editor.itemId);
            //float�� ��� FloatField�� ����. ���ڴ� bool ��ġ�� float�� ���� �� �ܿ� ����
        }

        if (GUI.changed)    //������ ���� �� ����ȴ�. �� �ڵ尡 ������ �ν����� â���� ��ȭ�� ������ ������ ���� �ʴ´�.
            EditorUtility.SetDirty(target);
    }

    void UseProperty(string propertyName)   //�ش� ������ ������ pubilc ���·� ���
    {   //�迭�� ��� �̰����� �ҷ����� ����� ��ü������ �������� �ʴ´�. ���� ����� �ְ����� �� ����� ���� ���� ���� �״�θ� ������ �� �ִ�.
        SerializedProperty tps = serializedObject.FindProperty(propertyName);   //�������� �Է��ؼ� ã�´�.
        EditorGUI.BeginChangeCheck();   //Begin�� End�� ���� �ٲ�� ���� �˻��Ѵ�.
        EditorGUILayout.PropertyField(tps, true);   //������ �´� �ʵ� ����. ������ true�κ��� includeChildren�ν� �ڽĿ� �ش��ϴ� �κб��� ��� �����´ٴ� ���̴�.
                                                    //���� ���⼭ false�� �ϸ� ������ ��ü�� �ν����� â�� ������ �迭�׸��� �ƿ� ���� �ʾ� �̸����� �׸��� �ȴ�.
        if (EditorGUI.EndChangeCheck()) //������� �˻��ؼ� �ʵ忡 ��ȭ�� ������
            serializedObject.ApplyModifiedProperties(); //���� ������ �����Ų��.

        //������ ��� ���� ��ũ��Ʈ�� �ִ� ���� �����´�.
    }
}
