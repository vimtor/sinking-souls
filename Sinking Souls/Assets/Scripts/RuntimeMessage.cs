using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Run Time Message")]
public class RuntimeMessage : ScriptableObject {
    public string messageKeyboard;
    public string messageController;
    public string teller;
    public Sprite face;
    public float duration;
}

public abstract class MessageCondition : ScriptableObject {
    public bool completed;
    public RuntimeMessage message;
    public abstract bool Check();
    public abstract void reStart(bool deComplete);
}
