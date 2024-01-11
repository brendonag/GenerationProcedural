using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGenerateDungeon : MonoBehaviour
{
    private Vector3 m_position = Vector3.zero;
    private int m_last = -1;
    private List<GameObject> m_Rooms = new List<GameObject>();
    void Start()
    {
        m_Rooms.Add(Instantiate(GameManager.instance.m_Hub[0],m_position, Quaternion.identity));

        for(int i = 1; i < GameManager.instance.Levels[0].Room; i++)
        {
            int l_random = 0;
            bool l_spawn = false;
            Vector3 l_position = m_position;
            do
            {
                l_spawn = true;
                do
                {
                    l_random = GameManager.instance.m_Random.Next(0, 4);
                } while (l_random == m_last);

                switch (l_random)
                {
                    case 0: l_position += Vector3.up; break;

                    case 1: l_position += Vector3.down; break;

                    case 2: l_position += Vector3.left; break;

                    case 3: l_position += Vector3.right; break;
                }

                foreach (GameObject l_room in m_Rooms)
                {
                    if(l_position == l_room.transform.position)
                    {
                        l_spawn = false;
                        l_position = m_position;
                    }                        
                }

            } while (!l_spawn);
            
            m_last = l_random;
            m_position = l_position;
            m_Rooms.Add(Instantiate(GameManager.instance.m_Hub[0], m_position, Quaternion.identity));
        }
    }

}
