using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Action")]
    [SerializeField] private InputActionReference pauseAction;

    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;

    private bool isInPause;

    public void OnEnable()
    {
        pauseAction.action.Enable();
        pauseAction.action.started += togglePause;
        pausePanel.SetActive(false); 
    }

    public void OnDisable()
    {
        pauseAction.action.Disable();
        pauseAction.action.started -= togglePause;
        if (isInPause) { pausePanel.SetActive(false); Time.timeScale = 1; }
    }

    public void togglePause(InputAction.CallbackContext context)
    {
        isInPause = !isInPause;
        if (isInPause)
        {
            Time.timeScale = 0f;
            gameObject.GetComponent<MiniGame>().disableActions();
        }
        else
        {
            Time.timeScale = 1f;
            gameObject.GetComponent<MiniGame>().enableActions();
        }
        pausePanel.SetActive(isInPause);
    }

    public void goToTitleScreen() { SceneManager.LoadScene("Title Screen"); }
}
