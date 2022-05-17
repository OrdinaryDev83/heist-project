using UnityEngine;
using System.Collections;
using UnityEditor;

public static class AIHelperEditor
{
    public static void SetGreen(GameObject go)
    {
        var iconContent = EditorGUIUtility.IconContent("sv_icon_dot3_pix16_gizmo");
        EditorGUIUtility.SetIconForObject(go, (Texture2D)iconContent.image);
    }

    public static void SetRed(GameObject go)
    {
        var iconContent = EditorGUIUtility.IconContent("sv_icon_dot6_pix16_gizmo");
        EditorGUIUtility.SetIconForObject(go, (Texture2D)iconContent.image);
    }
}