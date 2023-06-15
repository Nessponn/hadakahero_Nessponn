using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private int HP = 1000;
    [SerializeField] private Image HP_Gage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HP_Gage.fillAmount = (float)HP / 1000;
    }

    //プレイヤーのダメージを受ける管理はここで行う
    public void TakeDamage(int Damage_num)
    {
        HP -= Damage_num;
    }
}
