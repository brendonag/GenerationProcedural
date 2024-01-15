using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RunGenerateDungeon : MonoBehaviour
{
    private Vector2 m_position = Vector2.zero;
    private int m_last = -1;

    private List<GameObject> m_Rooms = new List<GameObject>();
    //[SerializeField]public bool[,] m_map;
    [SerializeField] private Dictionary<Vector2,Rooms> m_mapPosition = new Dictionary<Vector2, Rooms>();

    List<Vector2> m_roomsLeft = new List<Vector2>();
    List<Vector2> m_roomsRight = new List<Vector2>();
    List<Vector2> m_roomsTop = new List<Vector2>();
    List<Vector2> m_roomsBottom = new List<Vector2>();

    List<Vector2> m_bRoomsLeft = new List<Vector2>();
    List<Vector2> m_bRoomsRight = new List<Vector2>();
    List<Vector2> m_bRoomsTop = new List<Vector2>();
    List<Vector2> m_bRoomsBottom = new List<Vector2>();

    private void Awake()
    {
        //m_map = new bool[GameManager.instance.Levels[0].Room * 2, GameManager.instance.Levels[0].Room * 2];
        m_roomsTop.Add(Vector3.zero);
        m_roomsRight.Add(Vector3.zero);
        m_roomsBottom.Add(Vector3.zero);
        m_roomsLeft.Add(Vector3.zero);

        m_bRoomsTop.Add(Vector3.zero);
        m_bRoomsRight.Add(Vector3.zero);
        m_bRoomsBottom.Add(Vector3.zero);
        m_bRoomsLeft.Add(Vector3.zero);
    }

    void Start()
    {
        m_Rooms.Add(Instantiate(GameManager.instance.m_start,m_position, Quaternion.identity));

        Rooms l_start = new Rooms();
        l_start.m_start = true;
        m_mapPosition.Add(m_position,l_start);

        for (int i = 1; i < GameManager.instance.Levels[0].Room-1; i++)
        {
            GenerateRoom(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)]);
        }
        

        for(int i = 0;i< GameManager.instance.Levels[0].NBranche.Count;i++)
        {   
            switch(i%4)
            {
                case 0:
                    m_position = m_roomsRight[GameManager.instance.m_Random.Next(0, m_roomsRight.Count)];
                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.right;
                        //m_Rooms.Add(Instantiate(GameManager.instance.m_start, m_position, Quaternion.identity));

                        Rooms l_room = new Rooms();
                        if(y == 0)
                        {
                            l_room.m_lock = true;
                        }
                        m_mapPosition.Add(m_position,l_room);
                    }
                    break;

                case 1:
                    m_position = m_roomsBottom[GameManager.instance.m_Random.Next(0, m_roomsBottom.Count)];
                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.down;
                        //m_Rooms.Add(Instantiate(GameManager.instance.m_start, m_position, Quaternion.identity));

                        Rooms l_room = new Rooms();
                        if (y == 0)
                        {
                            l_room.m_lock = true;
                        }
                        m_mapPosition.Add(m_position, l_room);
                    }
                    break;

                case 2:
                    m_position = m_roomsLeft[GameManager.instance.m_Random.Next(0, m_roomsLeft.Count)];
                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.left;
                        //m_Rooms.Add(Instantiate(GameManager.instance.m_start, m_position, Quaternion.identity));

                        Rooms l_room = new Rooms();
                        if (y == 0)
                        {
                            l_room.m_lock = true;
                        }
                        m_mapPosition.Add(m_position, l_room);
                    }
                    break;

                case 3:
                    m_position = m_roomsTop[GameManager.instance.m_Random.Next(0, m_roomsTop.Count)];
                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.up;
                        //m_Rooms.Add(Instantiate(GameManager.instance.m_start, m_position, Quaternion.identity));

                        Rooms l_room = new Rooms();
                        if (y == 0)
                        {
                            l_room.m_lock = true;
                        }

                        m_mapPosition.Add(m_position, l_room);
                    }
                    break;
            }
            

            for(int y = 2; y < GameManager.instance.Levels[0].NBranche[i].Room;y++)
            {
                GenerateBranche(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)], (i+2) % 4);
            }
        }

        GenerateRoom(GameManager.instance.m_end);

        SpawnDungeon();
    }

    private void GenerateRoom(GameObject p_room)
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

            Rooms l_tempo = new Rooms();
            if (m_mapPosition.TryGetValue(l_position, out l_tempo))
            {
                l_spawn = false;
                l_position = m_position;
            }

        } while (!l_spawn);

        m_last = (l_random + 2) % 4;
        m_position = l_position;
        //m_Rooms.Add(Instantiate(p_room, m_position, Quaternion.identity));
        m_mapPosition.Add(m_position, new Rooms());
        switch (m_last)
        {
            case 2: 
                if (m_position.x >= m_roomsRight[0].x)
                {
                    if(m_position.x > m_roomsRight[0].x)
                    {
                        m_roomsRight.Clear();
                    }
                    m_roomsRight.Add(m_position);
                }                  
                break;

            case 3:
                if (m_position.y <= m_roomsBottom[0].y)
                {
                    if (m_position.y < m_roomsBottom[0].y)
                    {
                        m_roomsBottom.Clear();
                    }
                    m_roomsBottom.Add(m_position);
                }                    
                break;

            case 0:
                if (m_position.x <= m_roomsLeft[0].x)
                {
                    if (m_position.x < m_roomsLeft[0].x)
                    {
                        m_roomsLeft.Clear();
                    }
                    m_roomsLeft.Add(m_position);
                }             
                break;

            case 1:
                if (m_position.y >= m_roomsTop[0].y)
                {
                    if (m_position.y > m_roomsTop[0].y)
                    {
                        m_roomsTop.Clear();
                    }
                    m_roomsTop.Add(m_position);
                }                   
                break;
        }


        bool l_top = false;
        bool l_bottom = false;
        bool l_left = false;
        bool l_right = false;
        Vector2 [] l_Dir = {Vector2.down, Vector2.up ,Vector2.right,Vector2.left};

        for(int i = 0;i<l_Dir.Length;i++)
        {
            Rooms l_tempo = new Rooms();
            switch(i)
            {
                case 0: l_bottom = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 1: l_top = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 2: l_right = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 3: l_left = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
            }
           
        }
        /*
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
        */
        if (l_top && l_bottom && l_left && l_right)
        {
            m_last = -1;
            switch(GameManager.instance.m_Random.Next(0,4))
            {
                case 0: m_position = m_roomsTop[GameManager.instance.m_Random.Next(0, m_roomsTop.Count)]; break;
                case 1: m_position = m_roomsBottom[GameManager.instance.m_Random.Next(0, m_roomsBottom.Count)]; break;
                case 2: m_position = m_roomsLeft[GameManager.instance.m_Random.Next(0, m_roomsLeft.Count)]; break;
                case 3: m_position = m_roomsRight[GameManager.instance.m_Random.Next(0, m_roomsRight.Count)]; break;
            }               
        } 
    }

    private void GenerateBranche(GameObject p_room,int p_deleteDir)
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
            } while (l_random == m_last || l_random == p_deleteDir);

            switch (l_random)
            {
                case 0: l_position += Vector3.right; break;

                case 1: l_position += Vector3.down; break;

                case 2: l_position += Vector3.left; break;

                case 3: l_position += Vector3.up; break;
            }

            Rooms l_tempo = new Rooms();
            if (m_mapPosition.TryGetValue(l_position, out l_tempo))
            {
                l_spawn = false;
                l_position = m_position;
            }
        } while (!l_spawn);

        m_last = (l_random + 2) % 4;
        m_position = l_position;
        //m_Rooms.Add(Instantiate(p_room, m_position, Quaternion.identity));
        m_mapPosition.Add(m_position, new Rooms());

        switch ((p_deleteDir+2)%4)
        {
            case 2:
                if (m_position.x >= m_bRoomsRight[0].x)
                {
                    if (m_position.x > m_bRoomsRight[0].x)
                    {
                        m_roomsRight.Clear();
                    }                    
                    m_roomsRight.Add(m_position);
                }
                break;

            case 3:
                if (m_position.y <= m_bRoomsBottom[0].y)
                {
                    if (m_position.y < m_bRoomsBottom[0].y)
                    {
                        m_roomsBottom.Clear();
                    }                    
                    m_roomsBottom.Add(m_position);
                }
                break;

            case 0:
                if (m_position.x <= m_bRoomsLeft[0].x)
                {
                    if (m_position.x < m_bRoomsLeft[0].x)
                    {
                        m_roomsLeft.Clear();
                    }                    
                    m_roomsLeft.Add(m_position);
                }
                break;

            case 1:
                if (m_position.y >= m_bRoomsTop[0].y)
                {
                    if (m_position.y > m_bRoomsTop[0].y)
                    {
                        m_roomsTop.Clear();
                    }                    
                    m_roomsTop.Add(m_position);
                }
                break;
        }

        bool l_top = false;
        bool l_bottom = false;
        bool l_left = false;
        bool l_right = false;
        Vector2[] l_Dir = { Vector2.down, Vector2.up, Vector2.right, Vector2.left };

        for (int i = 0; i < l_Dir.Length; i++)
        {
            Rooms l_tempo = new Rooms();
            switch (i)
            {
                case 0: l_bottom = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 1: l_top = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 2: l_right = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 3: l_left = m_mapPosition.TryGetValue(m_position + l_Dir[i], out l_tempo); break;
            }

        }
        if (l_top && l_bottom && l_left && l_right)
        {
            m_last = -1;
            switch ((p_deleteDir+2)%4)
            {
                case 0: m_position = m_roomsTop[GameManager.instance.m_Random.Next(0, m_roomsTop.Count)]; break;
                case 1: m_position = m_roomsBottom[GameManager.instance.m_Random.Next(0, m_roomsBottom.Count)]; break;
                case 2: m_position = m_roomsLeft[GameManager.instance.m_Random.Next(0, m_roomsLeft.Count)]; break;
                case 3: m_position = m_roomsRight[GameManager.instance.m_Random.Next(0, m_roomsRight.Count)]; break;
            }
        }
    }

    private void SpawnDungeon()
    {
        foreach(var l_room in m_mapPosition)
        {
            Instantiate(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)], 
                new Vector3(l_room.Key.x * 10, l_room.Key.y * 8, 0), Quaternion.identity);

        }

    }

}
