using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveCharacter : MonoBehaviour
{

    Rigidbody2D character;
    public float vel; //VELOCIDADE
    public float jumpVel; //VELOCIDADE DO PULO
    bool Jump; //PULO
    int Stars;
    int Lifes;
    public Text LifesTxt;
    public Text StarsTxt;
    public Text GameOverTxt;
    public Text winTxt;
    public Button start;
    public Button restart;
    public Text PausedTxt;
    public Button Continue;
    public Animator heroAnim;
    public GameObject door;
    public AudioSource gameAudio;
    public AudioClip[] sounds;



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0; //Pausado
        character = gameObject.GetComponent<Rigidbody2D>();
        Stars = 0;
        Lifes = 3;
        winTxt.enabled = false;
        LifesTxt.text = Lifes.ToString();
        StarsTxt.text = Stars.ToString();
        start.gameObject.SetActive(true);
        restart.gameObject.SetActive(false);
        start.gameObject.SetActive(true);
        PausedTxt.gameObject.SetActive(false);
        Continue.gameObject.SetActive(false);
        GameOverTxt.enabled = false;
        heroAnim.SetBool("Jump", false);
        heroAnim.SetBool("Walk", false);
        heroAnim.SetBool("Run", false);
    }

    // Update is called once per frame
    void Update()
    {
        //Movimento Personagem

        
        //ANDAR DIREITA
        if (Input.GetKey(KeyCode.RightArrow))
        {
            character.transform.Translate(vel * Time.deltaTime, 0, 0);
            heroAnim.SetBool("Walk", true);
            if(this.gameObject.transform.localScale.x < 0)
            {
                this.gameObject.transform.localScale = new Vector3((this.gameObject.transform.localScale.x * -1), this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);

            }
        }
        //PARAR ANDAR DIREITA
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {

            heroAnim.SetBool("Walk", false);
        }

        //ANDAR ESQUERDA
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            character.transform.Translate(-vel * Time.deltaTime, 0, 0);
            heroAnim.SetBool("Walk", true);

            if (this.gameObject.transform.localScale.x > 0)
            {
                this.gameObject.transform.localScale = new Vector3((this.gameObject.transform.localScale.x * -1), this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);

            }
        }
        //PARAR ANDAR ESQUERDA
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            heroAnim.SetBool("Walk", false);
        }

        //PULAR
        if (Input.GetKeyDown(KeyCode.Space) && Jump)
        {
            character.AddForce(Vector2.up * jumpVel, ForceMode2D.Impulse);
            heroAnim.SetBool("Jump", true);

        }

        if (Input.GetKey(KeyCode.W))
        {
            vel = 25;
            heroAnim.SetBool("Run", true);



        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            vel = 15;
            heroAnim.SetBool("Run", false);

        }

        //PAUSE
        if (Input.GetKey(KeyCode.P))
        {
            Paused();
            PausedTxt.gameObject.SetActive(true);

        }

        //DESPAUSE
        if (Input.GetKey(KeyCode.C))
        {
            Continued();
            PausedTxt.gameObject.SetActive(false);
        }






    }

    //COLISÃO COM O CHÃO
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Jump = true;
            heroAnim.SetBool("Jump", false);
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1);



        }

        if (collision.gameObject.CompareTag("flag"))
        {
            Win();

        }

        if (collision.gameObject.CompareTag("venon"))
        {
            gameAudio.PlayOneShot(sounds[3]);
            Lifes = 0;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
            LifesTxt.text = Lifes.ToString();
            GameOver();
          
           

        }

        if (collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("blade"))
        {
            gameAudio.PlayOneShot(sounds[3]);
            Lifes -= 1;
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 0, 0);
            LifesTxt.text = Lifes.ToString();
            if (Lifes <= 0)
            {
                GameOver();
            }

        }


        if (collision.gameObject.CompareTag("button"))
        {
            gameAudio.PlayOneShot(sounds[0]);
            door.transform.eulerAngles = Vector3.Lerp(door.transform.localEulerAngles, new Vector3(0, 90, 0 ), 0.9f);
            door.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameAudio.PlayOneShot(sounds[0]);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            Jump = false;
        }
    }
    //Pegar Objetos e destrui-los depois
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("star"))
        {

            Stars += 1;
            gameAudio.PlayOneShot(sounds[2]);
            StarsTxt.text = Stars.ToString();
            Destroy(collision.gameObject);


        }

        if (collision.gameObject.CompareTag("life"))
        {
            Lifes += 1;
            gameAudio.PlayOneShot(sounds[1]);
            LifesTxt.text = Lifes.ToString();
            Destroy(collision.gameObject);
           

        }
    }

    public void Starting()
    {
        start.gameObject.SetActive(false);
        Time.timeScale = 1; //Despause
    }

    public void Restart()
    {
        restart.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        gameAudio.PlayOneShot(sounds[4]);
        GameOverTxt.enabled = true;
        restart.gameObject.SetActive(true);
        Time.timeScale = 0;
      
    }
    //FUNÇÃO PAUSE
    public void Paused()
    {
            Time.timeScale = 0;
            Continue.gameObject.SetActive(true);
    }
    //FUNÇÃO DESPAUSE
    public void Continued()
    {
        Time.timeScale = 1;
        Continue.gameObject.SetActive(false);

    }

    private void Win()
    {
        winTxt.enabled = true;
        gameAudio.PlayOneShot(sounds[5]);
        heroAnim.SetBool("Jump", true);
        restart.gameObject.SetActive(true);

    }

}
