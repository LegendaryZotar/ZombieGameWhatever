using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
	bool showDefaultViewSettings = true;
	bool showDefaultAdvanced = true;

	bool showAimViewSettings = false;

	bool showInfo = false;

	SerializedProperty YClampProperty;
	SerializedProperty DistanceToPlayerProperty;
	SerializedProperty CamDistanceToPlayerProperty;

	SerializedProperty AimExitClampProperty;
	SerializedProperty AimClampProperty;
	SerializedProperty LayersToCheckProperty;
	private void OnEnable()
	{
		DistanceToPlayerProperty = serializedObject.FindProperty("DistanceToPlayer");

		LayersToCheckProperty = serializedObject.FindProperty("LayersToCheck");

		YClampProperty = serializedObject.FindProperty("YClamp");

		CamDistanceToPlayerProperty = serializedObject.FindProperty("CamDistanceToPlayer");

		AimExitClampProperty = serializedObject.FindProperty("AimExitClamp");

		AimClampProperty = serializedObject.FindProperty("AimClamp");


	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		CameraManager CM = (CameraManager)target;

		showDefaultViewSettings = EditorGUILayout.Foldout(showDefaultViewSettings, "Default View Settings");
		if (showDefaultViewSettings)
		{
			EditorGUI.indentLevel++;

			CM.XSens = EditorGUILayout.FloatField(new GUIContent("X Sensitivity", "Horizontal Aim Sensitivity"), CM.XSens);
			CM.YSens = EditorGUILayout.FloatField(new GUIContent("Y Sensitivity", "Vertical Aim Sensitivity"), CM.YSens);
			EditorGUILayout.PropertyField(DistanceToPlayerProperty, new GUIContent("Distance To Player", "Min and Max distance between camera and player"));
			EditorGUILayout.PropertyField(YClampProperty, new GUIContent("Y Clamp", "Clamp the Y axis when in 3D Person"));

			showDefaultAdvanced = EditorGUILayout.Foldout(showDefaultAdvanced, "Advanced Settings");
			if (showDefaultAdvanced)
			{
				EditorGUI.indentLevel++;
				CM.SprintCamOffset = EditorGUILayout.FloatField(new GUIContent("Sprint Cam Offset", "Amount to offset the cam when sprinting"), CM.SprintCamOffset);
				CM.Smooth = EditorGUILayout.FloatField(new GUIContent("Smooth"), CM.Smooth);
				EditorGUILayout.PropertyField(LayersToCheckProperty, new GUIContent("Layers To Collide", "Layers the cameara will collide with"));
				CM.DistanceBeforeColliding = EditorGUILayout.Slider(new GUIContent("Collision Distance", "Distance to an object before colliding"), CM.DistanceBeforeColliding, 0f, 1f);
				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

		showAimViewSettings = EditorGUILayout.Foldout(showAimViewSettings, "Aim View Settings");
		if (showAimViewSettings)
		{
			EditorGUI.indentLevel++;
			CM.XAimSens = EditorGUILayout.FloatField(new GUIContent("X Sensitivity", "Horizontal Aim Sensitivity"), CM.XAimSens);
			CM.YAimSens = EditorGUILayout.FloatField(new GUIContent("Y Sensitivity", "Vertical Aim Sensitivity"), CM.YAimSens);
			CM.YOffsetOnAim = EditorGUILayout.Slider(new GUIContent("Y Offset On Aim", "Offset the camera by this amount along the y axis when aiming."),
				CM.YOffsetOnAim, -1, 1);
			EditorGUILayout.PropertyField(AimClampProperty, new GUIContent("Y Aim Clamp", "Clamp the Y axis when in Aim View"));
			EditorGUILayout.PropertyField(AimExitClampProperty, new GUIContent("Y Aim Exit Clamp", "Clamp the Y axis when exiting Aim View"));

			EditorGUI.indentLevel--;
		}

		showInfo = EditorGUILayout.Foldout(showInfo, "Info");
		if (showInfo)
		{
			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField(CamDistanceToPlayerProperty, new GUIContent("Current Distance", "Camera distance to player."));

			EditorGUI.indentLevel--;
		}

		serializedObject.ApplyModifiedProperties();
	}
}