using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject genericDialogue;
    public Image characterFace;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI messageContent;

    private Queue<Dialogue> conversation;


    public void StartConversation(Dialogue[] dialogues, bool cinematic)
    {
        Debug.Log("Conversation started");

        genericDialogue.SetActive(true);
        conversation = new Queue<Dialogue>(dialogues);
        StartCoroutine(DisplayDialogue());
    }

    private void EndConversation()
    {
        Debug.Log("Ended conversation.");
        genericDialogue.SetActive(false);
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
        characterFace.sprite = dialogue.face;
        characterName.text = dialogue.name;
        messageContent.text = dialogue.message;

        Debug.Log(dialogue.message);

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
