using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MenuStyle{
	horizontal,vertical
}

public class GunInventory : MonoBehaviour {
	[Tooltip("Current weapon gameObject.")]
	public GameObject currentGun;
	private Animator currentHAndsAnimator;
	private int currentGunCounter = 0;

	[Tooltip("Put Strings of weapon objects from Resources Folder.")]
	public List<string> gunsIHave = new List<string>();
	[Tooltip("Icons from weapons.(Fetched when you run the game)MUST HAVE ICONS WITH CORRESPONDING NAMES IN RESOUCES FOLDER*")]
	public Texture[] icons;

	[HideInInspector]
	public float switchWeaponCooldown;


	void Awake(){
		StartCoroutine("UpdateIconsFromResources");

		StartCoroutine ("SpawnWeaponUponStart");//to start with a gun

		if (gunsIHave.Count == 0)
			print ("No guns in the inventory");
	}


	IEnumerator SpawnWeaponUponStart(){
		yield return new WaitForSeconds (0.5f);
		StartCoroutine("Spawn",0);
	}

	/* 
	 * Calculation switchWeaponCoolDown so it does not allow us to change weapons millions of times per second,
	 * and at some point we will change the switchWeaponCoolDown to a negative value so we have to wait until it
	 * overcomes 0.0f. 
	 */
	void Update(){

		switchWeaponCooldown += 1 * Time.deltaTime;
		if(switchWeaponCooldown > 1.2f && Input.GetKey(KeyCode.LeftShift) == false){
			Create_Weapon();
		}

	}


	/*
	 * Grabing the icons from the Resources/Weapo_Icons/ -> gun name of the image.
	 * (!!!!!!!1!READ IMPORTANT) 
	 * the weapon image to respond the weapon must have the same name as the WEAPON  with the extension _img.
	 * So if the gun prefab is called "Sniper_Piper" the corresponding image must be located in the location form previous,
	 * with the name "Sniper_Piper_img".
	 */
	IEnumerator UpdateIconsFromResources(){
		yield return new WaitForEndOfFrame ();

		icons = new Texture[gunsIHave.Count];
		for(int i = 0; i < gunsIHave.Count; i++){
			icons[i] = (Texture) Resources.Load("Weap_Icons/" + gunsIHave[i].ToString() + "_img");
		}

	}

	/*
	 * If used scroll mousewheel or arrows up and down the player will change weapon.
	 * GunPlaceSpawner is child of Player gameObject, where the gun is going to spawn and transition to our
	 * gun properties value.
	 */
	void Create_Weapon(){

		/*
		 * Scrolling wheel waepons changing
		 */
		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("Mouse ScrollWheel") > 0){
			switchWeaponCooldown = 0;

			currentGunCounter++;
			if(currentGunCounter > gunsIHave.Count-1){
				currentGunCounter = 0;
			}
			StartCoroutine("Spawn",currentGunCounter);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Mouse ScrollWheel") < 0){
			switchWeaponCooldown = 0;

			currentGunCounter--;
			if(currentGunCounter < 0){
				currentGunCounter = gunsIHave.Count-1;
			}
			StartCoroutine("Spawn",currentGunCounter);
		}

		/*
		 * Keypad numbers
		 */
		if(Input.GetKeyDown(KeyCode.Alpha1) && currentGunCounter != 0){
			switchWeaponCooldown = 0;
			currentGunCounter = 0;
			StartCoroutine("Spawn",currentGunCounter);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) && currentGunCounter != 1){
			switchWeaponCooldown = 0;
			currentGunCounter = 1;
			StartCoroutine("Spawn",currentGunCounter);
		}

	}


	IEnumerator Spawn(int _redniBroj){
		if (weaponChanging)
			weaponChanging.Play ();
		else
			print ("Missing Weapon Changing music clip.");
		if(currentGun){
			if(currentGun.name.Contains("Gun")){

				currentHAndsAnimator.SetBool("changingWeapon", true);

				yield return new WaitForSeconds(0.8f);//0.8 time to change waepon, but since there is no change weapon animation there is no need to wait fo weapon taken down
				Destroy(currentGun);

				GameObject resource = (GameObject) Resources.Load(gunsIHave[_redniBroj].ToString());
				currentGun = (GameObject) Instantiate(resource, transform.position, /*gameObject.transform.rotation*/Quaternion.identity);
				AssignHandsAnimator(currentGun);
			}
			else if(currentGun.name.Contains("Sword")){
				currentHAndsAnimator.SetBool("changingWeapon", true);
				yield return new WaitForSeconds(0.25f);//0.5f

				currentHAndsAnimator.SetBool("changingWeapon", false);

				yield return new WaitForSeconds(0.6f);//1
				Destroy(currentGun);

				GameObject resource = (GameObject) Resources.Load(gunsIHave[_redniBroj].ToString());
				currentGun = (GameObject) Instantiate(resource, transform.position, /*gameObject.transform.rotation*/Quaternion.identity);
				AssignHandsAnimator(currentGun);
			}
		}
		else{
			GameObject resource = (GameObject) Resources.Load(gunsIHave[_redniBroj].ToString());
			currentGun = (GameObject) Instantiate(resource, transform.position, /*gameObject.transform.rotation*/Quaternion.identity);

			AssignHandsAnimator(currentGun);
		}


	}


	void AssignHandsAnimator(GameObject _currentGun){
		if(_currentGun.name.Contains("Gun")){
			currentHAndsAnimator = currentGun.GetComponent<GunScript>().handsAnimator;
		}
	}


	[Header("GUI Gun preview variables")]
	[Tooltip("Weapon icons style to pick.")]
	public MenuStyle menuStyle = MenuStyle.horizontal;
	[Tooltip("Spacing between icons.")]
	public int spacing = 10;
	[Tooltip("Begin position in percetanges of screen.")]
	public Vector2 beginPosition;
	[Tooltip("Size of icon in percetanges of screen.")]
	public Vector2 size;


	public void DeadMethod(){
		Destroy (currentGun);
		Destroy (this);
	}


	private Vector2 vec2(Vector2 _vec2){
		return new Vector2(Screen.width * _vec2.x / 100, Screen.height * _vec2.y / 100);
	}


	[Header("Sounds")]
	[Tooltip("Sound of weapon changing.")]
	public AudioSource weaponChanging;
}
