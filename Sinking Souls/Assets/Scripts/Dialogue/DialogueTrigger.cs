using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        DialogueManager.Instance.StartConversation(dialogues);
        Destroy(gameObject);
    }
}
