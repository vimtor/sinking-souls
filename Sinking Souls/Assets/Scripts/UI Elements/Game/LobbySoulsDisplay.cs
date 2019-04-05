using UnityEngine;
using TMPro;

public class LobbySoulsDisplay : MonoBehaviour
{
    public TextMeshProUGUI m_SoulsNumber;
    public bool DisplayRunSouls = false;

	void Update ()
    {
        if(DisplayRunSouls)
            m_SoulsNumber.text = GameController.instance.runSouls.ToString();
        else
            m_SoulsNumber.text = GameController.instance.lobbySouls.ToString();

    }
}
