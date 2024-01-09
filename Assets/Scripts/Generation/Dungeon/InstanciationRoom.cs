using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class InstanciationRoom : MonoBehaviour
{
    public List<RoomParams> m_listRooms = new List<RoomParams>();
    RoomParams roomParams;

    // Start is called before the first frame update
    void Start()
    {
        Instanciation(3);

        print(m_listRooms.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Instanciation(int p_index)
    {
        m_listRooms.Add(InitRoom(true, 4));

        for(int i = 0; i < p_index; i++)
        {
            m_listRooms.Add(CreateRoom(8));
        }

        m_listRooms.Add(EndRoom(true));
    }


    //ROOMS
    private RoomParams InitRoom(bool p_s, int p_door)
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isStart = p_s;
        l_r.m_numberOfDoors = p_door -1;
        return l_r;
    }

    private RoomParams EndRoom(bool p_e)
    {
        RoomParams l_r = new RoomParams();
        l_r.m_isEnd = p_e;
        l_r.m_numberOfDoors = 1;
        return l_r;
    }

    private RoomParams CreateRoom(int p_door)
    {
        RoomParams l_r = new RoomParams();
        l_r.m_numberOfDoors = p_door;
        return l_r;
    }

    private void Tutu()
    {
       // int l_tutu = Random.Range(0, 1);
    }
}
