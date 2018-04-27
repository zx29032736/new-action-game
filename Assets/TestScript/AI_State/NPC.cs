using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType { Shop, Quest, Normal }

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(SphereCollider))]
public class NPC : AI
{
    public CharacterSentence sentence;

    bool isPlayerEnter = false;
    Transform player = null;
    //public string npcName;
    public List<string> storeItemsName = new List<string>();
    public NpcNameUI uiPrefab;

    private void OnEnable()
    {
        QuestController.onQuestStateChanged += OnQuestChanged;
    }

    private void OnDisable()
    {
        QuestController.onQuestStateChanged -= OnQuestChanged;
    }

    void OnQuestChanged(bool enable, Quest quest)
    {
        for (int i = 0; i < sentence.selections.Length; i++)
        {
            if (sentence.selections[i].title == quest.questName)
            {
                sentence.selections[i].enable = enable;
            }
        }
    }

    private void Start()
    {
        aI_Type = AI_Type.NPC;
        characterController = GetComponent<CharacterController>();
        stateMachine = GetComponent<StateMachine>();
        animator = GetComponent<Animator>();
        stat = GetComponent<EnemyStat>();

        stateMachine.SetUpMachine(gameObject.name, this);
        stateMachine.ChangeState("idle");


        // setting up npc's conoverations call back
        for (int i = 0; i < sentence.selections.Length; i++)
        {

            if (sentence.selections[i].type == SelectionType.Shop)
            {
                sentence.selections[i].callback = () =>
                {
                    GameplayManager.instance.storeController.Init(storeItemsName, () =>
                    {
                        stateMachine.ChangeState("idle");
                    });
                };
            }

            else if(sentence.selections[i].type == SelectionType.Quest)
            {
                // without this => i, in this loop, will awlays == sentence.selections.Length and make errors
                int index = i;
   
                sentence.selections[i].callback = () =>
                {
                    //Debug.Log(index);

                    GameplayManager.instance.questController.AddQuest(sentence.selections[index].title);
                    stateMachine.ChangeState("idle");
                };
            }

            else if (sentence.selections[i].type == SelectionType.Normal)
            {
                //maybe add some reward after talking
                sentence.selections[i].callback = () =>
                {
                    stateMachine.ChangeState("idle");
                };
            }
        }


        //ste up ui
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Instantiate<NpcNameUI>(uiPrefab, GameplayManager.instance.screenUiController.healthBarParent).SetUpUI(name, transform, player.transform);
        //GameplayManager.instance.screenUiController.DisplayNpcUI("asd", this, GameObject.FindGameObjectWithTag("Player").transform);
    }

    private void Update()
    {
        //if (isPlayerEnter && !characterInteracting)
        //{
        //    if (characterInteracting.interactingOBJ != transform)
        //        return;

        //    if (Input.GetKeyDown(keyCode))
        //    {
        //        Interact();
        //    }

        //    GameplayManager.instance.screenUiController.DisplayInteractUI(transform);
        //}
        UseGravity();

        stateMachine.UpdateState();
    }

    public void Interact()
    {
        //Debug.Log("name : " + name + " is interacting");

        stateMachine.ChangeState("stayState");

        GameplayManager.instance.dialogController.Init(name,sentence, () => 
        {
            
        });
    }

    public void LookAtPlayer()
    {
        if (player != null)
            transform.LookAt(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerEnter = true;
            
            if (player == null)
                player = other.transform;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (moveStateProperties.center != null)
            Gizmos.DrawWireSphere(moveStateProperties.center.position, moveStateProperties.maxDistanceToCenter);
    }
}
