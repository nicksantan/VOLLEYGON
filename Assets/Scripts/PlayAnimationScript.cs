using UnityEngine;
using System.Collections;
using com.FDT.EasySpritesAnimation;

public class PlayAnimationScript : MonoBehaviour {

		[SerializeField] SpriteAnimation spriteAnimation = null;
        
        public void PlayAnimation(){
		//	spriteAnimation.Play ( 0 );
		}
   
   public void GravChange(float whichDirection){
        if (Mathf.Sign(whichDirection) < 0)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
        else if (Mathf.Sign(whichDirection) > 0)
        {

            transform.localScale = new Vector3(1f, 1f, 1f);
        }
         
        PlayAnimation();
        Debug.Log("grav change received by animation script");
    }


}
