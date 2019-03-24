using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool cinematic;
    public Dialogue[] dialogues;

    public DialogueManager manager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Dialogue triggered");
        manager.StartConversation(dialogues);
    }
}
