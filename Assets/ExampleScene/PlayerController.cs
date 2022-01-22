using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameDataChannel GameDataChannel;
    public GameEventChannel GameEventChannel;
    public PlayerInput PlayerInput;
    public CharacterAnimatorController CharacterAnimatorController;
    public PlayerUse PlayerUse;

    public float Speed = 3;

    [ReadOnly] [SerializeField] Vector3 inputDir;
    [ReadOnly] [SerializeField] Vector3 moveDir;
    [ReadOnly] [SerializeField] int inputBlocked;

    [ReadOnly] [SerializeField] List<Useable> FloorUseables;



    // Start is called before the first frame update
    void Start()
    {
        var move = PlayerInput.currentActionMap.FindAction("Move");
        move.performed += OnMove;
        move.started += OnMove;
        move.canceled += OnMove;

        var use = PlayerInput.currentActionMap.FindAction("Use");
        use.performed += OnUse;

        GameEventChannel.RegisterListener<DialogueStartedGEM>(OnDialogueStarted);
        GameEventChannel.RegisterListener<DialogueFinishedGEM>(OnDialogueFinished);
    }

    private void OnDialogueStarted(DialogueStartedGEM arg0)
    {
        inputBlocked++;
    }

    private void OnDialogueFinished(DialogueFinishedGEM arg0)
    {
        inputBlocked--;
    }

    private void OnUse(InputAction.CallbackContext obj)
    {
        if (inputBlocked > 0)
            return;

        if (PlayerUse.TargetUseables.Count > 0)
        {
            PlayerUse.TargetUseables[0].Use();
        }
        else if (FloorUseables.Count > 0)
        {
            FloorUseables[0].Use();
        }
        else if (PlayerUse.CharacterUseables.Count > 0)
        {
            PlayerUse.CharacterUseables[0].Use();
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();

        if (moveDir != Vector3.zero)
            PlayerUse.transform.localPosition = moveDir + Vector3.up / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputBlocked == 0)
        {
            moveDir = new Vector3(
                inputDir.x * transform.right.x,
                0,
                inputDir.y * transform.forward.z);

            moveDir = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * moveDir;

            moveDir.Normalize();
        }
        else
        {
            moveDir = Vector3.zero;
        }


        if (moveDir.sqrMagnitude > 1e-5f)
        {
            CharacterAnimatorController.GlobalDirection = moveDir;
            CharacterAnimatorController.CharacterState = CharacterAnimationStateEnum.Walk;

            var target = transform.position + moveDir * Time.deltaTime * Speed;

            NavMesh.Raycast(target + Vector3.up, target + Vector3.down, out var hit, NavMesh.AllAreas);
            var mask = PhysicsCollisionMasks.MaskForLayer(LayerMask.NameToLayer("Player"));
            var unblocked = !Physics.Raycast(transform.position+ Vector3.up/2f, moveDir, out var hitInfo, Time.deltaTime * Speed, mask );
            
            if (unblocked)
            {
                transform.position = hit.position;
            }
            else
            {
                Debug.Log($"player blocked from moving by physics {hitInfo.collider.name} ");
            }
        }
        else
        {
            CharacterAnimatorController.CharacterState = CharacterAnimationStateEnum.Idle;
        }


    }

    public void LockInput()
    {
        inputBlocked++;
    }

    public void UnlockInput()
    {
        inputBlocked--;
    }


    private void OnTriggerEnter(Collider other)
    {
        var useable = other.GetComponent<Useable>();
        if (useable == null)
            return;

        if (other.tag.Equals("FloorUseable"))
        {
            FloorUseables.Add(useable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var useable = other.GetComponent<Useable>();
        if (useable == null)
            return;

        if (other.tag.Equals("FloorUseable"))
        {
            FloorUseables.Remove(useable);
        }


    }


}
