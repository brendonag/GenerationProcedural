using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Seed")]
    [Min(0)][SerializeField] private int m_seed;

    [Header("Level Manager")]
    [SerializeField] private LevelManager[] m_levels;
    public LevelManager[] Levels { get { return m_levels; } }
    #region room list
    [Header("One Door")]
    public List<GameObject> m_Down;
    public List<GameObject> m_Top;
    public List<GameObject> m_Right;
    public List<GameObject> m_Left;

    [Header("Two Door")]
    public List<GameObject> m_DownTop;
    public List<GameObject> m_DownLeft;
    public List<GameObject> m_DownRight;

    public List<GameObject> m_TopLeft;
    public List<GameObject> m_TopRight;
    
    public List<GameObject> m_LeftRight;

    [Header("Tree Door")]
    public List<GameObject> m_DownTopLeft;
    public List<GameObject> m_DownLeftRight;
    public List<GameObject> m_DownTopRight;
    public List<GameObject> m_TopLeftRight;

    [Header("Hub")]
    public List<GameObject> m_Hub;
    #endregion
    
    static public GameManager instance;
    [HideInInspector] public System.Random m_Random;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        m_Random = new System.Random(m_seed);
    }

}
