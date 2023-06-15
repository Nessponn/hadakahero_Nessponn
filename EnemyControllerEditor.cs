using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Nessponn
{
    
    [CustomEditor(typeof(EnemyController))]
    public class EnemyControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EnemyController EC = target as EnemyController;

            SerializedProperty colorTypeProperty = serializedObject.FindProperty("Enemy_Type");

            colorTypeProperty.enumValueIndex = EditorGUILayout.Popup("敵のタイプ", colorTypeProperty.enumValueIndex, new string[] { "近距離タイプ", "遠距離タイプ" });

            serializedObject.ApplyModifiedProperties();

            EC.Speed = EditorGUILayout.FloatField("移動速度", EC.Speed);
            EC.Power = EditorGUILayout.IntSlider("攻撃力", EC.Power, 0,1000);
            EC.Attack_Fre = EditorGUILayout.FloatField("攻撃速度", EC.Attack_Fre);
            EC.Attack_Mag = EditorGUILayout.FloatField("強攻撃の倍率", EC.Attack_Mag);
            EC.Search_Range = EditorGUILayout.FloatField("プレイヤーとの間合いの距離", EC.Search_Range);

            EC.Punch = EditorGUILayout.ObjectField("パンチ判定オブジェクト", EC.Punch, typeof(GameObject), true) as GameObject;

            if(EC.Enemy_Type == EnemyController.Type.Gunner)
            {
                EditorGUI.indentLevel++;
                EC.BulletSpeed = EditorGUILayout.FloatField("弾速", EC.BulletSpeed);

                EC.BulletSpeed_Mag = EditorGUILayout.FloatField("強攻撃の弾速", EC.BulletSpeed_Mag);
                EditorGUI.indentLevel--;
            }

            EditorUtility.SetDirty(EC);
        }
    }
    
}
