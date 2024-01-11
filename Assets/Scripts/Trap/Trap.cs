using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private TrapType m_type;

    public TrapType Type { get { return m_type; } set { m_type = value; } }


}
