using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Generator Configuration")]
public class LevelGeneratiorConfiguration : ScriptableObject {

    public List<ListWrapper> RoomsA = new List<ListWrapper>();
    public List<ListWrapper> RoomsB = new List<ListWrapper>();
    public List<ListWrapper> RoomsC = new List<ListWrapper>();
    public List<ListWrapper> RoomsD = new List<ListWrapper>();
    public List<ListWrapper> RoomsE = new List<ListWrapper>();

    public List<ListWrapper> RoomsEliteA = new List<ListWrapper>();
    public List<ListWrapper> RoomsEliteB = new List<ListWrapper>();
    public List<ListWrapper> RoomsEliteC = new List<ListWrapper>();
    public List<ListWrapper> RoomsEliteD = new List<ListWrapper>();
    public List<ListWrapper> RoomsEliteE = new List<ListWrapper>();

    public List<GameObject> RoomsBoss = new List<GameObject>();
    public List<GameObject> NextLevelRoom = new List<GameObject>();
    public List<GameObject> Taberns = new List<GameObject>();
}
