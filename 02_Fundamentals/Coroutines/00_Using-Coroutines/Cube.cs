using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Test : MonoBehaviour {
    private IEnumerator _currentMoveCoroutine;
    
    // Start is called before the first frame update
    void Start() {
        string[] msgs = { "Bu", "Bir", "Bant", "Kaydidir..."};
        StartCoroutine(PrintMessage(msgs, 1));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if( _currentMoveCoroutine != null )
                StopCoroutine(_currentMoveCoroutine);
            _currentMoveCoroutine = Move(Random.onUnitSphere * 5, 1);
            StartCoroutine(_currentMoveCoroutine);
        }
    }

    IEnumerator PrintMessage(string[] messages, float delay) {
        foreach (var message in messages) {
            print(message);
            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator Move(Vector3 destionation, float speed) {
        while ( transform.position != destionation ) {
            transform.position = Vector3.MoveTowards(transform.position, destionation, speed * Time.deltaTime);
            yield return null;            // Pause until next frame
        }
    }


}
