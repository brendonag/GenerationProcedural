using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private TrapType m_type;

    public TrapType Type { get { return m_type; } set { m_type = value; } }

    private Enemy m_enemy;

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
                    Player.Instance.ActiveAlarm(1);
                    break;
            }
        }

        if (collision.attachedRigidbody.gameObject.CompareTag("Enemy"))
        { 
            /*
            Enemy en = collision.GetComponent<Enemy>();
            Debug.Log(en);**/
            /*
            switch (Type)
            {
                case TrapType.MINE:
                    m_enemy.ApplyHit(null, 2);
                    break;
                case TrapType.PARA:
                    m_enemy.StunPlayer(1);
                    break;
                case TrapType.SLOW:
                    m_enemy.ChangePlayerSpeed(false, 1);
                    break;
                case TrapType.SPEED:
                    m_enemy.ChangePlayerSpeed(true, 1);
                    break;
                case TrapType.TP:
                    m_enemy.TpPlayer();
                    break;
                case TrapType.ALARME:
                    m_enemy.ActiveAlarm(1);
                    break;
            }
            */
        }
        
    }
}
