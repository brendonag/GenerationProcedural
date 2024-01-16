using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelManager 
{
    [SerializeField] private int m_nRoom;
    [SerializeField] private List<Branche> m_nBranche;
    [SerializeField] private int m_dificulty;
    public int Room { get => m_nRoom; }

    public List<Branche> NBranche {  get => m_nBranche; }

    public int Dificulty {  get => m_dificulty; }
}

[Serializable]
public struct Branche
{
    [SerializeField] [Min(3)]private int m_nRoom;
    public int Room { get => m_nRoom; }
}

public struct Rooms
{
    public bool m_lock;
    public bool m_start;
    public bool m_end;
    public Vector2 m_position;

}
