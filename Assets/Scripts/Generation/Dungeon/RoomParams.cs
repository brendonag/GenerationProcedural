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


    public bool m_isLeftAngleTop;
    public bool m_isRightAngleTop;

    public bool m_isLeftAngleBot;
    public bool m_isRightAngleBot;

    public bool m_isVerticalCorridor;
    public bool m_isHorizontalCorridor;

    public bool m_isThreeLeft;
    public bool m_isThreeRight;
    public bool m_isThreeTop;
    public bool m_isThreeBot;

    public bool m_isCarrefour;

    public bool m_isStart;
    public bool m_isEnd;
}
