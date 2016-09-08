using UnityEngine;
using System.Collections;

public class SpriteSwitch : MonoBehaviour {

	public SpriteRenderer spriteToChange;
	public Sprite newSprite;
	public Sprite[] spriteSelection;

	private Sprite oldSprite;

	void Awake(){
		oldSprite = spriteToChange.sprite;
	}

	public void Switch(){
		spriteToChange.sprite = newSprite;
	}

	public void SwitchBack(){
		spriteToChange.sprite = oldSprite;
	}

	void SwtichSpriteFromArray(int index){
		spriteToChange.sprite = spriteSelection [index];
	}
}
