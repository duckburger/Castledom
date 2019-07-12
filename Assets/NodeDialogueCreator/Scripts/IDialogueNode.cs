using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueNode 
{
    List<DialogueTransition> OutgoingTransitions();
    List<DialogueTransition> IncomingTransitions();

    List<PlayerDialogueNode> GetConnectedPlayerResponses();
    List<NPCDialogueNode> GetConnectedNPCLines();

    void DrawWindow();
    Rect WindowRect();
    void SetWindowRect(Rect rect);
    string DialogueLine();
    string WindowTitle();
    void DeleteAllTransitions();
    
}
