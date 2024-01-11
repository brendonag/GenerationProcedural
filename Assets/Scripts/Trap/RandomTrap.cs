using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTrap : MonoBehaviour
{
    private Trap m_trap;
    // Start is called before the first frame update
    void Start()
    {
        m_trap = GetComponent<Trap>();
        //Random.state = 1 ;
        Debug.Log(Random.Range(0,101));
        int l_tempo = Random.Range(0, 7);

        switch(l_tempo)
        {
            case 0: m_trap.Type = TrapType.MINE; break;
            case 1: m_trap.Type = TrapType.TP; break;
            case 2: m_trap.Type = TrapType.ALARME; break;
            case 3: m_trap.Type = TrapType.PARA; break;
            case 4: m_trap.Type = TrapType.REVERSECONTROL; break;
            case 5: m_trap.Type = TrapType.SLOW; break;
            case 6: m_trap.Type = TrapType.SPEED; break;
        }
        
    }
}
