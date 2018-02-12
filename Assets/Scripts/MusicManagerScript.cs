using UnityEngine;
using System.Collections;

public class MusicManagerScript : MonoBehaviour {

	public AudioSource introSource;
	public AudioSource rootSource;
	public AudioSource secondSource;
	public AudioSource fourthSource;
	public AudioSource fifthSource;

	public AudioSource introSourceSetOne;
	public AudioSource rootSourceSetOne;
	public AudioSource secondSourceSetOne;
	public AudioSource fourthSourceSetOne;
	public AudioSource fifthSourceSetOne;

	public AudioSource introSourceSetTwo;
	public AudioSource rootSourceSetTwo;
	public AudioSource secondSourceSetTwo;
	public AudioSource fourthSourceSetTwo;
	public AudioSource fifthSourceSetTwo;

	private float stored_introSourceVolume; 
	private float stored_rootSourceVolume; 
	private float stored_secondSourceVolume; 
	private float stored_fourthSourceVolume; 
	private float stored_fifthSourceVolume; 

	private float stored_introSourceSetOneVolume; 
	private float stored_rootSourceSetOneVolume; 
	private float stored_secondSourceSetOneVolume; 
	private float stored_fourthSourceSetOneVolume; 
	private float stored_fifthSourceSetOneVolume; 

	private float stored_introSourceSetTwoVolume; 
	private float stored_rootSourceSetTwoVolume; 
	private float stored_secondSourceSetTwoVolume; 
	private float stored_fourthSourceSetTwoVolume; 
	private float stored_fifthSourceSetTwoVolume; 

	public int whichSource;
	public float masterVolume;
	private int whichTrack;
	// Static singleton property
	public static MusicManagerScript Instance { get; private set; }


	void Awake(){
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		TurnOffEverything ();
		whichSource = 0;
		whichTrack = 0;


	SwitchToSource ();

	}

	public void SwitchToSource(){
		switch (whichSource) {
		case 0:
			introSource = introSourceSetOne;
			rootSource = rootSourceSetOne;
			secondSource = secondSourceSetOne;
			fourthSource = fourthSourceSetOne;
			fifthSource = fifthSourceSetOne;
			break;
		case 1:
			introSource = introSourceSetTwo;
			rootSource = rootSourceSetTwo;
			secondSource = secondSourceSetTwo;
			fourthSource = fourthSourceSetTwo;
			fifthSource = fifthSourceSetTwo;
			break;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void RestoreFromPause(){
		introSource.volume = stored_introSourceVolume;
		rootSource.volume = stored_rootSourceVolume;
		secondSource.volume = stored_secondSourceVolume;
		fourthSource.volume = stored_fourthSourceVolume;
		fifthSource.volume = stored_fifthSourceVolume;

		introSourceSetOne.volume = stored_introSourceSetOneVolume;
		rootSourceSetOne.volume = stored_rootSourceSetOneVolume;
		secondSourceSetOne.volume = stored_secondSourceSetOneVolume;
		fourthSourceSetOne.volume = stored_fourthSourceSetOneVolume;
		fifthSourceSetOne.volume = stored_fifthSourceSetOneVolume;

		introSourceSetTwo.volume = stored_introSourceSetTwoVolume;
		rootSourceSetTwo.volume = stored_rootSourceSetTwoVolume;
		secondSourceSetTwo.volume = stored_secondSourceSetTwoVolume;
		fourthSourceSetTwo.volume = stored_fourthSourceSetTwoVolume;
		fifthSourceSetTwo.volume = stored_fifthSourceSetTwoVolume;
	}

	public void storeVolumes(){
		stored_introSourceVolume = introSource.volume; 
		stored_rootSourceVolume = rootSource.volume;
		stored_secondSourceVolume = secondSource.volume; 
		stored_fourthSourceVolume = fourthSource.volume;
		stored_fifthSourceVolume = fifthSource.volume;

		stored_introSourceSetOneVolume = introSourceSetOne.volume;
		stored_rootSourceSetOneVolume = rootSourceSetOne.volume;
		stored_secondSourceSetOneVolume = secondSourceSetOne.volume; 
		stored_fourthSourceSetOneVolume = fourthSourceSetOne.volume;
		stored_fifthSourceSetOneVolume = fifthSourceSetOne.volume;

		stored_introSourceSetTwoVolume = introSourceSetTwo.volume;
		stored_rootSourceSetTwoVolume = rootSourceSetTwo.volume;
		stored_secondSourceSetTwoVolume = secondSourceSetTwo.volume; 
		stored_fourthSourceSetTwoVolume = fourthSourceSetTwo.volume;
		stored_fifthSourceSetTwoVolume = fifthSourceSetTwo.volume;
	}

	public void TurnOffEverything(){
		storeVolumes ();
		introSource.volume = 0;
		rootSource.volume = 0;
		secondSource.volume = 0;
		fourthSource.volume = 0;
		fifthSource.volume = 0;

		introSourceSetOne.volume = 0;
		rootSourceSetOne.volume = 0;
		secondSourceSetOne.volume = 0;
		fourthSourceSetOne.volume = 0;
		fifthSourceSetOne.volume = 0;

		introSourceSetTwo.volume = 0;
		rootSourceSetTwo.volume = 0;
		secondSourceSetTwo.volume = 0;
		fourthSourceSetTwo.volume = 0;
		fifthSourceSetTwo.volume = 0;
	}
	public void StartIntro(){
		TurnOffEverything ();
		introSource.volume = masterVolume;
	}

	public void StartRoot(){
		TurnOffEverything ();
		rootSource.volume = masterVolume;
	}
	public void StartMusic (){
		introSource.volume = 0;
		rootSource.volume = masterVolume;
	}

	public void StartFifth (){
		TurnOffEverything ();
		fifthSource.volume = masterVolume;
	}
	public void StartFourth (){
		TurnOffEverything ();
		fourthSource.volume = masterVolume;
	}

	public void FadeOutEverything(){
		introSource.volume -= .005f;
		rootSource.volume -= .005f;
		secondSource.volume -= .005f;
		fourthSource.volume -= .005f;
		fifthSource.volume -= .005f;
	}
	public void SwitchMusic(int whichTrack){
		TurnOffEverything ();
		switch(whichTrack){
		case 1:
			rootSource.volume = masterVolume;
			secondSource.volume = 0;
			break;
		case 2:
			rootSource.volume = 0;
			secondSource.volume = masterVolume;
			break;
		}			
	}
		
}
