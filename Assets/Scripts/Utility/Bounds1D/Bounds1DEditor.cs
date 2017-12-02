using UnityEngine;
using UnityEditor;
using System.Collections;

// Author: Pierre CAMILLI

[CanEditMultipleObjects]
[CustomPropertyDrawer(typeof(Bounds1D))]
public class Bounds1DEditor : PropertyDrawer {

    SerializedProperty Min, Max, Restrict;
    string name;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // get the name before it's gone
        name = property.displayName;

        // get the Min and Max values
        property.NextVisible(true);
        Min = property.Copy();
        property.NextVisible(true);
        Max = property.Copy();
        property.NextVisible(true);
        Restrict = property.Copy();

        Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));

        if(position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18f;
        }

        int nbParts = 3;
        float partWidth = contentPosition.width / nbParts;
        GUI.skin.label.padding = new RectOffset(6, 3, 6, 6);

        // show the Min and Max
        EditorGUIUtility.labelWidth = 28f;
        contentPosition.width *= 1f / nbParts;
        EditorGUI.indentLevel = 0;

        // Begin/end property & change check make each field
        // Behave correctly when multi-object editing.
        // MIN
        EditorGUI.BeginProperty(contentPosition, label, Min);
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(contentPosition, new GUIContent("Min"), Min.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                Min.floatValue = MinValue(value);
            }
        }
        EditorGUI.EndProperty();

        contentPosition.x += partWidth;

        // MAX
        EditorGUI.BeginProperty(contentPosition, label, Max);
        {
            EditorGUI.BeginChangeCheck();
            float value = EditorGUI.FloatField(contentPosition, new GUIContent("Max"), Max.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                Max.floatValue = MaxValue(value);
            }
        }
        EditorGUI.EndProperty();

        EditorGUIUtility.labelWidth = 64f;
        contentPosition.x += partWidth;

        EditorGUI.indentLevel++;

        // RESTRICT
        EditorGUI.BeginProperty(contentPosition, label, Restrict);
        {
            EditorGUI.BeginChangeCheck();
            bool value = EditorGUI.Toggle(contentPosition, new GUIContent("Restrict"), Restrict.boolValue);
            if (EditorGUI.EndChangeCheck())
            {
                Restrict.boolValue = value;
            }
        }
        EditorGUI.EndProperty();
    }

    float MinValue(float value)
    {
        if (Max.floatValue < value)
            return (Restrict.boolValue ? Max.floatValue : Max.floatValue = value);
        else
            return value;
    }

    float MaxValue(float value)
    {
        if (Min.floatValue > value)
            return (Restrict.boolValue ? Min.floatValue : Min.floatValue = value);
        else
            return value;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return Screen.width < 333 ? (16f + 18f) : 16f;
    }

}
