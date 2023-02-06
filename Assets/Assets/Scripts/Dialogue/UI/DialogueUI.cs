using System.Collections.Generic;
using Assets.Scripts.Dialogue.Data;
using Assets.Scripts.Quest.Logic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue.UI
{
    public class DialogueUI : Singleton<DialogueUI>
    {
        public GameObject dialogueBox;
        public Text dialogueTextMesh;
        public Image faceRight, faceLeft;
        public TextMeshProUGUI nameRight, nameLeft;
        public GameObject nextPage;
        public GameObject optionPannel;
        public GameObject optionButton;
        public List<GameObject> optionButtonList;
        [Header("Data")] public DialogueData_SO currentDialogueData;
        public int currentIndex = 0;
        public void UpdateData(DialogueData_SO data)
        {
            currentDialogueData = data;
            currentIndex = 0;
        }

        public void UpdateMainUI(int index,DialogueSpriteData_SO NPCSprite,DialogueSpriteData_SO playerSprite,bool isAtLeft)
        {
            if (index > Settings.dialogueMaxSize)
            {
                dialogueBox.SetActive(false);
                return;
            }
            dialogueBox.SetActive(true);  
            OptionButtonDestroy();
            optionButtonList = new List<GameObject>();
            //Debug.LogError(currentDialogueData.dialoguePieceList.Count+" "+index);
            Dialogue_Piece piece = currentDialogueData.dialoguePieceList[index];
          
            //显示左右图片
            if (isAtLeft)
            {
                if (GetDialogueSprite(piece.PlayerSpriteType, playerSprite) != null)
                {
                    faceLeft.gameObject.SetActive(true);
                    faceLeft.sprite = GetDialogueSprite(piece.PlayerSpriteType, playerSprite);
                    nameLeft.text = playerSprite.characterName;
                }else faceLeft.gameObject.SetActive(false);

                if (GetDialogueSprite(piece.NpcSpriteType, NPCSprite) != null)
                {
                    faceRight.gameObject.SetActive(true);
                    faceRight.sprite = GetDialogueSprite(piece.NpcSpriteType, NPCSprite);
                    nameRight.text = NPCSprite.characterName;
                }else faceRight.gameObject.SetActive(false);
            }
            else
            {
                if (GetDialogueSprite(piece.PlayerSpriteType, playerSprite) != null)
                {
                    faceRight.gameObject.SetActive(true);
                    faceRight.sprite = GetDialogueSprite(piece.PlayerSpriteType, playerSprite);
                    nameRight.text = playerSprite.characterName;
                }else faceLeft.gameObject.SetActive(false);

                if (GetDialogueSprite(piece.NpcSpriteType, NPCSprite) != null)
                {
                    faceLeft.gameObject.SetActive(true);
                    faceLeft.sprite = GetDialogueSprite(piece.NpcSpriteType, NPCSprite);
                    nameLeft.text = NPCSprite.characterName;
                }else faceLeft.gameObject.SetActive(false);
            }
            //显示下一页图片
            if (piece.dialogueOptionList.Count == 0&& piece.targetIndex<=Settings.dialogueMaxSize)
            {   
                nextPage.SetActive(true);
            }else nextPage.SetActive(false);
            //显示选项BUtton
            if(piece.dialogueOptionList.Count > 0)
            {
                optionPannel.SetActive(true);
                foreach (var option in piece.dialogueOptionList)
                {
                     GameObject ButtonInstance = Instantiate(optionButton, optionButton.transform.position, Quaternion.identity,optionPannel.transform);
                     optionButtonList.Add(ButtonInstance);
                     TextMeshProUGUI text = ButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                     text.text = option.text;
                     ButtonInstance.GetComponent<Button>().onClick.AddListener(() =>
                     {
                         if (piece.questData != null)
                         {
                             var newquest = new QuestTask();
                             newquest.questData =Instantiate(piece.questData);
                             if (option.taskQuest)
                             {
                                 //添加到任务列表
                                 if (QuestManager.Instance.HaveQuest(newquest.questData))
                                 {
                                     //判断是否完成
                                     
                                 }
                                 else
                                 {
                                     QuestManager.Instance.questTaskList.Add(newquest);
                                     QuestManager.Instance.GetTask(newquest.questData).IsStarted=true;
                                 }
                             }
                         }
                         UpdateMainUI(option.targetDialogueIndex,NPCSprite,playerSprite,isAtLeft);
                         EventHandler.CallDialogueStatueUpdate(option.targetDialogueIndex);
                     });
                }

            }else optionPannel.SetActive(false);
           
            dialogueTextMesh.text = "";
            //dialogueTextMesh.text = piece.text;
            dialogueTextMesh.DOText(piece.text, 1f);
        }

        public void OptionButtonDestroy()
        {
            foreach (var item in optionButtonList)
            {
                Destroy(item.gameObject);
            }
            optionButtonList.Clear();
        }
        public Sprite GetDialogueSprite(CharacterSpriteType type,DialogueSpriteData_SO sprite)
        {
            switch (type)
            {
                case CharacterSpriteType.Default:
                    return sprite.defaultSprite;
                case CharacterSpriteType.Happy:
                    return sprite.happySprite;
                case CharacterSpriteType.SupperHappy:
                    return sprite.supperHappySprite;
                case CharacterSpriteType.Surprise:
                    return sprite.surpriseSprite;
                case CharacterSpriteType.Suspect:
                    return sprite.suspectSprite;
                case CharacterSpriteType.Sad:
                    return sprite.sadSprite;
            }

            return null;
        }
    }
}