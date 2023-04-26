using UnityEditor;

/// <summary>
/// Populates the "keys" array form the Controller class with all the values from the OculusButton enum, and sets the "values" array to the same size.
/// </summary>
[CustomEditor(typeof(OculusButtonHighlighter))]
public class OculusButtonHighLighterEditor : Editor
{
    private SerializedProperty keysProperty;
    private SerializedProperty valuesProperty;

    private void OnEnable()
    {
        keysProperty = serializedObject.FindProperty("keys");
        valuesProperty = serializedObject.FindProperty("values");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Ensure that the target object is of type Controller
        if (!(target is OculusButtonHighlighter controller)) return;

        // Populate the keys array with all elements from the OculusButton enum
        var enumValues = System.Enum.GetValues(typeof(OculusButton));
        keysProperty.arraySize = enumValues.Length;
        valuesProperty.arraySize = enumValues.Length;
        for (int i = 0; i < enumValues.Length; i++)
        {
            keysProperty.GetArrayElementAtIndex(i).enumValueIndex = (int)enumValues.GetValue(i);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
