using UnityEditor;
using UnityEngine;

namespace MMCFeedbacks
{
    [CustomEditor(typeof(MMCFeedbacker))]
    public class MMCFeedbackerEditor : Editor
    {
        private MMCFeedbacker _mmcFeedbacker;

        private void OnEnable()
        {
            _mmcFeedbacker = (MMCFeedbacker)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!EditorApplication.isPlaying);
            if (GUILayout.Button("Play")) _mmcFeedbacker.Play();
            if (GUILayout.Button("Stop")) _mmcFeedbacker.Stop();
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }
    }
}