using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        DialogueManager.Instance.StartConversation(dialogues);
        Destroy(gameObject);
    }
}
