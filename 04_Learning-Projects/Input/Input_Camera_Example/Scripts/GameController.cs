using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{

    public GameObject[] gameCameras;
    private int _cameraIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        OnFocusCamera(1);
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButtonDown(0))
            ChangeCamera(1);
        if( Input.GetMouseButtonDown(1))
            ChangeCamera(-1);
    }

    void OnFocusCamera(int index)
    {
        for (int i = 0; i < gameCameras.Length; i++)
            gameCameras[i].SetActive( false );
        gameCameras[index].SetActive(true);
    }

    void ChangeCamera(int direction) {
        _cameraIndex = (_cameraIndex + direction + gameCameras.Length) % gameCameras.Length;
        OnFocusCamera(_cameraIndex);
    }
}
