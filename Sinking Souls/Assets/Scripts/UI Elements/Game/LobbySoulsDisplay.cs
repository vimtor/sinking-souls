using UnityEngine;
using TMPro;

public class LobbySoulsDisplay : MonoBehaviour
{
    public TextMeshProUGUI m_SoulsNumber;

	void Update ()
    {
        m_SoulsNumber.text = GameController.instance.LobbySouls.ToString();
    }
}
