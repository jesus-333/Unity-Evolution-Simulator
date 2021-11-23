using System;
using System.ComponentModel;
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

    /*
    Convert a char between a and z into a int between 0 and 25
    */
    public static int CharToIntLowerCase(char c){
        int tmp_int = (int)c;
        tmp_int = tmp_int - 97;
        return tmp_int;
    }

    /*
    Create an array of n linearly spaced valued. The first value is start and the last value is end.
    */
    public static float[] linspace(float start, float end, int n){
        // Create array
        float[] tmp_array = new float[n];

        // Evaluate step between numbers
        float step = (end - start)/(float)(n - 1);

        // Insert values
        tmp_array[0] = start;
        for(int i = 1; i < n - 1; i++){ tmp_array[i] = tmp_array[i - 1] + step; }
        tmp_array[n - 1] = tmp_array[n - 2] + step;

        return tmp_array;
    }

    public static void printObjectAttributes<T>(T obj){
        foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj)){
            Console.WriteLine(descriptor.Name);
        }
    }
}
