using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(W_Gun))]
public class GunEditor : Editor
{
	bool showGunSettings = true;
	bool showProjectileSettings = false;
	bool showOtherSettings = false;
	bool showDebugSettings = false;

	public override void OnInspectorGUI()
	{
		W_Gun gun = (W_Gun)target;

		showGunSettings = EditorGUILayout.Foldout(showGunSettings, "Gun Settings");
		if (showGunSettings)
		{
			EditorGUI.indentLevel = 1;

			gun.FireRate_Type = (W_Gun.FireRateTypes)EditorGUILayout.EnumPopup("FireRate Type", gun.FireRate_Type);
			gun.FireRate = EditorGUILayout.FloatField(new GUIContent("FireRate", "Minimum time between each shot"), gun.FireRate);
			gun.Fire_Point = (Transform)EditorGUILayout.ObjectField("Fire Point", gun.Fire_Point, typeof(Transform), true);
            gun.Damage = EditorGUILayout.FloatField(new GUIContent("Damage", "Damage per shot"), gun.Damage);
            gun.AmmoPerShot = EditorGUILayout.IntField(new GUIContent("AmmoPerShot", "Ammunition used per shot"), gun.AmmoPerShot);
            gun.ClipSize = EditorGUILayout.IntField(new GUIContent("ClipSize", "Maximum bullets loaded"), gun.ClipSize);
            gun.ReloadTime = EditorGUILayout.FloatField(new GUIContent("ReloadTime", "Time used for gun to reload in seconds"), gun.ReloadTime);


            EditorGUI.indentLevel = 0;
		}

		showProjectileSettings = EditorGUILayout.Foldout(showProjectileSettings, "Projectile Settings");
		if (showProjectileSettings)
		{
			EditorGUI.indentLevel = 1;

			gun.Projectile_Force = EditorGUILayout.FloatField(new GUIContent("Projectile Force", "Projectile force when shot"), gun.Projectile_Force);
			gun.Projectile_Range = EditorGUILayout.FloatField("Projectile Range", gun.Projectile_Range);
			gun.Projectile_Type = (W_Gun.ProjectileTypes)EditorGUILayout.EnumPopup("Projectile Type", gun.Projectile_Type);
			EditorGUI.indentLevel = 2;
			switch (gun.Projectile_Type)
			{
				case W_Gun.ProjectileTypes.Raycast:

					break;
				case W_Gun.ProjectileTypes.Prefab:
					gun.prefab_projectile = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", gun.prefab_projectile, typeof(GameObject), true);
					gun.Parent = EditorGUILayout.Toggle("Parent", gun.Parent);
					if (gun.Parent)
						gun.prefab_parent = (Transform)EditorGUILayout.ObjectField("Parent Transform", gun.prefab_parent, typeof(Transform), true);
					break;
				default:
					break;
			}
			EditorGUI.indentLevel = 1;
			EditorGUI.indentLevel = 0;
			if (GUI.changed)
				SceneView.RepaintAll();
		}

		showOtherSettings = EditorGUILayout.Foldout(showOtherSettings, "Other Settings");
		if (showOtherSettings)
		{
			EditorGUI.indentLevel = 1;
			gun.CustomCrossHair = EditorGUILayout.Toggle("Custom Crosshair", gun.CustomCrossHair);
			if (gun.CustomCrossHair)
			{
				gun.CrossHair = (Sprite)EditorGUILayout.ObjectField("Crosshair Sprite", gun.CrossHair, typeof(Sprite), true);
			}
			EditorGUI.indentLevel = 0;
		}

		showDebugSettings = EditorGUILayout.Foldout(showDebugSettings, "Debug Settings");
		if (showDebugSettings)
		{
			if (gun.Projectile_Type == W_Gun.ProjectileTypes.Raycast)
			{
				EditorGUI.indentLevel++;
				gun.raycast_show = EditorGUILayout.Toggle(new GUIContent("Show Raycast", "Show raycast (in inspector)"), gun.raycast_show);
				EditorGUI.BeginDisabledGroup(!gun.raycast_show);
				gun.raycast_Color = EditorGUILayout.ColorField("Raycast Color", gun.raycast_Color);
				EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel--;
			}
		}

	}

}