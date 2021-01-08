using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public enum Profession { None, Farmer, Sheppard, Twiner, Deserter, SectionMaster, DistrictMaster, CareTaker }

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NPC : MonoBehaviour, IFocusable, IHoverable
{
    public BTNode MasterBehaviourTree;
    public BTBlackboard Blackboard = new BTBlackboard();

    public NPCProfile Profile;
    public Profession Profession = Profession.None;
    public IItem Item;
    public House AssignedHouse;
    public Farm AssignedFarm;

    public Vector3 CurrentTargetDestination { get { return agent.destination; } }

    [Header("Focus Settings: ")]
    [SerializeField] private float zoomAmountOnFocus = 1;
    public float ZoomAmountOnFocus { get { return zoomAmountOnFocus; } set { zoomAmountOnFocus = value; } }

    [Header("References: ")]
    [SerializeField] GameObject selectionRing;
    [SerializeField] Material selectedRingMaterial;
    [SerializeField] Material unselectedRingMaterial;
    private Vector3 selectionRingDefaultScale;

    private bool canWalk = true;

    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        Blackboard.NPC = this;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        selectionRingDefaultScale = selectionRing.transform.localScale;
    }

    private void Start()
    {   
        InitializeBehaviourTree();
    }

    private void InitializeBehaviourTree()
    {

        #region Farming Behaviour

        //HARVESTING
        BTSelector _harvestingBehaviour = new BTSelector(Blackboard,
            new BTIsEverythingHarvested(Blackboard),
            new BTHarvestCrops(Blackboard)
            );

        //SEEDING
        BTSelector _seedingBehaviour = new BTSelector(Blackboard,
            new BTIsEverythingSeeded(Blackboard),
            new BTSequencer(Blackboard,

                new BTSelector(Blackboard,
                    new BTHasItemOfType(Blackboard, typeof(Seeds)),
                    new BTGetSeeds(Blackboard)
                    ),

                new BTPlantSeeds(Blackboard)
                )
            );

        //WATERING
        BTSelector _wateringBehaviour = new BTSelector(Blackboard,
            new BTIsEverythingWatered(Blackboard),

            new BTSequencer(Blackboard,
                new BTSelector(Blackboard,
                    new BTHasItemOfType(Blackboard, typeof(WateringCan)),
                    new BTGetWateringCan(Blackboard)
                    ),

                new BTSelector(Blackboard,
                    new BTCurrentItemIsFilledWithType(Blackboard, FillType.Water),
                    new BTGetWaterFromWell(Blackboard)
                    ),

                new BTWaterCrops(Blackboard, AssignedFarm)
                )
            );


        BTSequencer _farmingBehaviour = new BTSequencer(Blackboard,
            _harvestingBehaviour,
            _seedingBehaviour,
            _wateringBehaviour
            //_fertilizeBehaviour
            );

        #endregion

        BTSelector _checkForProfession = new BTSelector(Blackboard,
            new BTInvert(new BTHasProfession(Blackboard, Profession.None))
            //,BTDoOwnThing() (of een BTSelector _doOwnThing)
            );

        BTSequencer _handleProfession = new BTSequencer(Blackboard,
            new BTHasProfession(Blackboard, Profession.Farmer),
            _farmingBehaviour
            );

        MasterBehaviourTree = new BTSequencer(Blackboard, _checkForProfession, _handleProfession);
    }

    private void Update()
    {
        if (canWalk) { agent.isStopped = false; }
        else { agent.isStopped = true; }

        float _horizontalValue = Mathf.Max(Mathf.Abs(agent.velocity.x), Mathf.Abs(agent.velocity.z));
        animator.SetFloat("Horizontal", _horizontalValue);

        MasterBehaviourTree.Tick();
    }

    public Vector3 ClosestDestinationByPath(Vector3 _subjectedPos, List<Vector3> _targetPosList)
    {
        Vector3 _closestPos = _targetPosList[0];
        float _closestDistance = Mathf.Infinity;

        foreach (var _targetPos in _targetPosList)
        {
            NavMeshPath _path = new NavMeshPath();
            agent.CalculatePath(_targetPos, _path);

            float _distance = 0; 
            _distance += Vector3.Distance(_subjectedPos, _path.corners[0]);

            for (int i = 0; i < _path.corners.Length-1; i++)
            {
                _distance += Vector3.Distance(_path.corners[i], _path.corners[i + 1]);
            }


            if (_distance < _closestDistance)
            {
                _closestPos = _targetPos;
                _closestDistance = _distance;
            }
        }
        return _closestPos;
    }

    public bool ReachedTargetDestination(Vector3 _destination, float _allowedDistanceToDestination = 1f)
    {
        Vector3 _flatTargetPos = _destination;
        Vector3 _flatPos = transform.position;
        _flatTargetPos.y = _flatPos.y = 0;

        if (CurrentTargetDestination != _flatTargetPos) {SetTargetDestination(_flatTargetPos); }

        if (Vector3.Distance(_flatPos, _flatTargetPos) <= _allowedDistanceToDestination)  { return true;  }
        else { return false; }

    }

    public void SetTargetDestination(Vector3 _destination)
    {
        agent.SetDestination(_destination);
    }

    public void SetTargetDestination(Transform _destination)
    {
        if (_destination != null) { SetTargetDestination(_destination.position); }
        else { ResetNavigation(); }
    }

    public void SetTargetDestination(GameObject _destination)
    {
        if(_destination != null) { SetTargetDestination(_destination.transform.position); }
        else { ResetNavigation(); }
    }

    public void ResetNavigation()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void ResetAnimator()
    {
        animator.SetTrigger("Reset");
    }

    //fix deze minimalize optie
    public void StartSeedAnimation()
    {
        animator.SetBool("IsPlanting", true);
        canWalk = false;
    }

    public void StopSeedAnimation()
    {
        animator.SetBool("IsPlanting", false);
        canWalk = true;
    }

    public void StartWaterAnimation()
    {
        animator.SetBool("IsWatering", true);
        canWalk = false;
    }

    public void StopWaterAnimation()
    {
        animator.SetBool("IsWatering", false);
        canWalk = true;
    }

    public void SetProfession(int _profession)
    {
        Profession = (Profession)_profession;
    }

    public void ChangeFarm(bool _ownFarm)
    {
        if (_ownFarm) { AssignedFarm = AssignedHouse.CorrespondingFarm; }
        else { AssignedFarm = AssignedHouse.AdjecentCommunityFarm; }
    }

    public void OnFocus()
    {
        selectionRing.GetComponent<Renderer>().material = selectedRingMaterial;
        GameManager.Instance.NPCManager.SelectedNPC = this;
        GameManager.Instance.UIManager.ActivateProfessionSelectionMenu(Profession);
    }

    public void OnUnfocus()
    {
        selectionRing.GetComponent<Renderer>().material = unselectedRingMaterial;
        GameManager.Instance.NPCManager.SelectedNPC = null;
        GameManager.Instance.UIManager.DeactivateProfessionSelectionMenu();
    }

    public void OnHover()
    {
        selectionRing.transform.localScale = selectionRingDefaultScale * 1.5f;
    }

    public void OnUnhover()
    {
        selectionRing.transform.localScale = selectionRingDefaultScale;
    }
}

[System.Serializable]
public class NPCProfile
{
    public string FirstName;
    public string LastName;

    public int Age;
    public Date Birthday = new Date();
    public Date Deathday = new Date();

    public bool IsMale = true;
    public bool IsMature;

    public NPCProfile Mother;
    public NPCProfile Father;
    public List<NPCProfile> Siblings = new List<NPCProfile>();
    public List<NPCProfile> Offspring = new List<NPCProfile>();
    public NPCProfile Partner;
}
