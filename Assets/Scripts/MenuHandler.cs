using UnityEngine;
using UnityEngine.InputSystem;

public class MenuHandler : MonoBehaviour
{

    public static System.Action OnGamePaused;
    public static System.Action OnGameResumed;

    private void Awake()
    {
        InputSystem.actions.FindAction("Pause").started += (ctx) => OnGamePaused?.Invoke();
    }
    public void ResumeGame()
    {
        OnGameResumed?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
