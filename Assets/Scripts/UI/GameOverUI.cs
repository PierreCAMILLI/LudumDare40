using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : SingletonBehaviour<GameOverUI> {

    [SerializeField]
    CanvasRenderer _gameOverCanvas;

    [SerializeField]
    private float _fadeTime = 0.5f;
    private float _fadeVelocity;

    private float _alphaTarget;

    private bool _gameOverActive = false;
    public bool GameOverActive
    {
        get { return _gameOverActive; }
    }

    public void ToogleGameOver(bool toggle)
    {
        _gameOverActive = toggle;
        _alphaTarget = toggle ? 1f : 0f;
    }

    private void Start()
    {
        _gameOverCanvas.SetAlpha(0f);
    }

    private void Update()
    {
        float alpha = Mathf.SmoothDamp(_gameOverCanvas.GetAlpha(), _alphaTarget, ref _alphaTarget, _fadeTime, Mathf.Infinity, Time.deltaTime);
        _gameOverCanvas.SetAlpha(Mathf.Clamp01(alpha));
        if (_gameOverActive)
        {
            if (Controls.Instance.Player().Quit)
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            if(Controls.Instance.Player().Restart)
                UnityEngine.SceneManagement.SceneManager.LoadScene("TestScene");
        }
    }
}
