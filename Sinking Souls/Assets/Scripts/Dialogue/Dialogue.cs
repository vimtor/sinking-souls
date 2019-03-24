using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("Name of the character who talks.")]
    public string name;

    [Tooltip("Face which will appear in the dialogue.")]
    public Sprite face;

    [TextArea]
    public string message;
    public GameObject camera;

    [Tooltip("If true the following dialogue will play when this has finished.")]
    public bool automatic;
}
