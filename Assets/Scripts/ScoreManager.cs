using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    //Managing UI elemetns
    public Text atRiskBar;
    public Text successTimerBar;
    public Text highScoreBar;
    public Text episodeBar;
    public Text rewardBar;
    public Slider wandererDensityBar;
    public Slider mapSizeBar;

    //Key values
    public float successTimer;
    public float highScoreTimer;



    public GameObject playerObject;
    public AreaManagement manager;


    private void Start()
    {
        successTimer = 0f;
        highScoreTimer = 0f;
        playerObject = GameObject.FindGameObjectWithTag("Player");

    }

    //Update the UI elements
    private void Update()
    {
        string currentAtRisk = manager.numAtRisk.ToString();

        if (currentAtRisk != atRiskBar.text)
        {

            atRiskBar.text = currentAtRisk;

        }

        if (currentAtRisk == "0")
        {
            successTimer += Time.deltaTime;
            successTimerBar.text =  (Mathf.Round(successTimer * 100f) / 100f).ToString();

        }else
        {
            if (highScoreTimer < successTimer) {
                highScoreTimer = successTimer;
            }

            highScoreBar.text = (Mathf.Round(highScoreTimer * 100f) / 100f).ToString();
            successTimer = 0f;
        }

        int episodeVal = playerObject.GetComponent<AgentScript>().CompletedEpisodes;
        float rewardVal = playerObject.GetComponent<AgentScript>().GetCumulativeReward();
    episodeBar.text=episodeVal.ToString();
        rewardBar.text = rewardVal.ToString();


}

    //allows sliders to update game controller in runtime
public void UpdateValue()
    {
        manager.SetWandererDensity(wandererDensityBar.value);
        manager.ResizeMap(mapSizeBar.value);
    }



}
