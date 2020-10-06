using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PivotModder
{

    //#pragma warning disable CS0618
#pragma warning disable
    //[CustomEditor(typeof(GameObject))]
    public class InspectorAttacher //: DecoratorEditor
    {

        //public InspectorAttacher() : base("GameObjectInspector") { }
        //private bool resetFlags = true;
        private static HideFlags oldFlags;


        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        static void DrawGizmoForMyScript(Transform scr, GizmoType gizmoType)
        {

            if (Selection.activeGameObject == null) { return; }
            if (Selection.activeTransform == null || Selection.activeTransform is RectTransform) { return; }

            if (Selection.activeGameObject.GetComponent<PivotModderHost>() != null) { return; }


            PrefabType prefabType = PrefabUtility.GetPrefabType(Selection.activeGameObject);

            /*
            if (prefabType != PrefabType.None && prefabType != PrefabType.DisconnectedModelPrefabInstance && prefabType != PrefabType.DisconnectedPrefabInstance && prefabType != PrefabType.MissingPrefabInstance)
            { 
                bool positive = EditorUtility.DisplayDialog("Prefab Instance Detected",
                    "Poly Few doesn't show up for connected prefab instances until you disconnect them. Press \"Disconnect\" to proceed with the prefab disconnection.",
                    "Disconnect",
                    "Cancel");

                if(positive) { PrefabUtility.DisconnectPrefabInstance(Selection.activeGameObject); }
            }
            */

            prefabType = PrefabUtility.GetPrefabType(Selection.activeGameObject);

            if (prefabType != PrefabType.None && prefabType != PrefabType.DisconnectedModelPrefabInstance && prefabType != PrefabType.DisconnectedPrefabInstance && prefabType != PrefabType.MissingPrefabInstance)
            { return; }


            //When this inspector is created, also create the built-in inspector
            //defaultTransformEditor = Editor.CreateEditor(Selection.activeTransform, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));

            // Attach the inspector hosting script
            if (Selection.activeGameObject != null)
            {
                //Debug.Log("Adding hosting script to gameobject  " +Selection.activeGameObject.name);

                oldFlags = Selection.activeGameObject.hideFlags;

                PivotModderHost host = Selection.activeGameObject.AddComponent(typeof(PivotModderHost)) as PivotModderHost;
                host.hideFlags = HideFlags.DontSave;
                Selection.activeGameObject.hideFlags = HideFlags.DontSave;
                
                /*
                int moveUp = Selection.activeGameObject.GetComponents<Component>().Length - 2;
  
                for (int a = 0; a < moveUp; a++)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(host);
                }
                */

                Selection.activeGameObject.hideFlags = oldFlags;
                Selection.activeGameObject.GetComponent<PivotModderHost>().hideFlags = HideFlags.DontSave;
                
            }

            else
            {
                Debug.Log("ActiveSelection is null");
            }
        }


        void OnEnable() { }

        void OnDisable() { }

        /*
        public override void OnInspectorGUI()
        {
            if (resetFlags)
            {
                resetFlags = false;

                if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<PivotModderHost>())
                {
                    Selection.activeGameObject.hideFlags = oldFlags;
                    Selection.activeGameObject.GetComponent<PivotModderHost>().hideFlags = HideFlags.DontSave;
                }

            }
        }
        */
    }


}

