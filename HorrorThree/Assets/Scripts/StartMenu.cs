using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private Button buttonStart;

    [SerializeField]
    private Button buttonExit;

    private void ButtonClickToStart()
    {
        SceneManager.LoadScene(1);
    }

    private void ButtonClickToExit()
    {
        Application.Quit();
    }

    void Start()
    {
        buttonStart.onClick.AddListener(ButtonClickToStart);
        buttonExit.onClick.AddListener(ButtonClickToExit);
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        buttonStart.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
    }
}
