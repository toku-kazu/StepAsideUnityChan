using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    Animator myAnimator;
    //Unityちゃんを移動させるコンポーネントを入れる
    Rigidbody myRigidbody;
    //ジャンプするための力
    float upForce = 500.0f;
    //前進するための力
    float forwardForce = 800.0f;
    //左右に移動するための力
    float turnForce = 500.0f;
    //左右の移動できる範囲
    float movableRange = 3.4f;
    //動きを減速させる係数
    float coefficient = 0.95f;

    //ゲーム終了の判定
    bool isEnd = false;

    //ゲーム終了時に表示するテキスト
    GameObject stateText;
    //スコアを表示するテキスト
    GameObject scoreText;
    //得点
    int score = 0;

    //左ボタン押下の判定
    bool isLButtonDown = false;
    //右ボタン押下の判定
    bool isRButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {

        //Animatorコンポーネントを取得
        myAnimator = GetComponent<Animator>();
        //走るアニメーションを開始
        myAnimator.SetFloat("Speed", 1);

        //Rigidbodyコンポーネントを取得
        myRigidbody = GetComponent<Rigidbody>();

        //シーン中のstateTextオブジェクトを取得
        stateText = GameObject.Find("GameResultText");

        //シーン中のscoreTextオブジェクトを取得
        scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了ならUnityちゃんの動きを減衰する
        if (isEnd)
        {
            forwardForce *= coefficient;
            turnForce *= coefficient;
            upForce *= coefficient;
            myAnimator.speed *= coefficient;
        }


        //Unityちゃんに前方向の力を加える
        myRigidbody.AddForce(transform.forward * forwardForce);

        //Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる
        if ((Input.GetKey(KeyCode.LeftArrow) || isLButtonDown) && -movableRange < transform.position.x)
        {
            //左に移動
            myRigidbody.AddForce(-turnForce, 0, 0);
        }
        else if ((Input.GetKey(KeyCode.RightArrow) || isRButtonDown) && transform.position.x < movableRange)
        {
            //右に移動
            myRigidbody.AddForce(turnForce, 0, 0);
        }

        //Jumpステートの場合はJumpにfalseをセットする
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            myAnimator.SetBool("Jump", false);
        }

        //ジャンプしていない時にスペースが押されたらジャンプする
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y < 0.5f)
        {
            //ジャンプアニメを再生
            myAnimator.SetBool("Jump", true);
            //Unityちゃんに上方向の力を加える
            myRigidbody.AddForce(transform.up * upForce);
        }
    }

    //トリガーモードで他のオブジェクトと接触した場合の処理
    void OnTriggerEnter(Collider other)
    {
        


        //障害物に衝突した場合
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            isEnd = true;
            //stateTextにGAME OVERを表示
            stateText.GetComponent<Text>().text = "GAME OVER";
        }
        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            isEnd = true;
            //stateTextにGAME CLEARを表示
            stateText.GetComponent<Text>().text = "CLEAR!!";
        }
        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            // スコアを加算
            score += 10;

            //ScoreText獲得した点数を表示
            scoreText.GetComponent<Text>().text = "Score " + score + "pt";
            //パーティクルを再生
            GetComponent<ParticleSystem>().Play();
            //接触したコインのオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }
    //ジャンプボタンを押した場合の処理
    public void GetMyJumpButtonDown()
    {
        if(transform.position.y < 0.5f) {
            myAnimator.SetBool("Jump", true);
            myRigidbody.AddForce(transform.up * upForce);
        }
    }

    //左ボタンを押し続けた場合の処理
    public void GetMyLeftButtonDown()
    {
        isLButtonDown = true;
    }
    //左ボタンを離した場合の処理
    public void GetMyLeftButtonUp()
    {
        isLButtonDown = false;
    }
    //右ボタンを押し続けた場合の処理
    public void GetMyRightButtonDown()
    {
        isRButtonDown = true;
    }
    //右ボタンを離した場合の処理
    public void GetMyRightButtonUp()
    {
        isRButtonDown = false;
    }
}
