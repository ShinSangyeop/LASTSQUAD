using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    //public WeaponManager weaponManager;

<<<<<<< HEAD
    public BunkerDoor bunkerDoor;

    // 각 특전이 활성화 되었는지 확인하기 위한 변수
    public bool perk0_Active = false;
    public bool perk1_Active = false;
    public bool perk2_Active = false;

    private int maxStage = 30;
    public int _stage = 1; // 현재 몇 스테이지인지를 저장한 변수
    public int _remainEnemyCount; // 스테이지에 남은 적 수

    private float stageDelay = 60f;
    private float stageTime;

    // 멀티가 된다면 사용하게될? 플레이어들 목록
    // Photon에서는 PhotonNetwork.CurrentRoom.Players 을 사용해서 플레이어 목록을 확인할 수 있다.
    // 특전이 활성화되면 특전 활성화에 사용해야한다.
    public List<PlayerCtrl> players;

    // 적을 생성할 위치
    Transform[] enemyPoints;

    private IEnumerator _coEnemySpawn;



=======
>>>>>>> parent of 21a53d0 (20211012_enemy수정)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

<<<<<<< HEAD
=======
        instance = this;

        DontDestroyOnLoad(this.gameObject);
>>>>>>> parent of 21a53d0 (20211012_enemy수정)
    }

    // Start is called before the first frame update
    void Start()
    {
        _coEnemySpawn = EnemySpawn();

        /*

<<<<<<< HEAD
        // 지금은 게임 씬에서 시작하니까 바로 GameStart를 실행시킨다.
        GameStart();
        */
=======

>>>>>>> parent of 21a53d0 (20211012_enemy수정)
    }

    // Update is called once per frame
    void Update()
    {
        ExitGame();

<<<<<<< HEAD
        // 유니티 에디터에서 동작
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ExitGame();
        }

        // 윈도우에서 동작
#elif UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ExitGame();
        }


#endif
=======
>>>>>>> parent of 21a53d0 (20211012_enemy수정)



        #region Get Skill Information Test Code
        //List<float> skillInfo = FindSkill.GetSkillInfo("Healing", 2);
        //for(int i = 0; i < skillInfo.Count; i++)
        //{
        //    Debug.Log(skillInfo[i]);
        //}
        #endregion


    }

<<<<<<< HEAD
    public void ExitGame()
=======

    private void ExitGame()
>>>>>>> parent of 21a53d0 (20211012_enemy수정)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;


#elif UNITY_STANDALONE_WIN
            Application.Quit();


#endif


        }
    }

<<<<<<< HEAD
    // 게임이 종료되면 실행되는 함수.
    public void GameOver()
    {
        // 대충 화면이 어두워지고 Game Over 라거나 패배 라는 글자가 뜨고
        // 잠시후에 로비로 돌아가는 버튼이 활성화되거나 하면 된다.

        GameObject gameCanvas = GameObject.Find("GameWorldCanvas");
        gameCanvas.transform.Find("GameOverPanel").gameObject.SetActive(true);

        StartCoroutine(CoFadeIn());
    }

    IEnumerator CoFadeIn()
    {
        GameObject gameCanvas = GameObject.Find("GameWorldCanvas");
        UnityEngine.UI.Image fadein = gameCanvas.transform.Find("GameOverPanel").Find("GameOverFadeIn").GetComponent<UnityEngine.UI.Image>();

        while (fadein.color.a < 1f)
        {
            Color _color = fadein.color;
            _color.a += Time.deltaTime;
            fadein.color = _color;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        gameCanvas.GetComponent<ButtonCtrl>().OnMainMenuButtonClick();
        GameFail();
    }


    /// <summary>
    /// 게임 씬으로 넘어갔을 때 실행될 함수
    /// </summary>
    public void GameStart(UnityEngine.AsyncOperation _operation, string _playerNickName, string _className)
    {
        string _class;
        if (_className == "소총병")
        {
            _class = "Soldier";
        }
        else if (_className == "의무병")
        {
            _class = "Medic";
        }
        else if (_className == "공병")
        {
            _class = "Engineer";
        }
        else
        {
            _class = null;
        }

        StartCoroutine(CoGameStart(_operation, _playerNickName, _class));

    }
=======

>>>>>>> parent of 21a53d0 (20211012_enemy수정)

    // 나중에 플레이어 정보가 DB에 있게되면 DB에서 플레이어 정보를 받아올 수 있을까?
    // 직업 설정은 어떻게 넘겨줄까?
    IEnumerator CoGameStart(UnityEngine.AsyncOperation _operation, string _playerNickName, string _className)
    {
        yield return _operation;

        // 벙커의 위치를 받아온다.
        bunkerDoor = GameObject.FindGameObjectWithTag("BUNKERDOOR").GetComponent<BunkerDoor>();

        // 플레이어 스폰 지점 목록을 가져온다.
        Transform[] points = GameObject.Find("PlayerSpawnPoints").GetComponentsInChildren<Transform>();

        // 부모의 위치도 포함되기 때문에 부모인 0을 제외한 1부터 시작한다.
        int idx = UnityEngine.Random.Range(1, points.Length);

        // 생성할 플레이어 프리팹을 찾음
        GameObject _playerPref = Resources.Load<GameObject>("Prefabs/Player/Player");
        // 생성할 위치를 설정
        players.Add(Instantiate(_playerPref, points[idx].position, Quaternion.identity).GetComponent<PlayerCtrl>());
        // 플레이어의 이름 및 직업 설정
        // Lobyy에서 입력받은 플레이어 이름과 선택된 직업을 사용해서 설정해야한다.
        players[0].playerName = _playerNickName;

        //players[0].playerClass = PlayerClass.ePlayerClass.Medic;
        players[0].playerClass = (PlayerClass.ePlayerClass)System.Enum.Parse(typeof(PlayerClass.ePlayerClass), _className);


        enemyPoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();
        Debug.Log("EnemyPoints: " + enemyPoints.Length);

        yield return new WaitForSeconds(1f);

        StartCoroutine(_coEnemySpawn);
    }


    internal void GameFail()
    {
        // 테스트 중에는 오류가 발생할 수 있지만
        // 일반적으로 게임 중에는 항상 적은 스폰되고 있을 것이기 때문에 _coEnemeySpawn이 null일 확률은 없다.
        try
        {
            StopCoroutine(_coEnemySpawn);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
        }

        perk0_Active = false;
        perk1_Active = false;
        perk2_Active = false;

        players.Clear();
        bunkerDoor = null;
        enemies.Clear();

        CursorState.CursorLockedSetting(false);

    }


    // 테스트 코드

    public void EnemySpawnDebug()
    {
        if (_coEnemySpawn != null)
        {
            StopCoroutine(_coEnemySpawn);
            _coEnemySpawn = null;
        }
        else
        {
            _coEnemySpawn = EnemySpawn();
            StartCoroutine(_coEnemySpawn);
        }
    }

    public List<LivingEntity> enemies = new List<LivingEntity>();

    IEnumerator EnemySpawn()
    {
        /*
         * 0
         * 1/ 2 3 4 5 6 7
         * 8/ 9 10 11 12 13 14
         * 15/ 16 17 18 19 20 21
         */


        yield return new WaitForSeconds(3f);
        GameObject zombie = Resources.Load<GameObject>("Prefabs/Enemy/Zombie");
        while (true)
        {
            int idx = UnityEngine.Random.Range(2, 8);
            idx += (UnityEngine.Random.Range(0, 3) * 7);
            if (enemies.Count < 15)
            {
                enemies.Add(Instantiate(zombie, enemyPoints[idx].position, Quaternion.identity).GetComponent<LivingEntity>());
            }


            //Debug.Log("____ ENEMY COUNT: " + enemies.Count + " ____");

            yield return new WaitForSeconds(0.5f);
        }

    }



}
