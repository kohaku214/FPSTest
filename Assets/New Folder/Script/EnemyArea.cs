using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyArea : MonoBehaviour
{
    public GameObject[] EnemyList;

    void Start()
    {
        foreach (var enemy in EnemyList)
        {
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            foreach (var enemy in EnemyList)
            {
                enemy.SetActive(true);
            }

            //��x�G��L����������A������Ȃ��悤�ɂ���
            var collider = GetComponent<Collider>();
            collider.enabled = false;
        }
    }
}