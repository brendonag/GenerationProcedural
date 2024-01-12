using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Seed")]
    [SerializeField] private bool m_RandomSeed = true;
    [SerializeField] private int m_seed;
    

    [Header("Level Manager")]
    [SerializeField] private LevelManager[] m_levels;
    public LevelManager[] Levels { get { return m_levels; } }

    [Header("Room")]
    public GameObject m_start;
    public List<GameObject> m_Rooms;
    public GameObject m_end;
    


    
    static public GameManager instance;
    [HideInInspector] public System.Random m_Random;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        if(m_RandomSeed)
        { 
            m_seed = Random.Range(int.MinValue,int.MaxValue);
        }
        m_Random = new System.Random(m_seed);        
    }

}
