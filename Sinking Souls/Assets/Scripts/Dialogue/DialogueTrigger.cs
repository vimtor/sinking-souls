using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool cinematic;
    public Dialogue[] dialogues;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        DialogueManager.Instance.StartConversation(dialogues, cinematic);
        Destroy(gameObject);
    }
}
