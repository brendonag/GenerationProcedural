using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationRoomGraph : MonoBehaviour
{
    public List<RoomParams> m_listRooms = new List<RoomParams>();

    // Start is called before the first frame update
    void Start()
    {
        Instanciation(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Instanciation(int p_index)
    {
        m_listRooms.Add(InitRoom(true, 4));

        int l_randDoor = RandomInt(2, 5);
        int l_randTraps= RandomInt(0, 8);

        for (int i = 0; i < p_index; i++)
        {
            m_listRooms.Add(CreateRoom(i, l_randDoor, l_randTraps));
        }

        m_listRooms.Add(EndRoom(true));

        DebugDoorPosition();
    }

    //ROOMS
    private RoomParams InitRoom(bool p_s, int p_door)
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isStart = p_s;
        l_r.m_numberOfDoors = 1;

        RoomParams l_getDoor = CreateDoors(l_r.m_numberOfDoors);

        l_r.m_top = l_getDoor.m_top;
        l_r.m_bot = l_getDoor.m_bot;
        l_r.m_left = l_getDoor.m_left;
        l_r.m_right = l_getDoor.m_right;

        return l_r;
    }

    private RoomParams EndRoom(bool p_e)
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isEnd = p_e;
        l_r.m_numberOfDoors = 1;

        RoomParams l_getDoor = CreateDoors(l_r.m_numberOfDoors);

        l_r.m_top = l_getDoor.m_top;
        l_r.m_bot = l_getDoor.m_bot;
        l_r.m_left = l_getDoor.m_left;
        l_r.m_right = l_getDoor.m_right;

        return l_r;
    }

    private RoomParams CreateRoom(int p_id, int p_door, int p_traps)
    {
        RoomParams l_r = new RoomParams();

        l_r.m_id = p_id;
        l_r.m_numberOfDoors = p_door;
        l_r.m_numberOfTraps = p_traps;

        RoomParams l_getDoor = CreateDoors(p_door);

        l_r.m_top = l_getDoor.m_top;
        l_r.m_bot = l_getDoor.m_bot;
        l_r.m_left = l_getDoor.m_left;
        l_r.m_right = l_getDoor.m_right;

        return l_r;
    }

    private RoomParams CreateDoors(int p_door)
    {
        RoomParams l_roomDoor = new RoomParams();

        int l_maxDoorNumber = p_door;

        List<int> l_listDoorPosition = new List<int>();
        for (int i = 0; i <= 3; i++) { l_listDoorPosition.Add(i); }

        while (l_maxDoorNumber != 0)
        {
            int l_rnd = RandomInt(0, l_listDoorPosition.Count);
            int l_doorRnd= l_listDoorPosition[l_rnd];

            
            switch (l_doorRnd)
            {
                case 0:
                    l_roomDoor.m_top = true;
                    break;
                case 1:
                    l_roomDoor.m_bot = true;
                    break;
                case 2:
                    l_roomDoor.m_left = true;
                    break;
                case 3:
                    l_roomDoor.m_right = true;
                    break;
            }

            l_listDoorPosition.RemoveAt(l_rnd);
            l_maxDoorNumber--;
        }

        return l_roomDoor;
    }

    //OTHER FUNCTIONS
    private int RandomInt(int p_min, int p_max)
    {
        int l_r = Random.Range(p_min, p_max);

        return l_r;
    }

    private void DebugDoorPosition()
    { 
        for (int i = 0; i < m_listRooms.Count; i++)
        {
            if (m_listRooms[i].m_top) { Debug.Log("Room " + i + " door isTop"); }
            if (m_listRooms[i].m_bot) { Debug.Log("Room " + i + " door isBot"); }
            if (m_listRooms[i].m_right) { Debug.Log("Room " + i + " door isLeft"); }
            if (m_listRooms[i].m_left) { Debug.Log("Room " + i + " door isRight"); }
        }
    }
}
