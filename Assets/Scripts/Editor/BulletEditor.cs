using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BulletScript))]
public class BulletEditor : Editor
{

    bool showBulletSettings = true;

    public override void OnInspectorGUI()
    {
        BulletScript bullet = (BulletScript)target;

        showBulletSettings = EditorGUILayout.Foldout(showBulletSettings, "Bullet Settings");
        if (showBulletSettings)
        {
            EditorGUI.indentLevel = 1;

            bullet.bMass = (EditorGUILayout.FloatField(new GUIContent("Projectile mass", "Mass of Projectile in grams"), bullet.bMass));
        }
    }

}
