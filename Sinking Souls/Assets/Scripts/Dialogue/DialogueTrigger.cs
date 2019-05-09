using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public bool Tavern = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;


        if (Tavern)
        {
            if (GameController.instance.visitedTavern)
            {
                Destroy(gameObject);
                Debug.Log("Hola?");
                GameController.instance.player.GetComponent<Player>().Resume();
                return;
            }
            else
            {

                GameController.instance.visitedTavern = true;
                //SaveManager.Save();
            }
        }

        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        DialogueManager.Instance.StartConversation(dialogues);

        Destroy(gameObject);
    }
}
