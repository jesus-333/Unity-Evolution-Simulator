using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Sum the letter of a string as numbers
    */
    public static int addStringLetter(string s){
        int tot = 0;
        for(int i = 0; i < s.Length; i++){
            tot += CharToIntLowerCase(s[i]);
        }

        return tot;
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

    public static string generateRandomString(int str_length){
        string random_str = "";
        float tmp_char = 0f, type_of_char = 0f;

        for(int i = 0; i < str_length; i++){ // Generate upper case letter
            type_of_char = UnityEngine.Random.Range(0, 1f);
            if(type_of_char % 3 == 0){
                tmp_char = (int)UnityEngine.Random.Range(65f, 90f);
            } else if(type_of_char % 3 == 1) { // Generate lower case letter
                tmp_char = (int)UnityEngine.Random.Range(97f, 122f);
            } else { // Generate a number
                tmp_char = (int)UnityEngine.Random.Range(0f, 9f);
            }

            random_str += (char)tmp_char;
        }

        return random_str;
    }

    // Create a 2d array with specified dimension and fill it with random value
    public static float[,] fill2dArray(int n_rows, int n_cols, float min_value = 1f, float max_value = 1f){
        // Create the 2d Array
        float tmp_mat = new float[n_rows, n_cols];

        for(int i = 0; i < n_rows; i++){
            for(int j = 0; j < n_cols; j++){
                tmp_mat[i, j] = UnityEngine.Random.Range(min_value, max_value);
            }
        }

        return tmp_mat;
    }
}
