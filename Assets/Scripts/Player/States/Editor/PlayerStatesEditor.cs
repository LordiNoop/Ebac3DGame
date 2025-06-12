using System.Linq;
using UnityEditor;

[CustomEditor(typeof(PlayerStates))]
public class PlayerStatesEditor : Editor
{
    public bool showFoldout;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerStates ps = (PlayerStates)target;

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("StateMachine");

        if (ps.stateMachine == null) return;

        if (ps.stateMachine.CurrentState != null)
        {
            EditorGUILayout.LabelField("Current State: ", ps.stateMachine.CurrentState.ToString());
        }

        showFoldout = EditorGUILayout.Foldout(showFoldout, "Avaiable States");

        if (showFoldout)
        {
            if (ps.stateMachine.dictionaryState != null)
            {
                var keys = ps.stateMachine.dictionaryState.Keys.ToArray();
                var vals = ps.stateMachine.dictionaryState.Values.ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    EditorGUILayout.LabelField($"{keys[i]} :: {vals[i]}");
                }
            }
        }
    }
}
