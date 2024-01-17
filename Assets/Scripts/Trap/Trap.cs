using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{
    [SerializeField] private TrapType m_type;

    public TrapType Type { get { return m_type; } set { m_type = value; } }

    public GameObject _enemyPrefab;

    [SerializeField] private Sprite _bo01;
    [SerializeField] private Sprite _bo02;

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
                    break;
                case TrapType.PARA:
                    Player.Instance.StunPlayer(_durationPara);
                    break;
                case TrapType.REVERSECONTROL:
                    Player.Instance.ConfusePlayer(_durationConfuse);
                    break;
                case TrapType.SLOW:
                    Player.Instance.ChangePlayerSpeed(false, _durationSlow);
                    break;
                case TrapType.SPEED:
                    Player.Instance.ChangePlayerSpeed(true, _durationSpeed);
                    break;
                case TrapType.TP:
                    Player.Instance.TpPlayer();
                    break;
                case TrapType.ALARME:
                    Player.Instance.ActiveAlarm(CreatePrefab(_numberOfEnemies));
                    break;
                case TrapType.STRAIGHT:
                    Player.Instance.GoStraightAhead(_durationStraight);
                    break;
                case TrapType.BLACKOUT:
                    Player.Instance.GoBlackout(_bo02, _durationBlackout);
                    break;
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
                    break;
                case TrapType.PARA:
                    enemy.StunPlayer(_durationPara);
                    break;
                case TrapType.REVERSECONTROL:
                    enemy.ConfusePlayer(_durationConfuse);
                    break;
                case TrapType.SLOW:
                    enemy.ChangePlayerSpeed(false, _durationSlow);
                    break;
                case TrapType.SPEED:
                    enemy.ChangePlayerSpeed(true, _durationSpeed);
                    break;
                case TrapType.TP:
                    enemy.TpPlayer();
                    break;
                case TrapType.ALARME:
                    enemy.ActiveAlarm(CreatePrefab(_numberOfEnemies));
                    break;
                case TrapType.STRAIGHT:
                    enemy.GoStraightAhead(_durationStraight);
                    break;
                case TrapType.BLACKOUT:
                    Player.Instance.GoBlackout(_bo02, _durationBlackout);
                    break;
            }
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
    }
}
