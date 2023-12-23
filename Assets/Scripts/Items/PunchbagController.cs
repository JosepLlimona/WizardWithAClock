using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PunchbagController : MonoBehaviour, EnemyLife
{
    [SerializeField]
    private TextMeshProUGUI damageText;
    private ParticleSystem particles;
    public GameObject habitacio;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void changeLife(int damage)
    {
        damageText.text = damage.ToString();
        particles.Play();
    }

    public GameObject Habitacio{
        get{
            return habitacio;
        }
        set{
            habitacio = value;
        }
    }
}
