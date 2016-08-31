using UnityEngine;
using System.Collections;

public class RudessHeadAnimatorRandomizer : MonoBehaviour {

	public float blinkMinInterval;
	public float blinkMaxInterval;
	public float smileMinInterval;
	public float smileMaxInterval;

	private Animator anim;
	private bool counting = false;
	private bool animationType = false;						// Blink is true, smile is false
	private float countdownTimer;


	void Awake(){
		anim = GetComponentInChildren<Animator> ();
	}

	void Update(){
		if (counting) {
			countdownTimer -= Time.deltaTime;
			if (countdownTimer <= 0) {
				if (animationType)
					anim.SetTrigger ("blink");
				else
					anim.SetTrigger ("smile");
				
				counting = false;
			}
		} else
			ChooseAnimation ();
	}


	void ChooseAnimation(){
		if (Random.value <= 0.5f)
			animationType = true;
		else
			animationType = false;

						if (animationType)
			countdownTimer = Random.Range (blinkMinInterval, blinkMaxInterval);
		else
			countdownTimer = Random.Range (smileMinInterval, smileMaxInterval);
		
		counting = true;
	}
}
