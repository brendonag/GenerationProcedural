using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationRoomGraph : MonoBehaviour
{
    private List<RoomParams> m_listRooms = new List<RoomParams>();
    public List<RoomParams> MyListRooms
    {
        get { return m_listRooms; }
        set { m_listRooms = value; }
    }

    [SerializeField] private int m_maxLinkRooms;
    private int m_iteration = 0;

    void Start()
    {
        Init(m_maxLinkRooms);
    }

    private void Init(int p_max)
    {
        m_listRooms.Add(InitRoom());

        GenerateNextRoom(p_max);
    }

    //GENERATE ROOMS
    public void GenerateNextRoom(int p_max)
    {
        while(m_iteration != p_max)
        {
            m_iteration++;
        }

        Debug.Log(m_listRooms[m_listRooms.Count - 1].m_roomType);
        EndRoom();

        //DebugDoorPosition();
    }

    private void GoodPath(ref RoomParams p_room)
    {
        switch (p_room.m_roomType)
        {
            case RoomType.Top:
                break;
            case RoomType.Bottom:
                break;
            case RoomType.Left:
                break;
            case RoomType.Right:
                break;

            case RoomType.LeftAngleTop: 
                break;
            case RoomType.LeftAngleBottom:
                break;
            case RoomType.RightAngleTop:
                break;
            case RoomType.RightAngleBottom: 
                break;

            case RoomType.VerticalCorridor: 
                break;
            case RoomType.HorizontalCorridor: 
                break;

            case RoomType.ThreeLeft: 
                break;
            case RoomType.ThreeRight: 
                break;
            case RoomType.ThreeBottom:
                break;
            case RoomType.ThreeTop:
                break;

            case RoomType.Carrefour:
                break;

        }
    }

    //ROOMS
    private RoomParams InitRoom()
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isStart = true;
        l_r.m_numberOfDoors = 1;

        CreateDoors(ref l_r, l_r.m_numberOfDoors);

        return l_r;
    }

    private RoomParams EndRoom()
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isEnd = true;
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

    //DOORS
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
                    l_roomDoor.m_roomType = RoomType.Top;
                    break;
                case 1:
                    l_roomDoor.m_bot = true;
                    l_roomDoor.m_roomType = RoomType.Bottom;
                    break;
                case 2:
                    l_roomDoor.m_left = true;
                    l_roomDoor.m_roomType = RoomType.Left;
                    break;
                case 3:
                    l_roomDoor.m_right = true;
                    l_roomDoor.m_roomType = RoomType.Right;
                    break;
            }

            l_listDoorPosition.RemoveAt(l_rnd);
            l_maxDoorNumber--;
        }
        
        if (l_roomDoor.m_bot && l_roomDoor.m_top) { l_roomDoor.m_roomType = RoomType.VerticalCorridor; }
        if (l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.HorizontalCorridor; }

        if (l_roomDoor.m_bot && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.RightAngleBottom; }
        if (l_roomDoor.m_bot && l_roomDoor.m_left) { l_roomDoor.m_roomType = RoomType.LeftAngleBottom; }

        if (l_roomDoor.m_top && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.RightAngleTop; }
        if (l_roomDoor.m_top && l_roomDoor.m_left) { l_roomDoor.m_roomType = RoomType.LeftAngleTop; }

        if (l_roomDoor.m_bot && l_roomDoor.m_top && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.ThreeRight; }
        if (l_roomDoor.m_bot && l_roomDoor.m_top && l_roomDoor.m_left) { l_roomDoor.m_roomType = RoomType.ThreeLeft; }
        if (l_roomDoor.m_top && l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.ThreeTop; }
        if (l_roomDoor.m_bot && l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.ThreeBottom; }

        if (l_roomDoor.m_bot && l_roomDoor.m_top && l_roomDoor.m_left && l_roomDoor.m_right) { l_roomDoor.m_roomType = RoomType.Carrefour; }
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
            Debug.Log(m_listRooms[i].m_roomType);
        }
    }
}
