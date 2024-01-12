using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGenerateDungeon : MonoBehaviour
{
    private Vector3 m_position = Vector3.zero;
    private int m_last = -1;

    private List<GameObject> m_Rooms = new List<GameObject>();
    [SerializeField]public bool[,] m_map;

    Vector3 m_roomLeft = Vector3.zero;
    Vector3 m_roomRight = Vector3.zero;
    Vector3 m_roomTop = Vector3.zero;
    Vector3 m_roomBottom = Vector3.zero;

    List<Vector3> m_roomsLeft;
    List<Vector3> m_roomsRight;
    List<Vector3> m_roomsTop;
    List<Vector3> m_roomsBottom;

    private void Awake()
    {
        m_map = new bool[GameManager.instance.Levels[0].Room * 2, GameManager.instance.Levels[0].Room * 2];
    }

    void Start()
    {
        m_Rooms.Add(Instantiate(GameManager.instance.m_start,m_position, Quaternion.identity));
        m_map[GameManager.instance.Levels[0].Room, GameManager.instance.Levels[0].Room] = true;
        for(int i = 1; i < GameManager.instance.Levels[0].Room-1; i++)
        {
            SpawnRoom(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)]);
        }

        SpawnRoom(GameManager.instance.m_end);
    }

    private void SpawnRoom(GameObject p_room)
    {
        int l_random = 0;
        bool l_spawn = false;
        Vector3 l_position = m_position;



        do
        {
            l_spawn = true;
            do
            {
                l_random = GameManager.instance.m_Random.Next(0, 4);
            } while (l_random == m_last);

            switch (l_random)
            {
                case 0: l_position += Vector3.right ; break;

                case 1: l_position += Vector3.down ; break;

                case 2: l_position += Vector3.left ; break;

                case 3: l_position += Vector3.up ; break;
            }

            if( m_map[GameManager.instance.Levels[0].Room + (int)l_position.x , GameManager.instance.Levels[0].Room + (int)l_position.y] == true)
            {
                l_spawn = false;
                l_position = m_position;
            }
        } while (!l_spawn);

        m_last = (l_random + 2) % 4;
        m_position = l_position;
        m_Rooms.Add(Instantiate(p_room, m_position, Quaternion.identity));
        m_map[GameManager.instance.Levels[0].Room + (int)l_position.x, GameManager.instance.Levels[0].Room + (int)l_position.y] = true;

        switch(m_last)
        {
            case 2: 
                if (m_position.x >= m_roomRight.x)
                {
                    m_roomRight = m_position;
                    m_roomsRight.Add(m_roomRight);
                }                  
                break;

            case 3:
                if (m_position.y <= m_roomBottom.y)
                {
                    m_roomBottom = m_position; 
                    m_roomsBottom.Add(m_roomBottom);
                }                    
                break;

            case 0:
                if (m_position.x <= m_roomLeft.x)
                {
                    m_roomLeft = m_position;
                    m_roomsLeft.Add(m_roomLeft);
                }             
                break;

            case 1:
                if (m_position.y >= m_roomTop.y)
                {
                    m_roomTop = m_position;
                    m_roomsTop.Add(m_roomTop);
                }                   
                break;
        }


        bool l_top = false;
        bool l_bottom = false;
        bool l_left = false;
        bool l_right = false;

        if (m_map[GameManager.instance.Levels[0].Room + (int)m_position.x, GameManager.instance.Levels[0].Room + (int)m_position.y + 1] == true)
        {
            l_top = true;
        }

        if (m_map[GameManager.instance.Levels[0].Room + (int)m_position.x, GameManager.instance.Levels[0].Room + (int)m_position.y - 1] == true)
        {
            l_bottom = true;
        }

        if (m_map[GameManager.instance.Levels[0].Room + (int)m_position.x + 1, GameManager.instance.Levels[0].Room + (int)m_position.y ] == true)
        {
            l_right = true;
        }

        if (m_map[GameManager.instance.Levels[0].Room + (int)m_position.x - 1, GameManager.instance.Levels[0].Room + (int)m_position.y] == true)
        {
            l_left = true;
        }

        if (l_top && l_bottom && l_left && l_right)
        {
            m_last = -1;
            switch(GameManager.instance.m_Random.Next(0,4))
            {
                case 0: m_position = m_roomTop; break;
                case 1: m_position = m_roomBottom; break;
                case 2: m_position = m_roomLeft; break;
                case 3: m_position = m_roomRight; break;
            }               
        }

  
    }

}
