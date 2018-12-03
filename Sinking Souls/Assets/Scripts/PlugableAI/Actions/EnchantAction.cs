using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Enchant")]
public class EnchantAction : Action {

    public override void Act(AIController controller) {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enchant(Array.Find(enemies, enemy => enemy.GetComponent<AIController>().aiActive));
        elapsed = true;
    }

    private void Enchant(Enemy enchantedEnemy) {
        if (enchantedEnemy != null) {
            enchantedEnemy.health *= 2.0f;
        }
    }
}
