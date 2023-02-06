using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Astar;
using Assets.Scripts.NPC.Data;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.NPC
{
    [RequireComponent(typeof(Rigidbody2D) )]
    [RequireComponent(typeof(Animator) )]
    public class NPCMovement : MonoBehaviour
    {
        public ScheduleDetailsDataList_SO scheduleDataList;

        private SortedSet<ScheduleDetails> scheduleSet;

        private ScheduleDetails currentScheduleDetails;
        //临时状态信息
        private string currentScene;
        private string targetScene;
        private Vector3Int currentGridPosition;
        private Vector3Int targetGridPosition=new Vector3Int();
        private Vector3Int nextGridPosition;
        public string StartScene;

        
        
        [Header("移动属性")] public float normalSpeed = 2f;
        private float minSpeed = 1f;
        private float maxSpeed = 3f;
        private Vector2 dir;
        public bool isMoving;
        public bool isNPCMove = false;
        private Grid grid;
        private Rigidbody2D rigidbody2D;
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider2D;
        private Animator animator;
        private bool isInit=false;
        private Stack<MovementStep> movementSteps=new Stack<MovementStep>();
        private bool sceneLoad = false;
        private TimeSpan GameTime => TimeManager.Instance.GameTime;

        private float animationBreakTime;
        private bool canPlayStopAnimation=false;
        private AnimationClip stopAnimationClip;
        public AnimationClip blankAnimationClip;
        private AnimatorOverrideController animOverride;
        
        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
            currentScene = StartScene;
            animOverride = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animOverride;
            scheduleSet = new SortedSet<ScheduleDetails>();
            foreach (var item in scheduleDataList.scheduleList)
            {
                scheduleSet.Add(item);
            }
        }

        private void OnEnable()
        {
            EventHandler.AfterSceneLoadEvent += AfterSceneLoadEvent;
            EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;
            EventHandler.GameMinuteEvent += GameMinuteEvent;
        }

        

        private void OnDisable()
        {
            EventHandler.AfterSceneLoadEvent -= AfterSceneLoadEvent;
            EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
            EventHandler.GameMinuteEvent -= GameMinuteEvent;
        }
        private void GameMinuteEvent(int minute, int hour,int day,Season season )
        {
            int time = (hour * 100 + minute);
            ScheduleDetails matchScheduleDetails = null;
            foreach (var item in scheduleSet)
            {
                if (item.time == time)
                {
                    if(day!=item.day&& item.day!=0)
                        continue;
                    if(item.season!=season)
                        continue;
                    matchScheduleDetails = item;
                }
                else if(item.time>time)
                {
                    break;
                }
                
            }

            if (matchScheduleDetails != null)
            {
                BuildPath(matchScheduleDetails);
            }
        }
        private void BeforeSceneUnloadEvent()
        {
            sceneLoad = false;
        }

        private void Update()
        {
            if(sceneLoad)
                SwitchAnimation();
            if (animationBreakTime > 0)
                animationBreakTime -= Time.deltaTime;
            canPlayStopAnimation = animationBreakTime <= 0;
        }

        
        private void FixedUpdate()
        {
            if(sceneLoad)
                Movement();
        }

        private void AfterSceneLoadEvent()
        {
            grid = FindObjectOfType<Grid>();
            CheckVisiable();
            if (!isInit)
            {
                InitNPC();
                isInit = true;
            }
            sceneLoad = true;
        }

        private void CheckVisiable()
        {
            //Debug.Log(currentScene);
            if (currentScene == SceneManager.GetActiveScene().name)
            {
                SetActiveInscene();
            }
            else SetInActiveInscene();
        }

        private void InitNPC()
        {
            targetScene = currentScene;
            //保持NPC的位置在网格的中心
            currentGridPosition = grid.WorldToCell(transform.position);
            transform.position = new Vector3(currentGridPosition.x + Settings.gridCellSize / 2,
                currentGridPosition.y + Settings.gridCellSize / 2, 0);
            targetGridPosition = currentGridPosition;
        }

        private void Movement()
        {
            if (!isNPCMove)
            {
                
               
                if (movementSteps.Count > 0)
                {
                    MovementStep step = movementSteps.Pop();
                    currentScene = step.sceneName;
                    CheckVisiable();
                    nextGridPosition = (Vector3Int) step.gridPos;
                    TimeSpan stepTime = new TimeSpan(step.hour, step.minute, step.second);
                    MoveToGridPosition(nextGridPosition, stepTime);
                }
                else
                {
                    
                    if (!isMoving&& canPlayStopAnimation)
                    {
                        StartCoroutine(SetStopAnimation());
                    }
                }
            }
        }

        private void MoveToGridPosition(Vector3Int targetPos, TimeSpan time)
        {
            StartCoroutine(MoveRuntine(targetPos,time));
        }

        private IEnumerator MoveRuntine(Vector3Int targetPos, TimeSpan time)
        {
            isNPCMove = true;  
            var woldPos = GetWoldPosition(targetPos);
            if (time > GameTime)
            {
              
                float timeToMove = (float) (time.TotalSeconds - GameTime.TotalSeconds);
                float dis = Vector3.Distance(transform.position, woldPos);
                float speed = Mathf.Max(minSpeed, dis / timeToMove / Settings.secondThreshold);
                if (speed <= maxSpeed)
                {
                    while (Vector3.Distance(transform.position,woldPos) >Settings.pixelSize)
                    {
                        dir = (woldPos - transform.position).normalized;
                        Vector2 posOffset = new Vector2(dir.x * speed * Time.fixedDeltaTime,
                            dir.y * speed * Time.fixedDeltaTime);
                        
                        rigidbody2D.MovePosition(rigidbody2D.position+posOffset);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }

            rigidbody2D.position = woldPos;
            currentGridPosition = targetPos;
            isNPCMove = false;
        }

        private IEnumerator SetStopAnimation()
        {
            //强制面向镜头
            animator.SetFloat("DirY",-1);
            animator.SetFloat("DirX",0);
            animationBreakTime = Settings.animationBreakTime;
            if (stopAnimationClip != null)
            {
                animOverride[blankAnimationClip] = stopAnimationClip;
                animator.SetBool("EventAnimation",true);
                yield return null;
                animator.SetBool("EventAnimation",false);
            }
            else
            { 
                animOverride[stopAnimationClip] = blankAnimationClip;
                animator.SetBool("EventAnimation",false);
            }
        }
        private Vector3 GetWoldPosition(Vector3Int pos)
        {
            Vector3 woldPos = grid.CellToWorld(pos);
            return new Vector3(woldPos.x + Settings.gridCellSize / 2f, woldPos.y + Settings.gridCellSize / 2f);
        }
        public void BuildPath(ScheduleDetails scheduleDetails)
        {
            //TODO:跨场景
            movementSteps.Clear();
            currentScheduleDetails = scheduleDetails;
            targetGridPosition = (Vector3Int) scheduleDetails.targetGridPosition;
        
            stopAnimationClip = scheduleDetails.animAtStop;
            if (scheduleDetails.targetScene == currentScene)
            {
                Astar.Astar.Instance.BuildPath(scheduleDetails.targetScene,(Vector2Int)currentGridPosition,
                    scheduleDetails.targetGridPosition,movementSteps);
            }
            else
            {
                
                SceneRoute sceneRoute = NPCManager.Instance.GetSceneRoute(currentScene, scheduleDetails.targetScene);
                if (sceneRoute != null)
                {
                    for (int i = 0; i < sceneRoute.scenePathList.Count; i++)
                    {
                        Vector2Int fromPos, gotoPos;
                        ScenePath scenePath = sceneRoute.scenePathList[i];
                        if (scenePath.fromGridCell.x >= Settings.maxGridSize ||
                            scenePath.fromGridCell.y >= Settings.maxGridSize)
                        {
                            fromPos = (Vector2Int) currentGridPosition;
                            
                        }
                        else
                        {
                            fromPos = scenePath.fromGridCell;
                        }
                        
                        if (scenePath.gotoGridCell.x >= Settings.maxGridSize ||
                            scenePath.gotoGridCell.y >= Settings.maxGridSize)
                        {
                            gotoPos= (Vector2Int) targetGridPosition;
                            
                        }
                        else
                        {
                            gotoPos = scenePath.gotoGridCell;
                        }
                       
                        Astar.Astar.Instance.BuildPath(scenePath.sceneName,fromPos,gotoPos,movementSteps);
                    }
                }
            }
            if (movementSteps.Count > 1)
            {
                UpdateTimeOnPath();
            }
        }

        private void UpdateTimeOnPath()
        {
            MovementStep preMovement=null;
            TimeSpan currentGameTime=GameTime;
            foreach (var step in movementSteps)
            {
                if (preMovement == null)
                {
                    preMovement = step;
                }
                
                step.hour = currentGameTime.Hours;
                step.minute = currentGameTime.Minutes;
                step.second = currentGameTime.Seconds;
                TimeSpan timeSpan;
                if (MoveInDiagonal(step, preMovement))
                {
                  timeSpan = new TimeSpan(0, 0,
                        (int) (Settings.gridCellDiagonalSize / normalSpeed / Settings.secondThreshold));
                }else
                    timeSpan = new TimeSpan(0, 0,
                    (int) (Settings.gridCellSize / normalSpeed / Settings.secondThreshold));
                currentGameTime = currentGameTime.Add(timeSpan);
                preMovement = step;
            }
        }

        private bool MoveInDiagonal(MovementStep current, MovementStep pre)
        {
            return (current.gridPos.x != pre.gridPos.x)&&(current.gridPos.y!=pre.gridPos.y);
        }

        private void SwitchAnimation()
        {
           
            isMoving = transform.position != GetWoldPosition(targetGridPosition);
            animator.SetBool("IsMoving",isMoving);
            if (isMoving)
            {
                
                animator.SetBool("Exit",true);
                animator.SetFloat("DirY",dir.y);
                animator.SetFloat("DirX",dir.x);
            }
            else
            {
                animator.SetBool("Exit",false);
            }
        }
        private void SetActiveInscene()
        {
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;
           
            transform.GetChild(0).gameObject.SetActive(true);
        }
        private void SetInActiveInscene()
        {
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = false;
           
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}