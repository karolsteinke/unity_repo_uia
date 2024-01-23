using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(WeatherManager))]

public class Managers : MonoBehaviour {
    public static WeatherManager Weather {get; private set;}
    public static ImagesManager Images {get; private set;}
    private List<IGameManager> _startSequence;

    //Add all managers to '_startSequence' List and call 'StartupManagers' couroutine
    void Awake() {
        Weather = GetComponent<WeatherManager>();
        Images = GetComponent<ImagesManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Weather);
        _startSequence.Add(Images);

        StartCoroutine(StartupManagers());
    }

    //Coroutine to startup all managers and give them '_network' reference
    private IEnumerator StartupManagers() {     
        //Create 'NetworkService' class which every manager will receive as parameter
        //Call 'Startup' method for all managers
        NetworkService network = new NetworkService();
        foreach (IGameManager manager in _startSequence) {
            manager.Startup(network);
        }

        //Pause for one frame
        yield return null;

        //Repeat 'while' loop unitl all managers (modules) are ready
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
            //Pause for one frame
            yield return null;
        }

        Debug.Log("All managers started up");
    }
}
