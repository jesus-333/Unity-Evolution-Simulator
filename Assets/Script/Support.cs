using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Files used to define extra functions/class

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

/*
(NOT WORKING FOR NOW)
// Class to define an attribute read only in the inspector. Find on answer unity forum
https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
*/
// public class ReadOnlyAttribute : PropertyAttribute
// {
//
// }
//
// [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
// public class ReadOnlyDrawer : PropertyDrawer
// {
//     public override float GetPropertyHeight(SerializedProperty property,
//                                             GUIContent label)
//     {
//         return EditorGUI.GetPropertyHeight(property, label, true);
//     }
//
//     public override void OnGUI(Rect position,
//                                SerializedProperty property,
//                                GUIContent label)
//     {
//         GUI.enabled = false;
//         EditorGUI.PropertyField(position, property, label, true);
//         GUI.enabled = true;
//     }
// }

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


public static class SupportMethods{

    /*
    Convert an int between 0 and 25 into a lower case string
    */
    public static string IntToCharLowerCase(int x){
        if(x < 0 || x > 25){ throw new ArgumentException("Invalid value for the input x", nameof(x)); }

        char tmp_c;
        tmp_c = (char)(x + 97);
        return tmp_c + "";
    }

    /*
    Convert an int between 0 and 25 into a upper case string
    */
    public static string IntToCharUpperCase(int x){
        if(x < 0 || x > 25){ throw new ArgumentException("Invalid value for the input x", nameof(x)); }

        char tmp_c;
        tmp_c = (char)(x + 65);
        return tmp_c + "";
    }

    /*
    Convert an int between 0 and 51 into a upper case string
    0 to 25 will be converted in lower case while 26 to 51 in upper case
    */
    public static string IntToChar(int x){
        if(x < 0 || x > 51){ throw new ArgumentException("Invalid value for the input x", nameof(x)); }

        char tmp_c;
        if(x >= 25){ tmp_c = (char)(x + 97); }
        else{ tmp_c = (char)(x - 26 + 65); }

        return tmp_c + "";
    }
}
