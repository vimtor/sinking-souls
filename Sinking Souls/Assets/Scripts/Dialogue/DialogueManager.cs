using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("Configuration")]
    [Range(1, 5)] public int dialogueSpeed = 1;
    public string[] parseableSymbols;
    public string[] controllerSymbols;
    public string[] keyboardSymbols;

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


        conversation.Clear();
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        GameController.instance.player.GetComponent<Player>().Resume();

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
        lastCoroutine = StartCoroutine(TypeSentence((dialogue.message)));

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
        yield return new WaitUntil(() => InputManager.GetButtonA() || Input.GetKeyDown(KeyCode.E) );
        StartCoroutine(DisplayDialogue());
    }

    public void DisplayDialogue(Dialogue dialogue)
    {
        genericDialogue.SetActive(true);

        characterFace.sprite = dialogue.face;
        characterName.text = dialogue.name;

        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(TypeSentence((dialogue.message)));
    }

    public void HideDialogue()
    {
        genericDialogue.SetActive(false);
    }

    bool smallSizeMode = false;
    public float smallSize = 35;

    private IEnumerator TypeSentence(string sentence)
    {
        AudioManager.Instance.Play("Dialogue");

        messageContent.text = "";
        var parsedSentence = sentence;

        float time = 1 / Mathf.Pow(10, dialogueSpeed);
        for (int i = 0; i < parsedSentence.Length; i++)
        {
            string letter = parsedSentence[i].ToString();

            if (letter == "#")
            {
                i++;
                letter = parsedSentence[i].ToString();
                smallSizeMode = !smallSizeMode;

            }
            if (letter == "|")
            {
                i++;

                int aux = i;
                string sentenceToParse = "|";
                while(parsedSentence[aux].ToString() != "|")
                {
                    sentenceToParse += parsedSentence[aux];
                    aux++;
                }
                i = aux;
                letter = "<size=40><color=#ff0000ff>" + ParseAux(sentenceToParse) + "</color></size>";
            }
            if (smallSizeMode)
            {
                letter = letter = "<size=" + smallSize.ToString() + ">" + letter + "</size>";
            }
            messageContent.text += letter;
            GameController.instance.player.GetComponent<Player>().Stop();

            yield return new WaitForSecondsRealtime(time);

        }

        AudioManager.Instance.Stop("Dialogue");
    }

    private string ParseSentence(string sentence)
    {
        for (int i = 0; i < parseableSymbols.Length; i++)
        { 
            var symbol = InputManager.Xbox_One_Controller > 0 ? controllerSymbols[i] : keyboardSymbols[i];
            sentence = sentence.Replace(parseableSymbols[i],  symbol );
        }

        return sentence;
    }

    private string ParseAux(string sentence)
    {
        for (int i = 0; i < parseableSymbols.Length; i++)
        {
            var symbol = InputManager.Xbox_One_Controller > 0 ? controllerSymbols[i] : keyboardSymbols[i];
            sentence = sentence.Replace("|" + parseableSymbols[i], symbol);
        }

        return sentence;
    }
}
