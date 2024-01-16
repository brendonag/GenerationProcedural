using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOut_Following : MonoBehaviour
{
    public GameObject m_player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = m_player.transform.position;
    }
}
