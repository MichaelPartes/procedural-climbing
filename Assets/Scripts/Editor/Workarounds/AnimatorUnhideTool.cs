// -----------------------------------------------------------------------
// <copyright file="AnimatorUnhideTool.cs" company="KORION Interactive GmbH">
// Copyright (c) KORION Interactive GmbH. All rights reserved.
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential.
// </copyright>
// -----------------------------------------------------------------------

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

/// <summary>
/// There is a bug in Unity where the Inspector window hides information in the Animator when you select a Transition / Node / Blendtree.
/// This script may be used to unhide these information again.
///
/// https://answers.unity.com/questions/1736606/animation-state-of-controller-not-showing-in-inspe.html?childToView=1737595#answer-1737595
///
/// How to use:
/// - Select an AnimatorController
/// - Use Tools/Animator Unhide Fix
/// The animator should now again show the details.
/// </summary>
public static class AnimatorUnhideTool
{
    [MenuItem("Tools/Animator Unhide Fix")]
    private static void UnhideFix()
    {
        UnityEditor.Animations.AnimatorController ac = Selection.activeObject as UnityEditor.Animations.AnimatorController;

        if (ac == null)
        {
            string path = EditorUtility.OpenFilePanel("Select an AnimationController", string.Empty, "controller");

            if (path != null)
            {
                if (!path.StartsWith("/Assets"))
                {
                    int index = path.IndexOf("/Assets");

                    if (index != -1)
                        path = path.Substring(index + 1);
                }
            }

            if (!string.IsNullOrWhiteSpace(path))
            {
                ac = AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(path);
                Selection.activeObject = ac;
            }
        }

        if (ac == null)
        {
            Debug.LogError("No AnimationController selected! Please make sure you have an AnimationController selected in the Project window.");
            return;
        }

        foreach (UnityEditor.Animations.AnimatorControllerLayer layer in ac.layers)
        {
            foreach (UnityEditor.Animations.ChildAnimatorState curState in layer.stateMachine.states)
            {
                if (curState.state.hideFlags != 0)
                    curState.state.hideFlags = (HideFlags)1;

                if (curState.state.motion != null)
                {
                    if (curState.state.motion.hideFlags != 0)
                        curState.state.motion.hideFlags = (HideFlags)1;
                }
            }

            foreach (UnityEditor.Animations.ChildAnimatorStateMachine curStateMachine in layer.stateMachine.stateMachines)
            {
                foreach (UnityEditor.Animations.ChildAnimatorState curState in curStateMachine.stateMachine.states)
                {
                    if (curState.state.hideFlags != 0)
                        curState.state.hideFlags = (HideFlags)1;

                    if (curState.state.motion != null)
                    {
                        if (curState.state.motion.hideFlags != 0)
                            curState.state.motion.hideFlags = (HideFlags)1;
                    }
                }
            }
        }

        EditorUtility.SetDirty(ac);

        Debug.Log($"{nameof(AnimatorUnhideTool)}: Done!");
    }
}
#endif