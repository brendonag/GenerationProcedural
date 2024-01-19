using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heart collectible
/// </summary>
public class HeartCollectible : ACollectible {
    [SerializeField] private int _gainLife;
    protected override void OnCollect()
    {
        Player.Instance.life += _gainLife;
    }
}
