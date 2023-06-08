using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private Canvas menuCanvas;
    private Canvas hudCanvas;

    [SerializeField]
    private Button _toMainMenu, _resumeButton;

    private bool _paused = true;

    private void Awake()
    {
        _toMainMenu.onClick.AddListener(ExitGame);
        _resumeButton.onClick.AddListener(Pause);

        menuCanvas = GetComponent<Canvas>();
        hudCanvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();

        Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }


    public void Pause()
    {
        _paused = !_paused;

        menuCanvas.enabled = _paused;
        hudCanvas.enabled = !_paused;

        Time.timeScale = _paused ? 0 : 1;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
