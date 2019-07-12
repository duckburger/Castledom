using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class NPCDialogueNode : IDialogueNode
{
    public int id;
    public Rect windowRect;
    public string windowTitle;
    public List<DialogueTransition> outgoingTransitions = new List<DialogueTransition>();
    public List<DialogueTransition> incomingTransitions = new List<DialogueTransition>();
    [TextArea(3, 10)]
    public string dialogueLine;

    public NPCDialogueNode(Rect rect, string title)
    {
        this.windowRect = rect;
        this.windowTitle = title;
        id = UnityEngine.Random.Range(0, Int32.MaxValue);
    }

    public DialogueCharacter speaker;
    public void DrawWindow()
    {
        speaker = (DialogueCharacter) EditorGUILayout.ObjectField(speaker, typeof(DialogueCharacter), false);

        if (speaker == null)
        {
            EditorGUILayout.LabelField("Add a speaker to modify");
        }
        else
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Character Icon");
            speaker.icon = (Sprite) EditorGUILayout.ObjectField(GUIContent.none, speaker.icon, typeof(Sprite), false, GUILayout.ExpandWidth(true));
            GUILayout.Label("Character Name");
            speaker.name = EditorGUILayout.TextField(speaker.name);
            GUILayout.Label("Character Text Color");
            speaker.textColor = EditorGUILayout.ColorField(speaker.textColor);
            GUILayout.EndVertical();
            
            EditorGUILayout.LabelField("Dialogue Line");
            EditorStyles.textField.wordWrap = true;
            dialogueLine = EditorGUILayout.TextArea(dialogueLine, GUILayout.Height(88f));
            windowRect.height = 300f;
        }                
    }

    public void Drag(Vector2 dragDelta)
    {
        windowRect.position += dragDelta;
    }

    #region Interface Requirements

    public List<DialogueTransition> OutgoingTransitions()
    {
        return outgoingTransitions;
    }

    public List<DialogueTransition> IncomingTransitions()
    {
        return incomingTransitions;
    }

    public Rect WindowRect()
    {
        return windowRect;
    }

    public string WindowTitle()
    {
        return windowTitle;
    }

    public string DialogueLine()
    {
        return dialogueLine;
    }

    public List<PlayerDialogueNode> GetConnectedPlayerResponses()
    {
        List<PlayerDialogueNode> responses = new List<PlayerDialogueNode>();
        for (int i = 0; i < outgoingTransitions.Count; i++)
        {
            responses.Add(outgoingTransitions[i].endPlayerNode as PlayerDialogueNode);            
        }
        return responses;
    }

    public List<NPCDialogueNode> GetConnectedNPCLines()
    {
        List<NPCDialogueNode> npcLineNodes = new List<NPCDialogueNode>();
        for (int i = 0; i < outgoingTransitions.Count; i++)
        {
            npcLineNodes.Add(outgoingTransitions[i].endNPCNode as NPCDialogueNode);        
        }
        return npcLineNodes;
    }

    public void DeleteAllTransitions()
    {
        outgoingTransitions.Clear();
        incomingTransitions.Clear();
    }

    public void SetWindowRect(Rect rect)
    {
        this.windowRect = rect;
    }

    #endregion
}
