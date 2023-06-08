using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class StartGameUiController : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    private PlayerConfigService _playerService;

    [Inject]
    private void Construct(PlayerConfigService playerService)
    {
        _playerService = playerService;
    }

    private void Awake()
    {
        _startButton.onClick.AddListener(ButtonClick);
        _playerService.OnModelChanged.Subscribe((x) => { UpdateState(); }).AddTo(this);

        UpdateState();
    }

    public void ButtonClick()
    {
        if (_playerService.ModelsComplete())
            SceneManager.LoadScene(1);
    }

    private void UpdateState()
    {
        _startButton.interactable = _playerService.ModelsComplete();
    }
}
