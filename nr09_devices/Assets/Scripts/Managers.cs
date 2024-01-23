using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerManager))]
[RequireComponent (typeof(InventoryManager))]

public class Managers : MonoBehaviour {
    public static PlayerManager Player {get; private set;} //why capital case?
    public static InventoryManager Inventory {get; private set;}

    private List<IGameManager> _startSequence;

    void Awake() {
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
        _startSequence.Add(Inventory);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {
        //Call startup for all modules
        foreach (IGameManager manager in _startSequence) {
            manager.Startup();
        }

        yield return null; //pause for one frame

        //do while loop unitl all modules ready
        int numReady = 0;
        int numModules = _startSequence.Count;

        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;
            
            foreach (IGameManager manager in _startSequence) {
                if (manager.status == ManagerStatus.Started) {
                    numReady++;
                }
            }

            if (numReady > lastReady) {
                Debug.Log("Managers progress: " + numReady + "/" + numModules);
            }

            yield return null; //pause for one frame
        }

        Debug.Log("All managers started up");
    }
}
