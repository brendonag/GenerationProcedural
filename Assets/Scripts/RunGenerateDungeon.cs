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
    [SerializeField] private List<Dictionary<Vector2,Rooms>> m_mapPosition = new List<Dictionary<Vector2, Rooms>>();

    List<Vector2> m_roomsLeft = new List<Vector2>();
    List<Vector2> m_roomsRight = new List<Vector2>();
    List<Vector2> m_roomsTop = new List<Vector2>();
    List<Vector2> m_roomsBottom = new List<Vector2>();

    List<Vector2> m_bRoomsLeft = new List<Vector2>();
    List<Vector2> m_bRoomsRight = new List<Vector2>();
    List<Vector2> m_bRoomsTop = new List<Vector2>();
    List<Vector2> m_bRoomsBottom = new List<Vector2>();

    [SerializeField] private GameObject m_Player;

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
        for (int i = 0; i < GameManager.instance.Levels.Length; i++)
        {
            GameManager.instance.SetDifficulty(i);
            m_mapPosition.Add(new Dictionary<Vector2, Rooms>());
            Generate();
            
            if(i == 0)
            {
                GameManager.instance.m_player = Instantiate(m_Player, Vector3.zero, Quaternion.identity);
            }

            m_roomsTop.Clear();
            m_roomsRight.Clear();
            m_roomsBottom.Clear();
            m_roomsLeft.Clear();

            m_bRoomsTop.Clear();
            m_bRoomsRight.Clear();
            m_bRoomsBottom.Clear();
            m_bRoomsLeft.Clear();

            m_roomsTop.Add(Vector3.zero);
            m_roomsRight.Add(Vector3.zero);
            m_roomsBottom.Add(Vector3.zero);
            m_roomsLeft.Add(Vector3.zero);

            m_bRoomsTop.Add(Vector3.zero);
            m_bRoomsRight.Add(Vector3.zero);
            m_bRoomsBottom.Add(Vector3.zero);
            m_bRoomsLeft.Add(Vector3.zero);

            m_position = Vector2.zero;
        }
        GameManager.instance.SetDifficulty(0);

    }

    private void GenerateRoom(GameObject p_room, bool p_end = false)
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

            Rooms l_tempoHere = new Rooms();
            if (m_mapPosition[GameManager.instance.Difficulty-1].TryGetValue(l_position, out l_tempoHere))
            {
                l_spawn = false;
                l_position = m_position;
            }

        } while (!l_spawn);

        m_last = (l_random + 2) % 4;
        m_position = l_position;
        Rooms l_tempo = new Rooms();

        if(p_end == true)
        {
            l_tempo.m_end = true;
        }

        m_mapPosition[GameManager.instance.Difficulty - 1].Add(m_position, l_tempo);
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
            Rooms l_tempoHere = new Rooms();
            switch(i)
            {
                case 0: l_bottom = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempoHere); break;
                case 1: l_top = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempoHere); break;
                case 2: l_right = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempoHere); break;
                case 3: l_left = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempoHere); break;
            }
           
        }

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

    private void GenerateBranche(GameObject p_room,int p_deleteDir, bool p_end = false)
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
            if (m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(l_position, out l_tempo))
            {
                l_spawn = false;
                l_position = m_position;
            }
        } while (!l_spawn);

        m_last = (l_random + 2) % 4;
        m_position = l_position;
        Rooms l_tempoS = new Rooms();
        if (p_end == true)
        {
            l_tempoS.m_endB = true;
        }
        m_mapPosition[GameManager.instance.Difficulty - 1].Add(m_position,l_tempoS) ;

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
                case 0: l_bottom = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 1: l_top = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 2: l_right = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempo); break;
                case 3: l_left = m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position + l_Dir[i], out l_tempo); break;
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
        foreach(var l_room in m_mapPosition[GameManager.instance.Difficulty-1])
        {
            GameObject l_roomObject;
            if(l_room.Value.m_start)
            {
                l_roomObject = Instantiate(GameManager.instance.m_start,
                new Vector3(l_room.Key.x * 11 - 5.5f, l_room.Key.y * 9 - 4.5f, 0), Quaternion.identity);    
                l_roomObject.GetComponent<Room>().isStartRoom = true;
                GameManager.instance.Levels[GameManager.instance.Difficulty-1].m_firstRoom = l_roomObject.GetComponent<Room>();
            }
            else if(l_room.Value.m_end)
            {
                l_roomObject = Instantiate(GameManager.instance.m_end,
                new Vector3(l_room.Key.x * 11 - 5.5f, l_room.Key.y * 9 - 4.5f, 0), Quaternion.identity);
            }
            else if(l_room.Value.m_endB)
            {
                l_roomObject = Instantiate(GameManager.instance.m_roomKey,
                new Vector3(l_room.Key.x * 11 - 5.5f, l_room.Key.y * 9 - 4.5f, 0), Quaternion.identity);
            }
            else
            {
                l_roomObject = Instantiate(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)],
                new Vector3(l_room.Key.x * 11 - 5.5f, l_room.Key.y * 9 - 4.5f, 0), Quaternion.identity);
            }

            l_roomObject.GetComponent<Room>().position = new Vector2Int((int)l_room.Key.x, (int)l_room.Key.y);
            l_roomObject.transform.SetParent(GameManager.instance.Levels[GameManager.instance.Difficulty-1].m_ObjLevel.transform);


            Vector2[] l_Dir = { Vector2.down, Vector2.up, Vector2.right, Vector2.left };


            if (l_room.Value.m_lockO)
            {
                l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.WEST, l_roomObject.transform.position).SetState(Door.STATE.CLOSED);
            }
            if (l_room.Value.m_lockE)
            {
                l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.EAST, l_roomObject.transform.position).SetState(Door.STATE.CLOSED);
            }
            if (l_room.Value.m_lockS)
            {
                l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.SOUTH, l_roomObject.transform.position).SetState(Door.STATE.CLOSED);
            }
            if (l_room.Value.m_lockN)
            {
                l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.NORTH, l_roomObject.transform.position).SetState(Door.STATE.CLOSED);
            }
            
            for (int i = 0; i < l_Dir.Length; i++)
            {
                Rooms l_tempo = new Rooms();
                switch (i)
                {
                    case 0: 
                        if(!m_mapPosition[GameManager.instance.Difficulty - 1] .TryGetValue(l_room.Key + l_Dir[i], out l_tempo))
                        {
                            l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.SOUTH, l_roomObject.transform.position).SetState(Door.STATE.WALL);       
                        }
                        
                        break;

                    case 1:
                        if (!m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(l_room.Key + l_Dir[i], out l_tempo))
                        {
                           l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.NORTH, l_roomObject.transform.position).SetState(Door.STATE.WALL);
                        }

                        break;

                    case 2:
                        if (!m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(l_room.Key + l_Dir[i], out l_tempo))
                        {
                           l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.EAST, l_roomObject.transform.position).SetState(Door.STATE.WALL);
                        }

                        break;

                    case 3:
                        if (!m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(l_room.Key + l_Dir[i], out l_tempo))
                        {
                            l_roomObject.GetComponent<Room>().GetDoor(Utils.ORIENTATION.WEST, l_roomObject.transform.position).SetState(Door.STATE.WALL);
                        }

                        break;
                }

            }
        }
        

    }

    private void Generate()
    {
        Rooms l_start = new Rooms();
        l_start.m_start = true;
        m_mapPosition[GameManager.instance.Difficulty-1].Add(m_position, l_start);

        for (int i = 1; i < GameManager.instance.Levels[GameManager.instance.Difficulty - 1].Room - 1; i++)
        {
            GenerateRoom(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)]);
        }

        int l_startB = GameManager.instance.m_Random.Next(0, 4);

        for (int i = l_startB; i < GameManager.instance.Levels[GameManager.instance.Difficulty - 1].NBranche.Count + l_startB; i++)
        {
            Rooms l_tempo = new Rooms();
            switch (i % 4)
            {
                case 0:
                    m_position = m_roomsRight[GameManager.instance.m_Random.Next(0, m_roomsRight.Count)];

                    if(m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position,out l_tempo) && i!=l_startB)
                    {
                        l_tempo.m_lockE = true;
                        m_mapPosition[GameManager.instance.Difficulty - 1][m_position] = l_tempo;
                    }
                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.right;

                        Rooms l_room = new Rooms();
                        if (y == 0 && i != l_startB)
                        {
                            l_room.m_lock = true;
                            l_room.m_lockO = true;
                        }
                        m_mapPosition[GameManager.instance.Difficulty - 1].Add(m_position, l_room);
                    }
                    break;

                case 1:
                    m_position = m_roomsBottom[GameManager.instance.m_Random.Next(0, m_roomsBottom.Count)];
                    if (m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position, out l_tempo) && i != l_startB)
                    {
                        l_tempo.m_lockS = true;
                        m_mapPosition[GameManager.instance.Difficulty - 1][m_position] = l_tempo;
                    }
                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.down;

                        Rooms l_room = new Rooms();
                        if (y == 0 && i != l_startB)
                        {
                            l_room.m_lock = true;
                            l_room.m_lockN = true;
                        }
                        m_mapPosition[GameManager.instance.Difficulty - 1].Add(m_position, l_room);
                    }
                    break;

                case 2:
                    m_position = m_roomsLeft[GameManager.instance.m_Random.Next(0, m_roomsLeft.Count)];

                    if (m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position, out l_tempo) && i != l_startB)
                    {
                        l_tempo.m_lockO = true;
                        m_mapPosition[GameManager.instance.Difficulty - 1][m_position] = l_tempo;
                    }

                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.left;

                        Rooms l_room = new Rooms();
                        if (y == 0 && i != l_startB)
                        {
                            l_room.m_lock = true;
                            l_room.m_lockE = true;
                        }
                        m_mapPosition[GameManager.instance.Difficulty - 1].Add(m_position, l_room);
                    }
                    break;

                case 3:
                    m_position = m_roomsTop[GameManager.instance.m_Random.Next(0, m_roomsTop.Count)];

                    if (m_mapPosition[GameManager.instance.Difficulty - 1].TryGetValue(m_position, out l_tempo) && i != l_startB)
                    {
                        l_tempo.m_lockN = true;
                        m_mapPosition[GameManager.instance.Difficulty - 1][m_position] = l_tempo;
                    }

                    for (int y = 0; y < 2; y++)
                    {
                        m_position += Vector2.up;

                        Rooms l_room = new Rooms();
                        if (y == 0 && i != l_startB)
                        {
                            l_room.m_lock = true;
                            l_room.m_lockS = true;
                        }

                        m_mapPosition[GameManager.instance.Difficulty - 1].Add(m_position, l_room);
                    }
                    break;
            }


            for (int y = 2; y < GameManager.instance.Levels[GameManager.instance.Difficulty - 1].NBranche[i-l_startB].Room; y++)
            {
                if(y == GameManager.instance.Levels[GameManager.instance.Difficulty - 1].NBranche[i - l_startB].Room - 1 && i != GameManager.instance.Levels[GameManager.instance.Difficulty - 1].NBranche.Count + l_startB - 1)
                {
                    GenerateBranche(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)], (i + 2) % 4,true);
                }
                else
                {
                    GenerateBranche(GameManager.instance.m_Rooms[GameManager.instance.m_Random.Next(0, GameManager.instance.m_Rooms.Count)], (i + 2) % 4);
                }
                
            }
       
        }

        GenerateRoom(GameManager.instance.m_end, true);

        SpawnDungeon();

    }

}
