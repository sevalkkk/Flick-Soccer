using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    float score;
    float destroyedSign;
    public bool touched = false;

    private void Start()
    {
        score = PlayerPrefs.GetInt("destroyedSign");
        destroyedSign = PlayerPrefs.GetInt("destroyedSign");



    }

    private void Update()
    {

        PlayerPrefs.SetInt("score", (int)score);
        PlayerPrefs.SetInt("destroyedSign", (int)destroyedSign);
    }


    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.tag == "sign")
            {
                score += 10f;
                destroyedSign++;
                other.gameObject.SetActive(false);

            }
        }






    }
}
