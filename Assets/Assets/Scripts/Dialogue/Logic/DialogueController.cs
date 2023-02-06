using System;
using Assets.Scripts.Dialogue.Data;
using Assets.Scripts.Dialogue.UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Dialogue.Logic
{
    public class DialogueController : MonoBehaviour
    {
        public DialogueData_SO currentDialogueData;
        public bool canTalk;
        private int currentIndex = 0;
        public DialogueSpriteData_SO npcSprite;
        public DialogueSpriteData_SO playerSprite;
        private Transform playerPos => GameObject.FindWithTag("Player").transform;
        private bool isAtLeft=false;
        public bool isOpen = false;

        private void Awake()
        {
            isOpen = false;
            isAtLeft = false;
            currentIndex = 0;
            canTalk = false;
        }

        private void OnEnable()
        {
            EventHandler.DialogueStatueUpdate += DialogueStatueUpdate;
        }

        private void OnDisable()
        {
            EventHandler.DialogueStatueUpdate -= DialogueStatueUpdate;
        }

        private void DialogueStatueUpdate(int index)
        {
            currentIndex = index;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && currentDialogueData != null)
            {
                if (other.gameObject.transform.position.y >= transform.position.y)
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                else gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                canTalk = true;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                canTalk = false;
            }
        }
        private void Update()
        {
            if (isOpen&& Input.GetMouseButtonDown(0))
            {
                if (currentDialogueData.dialoguePieceList[currentIndex].dialogueOptionList.Count == 0)
                {
                    currentIndex=currentDialogueData.dialoguePieceList[currentIndex].targetIndex;
                    DialogueUI.Instance.UpdateMainUI(currentIndex,npcSprite,playerSprite,isAtLeft);
                    if (currentIndex > Settings.dialogueMaxSize)
                    {
                        //DialogueUI.Instance.OptionButtonDestroy();
                        isOpen = false;
                        EventHandler.CallExitDialogue();
                    }
                        
                }
            }
            //TODO:点击到NPC才触发
            if (CursorManager.Instance.currentSprite==CursorManager.Instance.dialogue)
            {
                if (canTalk )
                {
                    CursorManager.Instance.SetCursorValid();
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (!isOpen)
                        {
                            EventHandler.CallEnterDialogue();
                            OpenDialogue();
                        }
                        
                    }
                        
                }
                else
                {
                    CursorManager.Instance.SetCursorInValid();
                }
            }
            
        }

        void OpenDialogue()
        {  
            if(playerPos.position.x >= transform.position.x)
                isAtLeft = false;
            else isAtLeft = true;
            isOpen = true;
            //打开UI
            //传输对话内容
            
            DialogueUI.Instance.UpdateData(currentDialogueData);
            DialogueUI.Instance.UpdateMainUI(0,npcSprite,playerSprite,isAtLeft);
        }
        
    }
}