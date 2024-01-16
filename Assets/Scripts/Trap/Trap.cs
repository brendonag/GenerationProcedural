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

    private void Start()
    {

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
                    Player.Instance.ApplyHit(null, 2);
                    break;
                case TrapType.PARA:
                    Player.Instance.StunPlayer(1);
                    break;
                case TrapType.REVERSECONTROL:
                    Player.Instance.ConfusePlayer(1);
                    break;
                case TrapType.SLOW:
                    Player.Instance.ChangePlayerSpeed(false, 1);
                    break;
                case TrapType.SPEED:
                    Player.Instance.ChangePlayerSpeed(true, 1);
                    break;
                case TrapType.TP:
                    Player.Instance.TpPlayer();
                    break;
                case TrapType.ALARME:
                    Player.Instance.ActiveAlarm(CreatePrefab(1));
                    break;
                case TrapType.STRAIGHT:
                    Player.Instance.GoStraightAhead(1);
                    break;
                case TrapType.BLACKOUT:
                    Player.Instance.GoBlackout(_bo02, 3);
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
                    enemy.ApplyHit(null, 2);
                    break;
                case TrapType.PARA:
                    enemy.StunPlayer(1);
                    break;
                case TrapType.REVERSECONTROL:
                    enemy.ConfusePlayer(1);
                    break;
                case TrapType.SLOW:
                    enemy.ChangePlayerSpeed(false, 1);
                    break;
                case TrapType.SPEED:
                    enemy.ChangePlayerSpeed(true, 1);
                    break;
                case TrapType.TP:
                    enemy.TpPlayer();
                    break;
                case TrapType.ALARME:
                    enemy.ActiveAlarm(CreatePrefab(1));
                    break;
                case TrapType.STRAIGHT:
                    enemy.GoStraightAhead(1);
                    break;
                case TrapType.BLACKOUT:
                    Player.Instance.GoBlackout(_bo02, 3);
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
}
