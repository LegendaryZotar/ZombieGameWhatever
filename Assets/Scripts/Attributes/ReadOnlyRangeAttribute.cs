using System;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class ReadOnlyRangeAttribute : PropertyAttribute
{
	public readonly float min;
	public readonly float max;

	public ReadOnlyRangeAttribute(float min, float max)
	{
		this.min = min;
		this.max = max;
	}
}

[CustomPropertyDrawer(typeof(ReadOnlyRangeAttribute))]
public class ReadOnlyRangeDrawer : PropertyDrawer
{
    private float value;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        var ReadOnlyRange = (ReadOnlyRangeAttribute)base.attribute;
		
        if(property.propertyType == SerializedPropertyType.Float)
		{
            value = EditorGUI.Slider(position, label, property.floatValue, ReadOnlyRange.min, ReadOnlyRange.max);
            value = (float)Math.Round(value, 2);
            property.floatValue = value;
        }

        GUI.enabled = true;
    }
}
