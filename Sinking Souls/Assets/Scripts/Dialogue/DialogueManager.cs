using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public bool displayNext = false;
    private Queue<Dialogue> conversation;

    public void StartConversation(Dialogue[] dialogues)
    {
        Debug.Log("Conversation started");

        conversation = new Queue<Dialogue>(dialogues);
        displayNext = true;
    }

    private void EndConversation()
    {
        Debug.Log("Ended conversation.");

        displayNext = false;
    }

    private IEnumerator DisplayDialogue(Dialogue dialogue)
    {
        displayNext = false;

        Debug.Log(dialogue.text);
        
        if (dialogue.automatic)
        {
            yield return new WaitForSecondsRealtime(2.0f);

            displayNext = true;
            yield break;
        }

        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
        displayNext = true;
    }

    private void Update()
    {
        if (!displayNext) return;

        if (conversation.Count == 0)
        {
            EndConversation();
            return;
        }

        var nextDialogue = conversation.Dequeue();
        StartCoroutine(DisplayDialogue(nextDialogue));
    }

}
