using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(W_Gun))]
public class GunEditor : Editor
{
	bool showGunSettings = true;
	bool showAmmoSettings = true;
	bool showProjectileSettings = true;

	bool showOtherSettings = false;
	bool showDebugSettings = false;

	public override void OnInspectorGUI()
	{
		W_Gun gun = (W_Gun)target;

		showGunSettings = EditorGUILayout.Foldout(showGunSettings, "Gun Settings");
		if (showGunSettings)
		{
			EditorGUI.indentLevel++;

			gun.fireRateType = (W_Gun.FireRateTypes)EditorGUILayout.EnumPopup("FireRate Type", gun.fireRateType);
			gun.fireRate = EditorGUILayout.FloatField(new GUIContent("FireRate", "Minimum time between each shot in seconds"), gun.fireRate);
            gun.reloadTime = EditorGUILayout.FloatField(new GUIContent("ReloadTime", "Time used for gun to reload in seconds"), gun.reloadTime);
			gun.recoil = EditorGUILayout.FloatField(new GUIContent("Recoil"), gun.recoil);
			gun.firePoint = (Transform)EditorGUILayout.ObjectField("Fire Point", gun.firePoint, typeof(Transform), true);

            EditorGUI.indentLevel--;
		}

		showAmmoSettings = EditorGUILayout.Foldout(showAmmoSettings, "Ammo Settings");
		if (showAmmoSettings)
		{
			EditorGUI.indentLevel++;

            gun.clipSize = EditorGUILayout.IntField(new GUIContent("ClipSize", "Maximum bullets loaded"), gun.clipSize);
            gun.ammoPerShot = EditorGUILayout.IntField(new GUIContent("AmmoPerShot", "Ammunition used per shot"), gun.ammoPerShot);


			EditorGUI.indentLevel--;
		}

		showProjectileSettings = EditorGUILayout.Foldout(showProjectileSettings, "Projectile Settings");
		if (showProjectileSettings)
		{
			EditorGUI.indentLevel++;

			gun.projectType = (W_Gun.ProjectileTypes)EditorGUILayout.EnumPopup("Projectile Type", gun.projectType);

			if(gun.projectType == W_Gun.ProjectileTypes.Prefab)
				gun.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", gun.projectilePrefab, typeof(GameObject), true);

			gun.projectileForce = EditorGUILayout.FloatField(new GUIContent("Projectile Force", "Projectile force when shot"), gun.projectileForce);
			gun.projectileRange = EditorGUILayout.FloatField("Projectile Range", gun.projectileRange);
            gun.damage = EditorGUILayout.FloatField(new GUIContent("Damage", "Damage per shot"), gun.damage);

			EditorGUI.indentLevel--;
			if (GUI.changed)
				SceneView.RepaintAll();
		}

		showOtherSettings = EditorGUILayout.Foldout(showOtherSettings, "Other Settings");
		if (showOtherSettings)
		{
			EditorGUI.indentLevel++;

			gun.customCrossHairBool = EditorGUILayout.Toggle("Custom Crosshair", gun.customCrossHairBool);
			if (gun.customCrossHairBool)
			{
				EditorGUI.indentLevel++;

				gun.customCrossHair.crossHair = (Sprite)EditorGUILayout.ObjectField("Crosshair Sprite", gun.customCrossHair.crossHair, typeof(Sprite), true);
				gun.customCrossHair.tint = EditorGUILayout.ColorField("Crosshair Tint", gun.customCrossHair.tint);

				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

		showDebugSettings = EditorGUILayout.Foldout(showDebugSettings, "Debug Settings");
		if (showDebugSettings)
		{
			gun.raycastDirection = EditorGUILayout.Toggle(new GUIContent("Show Range Raycast"), gun.raycastDirection);
			if (gun.raycastDirection)
			{
				EditorGUI.indentLevel++;

				gun.directionColor = EditorGUILayout.ColorField(new GUIContent("Range Raycast Color"), gun.directionColor);

				EditorGUI.indentLevel--;
			}
		}

	}

}