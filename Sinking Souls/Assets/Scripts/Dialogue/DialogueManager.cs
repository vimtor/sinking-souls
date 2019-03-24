using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    private Queue<Dialogue> conversation;


    public void StartConversation(Dialogue[] dialogues, bool cinematic)
    {
        Debug.Log("Conversation started");

        conversation = new Queue<Dialogue>(dialogues);
        StartCoroutine(DisplayDialogue());
    }

    private void EndConversation()
    {
        Debug.Log("Ended conversation.");

        conversation.Clear();
    }

    private IEnumerator DisplayDialogue()
    {
        // displayNext = false;
        if (conversation.Count == 0)
        {
            EndConversation();
            yield break;
        }

        var dialogue = conversation.Dequeue();
        Debug.Log(dialogue.text);

        if (dialogue.automatic)
        {
            yield return new WaitForSecondsRealtime(2.0f);

            StartCoroutine(DisplayDialogue());
            yield break;
        }

        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
        StartCoroutine(DisplayDialogue());
    }
}
