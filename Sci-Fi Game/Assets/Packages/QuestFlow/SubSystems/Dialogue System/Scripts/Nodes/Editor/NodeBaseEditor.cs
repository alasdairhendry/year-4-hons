using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace QuestFlow.DialogueEngine
{
    public class NodeBaseEditor : NodeEditor
    {
        private GUIStyle attributeStyle = new GUIStyle ( NodeEditorResources.styles.nodeHeader );
        private GUIStyle nameStyle = new GUIStyle ( NodeEditorResources.styles.nodeHeader );

        protected void GetHeader (string attribute)
        {
            attributeStyle.alignment = TextAnchor.MiddleLeft;
            attributeStyle.normal.textColor = new Color ( 0.2f, 0.2f, 0.2f );
            nameStyle.alignment = TextAnchor.MiddleCenter;
            nameStyle.padding.left = -64;

            EditorGUILayout.BeginHorizontal ();
            GUILayout.Label ( "[" + attribute + "] ", attributeStyle, GUILayout.Height ( 30 ), GUILayout.Width ( 64 ) );
            GUILayout.Label ( target.name, nameStyle, GUILayout.Height ( 30 ) );
            EditorGUILayout.EndHorizontal ();
        }

        public override void AddContextMenuItems (GenericMenu menu)
        {
            // Actions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node)
            {
                XNode.Node node = Selection.activeObject as XNode.Node;
                menu.AddItem ( new GUIContent ( "Move To Top" ), false, () => NodeEditorWindow.current.MoveNodeToTop ( node ) );
                menu.AddItem ( new GUIContent ( "Rename" ), false, NodeEditorWindow.current.RenameSelectedNode );
            }

            // Add actions to any number of selected nodes
            menu.AddItem ( new GUIContent ( "Remove" ), false, NodeEditorWindow.current.RemoveSelectedNodes );

            // Custom sctions if only one node is selected
            if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node)
            {
                XNode.Node node = Selection.activeObject as XNode.Node;
                menu.AddCustomContextMenuItems ( node );
            }
        }
    }

}