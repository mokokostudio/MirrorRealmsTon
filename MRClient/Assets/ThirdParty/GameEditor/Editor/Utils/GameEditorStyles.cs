using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Editors {
    public static class GameEditorStyles {
        private static GUIStyle box;
        public static GUIStyle Box {
            get {
                if (box == null) {
                    box = new GUIStyle(GUI.skin.GetStyle("GroupBox"));
                    box.margin = new RectOffset(0, 0, 0, 0);
                    box.padding = new RectOffset(0, 0, 0, 0);
                    box.richText = true;
                }
                return box;
            }
        }

        private static GUIStyle boxButton;
        public static GUIStyle BoxButton {
            get {
                if (boxButton == null) {
                    boxButton = new GUIStyle("RL FooterButton");
                    boxButton.margin = new RectOffset(0, 0, 0, 0);
                    boxButton.stretchWidth = true;
                    boxButton.fixedHeight = 0;
                    boxButton.richText = true;
                }
                return boxButton;
            }
        }

        private static GUIStyle foldout;
        public static GUIStyle Foldout {
            get {
                if (foldout == null) {
                    foldout = new GUIStyle(SirenixGUIStyles.Foldout);
                    foldout.margin = new RectOffset(0, 0, 0, 0);
                    foldout.richText = true;
                    foldout.stretchWidth = false;
                }
                return foldout;
            }
        }

        private static GUIStyle label;
        public static GUIStyle Label {
            get {
                if (label == null) {
                    label = new GUIStyle("label");
                    label.richText = true;
                }
                return label;
            }
        }

        private static GUIStyle prefixBlock;
        public static GUIStyle PrefixBlock {
            get {
                if (prefixBlock == null) {
                    prefixBlock = new GUIStyle("VerticalScrollbarThumb");
                    prefixBlock.overflow = new RectOffset(0, 0, 0, 0);
                    prefixBlock.imagePosition = ImagePosition.ImageAbove;
                    prefixBlock.overflow = new RectOffset(0, 0, 0, 0);
                    prefixBlock.fixedWidth = 6;
                }
                return prefixBlock;
            }
        }

        private static GUIStyle inlineButton;
        public static GUIStyle InlineButton {
            get {
                if (inlineButton == null) {
                    inlineButton = new GUIStyle(EditorStyles.miniButton);
                    inlineButton.padding = new RectOffset(2, 2, 2, 2);
                }
                return inlineButton;
            }
        }

        private static GUIStyle toolbarLabel;
        public static GUIStyle ToolbarLabel {
            get {
                if (toolbarLabel == null) {
                    toolbarLabel = new GUIStyle(SirenixGUIStyles.Label);
                    toolbarLabel.alignment = TextAnchor.MiddleLeft;
                    toolbarLabel.padding = new RectOffset(21, 4, 4, 4);
                    toolbarLabel.imagePosition = ImagePosition.TextOnly;
                }
                return toolbarLabel;
            }
        }
        private static GUIStyle toolbarButton;
        public static GUIStyle ToolbarButton {
            get {
                if (toolbarButton == null) {
                    toolbarButton = new GUIStyle(SirenixGUIStyles.ToolbarButton);
                    toolbarButton.alignment = TextAnchor.MiddleLeft;
                    toolbarButton.padding = new RectOffset(21, 4, 4, 4);
                    toolbarButton.imagePosition = ImagePosition.TextOnly;
                }
                return toolbarButton;
            }
        }

        private static GUIStyle dropDownMiniButton;
        public static GUIStyle DropDownMiniButton {
            get {
                if (dropDownMiniButton == null) {
                    dropDownMiniButton = new GUIStyle(SirenixGUIStyles.DropDownMiniButton);
                    dropDownMiniButton.richText = true;
                }
                return dropDownMiniButton;
            }
        }

        private static OdinMenuStyle odinMenuStyle;
        public static OdinMenuStyle OdinMenuStyle {
            get {
                if (odinMenuStyle == null) {
                    odinMenuStyle = new OdinMenuStyle {
                        Height = 30,
                        IconSize = 20,
                        BorderPadding = 5,
                        TriangleSize = 20,
                        TrianglePadding = 5,
                        Offset = 10
                    };
                }
                return odinMenuStyle;
            }
        }

        private static OdinMenuStyle odinListStyle;
        public static OdinMenuStyle OdinListStyle {
            get {
                if (odinListStyle == null) {
                    odinListStyle = new OdinMenuStyle {
                        Height = 22,
                        IconSize = 20,
                        BorderPadding = 0,
                        TriangleSize = 20,
                        TrianglePadding = 5,
                        Offset = 5
                    };
                }
                return odinListStyle;
            }
        }

        public static Color TilteBGColor => new Color(1, 1, 1, .05f);
        public static Color TilteBGColorSelected => new Color(1, 1, 1, .1f);
        public static Color BoxContentColor => new Color(0, 0, 0, .05f);
        public static Color BorderColor => SirenixGUIStyles.BorderColor;

        public static Color ErrorColor => new Color(1, 0, 0);
        public static Color StaticColor => new Color(.5f, 1, 1);
        public static Color APIFieldColor => new Color(.5f, 1, .5f);
        public static Color APINameColor => new Color(.75f, .75f, 1);
        public static Color APIReferenceColor => new Color(1, .5f, 1);
        public static Color VariableColor => new Color(1, .75f, .5f);
        public static Color MaskColor => new Color(0, 0, 0, .5f);
        public static Color HoverColor => new Color(1, 1, 1, .5f);
        public static Color ActiveColor => Color.white;
        public static Color InactiveColor => new Color(0.75f, 0.75f, 0.75f, 1f);

        public static Color SliderBoaderColor => new Color(0.1f, 0.1f, 0.1f, 1f);
        public static Color SliderDisableColor => new Color(0.17f, 0.17f, 0.17f, 1f);
        public static Color SliderBackgroundColor => new Color(0.22f, 0.22f, 0.22f, 1f);
        public static Color SliderFrameColor => new Color(.75f, .75f, .75f, 1f);
    }
}
