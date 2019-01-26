using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	/*public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	public float moveSpeed = 6;

    public float impact;
    float playerImpact;
    float weaponImpact;
    Vector2 position;
    Vector2 prevPosition;
    Vector2 weaponPosition;
    Vector2 weaponPrevPosition;

    public List<GameObject> weaponList = new List<GameObject>();
    public GameObject weaponEquipped;
    public float weaponRadius;
    float xWeaponVelocity = 0.0f;
    float yWeaponVelocity = 0.0f;
    public float weaponSmoothing = 0.4f;
    private float weaponVelocity;
    public float radiusBoost;
    public float minimumDist;

    public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
	float timeToWallUnstick;

	public float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	Vector2 directionalInput;
    Vector2 weaponInput;
    Vector2 weaponPrevInput;
    bool wallSliding;
	int wallDirX;

    //Poubelle
    int setInputCd;
    public int setInputCdMax;

    void Start() {
		controller = GetComponent<Controller2D> ();
        weaponEquipped = weaponList[0].gameObject;

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
    }

	void Update() {
		CalculateVelocity ();
		HandleWallSliding ();

		controller.Move (velocity * Time.deltaTime, directionalInput);
        
        CalculateImpact();
        
        if (Vector2.Distance(weaponPosition, weaponPrevPosition) > minimumDist)
        {
            weaponVelocity += 0.004f;
            if (weaponSmoothing > 0.05)
            {
                weaponSmoothing -= weaponVelocity;
            }
            //if (weaponImpact > 0) weaponSmoothing /= (weaponImpact * 10);
        }
        else if (Vector2.Distance(weaponInput, weaponPrevInput) < minimumDist)
        {
            weaponSmoothing = 0.3f;
            weaponVelocity = 0.0f;
        }
        MoveWeapon();

        if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	public void SetInputs (Vector2 inputLeftStick, Vector2 inputRightStick) {
		directionalInput = inputLeftStick;
        //Cooldown for previousInput
        if (setInputCd > 0) {
            setInputCd -= 1;
        }
        else {
            weaponPrevInput = weaponInput;
            setInputCd = setInputCdMax;
        }
        weaponInput = inputRightStick;
	}

    public void MoveWeapon() {
        //weaponEquipped position = weaponInput * weaponRadius;
        if (weaponInput != Vector2.zero) weaponEquipped.transform.localPosition = new Vector2(Mathf.SmoothDamp(weaponPosition.x, weaponInput.x * weaponRadius, ref xWeaponVelocity, weaponSmoothing), Mathf.SmoothDamp(weaponPosition.y, weaponInput.y * weaponRadius, ref yWeaponVelocity, weaponSmoothing));
        //Rotate Weapon toward Direction
        var dir = Vector3.zero - weaponEquipped.transform.localPosition;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weaponEquipped.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //weaponEquipped.transform.right = Vector3.zero - weaponEquipped.transform.localPosition;
    }

    public void SwitchWeapon() {
        //Switches to other weapon
        if (weaponEquipped == weaponList[0]) {
            weaponEquipped = weaponList[1];
        }
        else weaponEquipped = weaponList[0];
    }

    public void CalculateImpact() {
        //Calculates distance between last frame and this one
        prevPosition = position;
        position = transform.position;
        weaponPrevPosition = weaponPosition;
        weaponPosition = weaponEquipped.transform.localPosition;
        //impact of the player
        if (position != prevPosition)
        {
            //Calculate impact
            playerImpact = Mathf.Round(Vector2.Distance(prevPosition, position) * 100) / 100;
        }
        else playerImpact = 0;
        //impact of the weapon
        if (weaponPosition != weaponPrevPosition)
        {
            //Calculate impact
            weaponImpact = Mathf.Round(Vector2.Distance(weaponPrevPosition, weaponPosition) * 100) / 100;
        }
        else weaponImpact = 0;

        impact = playerImpact + weaponImpact;
    }

    public void OnJumpInputDown() {
		if (wallSliding) {
			if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}
		

	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {
					timeToWallUnstick = wallStickTime;
				}
			}
			else {
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}*/
}
