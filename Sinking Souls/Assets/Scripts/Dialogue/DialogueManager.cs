using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Setup")]
    public GameObject genericDialogue;
    public Image characterFace;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI messageContent;
    public GameObject blackFrames;
    
    private GameObject lastCamera;
    private Queue<Dialogue> conversation;


    #region SINGLETON
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public void StartConversation(Dialogue[] dialogues, bool cinematic)
    {
        genericDialogue.SetActive(true);

        if (cinematic)
        {
            blackFrames.SetActive(true);
        }

        conversation = new Queue<Dialogue>(dialogues);

        GameController.instance.player.GetComponent<Player>().Stop();

        StartCoroutine(DisplayDialogue());
    }

    private void EndConversation()
    {
        genericDialogue.SetActive(false);
        lastCamera.SetActive(false);
        blackFrames.SetActive(false);

        GameController.instance.player.GetComponent<Player>().Resume();

        conversation.Clear();
    }

    private IEnumerator DisplayDialogue()
    {
        // End conversation if the dialogue has ended.
        if (conversation.Count == 0)
        {
            EndConversation();
            yield break;
        }

        // Update the interface.
        var dialogue = conversation.Dequeue();
        characterFace.sprite = dialogue.face;
        characterName.text = dialogue.name;
        messageContent.text = dialogue.message;

        // Update the camera if needed.
        if (dialogue.camera != null)
        {
            dialogue.camera.SetActive(true);

            // To avoid accumulating active cameras.
            if (lastCamera != null) lastCamera.SetActive(false); 
            lastCamera = dialogue.camera;
        }

        // Display the next dialogue if automatic.
        if (dialogue.automatic)
        {
            yield return new WaitForSecondsRealtime(2.0f);

            StartCoroutine(DisplayDialogue());
            yield break;
        }

        // Or wait unity the continue button is pressed.
        yield return new WaitUntil(() => InputManager.ButtonA);
        StartCoroutine(DisplayDialogue());
    }
}
