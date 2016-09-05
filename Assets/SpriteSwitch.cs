using UnityEngine;
using System.Collections;

public class SpriteSwitch : MonoBehaviour {

	public SpriteRenderer spriteToChange;
	public Sprite newSprite;
	public Sprite[] spriteSelection;

	public void Switch(){
		spriteToChange.sprite = newSprite;
	}

	void SwtichSpriteFromArray(int index){
		spriteToChange.sprite = spriteSelection [index];
	}
}
