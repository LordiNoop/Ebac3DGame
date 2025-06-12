using UnityEditor;
using System.Linq;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public bool showFoldout;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameManager gm = (GameManager)target;

        EditorGUILayout.Space(30);
        EditorGUILayout.LabelField("StateMachine");

        if (gm.stateMachine == null) return;

        if (gm.stateMachine.CurrentState != null)
        {
            EditorGUILayout.LabelField("Current State: ", gm.stateMachine.CurrentState.ToString());
        }

        showFoldout = EditorGUILayout.Foldout(showFoldout, "Avaiable States");

        if (showFoldout)
        {
            if (gm.stateMachine.dictionaryState != null)
            {
                var keys = gm.stateMachine.dictionaryState.Keys.ToArray();
                var vals = gm.stateMachine.dictionaryState.Values.ToArray();

                for (int i = 0; i < keys.Length; i++)
                {
                    EditorGUILayout.LabelField($"{keys[i]} :: {vals[i]}");
                }
            }
        }
    }
}
