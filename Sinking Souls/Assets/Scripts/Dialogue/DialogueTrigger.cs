using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    public bool Tavern = false;
    public bool lobby = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;


        if (Tavern)
        {
            if (GameController.instance.visitedTavern)
            {
                Destroy(gameObject);
                GameController.instance.player.GetComponent<Player>().Resume();
                return;
            }
            else
            {

                GameController.instance.visitedTavern = true;
                SaveManager.Save();
            }
        }
        if (lobby)
        {
            if (GameController.instance.visitedLobby)
            {
                Destroy(gameObject);
                GameController.instance.player.GetComponent<Player>().Resume();
                return;
            }
            else
            {

                GameController.instance.visitedLobby = true;
                SaveManager.Save();
                Destroy(GameObject.Find("Bubble"));
            }
        }

        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        DialogueManager.Instance.StartConversation(dialogues);

        Destroy(gameObject);
    }
}
