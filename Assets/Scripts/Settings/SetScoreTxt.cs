using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetScoreTxt : MonoBehaviour
{
    private TMP_Text textTMP;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState=CursorLockMode.None;
        
        textTMP = GetComponent<TMP_Text>();

        int score = PlayerPrefs.GetInt("Score");

        if (score == 100)
        {
            textTMP.text = "Congratulations!!! You've got a max score. It's good to see such a qualified soldier";
        }
        else if (score > 80)
        {
            textTMP.text = "Good job soldier, now you're ready for duty";
        }
        else if (score > 60)
        {
            textTMP.text = "It's not a bad job, but you're not ready for duty yet. Take a second chance";
        }
        else
        {
            textTMP.text = "You're not ready. Better luck next year";
        }
    }
}
