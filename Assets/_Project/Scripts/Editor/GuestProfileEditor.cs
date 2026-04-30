using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GuestProfile))]
public class GuestProfileEditor : Editor
{
    private GuestProfile _guestProfile;

    private void OnEnable()
    {
        _guestProfile =  target as GuestProfile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_guestProfile.personalInfo.portrait == null)
        {
            return;
        }

        Texture2D texture = AssetPreview.GetAssetPreview(_guestProfile.personalInfo.portrait);
        GUILayout.Label("", GUILayout.Height(80), GUILayout.Width(80));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}
