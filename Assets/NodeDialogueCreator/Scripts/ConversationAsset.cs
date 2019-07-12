using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ConversationAsset", menuName = "Dialogue Editor/Conversation Asset")]
public class ConversationAsset : ScriptableObject
{
    public List<NPCDialogueNode> allNPCNodes = new List<NPCDialogueNode>();
    public List<PlayerDialogueNode> allPlayerNodes = new List<PlayerDialogueNode>();
    public List<DialogueTransition> allTransitions = new List<DialogueTransition>();

}
