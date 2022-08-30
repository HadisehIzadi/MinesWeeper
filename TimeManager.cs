using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeManager : MonoBehaviour
{
	
		private int timer;
	public Text TimerText;
	
	
    // Start is called before the first frame update
    void Start()
    {
    	timer = 0 ;
    	IncreaseTime();
    }

    
     public void IncreaseTime()
    {
    	timer++;
    	TimerText.text = timer.ToString();
    	Invoke("IncreaseTime" ,1f);
    	
    }
}
