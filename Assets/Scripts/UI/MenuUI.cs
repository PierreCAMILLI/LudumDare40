using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : SingletonBehaviour<MenuUI>
{
    [SerializeField]
    RectTransform _tutorialTransform;
    Vector3 _tutorialBasePosition;

    Vector3 _tutorialTarget;
    Vector3 _tutorialVelocity;
    [SerializeField]
    float _tutorialSmoothTime = 0.5f;

#region Selections
    public void OnSelectPlay()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void OnSelectTutorial()
    {
        Debug.Log("Tutorial");
        // _tutorialTarget = Camera.main.ViewportToScreenPoint(new Vector3(0.5f,0.5f,0.0f));
        _tutorialTarget = Vector3.zero;
    }

    public void OnSelectExit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    #endregion

    private void Start()
    {
        _tutorialBasePosition = _tutorialTransform.localPosition;
        _tutorialTarget = _tutorialTransform.localPosition;
    }

    private void Update()
    {
        _tutorialTransform.localPosition = Vector3.SmoothDamp(_tutorialTransform.localPosition, _tutorialTarget, ref _tutorialVelocity, _tutorialSmoothTime, Mathf.Infinity, Time.deltaTime);
    }

    public void CloseTutorial()
    {
        _tutorialTarget = _tutorialBasePosition;
    }

}
