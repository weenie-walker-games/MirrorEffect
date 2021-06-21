using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

namespace WeenieWalker
{
    public class MirrorEffect : MonoBehaviour
    {
        //Store the player character that will be mirrored
        Char originalCharacter;

        //Info about dummy character
        [Tooltip("The sprite slot for the dummy character")]
        public SpriteRenderer mirrorRenderer;
        [Tooltip("The animator component of the dummy character")]
        public Animator mirrorAnimator;
        [Tooltip("Use this to adjust the size of the dummy character in the mirror")]
        public float playerInMirrorHeight = 0.6f;

        //Get the sprite renderer and animator off the player character
        SpriteRenderer characterToMirror;
        Animator characterAnimator;


        SpriteDirectionData directionData;
        Coroutine mirrorRoutine;
        bool isMirroring = false;   //Used to run the coroutine
  
        private void Start()
        {
           
            mirrorRenderer.enabled = false;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Use a collider to start the mirror effect, which should save on performance
            if (collision.gameObject.CompareTag("Player"))
            {
                originalCharacter = collision.gameObject.GetComponent<Char>();

                //Just in case there isn't a Char script attached, exit without doing anything
                if (originalCharacter == null)
                    return;

                //Copy the original character's info to the mirror character
                characterToMirror = originalCharacter.GetComponentInChildren<SpriteRenderer>();
                characterAnimator = originalCharacter.GetAnimator();
                mirrorAnimator.runtimeAnimatorController = characterAnimator.runtimeAnimatorController;
                directionData = originalCharacter.spriteDirectionData;

                //Turn on the mirror effect and start the coroutine that updates as the player moves
                mirrorRenderer.enabled = true;
                isMirroring = true;
                mirrorRoutine = StartCoroutine(UpdateMirror());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //After the player exits the mirror, stop the effect
            if (collision.gameObject.CompareTag("Player"))
            {
                //Reset everything
                characterToMirror = null;
                characterAnimator = null;
                mirrorAnimator.runtimeAnimatorController = null;
                directionData = null;
                mirrorRenderer.enabled = false;
                originalCharacter = null;
                isMirroring = false;
                StopCoroutine(mirrorRoutine);
            }
        }

        IEnumerator UpdateMirror()
        {
            while (isMirroring)
            {
                //Get Animation and Sprite data
                CharState charState = originalCharacter.charState;
                string currentDirection = originalCharacter.GetSpriteDirection();
                string newDirection = ReturnReverse();
                string newState = GetAnimStateString();

                //Assign the animation to the mirror
                mirrorAnimator.Play(newState + newDirection);


                //Adjust scale for the mirror image, flipping for left/right
                Vector3 newScale = Vector3.one;

                newScale *= (1 / originalCharacter.spriteScale) * playerInMirrorHeight;
                if (newDirection == "_R" || newDirection == "_L" || newDirection == "_UR" || newDirection == "_UL"|| newDirection == "_DR" || newDirection == "_DL")
                    newScale.x *= -1f;

                mirrorRenderer.transform.localScale = newScale;


                //Get player location and adjust the mirror reflection
                Vector2 playerPos = originalCharacter.transform.position;
                playerPos.y = mirrorRenderer.transform.position.y;

                mirrorRenderer.transform.position = playerPos;

                yield return null;
            }
        }
        

        private string GetAnimStateString()
        {
            string returnString = "";

            CharState currentState = originalCharacter.charState;
            Speech currentSpeech = originalCharacter.GetCurrentSpeech();

            if(currentSpeech != null)
            {
                returnString = originalCharacter.talkAnimSprite;
                return returnString;
            }
  
            switch (currentState)
            {
                case CharState.Idle:
                    returnString = originalCharacter.idleAnimSprite;
                    break;
                case CharState.Move:
                    returnString = originalCharacter.walkAnimSprite;
                    break;
                case CharState.Decelerate:
                    returnString = originalCharacter.walkAnimSprite;
                    break;
                default:

                    break;
            }

            return returnString;
        }

        private string ReturnReverse()
        {
            string current = originalCharacter.GetSpriteDirection();

            string returnDirection = "_D";

            switch (current)
            {
                case "_D":
                    returnDirection = "_U";
                    break;
                case "_L":
                    returnDirection = "_R";
                    break;
                case "_R":
                    returnDirection = "_L";
                    break;
                case "_U":
                    returnDirection = "_D";
                    break;
                case "_UR":
                    returnDirection = "_DL";
                    break;
                case "_UL":
                    returnDirection = "_DR";
                    break;
                case "_DR":
                    returnDirection = "_UL";
                    break;
                case "_DL":
                    returnDirection = "_UR";
                    break;


            }

            return returnDirection;
        }
    }
}
