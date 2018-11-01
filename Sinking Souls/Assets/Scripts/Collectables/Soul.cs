using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Soul")]
public class Soul : ScriptableObject {

    public Material color;
    public GameObject soul;
    public int value;

    public void Spawn(Vector3 pos) {
        for(int i = 0; i < value; i++) {
            SpawnEach(pos);
        }
    }

    private void SpawnEach(Vector3 pos) {
        GameObject newSoul = Instantiate(soul);
        Vector3 offset = new Vector3(Random.value, Random.value, Random.value);
        newSoul.transform.position = pos + offset;
        newSoul.transform.parent = null;
        newSoul.GetComponent<Renderer>().material = color;
        newSoul.AddComponent<SoulBehaviour>();
    }
}
