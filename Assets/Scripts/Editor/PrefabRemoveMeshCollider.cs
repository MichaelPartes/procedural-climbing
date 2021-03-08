using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabRemoveMeshCollider : EditorWindow
{
    List<GameObject> selectedObject;

    [MenuItem("Tools/Mesh Collider Remover")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabRemoveMeshCollider));
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove Mesh Collider", EditorStyles.boldLabel);

        //selectedObject = EditorGUILayout.ObjectField("Prefab selected", selectedObject, typeof(GameObject), false) as GameObject;
       
        if(GUILayout.Button("Remove Collider"))
        {
            selectedObject = new List<GameObject>();
            selectedObject.Add(Selection.activeObject as GameObject);

            foreach(GameObject selObj in selectedObject)
            {
                foreach(MeshCollider meshColl in selObj.transform)
                {
                    DestroyImmediate(meshColl, true);

                }
                //DestroyImmediate(selObj.GetComponent<MeshCollider>(), true);
            }
        }        
    }
}
