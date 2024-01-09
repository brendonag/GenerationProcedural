using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomParams
{
    public int m_id;
    public int m_numberOfDoors;
    public int m_numberOfTraps;

    public bool m_top;
    public bool m_bot;
    public bool m_left;
    public bool m_right;

    public bool m_isStart;
    public bool m_isEnd;
}
