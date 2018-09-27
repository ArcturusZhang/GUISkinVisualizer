using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GuiSkinVisualizeEditor : EditorWindow
    {
        [MenuItem("GUISkin Visualizer/Show Editor")]
        public static void ShowEditor()
        {
            var editor = GetWindow<GuiSkinVisualizeEditor>();
            editor.ShowTab();
        }

        private GUISkin skin;
        private GuiElementType selectedType;
        private float previewWidth = 200;
        private float previewHeight = 100;
        private List<string> styles = new List<string>();
        private int index = 0;
        private bool toggled = false;
        private string fieldText = "I am a text field";
        private string areaText = "I am a text area";

        private void OnGUI()
        {
            GUILayout.Space(5);
            skin = EditorGUILayout.ObjectField("Choose GUISkin to visualize", skin, typeof(GUISkin), false) as GUISkin;
            LoadStyles();
            selectedType = (GuiElementType) EditorGUILayout.EnumPopup("Choose GUI Element to show", selectedType);
            index = EditorGUILayout.Popup("Choose", index, styles.ToArray());
            EditorGUILayout.Separator();
            GUILayout.Label("Preview Size: ");
            using (new GUILayout.HorizontalScope())
            {
                previewWidth = EditorGUILayout.FloatField("Width", previewWidth);
                previewHeight = EditorGUILayout.FloatField("Height", previewHeight);
            }
            if (GUILayout.Button("Refresh now")) Repaint();
            EditorGUILayout.Separator();
            DrawPreview();
        }

        private void LoadStyles()
        {
            if (skin == null) return;
            styles.Clear();
            styles.Add(skin.box.name);
            styles.Add(skin.button.name);
            styles.Add(skin.toggle.name);
            styles.Add(skin.label.name);
            styles.Add(skin.textField.name);
            styles.Add(skin.textArea.name);
            styles.AddRange(skin.customStyles.Select(style => style.name));
        }

        private void DrawPreview()
        {
            if (skin == null) return;
            var style = skin.GetStyle(styles[index]);
            var rect = GUILayoutUtility.GetRect(previewWidth, previewHeight, style);
            rect = new Rect(rect.x, rect.y, previewWidth, previewHeight);
            switch (selectedType)
            {
                case GuiElementType.Box:
                    GUI.Box(rect, "I am a box", style);
                    break;
                case GuiElementType.Button:
                    if (GUI.Button(rect, "I am a button", style)) Debug.Log("Button clicked");
                    break;
                case GuiElementType.Toggle:
                    toggled = GUI.Toggle(rect, toggled, "I am a toggle", style);
                    Debug.Log(toggled ? "Checked" : "Unchecked");
                    break;
                case GuiElementType.Label:
                    GUI.Label(rect, "I am a label", style);
                    break;
                case GuiElementType.TextField:
                    fieldText = GUI.TextField(rect, fieldText, style);
                    break;
                case GuiElementType.TextArea:
                    areaText = GUI.TextArea(rect, areaText, style);
                    break;
            }
        }
    }

    public enum GuiElementType
    {
        Box,
        Button,
        Toggle,
        Label,
        TextField,
        TextArea,
    }
}