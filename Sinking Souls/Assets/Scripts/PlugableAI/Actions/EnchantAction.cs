using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Enchant")]
public class EnchantAction : Action {
    public float healPerSecond = 10;
    public override void UpdateAction(AIController controller) {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy en in enemies) {
            if (en.GetComponent<AIController>().aiActive && en.gameObject != controller.gameObject) Enchant(en);
        }
        elapsed = true;
    }

    private void Enchant(Enemy enchantedEnemy) {
        if (enchantedEnemy != null) {
            enchantedEnemy.Heal(healPerSecond* Time.deltaTime);
            Debug.Log(enchantedEnemy.gameObject.name);
        }
    }
}
