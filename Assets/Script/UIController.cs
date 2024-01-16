using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIController : MonoBehaviour
{
    public bool cursorLocked = true;
    [SerializeField]
    private GameObject settingsPanel;

    void Start() {
        settingsPanel.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        bool needsUpdate = false;
        if (Input.GetKeyDown("escape")) {
            cursorLocked = !cursorLocked;
            needsUpdate = true;
        }

        if (needsUpdate) {
            Cursor.visible = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            settingsPanel.SetActive(!cursorLocked);
        }
    }

}
