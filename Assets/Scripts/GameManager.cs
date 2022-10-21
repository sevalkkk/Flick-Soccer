using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject signPrefab;

    [SerializeField] float ballForce;
    float minDragDistance = 15f;
    Vector3 mouseStart;
    Vector3 mouseEnd;
    GameObject ballInstance;
    GameObject signInstance;
    float signCount = 0;
    
    public GameObject gameOverPanel;
    public TMP_Text levelText;
    float level = 0;
    float maxDestroyedSign = 2;
    float maxSignCount = 5;
    public float zDepth = 25f;
    float destroyedSign;
    bool touched;
    float score;
    float remainingSign;


    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
    }
    private void Start()
    {
       
       
        CreateBall();
        CreateSign();
    }

    private void Update()
    {
        
        destroyedSign = PlayerPrefs.GetInt("destroyedSign");
        touched = GameObject.FindObjectOfType<BallController>().touched;
        if(Input.GetMouseButtonDown(0))
        {
            mouseStart = Input.mousePosition;
        }

        if(Input.GetMouseButtonUp(0))
        {
            mouseEnd = Input.mousePosition;

            if(Vector3.Distance(mouseEnd,mouseStart) > minDragDistance)
            {
                //throw ball
                Vector3 hitPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDepth);

                hitPosition = Camera.main.ScreenToWorldPoint(hitPosition);
                Debug.Log(hitPosition);
                ballInstance.transform.LookAt(hitPosition);

                

                //if we add a force with the global direction , then the ball will go in first position , not rotating position.
                // we have to use addrelativeforce bcz its local.
                // rb = ballInstance.GetComponent<Rigidbody>();

                ballInstance.GetComponent<Rigidbody>().AddRelativeForce(ballInstance.transform.forward * ballForce,ForceMode.Impulse);
                //we are going to call it after 2 seconds.
                Invoke("CreateBall", 2f);
                Invoke("CreateSign", 7f);
                

            }

        }



        // finallySignCount = PlayerPrefs.GetInt("signCount")- PlayerPrefs.GetInt("destroyedSign");
        //signCount = finallySignCount;



        remainingSign = signCount - destroyedSign;
      
        if (remainingSign > maxSignCount)
        {
            GameOver();
        }
        if (destroyedSign > maxDestroyedSign)
        {
            levelUp();
        }





        PlayerPrefs.SetInt("signCount", (int)signCount);
        PlayerPrefs.SetInt("destroyedSign", (int)destroyedSign);
    }
    public void CreateBall()
    {
        
        //identity mean is default rotation.
         ballInstance = Instantiate(ballPrefab, ballPrefab.transform.position, Quaternion.identity) ;

        Debug.Log("created" + signCount);
        Debug.Log("destroyed" + destroyedSign);
        Debug.Log("remaining" + remainingSign);
    }

    public void CreateSign()
    {
        signInstance = Instantiate(signPrefab, new Vector3(Random.Range(-4, 4), signPrefab.transform.position.y,
            Random.Range(3, 5.5f)), Quaternion.identity);
        signCount++;
       
       
    }

    public void Destroy(GameObject g)
    {
        Destroy(g);
    }

    public void GameOver()
    {
        Debug.Log("game" +
            "over");
        gameOverPanel.SetActive(true);
        maxSignCount += 2;
        CancelInvoke("CreateBall");
        CancelInvoke("CreateSign");
    }

    public void levelUp()
    {
       level++;
       maxDestroyedSign += 2;
       
       levelText.text = level.ToString();
        
    }
}
