using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nessponn
{
    public class EnemyController : MonoBehaviour
    {
        //敵の身動きを取れなくしたなどのオプションを実装する場合
        //こちらのスクリプトをつけ消しするだけで簡単に行えます

        public enum Type
        {
            Fighter,Gunner,
        }
        public Type Enemy_Type = Type.Fighter;

        [SerializeField] public float Speed = 0.5f;//移動速度

        [SerializeField] public int Power;//敵の攻撃力
        [SerializeField] public float Attack_Mag = 1.3f;//強攻撃の倍率
        [SerializeField] public float Attack_Fre = 3;//一秒間に行う攻撃の数
        [SerializeField] public float Search_Range = 0.5f;//プレイヤーのサーチ範囲

        [SerializeField] public float BulletSpeed = 1f;
        [SerializeField] public float BulletSpeed_Mag = 3f;

        private int hori;
        private int vert;

        private Animator Anim;//アニメーター

        private Rigidbody2D rbody;

        private bool Attack;//アタック中であるか

        [SerializeField] public GameObject Punch;
        private enum Move
        {
            Left,LowerLeft,UpperLeft,
            Right,LowerRight,UpperRight,
            Up,Down,Stop
        }
        Move _movedir = Move.Stop;
        // Start is called before the first frame update
        void Start()
        {
            rbody = GetComponent<Rigidbody2D>();
            Anim = GetComponent<Animator>();
            hori = 0;
            vert = 0;
        }

        // Update is called once per frame
        void Update()
        {
            var Player = GameObject.FindWithTag("Player");

            Vector3 localvec = this.transform.localPosition - Player.transform.localPosition;
            if(Mathf.Abs(localvec.x) <= Search_Range && Mathf.Abs(localvec.y) <= Search_Range)
            {
                _movedir = Move.Stop;
                if (!Attack)
                {
                    StartCoroutine(Attack_Trigger());
                }
            }
            else if (Player.transform.localPosition.x - this.transform.localPosition.x > Search_Range)//プレイヤーが左にいるとき
            {
                //敵の位置より高いか、低いか？
                //Debug.Log("プレイヤーが右にいるッ！！！");
                //敵の位置より高いか、低いか？
                if(Mathf.Abs(Player.transform.localPosition.y - this.transform.localPosition.y) <= Search_Range)
                {
                    _movedir = Move.Right;
                }
                else if (Player.transform.localPosition.y - this.transform.localPosition.y > Search_Range)//プレイヤーが上にいるとき
                {
                    _movedir = Move.UpperRight;
                }
                else if (Player.transform.localPosition.y - this.transform.localPosition.y < Search_Range)//プレイヤーが下にいるとき
                {
                    _movedir = Move.LowerRight;
                }
            }
            else if (Player.transform.localPosition.x - this.transform.localPosition.x < -Search_Range)//プレイヤーが右にいるとき
            {

                //Debug.Log("プレイヤーが左にいるッ！！！");

                if(Mathf.Abs(Player.transform.localPosition.y - this.transform.localPosition.y) <= Search_Range)
                {
                    _movedir = Move.Left;
                }
                else if (Player.transform.localPosition.y - this.transform.localPosition.y > Search_Range)//プレイヤーが上にいるとき
                {
                    _movedir = Move.UpperLeft;
                }
                else if (Player.transform.localPosition.y - this.transform.localPosition.y < Search_Range)//プレイヤーが下にいるとき
                {
                    _movedir = Move.LowerLeft;
                }
            }
            else if (Player.transform.localPosition.y - this.transform.localPosition.y > 0)//プレイヤーが上にいるとき
            {
                _movedir = Move.Up;
            }
            else if(Player.transform.localPosition.y - this.transform.localPosition.y < 0)//プレイヤーが上にいるとき
            {
                _movedir = Move.Down;
            }

            //プレイヤーがいる方向によって、_movedirの状態を変更する
            switch (_movedir)
            {
                    case Move.Stop:
                    //hori = 0;
                    //vert = 0;
                    rbody.velocity = new Vector2(0, 0);
                    //Debug.Log("停止");
                    break;
                    case Move.Left:
                    hori = -1;
                    vert = 0;
                        rbody.velocity = new Vector2(-Speed, 0);
                    //Debug.Log("左");

                        break;
                    case Move.LowerLeft:
                    hori = -1;
                    vert = -1;
                    rbody.velocity = new Vector2(-Speed / 2, -Speed );
                    //Debug.Log("左下");

                    break;
                    case Move.UpperLeft:
                    hori = -1;
                    vert = 1;
                    rbody.velocity = new Vector2(-Speed / 2, Speed );
                    //Debug.Log("左上");
                    break;
                    case Move.Right:
                    hori = 1;
                    vert = 0;
                    rbody.velocity = new Vector2(Speed, 0);
                    //Debug.Log("右");
                    hori = 1;
                    vert = 0;
                    break;
                    case Move.LowerRight:
                    hori = 1;
                    vert = -1;
                    rbody.velocity = new Vector2(Speed / 2, -Speed);
                   // Debug.Log("右下");
                    break;
                    case Move.UpperRight:
                    hori = 1;
                    vert = 1;
                    rbody.velocity = new Vector2(Speed / 2, Speed );
                   // Debug.Log("右上");
                    break;
                    case Move.Up:
                    hori = 0;
                    vert = 1;
                    rbody.velocity = new Vector2(0, Speed);
                    //Debug.Log("上");

                    break;
                    case Move.Down:
                    hori = 0;
                    vert = -1;
                    rbody.velocity = new Vector2(0, -Speed);
                    //Debug.Log("下");

                    break;
            }
            //上下左右の４方向から処理
            //左

            //右

            //上

            //下

            Anim.SetFloat("horizontal", hori);
            Anim.SetFloat("vertical", vert);

        }

        private IEnumerator Attack_Trigger()
        {
            Attack = true;
            Anim.SetTrigger("attack");
            //Debug.Log("攻撃だッ！");

            AnimatorStateInfo stateInfo = Anim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsTag("LowPower_Attack"))
            {
                //Debug.Log("弱攻撃ですッ！");
                EnemyAttack_Low();
            }
            else if (stateInfo.IsTag("HighPower_Attack"))
            {
                EnemyAttack_High();
            }
            
                //任意のメソッドをここから打ち出す
                yield return new WaitForSeconds(1 / Attack_Fre);//アクションの待機
            if (_movedir == Move.Stop) StartCoroutine(Attack_Trigger());
            else
            {
                Attack = false;
            }
        }

        //アニメーションイベントなどで実行すると任意のタイミングで実行させやすい
        public void EnemyAttack_Low ()//弱攻撃
        {
            if(Enemy_Type == Type.Fighter)
            {
                if (hori == 1 && vert == 0)//右
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("右");
                }
                else if (hori == -1 && vert == 0)//左
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("左");
                }
                else if (hori == 0 && vert == 1)//上
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("上");
                }
                else if (hori == 0 && vert == -1)
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("下");
                }
            }
            if(Enemy_Type == Type.Gunner)
            {
                if (hori == 1 && vert == 0)//右
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(1);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    //Debug.Log("右");
                }
                else if (hori == -1 && vert == 0)//左
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(2);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    //Debug.Log("左");
                }
                else if (hori == 0 && vert == 1)//上
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(3);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    //Debug.Log("上");
                }
                else if (hori == 0 && vert == -1)
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(4);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed);
                    obj.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                    //Debug.Log("下");
                }
            }
        }

        public void EnemyAttack_High()//強攻撃
        {
            if(Enemy_Type == Type.Fighter)
            {

                if (hori == 1 && vert == 0)//右
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter((int)(Power * Attack_Mag));
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("右");
                }
                else if (hori == -1 && vert == 0)//左
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter((int)(Power * Attack_Mag));
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("左");
                }
                else if (hori == 0 && vert == 1)//上
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter((int)(Power * Attack_Mag));
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("上");
                }
                else if (hori == 0 && vert == -1)
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().PowerSetter((int)(Power * Attack_Mag));
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    Destroy(obj, 0.3f);
                    //Debug.Log("下");
                }
            }
            if (Enemy_Type == Type.Gunner)
            {
                if (hori == 1 && vert == 0)//右
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(1);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed_Mag);
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    //Debug.Log("右");
                }
                else if (hori == -1 && vert == 0)//左
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(2);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed_Mag);
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    //Debug.Log("左");
                }
                else if (hori == 0 && vert == 1)//上
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(3);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed_Mag);
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    //Debug.Log("上");
                }
                else if (hori == 0 && vert == -1)
                {
                    GameObject obj = Instantiate(Punch, new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z), Quaternion.identity);
                    obj.GetComponent<EnemyPunch>().pos_Setter(this.gameObject);
                    obj.GetComponent<EnemyPunch>().PowerSetter(Power);
                    obj.GetComponent<EnemyPunch>().Direction_Setter(4);
                    obj.GetComponent<EnemyPunch>().Bullet_Setter(BulletSpeed_Mag);
                    obj.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                    //Debug.Log("下");
                }
            }
        }
    }
}
