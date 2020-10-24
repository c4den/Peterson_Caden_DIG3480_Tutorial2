using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    Animator anim;

    public Text score;
    public Text winText;
    public Text livesText;
    public AudioClip ambientMusic;
    public AudioClip winSound;
    public AudioSource musicSource;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    private int scoreValue = 0;
    private int livesValue = 3;
    private int teleported = 0;
    private int soundflag = 0;

    private bool facingRight = true;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = "Score: " + scoreValue.ToString();
        winText.text = "";
        livesText.text = "Lives: " + livesValue.ToString();
        musicSource.clip = ambientMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if(facingRight == false && hozMovement > 0){
            Flip();
        } else if(facingRight == true && hozMovement < 0){
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if(Input.GetKey("escape")){
            Application.Quit();
        }
    }

    void Update(){
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)){
            anim.SetInteger("State", 2);
        }else{
            anim.SetInteger("State", 0);
        }

        if(!isOnGround){
            anim.SetInteger("State", 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            
        }

        if(scoreValue >= 8){
            if(soundflag == 0){
                musicSource.clip = winSound;
                musicSource.Play();
                soundflag++;
            }
            winText.text = "You Win! Game Created by Caden Peterson";
        }

        //displaces player to new level after score becomes 4
        if(scoreValue == 4 && teleported == 0){
            transform.position = new Vector2(1.44f, 50.45f);
            teleported = 1;
            livesValue = 3;
            livesText.text = "Lives: " + livesValue.ToString();
        }

        if(livesValue <= 0){
            winText.text = "You Lost, Please Try again.";
        }

        if(collision.collider.tag == "Enemy"){
            livesValue -= 1;
            livesText.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip(){
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}