using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CoroutineHandler CoroutineHandler;
    public UIManager UIManager;
    public EventManager EventManager;
    public TimeSystem TimeSystem;
    public MoneyGraphSystem MoneyGraphSystem;
    public StructuresManager StructuresManager;
    public NPCManager NPCManager;
    public CropsManager CropsManager;
    public AudioManager AudioManager;
    public CameraController CameraController;
    public CreditsBehaviour CreditsBehaviour;
    public TimeJumpAlterations TimeJumpAlterations;
    public TimeJumpAlterations EasterEggAlterations;

    [Header("MiniGames:")]
    public CrowsManager CrowsManager;
    public CrowsMinigame CrowsMinigame;
    public Ship Ship;

    public float ColonistContentmentLevel { get { return colonistContentmentLevel; } set { colonistContentmentLevel = Mathf.Clamp(value, 0, 100); } }
    public float SubCommissionLevel { get { return subCommissionContentmentLevel; } set { subCommissionContentmentLevel = Mathf.Clamp(value, 0, 100); } }
    public float Money { get { return money; } set { money = Mathf.Clamp(value, 0, 100); } }
    public float MoneyOfPreviousMonth { get { return moneyOfPreviousMonth; } set { moneyOfPreviousMonth = Mathf.Clamp(value, 0, 100); } }

    [SerializeField, Range(0, 100)] private float colonistContentmentLevel;
    [SerializeField, Range(0, 100)] private float subCommissionContentmentLevel;
    [SerializeField, Range(0, 100)] private float money;
    private float moneyOfPreviousMonth;
    public float MoneyMaximum { get; private set; }

    private bool pauseMenuIsOn = false;

    private void Awake()
    {
        Instance = Instance ?? this;
        MoneyMaximum = 100;

        moneyOfPreviousMonth = Money;
        Money = 75;

        SaveSystem.LoadGame();

        UIManager.UpdateIncome(moneyOfPreviousMonth, Money);
        UIManager.UpdateContentmentLevels(subCommissionContentmentLevel / 100f, colonistContentmentLevel / 100f, true);
    }

    private void Start()
    {
        Date _startingDate = new Date();
        _startingDate.Day = 1;
        _startingDate.Month = 10;
        _startingDate.Year = 1818;

        if (SaveSystem.SaveData.EasterEggtivated) { EasterEggAlterations.Alter(); }

        if (TimeSystem.CurrentDate != _startingDate) { TimeJumpAlterations.Alter(); OnNextDay(); }
        else { EventManager.StartEventOfCurrentDate(); }
    }

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    if (Time.timeScale >= 0) { Time.timeScale += 1; }
        //    else { Time.timeScale += 0.1f; }
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    if (Time.timeScale >= 0) { Time.timeScale -= 1; }
        //    else { Time.timeScale -= 0.1f; }
        //}

        //if (Input.GetKeyDown(KeyCode.Space)) { Time.timeScale = 1; }


        CheckForShipment();

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1) { PauseGame(); UIManager.ActivatePauseMenu(); pauseMenuIsOn = true; }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0 && pauseMenuIsOn) { ResumeGame(); UIManager.DeactivatePauseMenu(); pauseMenuIsOn = false; }
    }

    private bool CheckIfColonistEscapes()
    {
        if (colonistContentmentLevel < 40f) { return Random.Range(0f, colonistContentmentLevel / 100f) < 5f; }
        else { return false; }

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        CameraController.IsEnabled = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        TimeSystem.IsPaused = false;
        AudioListener.pause = false;
        CameraController.IsEnabled = true;
        //moneyOfPreviousMonth = Money;
        //Money = Random.Range(0, MoneyMaximum); //HAAL DIT WEG
        //UIManager.UpdateIncome(moneyOfPreviousMonth, Money);
    }

    public void QuitGame()
    {
        Application.Quit();

        //Dit zorge er voor dat het spel vast hing nadat je weer gaat spelen vanaf het main menu
        //CoroutineHandler.StopAllCoroutines();
        //SceneManager.LoadScene(0);
    }

    public void AdjustMoneyAndContentmentLevels(float _subcomission, float _colonist, float _money)
    {
        SubCommissionLevel += _subcomission;
        ColonistContentmentLevel += _colonist;
        Money += _money;

        UIManager.UpdateContentmentLevels(subCommissionContentmentLevel / 100f, ColonistContentmentLevel / 100f);
        UIManager.UpdateIncome(moneyOfPreviousMonth, Money);
    }

    public void OnNightTime()
    {
        TimeSystem.IsPaused = true;

        //Save current date before it changes to the next day and breaks the system
        EventManager.TimeJumpCheckDate.Day = TimeSystem.CurrentDate.Day;
        EventManager.TimeJumpCheckDate.Month = TimeSystem.CurrentDate.Month;
        EventManager.TimeJumpCheckDate.Year = TimeSystem.CurrentDate.Year;

        TimeSystem.SetTimeToMorning();

        //Handle TimeJump already so the right date is saved in the save file
        TimeJump _timeJump = EventManager.CheckForTimeJumpAndReturnJump();
        if (_timeJump != null) { TimeSystem.SetDate(_timeJump.To); }

        //Save the game
        SaveSystem.UpdateSaveData();
        SaveSystem.SaveGame();

        UnityAction _onFinished = null;
        _onFinished += NPCManager.TeleportAllNPCsToTheirHome;
        _onFinished += CameraController.Unfocus;
        _onFinished += PauseGame;

        //Check for escaping colonists (only if they are unhappy)
        if (CheckIfColonistEscapes()) { _onFinished += EventManager.StartColonistEscapesEvent; }
        else { _onFinished += UIManager.ActivateEndOfDayWindow; }

        NPCManager.MakeAllNPCsGoHome();
        UIManager.Fade(false, 5f, _onFinished);
    }

    public void OnNextDay()
    {
        StructuresManager.OnNextDay();
        CropsManager.AdvanceAllCrops();

        UIManager.DeactivateEndOfDayWindow();
        AudioManager.RestartMusic();

        Ship.HasReceivedShipment = false;
        Ship.gameObject.SetActive(false);

        if (CrowsManager.OnNextDay()) { return; }
        if (EventManager.CheckForTimeJump()) { return; }

        EventManager.StartEventOfCurrentDate();
        ResumeGame();

        UIManager.Fade(true, 3f);
    }

    public void CheckForShipment()
    {
        if (TimeSystem.CurrentDate < Ship.MinimalDateBeforeArrival) { return; }
        if (TimeSystem.GetCurrentTime() < Ship.TimeOfArrival) { return; }
        if (Ship.HasReceivedShipment) { return; }

        if (!Ship.CompletedShipmentPreviousDay) { EventManager.StartShipmentEvent(); }
        Ship.gameObject.SetActive(true);
    }
}
