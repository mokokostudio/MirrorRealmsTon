using GameEditor.Editors;
using GameEditor.Editors.Domain;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.Expando;
using UnityEditor;
using UnityEngine;

[DrawerPriority(0, .6f, 0)]
public class ForkDrawer : OdinValueDrawer<EFork> {
    protected override void DrawPropertyLayout(GUIContent label) {
        //CallNextDrawer(label);
        for (int i = 0; i < Property.Children[0].Children.Count; i++)
            DrawChild(Property.Children[0].Children[i], true, i == 0);
        DrawChild(Property.Children[1], false);
    }

    private void DrawChild(InspectorProperty target, bool isIF, bool isFrist = false) {
        GameEditorGUI.DrawLine();
        bool expand = target.State.Expanded;
        var rect = EditorGUILayout.BeginHorizontal(GameEditorStyles.BoxButton);
        string title = isIF ? "Èç¹û" : "·ñÔò";
        if (!expand)
            title += " " + GameEditorStyles.APINameColor.Dye(target.ValueEntry.WeakSmartValue.ToString());
        EditorGUI.DrawRect(rect, expand ? GameEditorStyles.TilteBGColorSelected : GameEditorStyles.TilteBGColor);
        target.State.Expanded = SirenixEditorGUI.Foldout(expand, GUIHelper.TempContent(title), GameEditorStyles.Foldout);

        if (isIF && isFrist && GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("plus"))) {
            ValueEntry.SmartValue.elseif.Add(new EFork.EELSEIF());
            target.Parent.SetDirty();
        }
        if (isIF && !isFrist && GameEditorGUI.ToolbarIconButton(GameEditorGUI.GetIcon("delete"))) {
            ValueEntry.SmartValue.elseif.Remove(target.ValueEntry.WeakSmartValue as EFork.EELSEIF);
            target.Parent.SetDirty();
        }

        EditorGUILayout.EndHorizontal();
        if (SirenixEditorGUI.BeginFadeGroup(target, expand)) {
            for (int i = 0; i < target.Children.Count; i++) {
                GameEditorGUI.DrawLine();
                target.Children[i].Draw();
            }
        }
        SirenixEditorGUI.EndFadeGroup();
    }
}
