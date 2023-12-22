using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PunchbagController : MonoBehaviour, EnemyLife
{
    [SerializeField]
    private TextMeshProUGUI damageText;
    private ParticleSystem particles;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void changeLife(int damage)
    {
        damageText.text = damage.ToString();
        particles.Play();
    }
}
