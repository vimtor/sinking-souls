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

    [Header("Extras")]
    public GameObject blackFrames;
    public GameObject inGameUI;

    private GameObject lastCamera;
    private Coroutine lastCoroutine;
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

    public void StartConversation(Dialogue[] dialogues)
    {
        genericDialogue.SetActive(true);
        inGameUI.SetActive(false);

        conversation = new Queue<Dialogue>(dialogues);

        GameController.instance.player.GetComponent<Player>().Stop();

        StartCoroutine(DisplayDialogue());
    }

    private void EndConversation()
    {
        genericDialogue.SetActive(false);
        blackFrames.SetActive(false);
        if (lastCamera != null) lastCamera.SetActive(false);
        inGameUI.SetActive(true);

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

        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(TypeSentence(dialogue.message));

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
        yield return new WaitForSecondsRealtime(0.1f);
        yield return new WaitUntil(() => InputManager.GetButtonA());
        StartCoroutine(DisplayDialogue());
    }

    public void DisplayDialogue(Dialogue dialogue)
    {
        genericDialogue.SetActive(true);

        characterFace.sprite = dialogue.face;
        characterName.text = dialogue.name;

        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(TypeSentence(dialogue.message));
    }

    public void HideDialogue()
    {
        genericDialogue.SetActive(false);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        messageContent.text = "";
        var parsedSentence = ParseSentence(sentence);

        foreach (char letter in parsedSentence)
        {
            messageContent.text += letter;

            yield return null;
            yield return null;
        }
    }

    private string ParseSentence(string sentence)
    {
        string[] parseableSymbols = { "(X)", "(Y)" };

        string[] controllerSymbols = {"XD", "YD"};
        string[] keyboardSymbols = {"XD", "YD"};

        for (int i = 0; i < parseableSymbols.Length; i++)
        {
            var symbol = InputManager.Xbox_One_Controller == 1 ? controllerSymbols[i] : keyboardSymbols[i];
            sentence.Replace(parseableSymbols[i], symbol);
        }

        return sentence;
    }
}
