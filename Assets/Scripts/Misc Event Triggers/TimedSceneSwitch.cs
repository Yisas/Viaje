using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSceneSwitch : SceneSwitch {

    public float countdownTime;

	void Update () {

        countdownTime -= Time.deltaTime;

        if(countdownTime <= 0)
        {
            Destroy(GameController.GetInstance());

            SplashCreenSwitch();
        }
	}
}
