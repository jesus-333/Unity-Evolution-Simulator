using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Zone))]
public class customInspector : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();

        Zone zone_script = (Zone)target;

        if(GUILayout.Button("Save current zone")){
            zone_script.saveZone();
        }

    }
}
