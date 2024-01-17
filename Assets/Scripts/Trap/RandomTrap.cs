using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomTrap : MonoBehaviour
{
    private Trap m_trap;

    private int _difficulty = 1;

    //ETAGE 1
    /*
    MINE
    TP
    ALARME
    REVERSECONTROL
    SLOW
    PARA
    */

    //ETAGE 2
    /*
    MINE
    TP
    ALARME
    REVERSECONTROL
    SLOW
    PARA
    SPEED
    BLACKOUT
    */

    //ETAGE 3
    /*
    MINE
    TP
    ALARME
    REVERSECONTROL
    SLOW
    PARA
    SPEED
    BLACKOUT
    STRAIGHT
    */

    // Start is called before the first frame update
    void Start()
    {
        m_trap = GetComponent<Trap>();

        _difficulty = GameManager.instance.Levels[0].Dificulty;
        AdaptDifficulty(1);

    }

    private void AdaptDifficulty(int difficulty)
    {
        switch(difficulty)
        {
            case 1:
                OnEasy();
                
                break;

            case 2:
                OnMedium();
                break;

            case 3:
                OnHard();
                break;

            default: OnEasy();
                break;
        }

        m_trap.Difficulty = difficulty;
    }

    private void OnEasy()
    {
        /*    
        MINE
        TP
        ALARME
        REVERSECONTROL
        SLOW
        PARA
        */

        int pMi = 20;
        int pTp = 10;
        int pAl = 20;
        int pRe = 10;
        int pSl = 20;
        int pPa = 20;

        int l_rand = GameManager.instance.m_Random.Next(0, 100);

        if(l_rand < pMi) { m_trap.Type = TrapType.MINE; }
        else if(l_rand < pMi + pTp) { m_trap.Type = TrapType.TP; }
        else if (l_rand < pMi + pTp + pAl) { m_trap.Type = TrapType.ALARME; }
        else if (l_rand < pMi + pTp + pAl + pRe) { m_trap.Type = TrapType.REVERSECONTROL; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl) { m_trap.Type = TrapType.SLOW; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa) { m_trap.Type = TrapType.PARA; }
    }

    private void OnMedium()
    {
        /*
        MINE
        TP
        ALARME
        REVERSECONTROL
        SLOW
        PARA
        SPEED
        BLACKOUT
        */

        int pMi = 10;
        int pTp = 20;
        int pAl = 10;
        int pRe = 10;
        int pSl = 10;
        int pPa = 10;
        int pSp = 10;
        int pBl = 10;

        int l_rand = GameManager.instance.m_Random.Next(0, 100);

        if (l_rand < pMi) { m_trap.Type = TrapType.MINE; }
        else if (l_rand < pMi + pTp) { m_trap.Type = TrapType.TP; }
        else if (l_rand < pMi + pTp + pAl) { m_trap.Type = TrapType.ALARME; }
        else if (l_rand < pMi + pTp + pAl + pRe) { m_trap.Type = TrapType.REVERSECONTROL; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl) { m_trap.Type = TrapType.SLOW; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa) { m_trap.Type = TrapType.PARA; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa + pSp) { m_trap.Type = TrapType.SPEED; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa + pSp + pBl) { m_trap.Type = TrapType.BLACKOUT; }
    }

    private void OnHard()
    {
        /*
        MINE
        TP
        ALARME
        REVERSECONTROL
        SLOW
        PARA
        SPEED
        BLACKOUT
        STRAIGHT
        */

        int pMi = 10;
        int pTp = 10;
        int pAl = 10;
        int pRe = 10;
        int pSl = 10;
        int pPa = 10;
        int pSp = 10;
        int pBl = 10;
        int pSt = 20;

        int l_rand = GameManager.instance.m_Random.Next(0, 100);

        if (l_rand < pMi) { m_trap.Type = TrapType.MINE; }
        else if (l_rand < pMi + pTp) { m_trap.Type = TrapType.TP; }
        else if (l_rand < pMi + pTp + pAl) { m_trap.Type = TrapType.ALARME; }
        else if (l_rand < pMi + pTp + pAl + pRe) { m_trap.Type = TrapType.REVERSECONTROL; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl) { m_trap.Type = TrapType.SLOW; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa) { m_trap.Type = TrapType.PARA; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa + pSp) { m_trap.Type = TrapType.SPEED; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa + pSp + pBl) { m_trap.Type = TrapType.BLACKOUT; }
        else if (l_rand < pMi + pTp + pAl + pRe + pSl + pPa + pSp + pBl + pSt) { m_trap.Type = TrapType.STRAIGHT; }
    }

}
