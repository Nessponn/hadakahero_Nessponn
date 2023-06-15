using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPunch : MonoBehaviour
{
    //敵の攻撃用スクリプト

    private int Punch_Power;
    private int direction_num = 0;
    private float Bullet_Speed;//弾速

    private GameObject pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //敵の移動処理
    void Update()
    {
        if(direction_num == 1)//1 = 右
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(Bullet_Speed,0);
        }
        else if (direction_num == 2)//2 = 左
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-Bullet_Speed,0);
        }
        else if (direction_num == 3)//3 = 上
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,Bullet_Speed);
        }
        else if (direction_num == 4)//4 = 下
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,-Bullet_Speed);
        }

        if(Mathf.Abs(this.gameObject.transform.position.x - pos.transform.position.x) > 10 ||
            Mathf.Abs(this.gameObject.transform.position.y - pos.transform.position.y) > 10)
        {
            Destroy(this.gameObject);
        }
    }

    //出現時の位置指定メソッド
    public void pos_Setter(GameObject obj)
    {
        pos = obj;
    }

    //敵の遠距離攻撃の弾速調整メソッド（遠距離攻撃の敵のみ）
    public void Bullet_Setter(float Speed)
    {
        Bullet_Speed = Speed;
    }

    //敵の移動方向指定メソッド
    public void Direction_Setter(int num)//1 = 右　2 = 左 3 = 上 4 = 下
    {
        direction_num = num;
    }

    //敵の攻撃力指定メソッド
    public void PowerSetter(int Power)
    {
        Punch_Power = Power;
    }

    //プレイヤーへのダメージ付与
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStatus>().TakeDamage(Punch_Power);
        }
    }
}
