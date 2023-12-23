using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyLife
{
    GameObject Habitacio{
        get;
        set;
    }    
    void changeLife(int damage);
}
