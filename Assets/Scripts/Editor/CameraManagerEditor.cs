using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Experimental.AI;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
	bool showDefaultViewSettings = true;
	bool showDefaultAdvanced = true;

	bool showAimViewSettings = true;

	bool showDebugSettings = true;

	bool showInfo = true;

	SerializedProperty YClampProperty;
	SerializedProperty distanceToPlayerProperty;
	SerializedProperty camDistanceToPlayerProperty;
	SerializedProperty canMoveCamProperty;

	SerializedProperty aimClampProperty;
	SerializedProperty layersToCheckProperty;

	private void OnEnable()
	{
		canMoveCamProperty = serializedObject.FindProperty("canMoveCam");

		distanceToPlayerProperty = serializedObject.FindProperty("distanceToPlayer");

		layersToCheckProperty = serializedObject.FindProperty("layersToCheck");

		YClampProperty = serializedObject.FindProperty("yClamp");

		camDistanceToPlayerProperty = serializedObject.FindProperty("camDistanceToPlayer");

		aimClampProperty = serializedObject.FindProperty("aimYClamp");


	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		CameraManager CM = (CameraManager)target;

		showDefaultViewSettings = EditorGUILayout.Foldout(showDefaultViewSettings, "Default View Settings");
		if (showDefaultViewSettings)
		{
			EditorGUI.indentLevel++;

			CM.sens.x = EditorGUILayout.FloatField(new GUIContent("X Sensitivity", "Horizontal Aim Sensitivity"), CM.sens.x);
			CM.sens.y = EditorGUILayout.FloatField(new GUIContent("Y Sensitivity", "Vertical Aim Sensitivity"), CM.sens.y);
			EditorGUILayout.PropertyField(distanceToPlayerProperty, new GUIContent("Distance To Point", "Min and Max distance between camera and player"));
			EditorGUILayout.PropertyField(YClampProperty, new GUIContent("Y Clamp", "Clamp the Y axis when in 3D Person"));

			showDefaultAdvanced = EditorGUILayout.Foldout(showDefaultAdvanced, "Advanced Settings");
			if (showDefaultAdvanced)
			{
				EditorGUI.indentLevel++;

				CM.smooth = EditorGUILayout.FloatField(new GUIContent("Smooth"), CM.smooth);
				CM.sprintCamOffset = EditorGUILayout.FloatField(new GUIContent("Sprint Cam Offset", "Amount to offset the cam when sprinting"), CM.sprintCamOffset);
				CM.distanceBeforeColliding = EditorGUILayout.Slider(new GUIContent("Collision Distance", "Distance to an object before colliding"), CM.distanceBeforeColliding, 0f, 1f);
				EditorGUILayout.PropertyField(layersToCheckProperty, new GUIContent("Layers To Collide", "Layers the cameara will collide with"));
				
				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

		showAimViewSettings = EditorGUILayout.Foldout(showAimViewSettings, "Aim View Settings");
		if (showAimViewSettings)
		{
			EditorGUI.indentLevel++;

			CM.aimSens.x = EditorGUILayout.FloatField(new GUIContent("X Sensitivity", "Horizontal Aim Sensitivity"), CM.aimSens.x);
			CM.aimSens.y = EditorGUILayout.FloatField(new GUIContent("Y Sensitivity", "Vertical Aim Sensitivity"), CM.aimSens.y);
			CM.aimDistanceToPlayer = EditorGUILayout.FloatField(new GUIContent("Aim Distance from point", "The distance to be from when the FollowPoint when aiming"), CM.aimDistanceToPlayer);
			EditorGUILayout.PropertyField(aimClampProperty, new GUIContent("Y Aim Clamp", "Clamp the Y axis when in Aim View"));

			EditorGUI.indentLevel--;
		}

		showDebugSettings = EditorGUILayout.Foldout(showDebugSettings, "Debug Settings");
		if (showDebugSettings)
		{
			EditorGUI.indentLevel++;

			CM.raycastDirection = EditorGUILayout.Toggle(new GUIContent("Show Direction Raycast", "Raycasts a line towards camera direction"), CM.raycastDirection);
			if (CM.raycastDirection)
			{
				EditorGUI.indentLevel++;

				CM.directionDistance = EditorGUILayout.FloatField(new GUIContent("Direction Raycast Length"), CM.directionDistance);
				CM.directionColor = EditorGUILayout.ColorField(new GUIContent("Direction Raycast Color"), CM.directionColor);

				EditorGUI.indentLevel--;
			}
			EditorGUI.indentLevel--;
		}

		showInfo = EditorGUILayout.Foldout(showInfo, "Info");
		if (showInfo)
		{
			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField(camDistanceToPlayerProperty, new GUIContent("Current Distance", "Camera distance to player."));
			EditorGUILayout.PropertyField(canMoveCamProperty, new GUIContent("canMove", "Weather the player can move the camera or not."));

			EditorGUI.indentLevel--;
		}

		serializedObject.ApplyModifiedProperties();
	}
}