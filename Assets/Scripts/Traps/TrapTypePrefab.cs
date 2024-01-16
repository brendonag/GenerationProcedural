using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;


public class TrapTypePrefab : MonoBehaviour
{
    public enum TrapType
    {
        Slow,
        Spike,
        Blade,
        Projectile,
    }

    public GameObject spikeTrapPrefab;
    public GameObject bladeTrapPrefab;
    public GameObject projectileTrapPrefab;
    public GameObject slowTrapPrefab;

    void Start()
    {
        GenerateRandomTrap();
    }

    void GenerateRandomTrap()
    {
        TrapType randomType = (TrapType)Random.Range(0, System.Enum.GetValues(typeof(TrapType)).Length);

        GameObject selectedTrapPrefab = null;

        switch (randomType)
        {
            case TrapType.Spike:
                selectedTrapPrefab = spikeTrapPrefab;
                break;
            case TrapType.Blade:
                selectedTrapPrefab = bladeTrapPrefab;
                break;
            case TrapType.Projectile:
                selectedTrapPrefab = projectileTrapPrefab;
                break;
            case TrapType.Slow:
                selectedTrapPrefab = slowTrapPrefab;
                break;
            default:
                // Handle default case
                break;
        }
    
    }

 }

