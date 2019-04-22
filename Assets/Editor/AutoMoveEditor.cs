using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AutoMove))]
public class AutoMoveEditor : Editor
{
    private SerializedObject autoMove;
    private SerializedProperty moveMode;
    private SerializedProperty isNormalizedDirection;
    // 锯齿型路线变换频率
    private SerializedProperty sawtoothFrequency;

    // 正弦型路线变换频率
    private SerializedProperty sineFrequency;
    // 正弦型路线最高点幅度
    private SerializedProperty sineAmplitude;

    // 波浪型路线变换频率
    private SerializedProperty billowFrequency;
    // 波浪型路线最高点幅度
    private SerializedProperty billowAmplitude;

    private void OnEnable()
    {
        autoMove = new SerializedObject(target);

        moveMode = autoMove.FindProperty("moveMode");
        isNormalizedDirection = autoMove.FindProperty("isNormalizedDirection");

        sawtoothFrequency = autoMove.FindProperty("sawtoothFrequency");
        sineFrequency = autoMove.FindProperty("sineFrequency");
        sineAmplitude = autoMove.FindProperty("sineAmplitude");
        billowFrequency = autoMove.FindProperty("billowFrequency");
        billowAmplitude = autoMove.FindProperty("billowAmplitude");

    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        autoMove.Update();

        EditorGUILayout.PropertyField(moveMode);
        switch (moveMode.enumValueIndex)
        {
            case 0:
                break;
            case 1:
                EditorGUILayout.PropertyField(sawtoothFrequency);
                EditorGUILayout.PropertyField(isNormalizedDirection);
                break;
            case 2:
                EditorGUILayout.PropertyField(sineFrequency);
                EditorGUILayout.PropertyField(sineAmplitude);
                EditorGUILayout.PropertyField(isNormalizedDirection);
                break;
            case 3:
                EditorGUILayout.PropertyField(billowFrequency);
                EditorGUILayout.PropertyField(billowAmplitude);
                EditorGUILayout.PropertyField(isNormalizedDirection);
                break;
        }

        autoMove.ApplyModifiedProperties();
    }
}
