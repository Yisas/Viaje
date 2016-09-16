using UnityEngine;
using System.Collections;

public class AuxFunctions : MonoBehaviour {

	public enum HitDirection
	{
		None,
		Top,
		Bottom,
		Forward,
		Back,
		Left,
		Right}

	;

	public static HitDirection ReturnDirection (Transform Object, Transform ObjectHit)
	{

		HitDirection hitDirection = HitDirection.None;
		RaycastHit2D myRayHit;
		Vector2 direction = (Object.position - ObjectHit.position).normalized;
		Vector2 origin = new Vector2 (Object.position.x, Object.position.y);

		myRayHit = Physics2D.Raycast (origin, direction);

		if (myRayHit.collider != null) {
			Vector3 myNormal = myRayHit.normal;
			myNormal = myRayHit.transform.TransformDirection (myNormal);
			if (Mathf.Sign (myNormal.x) == -1.0f)
				hitDirection = HitDirection.Right;
			else
				hitDirection = HitDirection.Left;
		}

		return hitDirection;
	}

    public void DeactivateColliders()
    {
        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.GetComponentInParent<Rigidbody2D>().isKinematic = false;
            col.GetComponentInParent<Animator>().enabled =false;
            col.isTrigger = true;
        }
    }

}
