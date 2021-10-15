using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCtrl : MonoBehaviour
{
    public void OnGameReadyButtonClick()
    {
        //    GameObject _lobby = transform.Find("LobbyPanel").gameObject;
        //    _lobby.SetActive(true);
        GameObject _mainButton = transform.Find("ButtonPanel").gameObject;
        _mainButton.SetActive(false);

        // 플레이어 닉네임 입력하는 창
        // 나중에는 아이디 비밀번호를 입력하고
        // 닉네임을 받아와서 닉네임을 자동 입력하는 방식으로 처리.
        GameObject _login = transform.Find("LoginPanel").gameObject;
        _login.SetActive(true);

    }


    public void OnGameStartButtonClick()
    {

        // 로딩
        StartCoroutine(GameSceneLoad());


    }

    IEnumerator GameSceneLoad()
    {
        AsyncOperation _operation = SceneManager.LoadSceneAsync("MapScene");
        _operation.allowSceneActivation = false;

        Debug.Log("___ Loading... ___");
        while (_operation.progress < 0.9f)
        {
            yield return null;
        }
        Debug.Log("____ Loading almost complete ____ ");


        //Game Manager Game Start
        GameManager.instance.GameStart(_operation,
            transform.Find("LobbyPanel").Find("PlayerListPanel").Find("PlayerInfoPanel_0").Find("PlayerNameText").GetComponent<UnityEngine.UI.Text>().text,
            transform.Find("LobbyPanel").Find("PlayerListPanel").Find("PlayerInfoPanel_0").Find("ClassDropdown").Find("Label").GetComponent<UnityEngine.UI.Text>().text);
        _operation.allowSceneActivation = true;

    }


    public void OnGameExitButtonClick()
    {
        GameManager.instance.ExitGame();
    }


    public void OnLobbyExitButtonClick()
    {
        GameObject _lobby = transform.Find("LobbyPanel").gameObject;
        _lobby.SetActive(false);

        GameObject _mainButton = transform.Find("ButtonPanel").gameObject;
        _mainButton.SetActive(true);
    }


    public void OnLogInSelectButtonClick()
    {
        string _playerNickName = transform.Find("LoginPanel").Find("NickNameInputField").Find("Text").GetComponent<UnityEngine.UI.Text>().text;

        if (_playerNickName == "" || _playerNickName == null || _playerNickName.Substring(0, 1) == " ")
        {
            transform.Find("LoginPanel").Find("NickNameErrorText").gameObject.SetActive(true);
            return;
        }


        if (transform.Find("LoginPanel").Find("NickNameErrorText").gameObject.activeSelf)
        {
            transform.Find("LoginPanel").Find("NickNameErrorText").gameObject.SetActive(false);
        }

        GameObject _login = transform.Find("LoginPanel").gameObject;
        _login.SetActive(false);

        GameObject _lobby = transform.Find("LobbyPanel").gameObject;
        _lobby.SetActive(true);

        // 플레이어가 여러명이면 PlayerInfoPanel의 번호가 달라진다.
        transform.Find("LobbyPanel").Find("PlayerListPanel").Find("PlayerInfoPanel_0").Find("PlayerNameText").GetComponent<UnityEngine.UI.Text>().text = _playerNickName;
    }

    public void OnLogInExitButtonClick()
    {
        GameObject _login = transform.Find("LoginPanel").gameObject;
        _login.SetActive(false);

        GameObject _mainButton = transform.Find("ButtonPanel").gameObject;
        _mainButton.SetActive(true);
    }



    public void OnContinueGameButtonClick()
    {
        PlayerCtrl playerCtrl = transform.parent.parent.GetComponent<PlayerCtrl>();
        playerCtrl.MenuOpen();
    }


    public void OnMainMenuButtonClick()
    {
        StartCoroutine(MainMenuSceneLoad());
    }

    IEnumerator MainMenuSceneLoad()
    {
        AsyncOperation _operation = SceneManager.LoadSceneAsync("MainMenuScene");
        _operation.allowSceneActivation = false;

        Debug.Log("____ Loading... ____");
        while (_operation.progress < 0.9f)
        {
            yield return null;
        }
        Debug.Log("____ Loading almost Complete ____");

        GameManager.instance.GameFail();
        yield return null;
        _operation.allowSceneActivation = true;

    }


    public void OnKeyInformationOpenButtonClick()
    {
        transform.Find("KeyInfoPanel").gameObject.SetActive(true);
    }

    public void OnKeyInformationExitButtonClick()
    {
        transform.Find("KeyInfoPanel").gameObject.SetActive(false);
    }


}
