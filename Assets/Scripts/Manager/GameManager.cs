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
    public GameObject m_roomKey;

    private bool m_isMegaTrap = false;
    public bool IsMegaTrap { get { return m_isMegaTrap; } set { m_isMegaTrap = value; } }

    private bool m_isMegaTp;
    public bool IsMegaTp { get { return m_isMegaTp; } set { m_isMegaTp = value; } }

    private int _difficulty;
    public int Difficulty { get { return _difficulty; } }

    static public GameManager instance;
    [HideInInspector] public System.Random m_Random;
    [HideInInspector] public GameObject m_player;
    private void Awake()
    {
        if (instance == null) { instance = this; }

        if(m_RandomSeed)
        { 
            m_seed = Random.Range(int.MinValue,int.MaxValue);
        }
        m_Random = new System.Random(m_seed);

        for(int i = 0; i < m_levels.Length; i++)
        {
            m_levels[i].m_ObjLevel = new GameObject();
            if(i>0)
            {
                m_levels[i].m_ObjLevel.SetActive(false);
            }
        }


    }

    private void Start()
    {
        SetDifficulty(0);
    }

    public void NextFloor()
    {
        
                
        SetDifficulty(Difficulty);
        Bounds currentBounds = m_levels[Difficulty - 1].m_firstRoom.GetWorldBounds();
        Vector3 newPosition = currentBounds.center;

        m_player.transform.position = newPosition;

        m_player.GetComponent<Player>().EnterRoom(m_levels[Difficulty -1].m_firstRoom);


        m_levels[Difficulty - 1].m_ObjLevel.SetActive(true);
        m_levels[Difficulty - 2].m_ObjLevel.SetActive(false);

        ///CHANGE SCENE
    }

    public void SetDifficulty(int currentLevel)
    {
        _difficulty = Levels[currentLevel].Dificulty;
    }

    
}
