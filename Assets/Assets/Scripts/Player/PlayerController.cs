using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    public float speed;
    private float inputx=0;
    private float inputy=-1f;
    private Vector2 moveInput;
    private bool firstStart = true;
    private bool isMoving=false;
    private Animator[] _animators;

    //使用工具动画
    private float mouseX;
    private float mouseY;
    private bool useTool;
    private bool inputFlag = true;
    private void Awake()
    {
        firstStart = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animators = GetComponentsInChildren<Animator>();

    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent+=AfterSceneLoadEvent;
        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;
        EventHandler.MoveToPosition += MoveToPosition;
        EventHandler.MouseClickedEvent += MouseClickedEvent;
        EventHandler.EnterDialogue += EnterDialogue;
        EventHandler.ExitDialogue += ExitDialogue;
    }
    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent-=AfterSceneLoadEvent;
        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
        EventHandler.MoveToPosition -= MoveToPosition;
        EventHandler.MouseClickedEvent -= MouseClickedEvent;
        EventHandler.EnterDialogue -= EnterDialogue;
        EventHandler.ExitDialogue -= ExitDialogue;
    }
    private void ExitDialogue()
    {
        inputFlag = true;
    }

    private void EnterDialogue()
    {
        inputFlag = false;
    }
    private void MouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        //TODO:执行动画
        if (itemDetails.itemType != ItemType.Furniture && itemDetails.itemType !=
            ItemType.Commodity && itemDetails.itemType != ItemType.Seed)
        {
            mouseX = pos.x - transform.position.x;
            mouseY = pos.y - (transform.position.y+0.7f);

            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else mouseX = 0;

            StartCoroutine(UseToolRuntime(pos,itemDetails));
        }
        else EventHandler.CallExcuteActionAfterAnimation(pos,itemDetails);
    }

    private IEnumerator UseToolRuntime(Vector3 pos, ItemDetails itemDetails)
    {
        useTool = true;
        inputFlag = false;
        yield return null;

        foreach (var anim in _animators)
        {
            anim.SetTrigger("UseTool");
            anim.SetFloat("MouseX",mouseX);
            anim.SetFloat("MouseY",mouseY);
            anim.SetFloat("InputX",mouseX);
            anim.SetFloat("InputY",mouseY);
          
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExcuteActionAfterAnimation(pos,itemDetails);
        yield return new WaitForSeconds(0.25f);
        useTool = false;
        inputFlag = true;
    }
    private void MoveToPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void BeforeSceneUnloadEvent()
    {
        inputFlag = false;
    }

    private void AfterSceneLoadEvent()
    {
        inputFlag = true;
    }

    

    private void Update()
    {
        if (inputFlag)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimator();
    }

    private void FixedUpdate()
    {
        if(inputFlag)
            Move();
    }

    private void PlayerInput()
    {
        inputx = Input.GetAxisRaw("Horizontal");
        inputy=Input.GetAxisRaw("Vertical");
        if (inputx != 0 || inputy != 0)
        {
            inputx *= 1f;
            inputy *= 1f;
        }
        //跑步状态
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (inputx != 0|| inputy != 0)
            {
                inputx *=1.5f;
                inputy *= 1.5f;
            }
        }
        
        moveInput = new Vector2(inputx, inputy);
        if (inputx == 0 && inputy == 0)
            isMoving = false;
        else isMoving = true;
    }

    private void Move()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position+moveInput*speed*Time.deltaTime);
    }


    private void SwitchAnimator()
    {
        foreach (var anim in _animators)
        {
            anim.SetBool("IsMoving",isMoving);
            
            if (isMoving)
            {
                anim.SetFloat("InputX",inputx);
                anim.SetFloat("InputY",inputy);
                firstStart = false;
            }

            if (firstStart)
            {
                anim.SetFloat("InputY",-1);
            }
        }
    }
}
