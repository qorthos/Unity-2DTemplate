using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimatorController : MonoBehaviour
{
    public SpriteAnimator SpriteAnimator;

    public Vector3 GlobalDirection;
    [ReadOnly] [SerializeField] Vector3 percDirection;
    public CharacterAnimationStateEnum CharacterState;
    public string CustomActionName;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
    }


    void UpdateAnimation()
    {
        percDirection =  Quaternion.AngleAxis(-cam.transform.rotation.eulerAngles.y, Vector3.up) * GlobalDirection;

        var current = SpriteAnimator.CurrentSequenceName;
        var targetSequenceName = "Idle-x";

        switch (CharacterState)
        {
            case CharacterAnimationStateEnum.Idle:
                if (percDirection.x < 0)
                {
                    targetSequenceName = "Idle-x";
                }
                else
                {
                    targetSequenceName = "Idle+x";
                }

                break;

            case CharacterAnimationStateEnum.Walk:
                if (percDirection.x < 0)
                {
                    targetSequenceName = "Walk-x";
                }
                else
                {
                    targetSequenceName = "Walk+x";
                }

                break;

            case CharacterAnimationStateEnum.Custom:
                if (percDirection.x < 0)
                {
                    targetSequenceName = $"{CustomActionName}-x";
                }
                else
                {
                    targetSequenceName = $"{CustomActionName}+x";
                }

                break;
        }

        if (current != targetSequenceName)
        {
            SpriteAnimator.PlaySequence(targetSequenceName);
        }
    }
}

public enum CharacterAnimationStateEnum
{
    Idle,
    Walk,
    Custom,
}