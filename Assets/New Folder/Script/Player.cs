using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AdaptivePerformance.VisualScripting;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform[] RoutePoints;

    [Range(0, 200)]
    public float Speed = 50f;

    [Range(0, 50)]
    public float MoveSpeed = 20f;
    public float MoveRange = 40f;

    public float _initialLife = 100;
    public float Life = 100;
    public Image LifeGage;

    public Bullet BulletPrefab;

    bool _isHitRoutePoint;

    IEnumerator Move()
    {
        var prevPointpos = transform.position;
        var basePosition = transform.position;
        var movedPos = Vector2.zero;

        foreach (var nextPoint in RoutePoints)
        {
            _isHitRoutePoint = false;

            while (!_isHitRoutePoint)
            {
                var vec = nextPoint.position - prevPointpos;
                vec.Normalize();


                // プレイヤーの移動
                basePosition += vec * Speed * Time.deltaTime;

                // 上下左右に移動する処理
                // 行列によるベクトル変換に関する知識を利用しています。

                movedPos.x += Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
                movedPos.y += Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
                movedPos = Vector2.ClampMagnitude(movedPos, MoveRange);
                var worldMovedPos = Matrix4x4.Rotate(transform.rotation).MultiplyVector(movedPos);

                // ルート上の位置に上下左右の移動量を加えている
                transform.position = basePosition + worldMovedPos;

                transform.position += vec * Time.deltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), 5f);

                yield return null;

            }
            prevPointpos = nextPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RoutePoint")
        {
            other.gameObject.SetActive(false);
            _isHitRoutePoint = true;
        }
        else if (other.gameObject.tag == "Enemy")
        {
            Life -= 10f;
            LifeGage.fillAmount = Life / _initialLife;

            other.gameObject.SetActive(false);
            Object.Destroy(other.gameObject); //当たった敵は削除する

            if (Life <= 0)
            {
                Camera.main.transform.SetParent(null);
                gameObject.SetActive(false);
                var sceneManager = Object.FindObjectOfType<SceneManager>();
                sceneManager.ShowGameClear();
            }
        }
        else if (other.gameObject.tag == "ClearRoutePoint")
        {
            var sceneManager = Object.FindObjectOfType<SceneManager>();
            sceneManager.ShowGameClear();
            _isHitRoutePoint = true;
        }
    }


    void Start()
    {
        StartCoroutine(Move());
    }

    public void ShotBullet(Vector3 targetPos)
    {
        var bullet = Object.Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        bullet.Init(transform.position, targetPos);
    }
}
