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

        int l_randDoor = RandomInt(1, 5);
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

        CreateDoors(ref l_r, l_r.m_numberOfDoors);

        return l_r;
    }

    private RoomParams EndRoom(bool p_e)
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isEnd = p_e;
        l_r.m_numberOfDoors = 1;

        CreateDoors(ref l_r, l_r.m_numberOfDoors);

        return l_r;
    }

    private RoomParams CreateRoom(int p_id, int p_door, int p_traps)
    {
        RoomParams l_r = new RoomParams();

        l_r.m_id = p_id;
        l_r.m_numberOfDoors = p_door;
        l_r.m_numberOfTraps = p_traps;

        CreateDoors(ref l_r, p_door);

        return l_r;
    }

    private void CreateDoors(ref RoomParams l_roomDoor, int p_door)
    {
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
        
        if (l_roomDoor.m_bot && l_roomDoor.m_top) { l_roomDoor.m_isVerticalCorridor = true; }
        if (l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_isHorizontalCorridor = true; }

        if (l_roomDoor.m_bot && l_roomDoor.m_right) { l_roomDoor.m_isRightAngleBot = true; }
        if (l_roomDoor.m_bot && l_roomDoor.m_left) { l_roomDoor.m_isLeftAngleBot = true; }

        if (l_roomDoor.m_top && l_roomDoor.m_right) { l_roomDoor.m_isRightAngleTop = true; }
        if (l_roomDoor.m_top && l_roomDoor.m_left) { l_roomDoor.m_isLeftAngleTop = true; }

        if (l_roomDoor.m_bot && l_roomDoor.m_top && l_roomDoor.m_right) { l_roomDoor.m_isThreeRight = true; }
        if (l_roomDoor.m_bot && l_roomDoor.m_top && l_roomDoor.m_left) { l_roomDoor.m_isThreeLeft = true; }
        if (l_roomDoor.m_top && l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_isThreeTop = true; }
        if (l_roomDoor.m_bot && l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_isThreeBot = true; }

        if (l_roomDoor.m_bot && l_roomDoor.m_top && l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_isCarrefour = true; }
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

            if (m_listRooms[i].m_isVerticalCorridor) { Debug.Log("Room " + i + " isVerticalCorridor"); }
            if (m_listRooms[i].m_isHorizontalCorridor) { Debug.Log("Room " + i + " isHorizontalCorridor"); }

            if (m_listRooms[i].m_isRightAngleBot) { Debug.Log("Room " + i + " isRightAngleBot"); }
            if (m_listRooms[i].m_isLeftAngleBot) { Debug.Log("Room " + i + " isLeftAngleBot"); }

            if (m_listRooms[i].m_isRightAngleTop) { Debug.Log("Room " + i + " isRightAngleTop"); }
            if (m_listRooms[i].m_isLeftAngleTop) { Debug.Log("Room " + i + " isLeftAngleTop"); }

            if (m_listRooms[i].m_isThreeRight) { Debug.Log("Room " + i + " isThreeRight"); }
            if (m_listRooms[i].m_isThreeLeft) { Debug.Log("Room " + i + " isThreeLeft"); }
            if (m_listRooms[i].m_isThreeTop) { Debug.Log("Room " + i + " isThreeTop"); }
            if (m_listRooms[i].m_isThreeBot) { Debug.Log("Room " + i + " isThreeBot"); }

            if (m_listRooms[i].m_isCarrefour) { Debug.Log("Room " + i + " isCarrefour"); }
        }
    }
}
