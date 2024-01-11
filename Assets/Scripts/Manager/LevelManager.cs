using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelManager 
{
    [SerializeField] private int m_nRoom;
    [SerializeField] private int m_dificulty;
    public int Room { get => m_nRoom; }
    public int Dificulty {  get => m_dificulty; }
}
