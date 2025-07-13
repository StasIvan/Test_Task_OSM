using Base;
using Base.Builders;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RoadConstructorEditor : EditorWindow
    {
        private GameObject _targetParent;
        private Material _roadMaterial;
        private string _customLabel = "zurichSmall";
        private bool _isGenerateLineRenderers;

        [MenuItem("Tools/OSM/Build Spline Road")]
        public static void ShowWindow()
        {
            GetWindow<RoadConstructorEditor>("Spline Road Builder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Spline Road Network Builder", EditorStyles.boldLabel);

            _customLabel = EditorGUILayout.TextField("Name OSM file", _customLabel);
            _isGenerateLineRenderers = EditorGUILayout.Toggle("Generate Line Renderers", _isGenerateLineRenderers);
            _targetParent = (GameObject)EditorGUILayout.ObjectField("Target Parent", _targetParent, typeof(GameObject), true);
            _roadMaterial = (Material)EditorGUILayout.ObjectField("Road Material", _roadMaterial, typeof(Material), false);
            
            if (GUILayout.Button("Build Road Network"))
            {
                if (_targetParent == null || _roadMaterial == null || _customLabel == null || _customLabel == "")
                {
                    Debug.LogError("Please assign both Target Parent and Road Material.");
                    return;
                }

                SplineRoad builder = new SplineRoad(new OSMImport(_customLabel), new RoadGraphBuilder(),
                    new TransitionBuilder(), new RoadBuilder(_isGenerateLineRenderers));
                
                builder.Build(_targetParent.transform, _roadMaterial);
            }
        }
    }
}