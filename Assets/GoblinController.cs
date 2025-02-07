using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        speed = 2.5f;
    }
}
