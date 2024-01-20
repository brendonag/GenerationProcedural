using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour
{
    [SerializeField] private TrapType m_type;

    public TrapType Type { get { return m_type; } set { m_type = value; } }

    public GameObject _enemyPrefab;

    //SPRITES
    [SerializeField] private Sprite _bo01;
    [SerializeField] private Sprite _bo02;
    [SerializeField] private SpriteRenderer _effects;
    [SerializeField] private GameObject _explo;
    
    [SerializeField] private List<Sprite> _listSprite = new List<Sprite>();

    //PARAMETERS
    private int _difficulty;
    public int Difficulty { set { _difficulty = value; } }

    private int _damageMine = 1;
    private int _numberOfEnemies = 1;

    private float _durationPara = 1;
    private float _durationConfuse = 1;
    private float _durationSpeed = 1;
    private float _durationSlow = 1;
    private float _durationStraight = 1;
    private float _durationBlackout = 3;

    private bool isLastLevel = false;

    private void Start()
    {
        ChangeParameters(_difficulty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player.Instance == null)
            return;
        if (collision.attachedRigidbody.gameObject == Player.Instance.gameObject)
        {
            switch (Type)
            {
                case TrapType.MINE:
                    Player.Instance.ApplyHit(null, _damageMine);
                    _effects.sprite = _listSprite[0];
                    break;
                case TrapType.PARA:
                    Player.Instance.StunPlayer(_durationPara);
                    _effects.sprite = _listSprite[2];
                    break;
                case TrapType.REVERSECONTROL:
                    Player.Instance.ConfusePlayer(_durationConfuse);
                    _effects.sprite = _listSprite[3];
                    break;
                case TrapType.SLOW:
                    Player.Instance.ChangePlayerSpeed(false, _durationSlow);
                    _effects.sprite = _listSprite[5];
                    break;
                case TrapType.SPEED:
                    Player.Instance.ChangePlayerSpeed(true, _durationSpeed);
                    _effects.sprite = _listSprite[4];
                    break;
                case TrapType.TP:
                    Player.Instance.TpPlayer();
                    _effects.sprite = _listSprite[8];
                    break;
                case TrapType.ALARME:
                    Player.Instance.ActiveAlarm(CreatePrefab(_numberOfEnemies));
                    _effects.sprite = _listSprite[1];
                    break;
                case TrapType.STRAIGHT:
                    Player.Instance.GoStraightAhead(_durationStraight);
                    _effects.sprite = _listSprite[6];
                    break;
                case TrapType.BLACKOUT:
                    Player.Instance.GoBlackout(ChangeBlackout(isLastLevel), _durationBlackout);
                    _effects.sprite = _listSprite[7];
                    break;
                case TrapType.MEGATRAP:
                    GameManager.instance.NextFloor();
                    _effects.sprite = _listSprite[8];
                    break;
                case TrapType.MEGATP:
                    Player.Instance.GoToSpecialRoom();
                    _effects.sprite = _listSprite[8];
                    break;
            }
            if(Type != TrapType.MEGATRAP)
            {
                StartCoroutine(ActiveTrap());
            }

        }

        if (collision.attachedRigidbody.gameObject.CompareTag("Enemy"))
        {
            GameObject obj = collision.attachedRigidbody.gameObject;
            Enemy enemy = obj.GetComponent<Enemy>();

            switch (Type)
            {
                case TrapType.MINE:
                    enemy.ApplyHit(null, _damageMine);
                    _effects.sprite = _listSprite[0];
                    break;
                case TrapType.PARA:
                    enemy.StunPlayer(_durationPara);
                    _effects.sprite = _listSprite[2];
                    break;
                case TrapType.REVERSECONTROL:
                    enemy.ConfusePlayer(_durationConfuse);
                    _effects.sprite = _listSprite[3];
                    break;
                case TrapType.SLOW:
                    enemy.ChangePlayerSpeed(false, _durationSlow);
                    _effects.sprite = _listSprite[5];
                    break;
                case TrapType.SPEED:
                    enemy.ChangePlayerSpeed(true, _durationSpeed);
                    _effects.sprite = _listSprite[4];
                    break;
                case TrapType.TP:
                    enemy.TpPlayer();
                    _effects.sprite = _listSprite[8];
                    break;
                case TrapType.ALARME:
                    enemy.ActiveAlarm(CreatePrefab(_numberOfEnemies));
                    _effects.sprite = _listSprite[1];
                    break;
                case TrapType.STRAIGHT:
                    enemy.GoStraightAhead(_durationStraight);
                    _effects.sprite = _listSprite[6];
                    break;
                case TrapType.BLACKOUT:
                    Player.Instance.GoBlackout(ChangeBlackout(isLastLevel), _durationBlackout);
                    _effects.sprite = _listSprite[7];
                    break;
            }

            StartCoroutine(ActiveTrap());
        }
    }
    private List<GameObject> CreatePrefab(int max)
    {
        List<GameObject> list = new List<GameObject>();

        for(int i = 0; i < max; i++)
        {
            GameObject obj = Instantiate(_enemyPrefab, Vector3.zero, Quaternion.identity);
            list.Add(obj);
        }
        return list;
    }


    private void ChangeParameters(int difficulty)
    {
        switch(difficulty)
        {
            case 1:
                Easy();
                break;
            case 2:
                Medium();
                break;
            case 3:
                Hard();
                break;
        }
    }

    private Sprite ChangeBlackout(bool lastLevel)
    {
        if (lastLevel) { return _bo02; }
        else
        {
            return _bo01;
        }
    }
    private void Easy()
    {
        _damageMine = 1;
        _numberOfEnemies = 1;

        _durationPara = 1;
        _durationConfuse = 2;
        _durationSpeed = 2;
        _durationSlow = 2;
        _durationStraight = 1;
        _durationBlackout = 3;

        isLastLevel = false;
    }
    private void Medium()
    {
        _damageMine = 1;
        _numberOfEnemies = 2;

        _durationPara = 1.5f;
        _durationConfuse = 3;
        _durationSpeed = 3;
        _durationSlow = 3;
        _durationStraight = 1;
        _durationBlackout = 4;

        isLastLevel = false;
    }
    private void Hard()
    {
        _damageMine = 2;
        _numberOfEnemies = 3;

        _durationPara = 2;
        _durationConfuse = 5;
        _durationSpeed = 5;
        _durationSlow = 5;
        _durationStraight = 2.5f;
        _durationBlackout = 4;

        isLastLevel = true;
    }

    private IEnumerator ActiveTrap()
    {
        StartCoroutine(LaunchFeedback(Vector3.zero, new Vector3(0.2f,0.2f,0.2f), 0.25f));
        StartCoroutine(FadeEffects(1));
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(LaunchFeedback(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(.19f, .19f, .19f), 0.25f));
    }
    private IEnumerator LaunchFeedback(Vector3 p_start, Vector3 p_end, float p_duration)
    {
        float elapsedTime = 0;
        float duration = p_duration;

        Vector3 _start = p_start;
        Vector3 _end = p_end;

        while (elapsedTime < duration)
        {
            Vector3 newScale = Vector3.Lerp(_start, _end, elapsedTime / duration);

            _explo.transform.localScale = newScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator FadeEffects(float p_duration)
    {
        float elapsedTime = 0;
        float duration = p_duration;

        while (elapsedTime < duration)
        {
            Vector4 _newColor = Vector4.Lerp(new Vector4(1,1,1, 1), new Vector4(1,1,1,0), elapsedTime / duration);

            _effects.color = _newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _effects.color = new Vector4(1, 1, 1, 0);
    }
}
