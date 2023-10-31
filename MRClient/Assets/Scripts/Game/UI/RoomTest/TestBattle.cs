using BDFramework.ScreenView;
using BDFramework.UFlux;
using Cysharp.Threading.Tasks;
using MR.Battle;
using MR.Net.Frame;
using MR.Net.Proto.Battle;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using TrueSync;
using UI.GameUI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TestBattle : MonoBehaviour {
    public static GameStartS2C StartMsg { get; set; }
    public static Dictionary<int, string> PlayerNameDic { get; set; }
    public static Dictionary<int, ulong[]> PlayerWeaponDic { get; set; }
    public static List<int> Offlines { get; set; }

    public TMP_Text txtTime;
    public TMP_Text txtKillNumA;
    public TMP_Text txtKillNumB;
    public InputActionAsset inputActionAsset;

    public GameObject battleGroup;
    public GameObject ControlGroup;
    public GameObject ViewGroup;
    public GameObject cardGroup;

    public Button btnWatchNext;

    public Button[] btnWeapons;

    public GameObject hint1Go;
    public GameObject hint2Go;

    [BoxGroup("MoveBtn")] public GameObject moveFGGo;

    [BoxGroup("AtkBtn")] public GameObject atkBtnGo;

    [BoxGroup("WaitDraw")] public GameObject waitDrawGo;
    [BoxGroup("WaitDraw")] public Image waitDrawProgress;

    [BoxGroup("DefBtn")] public GameObject defBtnGo;

    [BoxGroup("DiscardBtn")] public GameObject discardBtnGo;

    [BoxGroup("DodgeBtn")] public Image dodgeBtnProgress;
    [BoxGroup("DodgeBtn")] public Color dodgeEnableColor;
    [BoxGroup("DodgeBtn")] public Color dodgeDisableColor;

    [BoxGroup("PlayerSelf")] public GameObject playerSelfGroupGo;
    [BoxGroup("PlayerSelf")] public Slider playerSelfHPSld;
    [BoxGroup("PlayerSelf")] public Slider playerSelfSPSld;
    [BoxGroup("PlayerSelf")] public Transform playerSelfGems;

    [BoxGroup("Score")] public TestRoomScore score;

    [BoxGroup("Templates")] public GameObject gemTemplate;
    [BoxGroup("Templates")] public GameObject killHintTemplate;

    private TcpInstance m_Tcp;
    private KcpInstance m_Kcp;
    private Battle m_Battle;
    private bool m_GameRun;

    private InputAction m_MoveAction;
    private InputAction m_LookAction;
    private InputAction m_MouseLookAction;
    private InputAction m_DodgeAction;
    private InputAction m_FightAction;
    private InputAction m_MouseFightAction;
    private InputAction m_DefenseAction;
    private InputAction m_DiscardAction;
    private InputAction m_Skill1Action;
    private InputAction m_Skill2Action;
    private InputAction m_Skill3Action;

    private PlayerCD m_WatchPlayer;
    private PlayerState m_WatchPlayerState;

    private bool m_MouseCameraMode;
    private List<GameObject> m_Gems = new List<GameObject>();
    private int m_KillRecordIdx;

    private GameObject m_AimEffect;
    private LocationCD m_AimLoction;
    private Transform m_AimTrans;

    private void Awake() {
        score.gameObject.SetActive(false);
        moveFGGo.SetActive(false);
        gemTemplate.SetActive(false);
        hint1Go.SetActive(false);
        hint2Go.SetActive(false);

        LoadBattle();

        m_Tcp = Net.GetTcp();
        Regist();

        foreach (var map in inputActionAsset.actionMaps)
            foreach (var action in map)
                action.Enable();

        Offlines = new List<int>();
        BGMManager.Play(BGMManager.BGMType.Fight);
    }

    public async UniTask LoadBattle() {
        await UFluxUtils.AsyncLoadScene("Battlefield7");
        BattleResources.AsPreLoad();
        var ao = UFluxUtils.LoadAsset<GameObject>("Logic/BattleCamera.prefab");
        await ao;
        Instantiate(ao.Result);

        var aimo = UFluxUtils.LoadAsset<GameObject>("Effects/FX_Common_Aim_Loop.prefab");
        await aimo;
        m_AimEffect = Instantiate(aimo.Result);
        m_AimEffect.SetActive(false);

        foreach (var item in UFluxUtils.TaskList)
            await item.ToUniTask();

        BattleResources.HidePool();
        ConnectKcp();
        BattleStart();
    }

    private void OnReturn() {
        ScreenViewManager.Inst.MainLayer.BeginNavTo(ScreenViewEnum.Main);
        UIManager.Inst.ScalMaxWindow(WinEnum.Win_BattleReady);
        UFluxUtils.AsyncUnLoadkScene("Battlefield7");
        UIManager.Inst.SendMessage(WinEnum.Win_Loading, new UIMsg_Loading());
        Destroy(gameObject);
        BGMManager.Play(BGMManager.BGMType.Main);
    }

    private void Update() {
        if (m_GameRun) {
            txtTime.text = m_Battle.RemainTime.ToString("0");

            var list = m_Battle.Update(Time.deltaTime);

            if (m_Battle.GameOver) {
                m_GameRun = false;
                var tmp = new List<ScorePB>();
                foreach (var data in m_Battle.Score.Datas) {
                    var v = data.Value;
                    tmp.Add(new ScorePB { Index = data.Key, Kill = v.kill, Die = v.die, Output = v.output.RawValue, Take = v.take.RawValue });
                }
                m_Tcp.Send(new GameResultC2S { ScorePBs = tmp.ToArray() });
                return;
            }

            foreach (var data in list)
                OnGetData(data);
            var player = m_Battle.CameraPlayer;
            if (player != null) {
                var playerState = m_Battle.CameraPlayer.State;
                if (m_WatchPlayer != player || m_WatchPlayerState != playerState) {
                    m_WatchPlayer = player;
                    m_WatchPlayerState = playerState;
                    switch (playerState) {
                        case PlayerState.WaitCard:
                            cardGroup.SetActive(false);
                            SetCanAtk(false);
                            SetCanDef(false);
                            SetCanDiscard(false);
                            ChangeMouseMode(StartMsg.Index == -1);
                            break;
                        case PlayerState.ShowHand:
                            cardGroup.SetActive(true);
                            ClearGem();
                            for (int i = 0; i < 3; i++) {
                                SetCardToButton(player.Weapons[i].Icon, btnWeapons[i].transform);
                                SetGem(player.HandCard[i].Cards.Count, btnWeapons[i].transform);
                            }
                            SetCanAtk(false);
                            SetCanDef(false);
                            SetCanDiscard(false);
                            ChangeMouseMode(true);
                            break;
                        case PlayerState.WaitFightStart:
                            cardGroup.SetActive(false);
                            SetCanAtk(false);
                            SetCanDef(false);
                            SetCanDiscard(false);
                            ChangeMouseMode(StartMsg.Index == -1);
                            break;
                        case PlayerState.Fight:
                            cardGroup.SetActive(false);
                            SetCanAtk(true);
                            SetCardToButton(player.Weapons[player.SelectCard].Icon, atkBtnGo.transform);
                            ShowPlayerGem(player.HandCard[player.SelectCard].Cards.Count);
                            SetCanDef(Config.Equips.WeaponType[player.Weapons[player.SelectCard].Type].CanDef != 0);
                            SetCanDiscard(true);
                            ChangeMouseMode(StartMsg.Index == -1);
                            break;
                        case PlayerState.WaitFightEnd:
                            cardGroup.SetActive(false);
                            SetCanAtk(false);
                            SetCanDef(false);
                            SetCanDiscard(false);
                            ChangeMouseMode(StartMsg.Index == -1);
                            break;
                    }
                }


                switch (playerState) {
                    case PlayerState.WaitCard:
                        waitDrawProgress.fillAmount = m_WatchPlayer.StateTime.AsFloat() / Config.Battle.Constant.DualDuration;
                        break;
                }

                var ep = m_WatchPlayer.EP.AsFloat();
                dodgeBtnProgress.fillAmount = ep / Config.Battle.Constant.EPMax;
                dodgeBtnProgress.color = ep > Config.Battle.Constant.EpReductByDodge ? dodgeEnableColor : dodgeDisableColor;

                SetPlayer(playerSelfGroupGo, playerSelfHPSld, playerSelfSPSld, player.Unit);

                int killA = m_Battle.CameraPlayer.Index < 3 ? m_Battle.Score.killA : m_Battle.Score.killB;
                int killB = m_Battle.CameraPlayer.Index < 3 ? m_Battle.Score.killB : m_Battle.Score.killA;
                txtKillNumA.text = NumberToSpriteStr(killA);
                txtKillNumB.text = NumberToSpriteStr(killB);
            }

            if (m_Battle.NearOver)
                BGMManager.Play(BGMManager.BGMType.Fight2);
            hint1Go.SetActive(m_Battle.NearOver);
            hint2Go.SetActive(m_Battle.Over);

            if (m_KillRecordIdx < m_Battle.Score.KillRecord.Count) {
                var data = m_Battle.Score.KillRecord[m_KillRecordIdx++];
                var go = Instantiate(killHintTemplate, transform, true);
                Destroy(go, 2);
                var trans = go.transform;
                if (data.killer < 3 == player.Index < 3) {
                    trans.Find("HeadA/Bg2").gameObject.SetActive(false);
                    trans.Find("HeadA/Name1").GetComponent<TMP_Text>().text = PlayerNameDic[data.killer];
                    trans.Find("HeadA/Name2").gameObject.SetActive(false);
                    trans.Find("HeadB/Bg1").gameObject.SetActive(false);
                    trans.Find("HeadB/Name1").gameObject.SetActive(false);
                    trans.Find("HeadB/Name2").GetComponent<TMP_Text>().text = PlayerNameDic[data.killed];
                    trans.Find("HeadB/Kill1").gameObject.SetActive(false);
                } else {
                    trans.Find("HeadA/Bg1").gameObject.SetActive(false);
                    trans.Find("HeadA/Name2").GetComponent<TMP_Text>().text = PlayerNameDic[data.killer];
                    trans.Find("HeadA/Name1").gameObject.SetActive(false);
                    trans.Find("HeadB/Bg2").gameObject.SetActive(false);
                    trans.Find("HeadB/Name2").gameObject.SetActive(false);
                    trans.Find("HeadB/Name1").GetComponent<TMP_Text>().text = PlayerNameDic[data.killed];
                    trans.Find("HeadB/Kill2").gameObject.SetActive(false);
                }

            }
            UpdateAim();
        }
    }

    private void UpdateAim() {
        var target = m_WatchPlayer?.Unit?.Target;
        if (target != null) {
            if (target != m_AimLoction) {
                m_AimLoction = target;
                m_AimTrans = m_WatchPlayer.Unit.Target.GetComponentData<UnitDisplayCD>().UnitDisplay.transform;
                m_AimEffect.SetActive(true);
            }
            m_AimEffect.transform.position = m_AimTrans.position;
        } else {
            m_AimEffect.SetActive(false);
        }
    }

    private string NumberToSpriteStr(int number) {
        var str = number.ToString();
        var result = "";
        foreach (var c in str)
            result += $"<sprite={c}>";
        return result;
    }

    private void ChangeMouseMode(bool free) {
#if UNITY_STANDALONE
        if (free) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            m_MouseCameraMode = false;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            m_MouseCameraMode = true;
        }
#endif
    }

    private void SetCardToButton(string icon, Transform trans) {
        var iconImg = trans.Find("Icon").GetComponentInChildren<Image>();
        var iconLight = trans.Find("IconLight").GetComponentInChildren<Image>();
        iconImg.sprite = UFluxUtils.LoadIcon(icon);
        iconLight.sprite = UFluxUtils.LoadIcon(icon + "_light");
    }

    private void SetGem(int num, Transform trans) {
        var gemGroup = trans.Find("GemGroup");
        for (int i = 0; i < num; i++) {
            var gem = Instantiate(gemTemplate, gemGroup, false);
            gem.SetActive(true);
            m_Gems.Add(gem);
        }
    }

    private void ShowPlayerGem(int num) {
        for (int i = 0; i < num; i++) {
            var go = Instantiate(gemTemplate, playerSelfGems, false);
            go.SetActive(true);
            go.GetComponent<Image>().CrossFadeAlpha(0, 1, false);
            Destroy(go, 1);
        }
    }

    private void ClearGem() {
        for (int i = 0; i < m_Gems.Count; i++)
            Destroy(m_Gems[i]);
        m_Gems.Clear();
    }

    private void SetPlayer(GameObject group, Slider hpSlider, Slider spSlider, UnitCD unit) {
        if (unit == null)
            group.SetActive(false);
        else {
            group.SetActive(true);

            hpSlider.value = unit.HP.AsFloat() / Config.Battle.Constant.HPMax;
            spSlider.value = unit.Player.SP.AsFloat() / Config.Battle.Constant.SPMax;
        }
    }

    private void SetCanAtk(bool enable) {
        atkBtnGo.SetActive(enable);
        waitDrawGo.SetActive(!enable);
        waitDrawProgress.fillAmount = 0;
    }

    private void SetCanDef(bool enable) {
        defBtnGo.SetActive(enable);
    }

    private void SetCanDiscard(bool show) {
        discardBtnGo.SetActive(show);
    }

    private void OnDestroy() {
        ChangeMouseMode(true);
        UnRegist();
        m_Kcp.Close();

        m_MoveAction.started -= MoveAction_started;
        m_MoveAction.performed -= MoveAction_performed;
        m_MoveAction.canceled -= MoveAction_canceled;

        m_LookAction.performed -= LookAction_performed;

        m_MouseLookAction.performed -= MouseLookAction_performed;

        m_DodgeAction.started -= DodgeAction_started;
        m_DodgeAction.canceled -= DodgeAction_canceled;

        m_FightAction.started -= FightAction_started;
        m_FightAction.canceled -= FightAction_canceled;

        m_MouseFightAction.started -= MouseFightAction_started;
        m_MouseFightAction.canceled -= MouseFightAction_canceled;

        m_DefenseAction.started -= DefenseAction_started;

        m_DiscardAction.started -= m_DiscardAction_started;

        m_Skill1Action.started -= OnSkill1;
        m_Skill2Action.started -= OnSkill2;
        m_Skill3Action.started -= OnSkill3;
    }

    private void Regist() {
        m_Tcp.Regist<GameRunS2C>(OnMsgGameRun);
        m_Tcp.Regist<GameEndS2C>(OnMsgGameEnd);
        m_Tcp.Regist<PlayerUpdateS2C>(OnPlayerUpdate);
    }

    private void UnRegist() {
        m_Tcp.UnRegist<GameRunS2C>(OnMsgGameRun);
        m_Tcp.UnRegist<GameEndS2C>(OnMsgGameEnd);
        m_Tcp.UnRegist<PlayerUpdateS2C>(OnPlayerUpdate);
    }

    private void ConnectKcp() {
        m_Kcp = Net.CreateKcp(StartMsg.Port);
        m_Kcp.Regist(OnReadData);
        m_Kcp.Send(BitConverter.GetBytes(StartMsg.Token));
    }

    private void BattleStart() {
        var bTr = GameObject.Find("Born").transform;
        TSVector[] borns = new TSVector[6];
        for (int i = 0; i < 6; i++)
            borns[i] = bTr.Find((i + 1).ToString()).transform.position.ToTSVector();

        m_Battle = new Battle(30, 2, StartMsg.GameTime, StartMsg.GameDelayTime, 3, Assembly.GetAssembly(GetType()));
        m_Battle.Init(StartMsg, borns, PlayerWeaponDic);
        if (StartMsg.Index == -1) {
            ControlGroup.SetActive(false);
            ViewGroup.SetActive(true);
        } else {
            ControlGroup.SetActive(true);
            ViewGroup.SetActive(false);
        }

        btnWatchNext.onClick.AddListener(m_Battle.WatchNext);

        m_MoveAction = inputActionAsset.FindActionMap("Player").FindAction("Move");
        m_MoveAction.started += MoveAction_started;
        m_MoveAction.performed += MoveAction_performed;
        m_MoveAction.canceled += MoveAction_canceled;

        m_LookAction = inputActionAsset.FindAction("Player/Look");
        m_LookAction.performed += LookAction_performed;

        m_MouseLookAction = inputActionAsset.FindAction("Player/MouseLook");
        m_MouseLookAction.performed += MouseLookAction_performed;

        m_DodgeAction = inputActionAsset.FindAction("Player/Dodge");
        m_DodgeAction.started += DodgeAction_started;
        m_DodgeAction.canceled += DodgeAction_canceled;

        m_FightAction = inputActionAsset.FindAction("Player/Fire");
        m_FightAction.started += FightAction_started;
        m_FightAction.canceled += FightAction_canceled;

        m_MouseFightAction = inputActionAsset.FindAction("Player/MouseFire");
        m_MouseFightAction.started += MouseFightAction_started;
        m_MouseFightAction.canceled += MouseFightAction_canceled;

        m_DefenseAction = inputActionAsset.FindAction("Player/Defense");
        m_DefenseAction.started += DefenseAction_started;

        m_DiscardAction = inputActionAsset.FindAction("Player/Discard");
        m_DiscardAction.started += m_DiscardAction_started;

        m_Skill1Action = inputActionAsset.FindAction("Player/Skill1");
        m_Skill1Action.started += OnSkill1;

        m_Skill2Action = inputActionAsset.FindAction("Player/Skill2");
        m_Skill2Action.started += OnSkill2;

        m_Skill3Action = inputActionAsset.FindAction("Player/Skill3");
        m_Skill3Action.started += OnSkill3;
    }

    public void PushInput<T>(T data) where T : class, IOperateData, new() =>
        m_Battle.OperateDataProcesser.PushInput(data);

    private void DodgeAction_started(InputAction.CallbackContext obj) {
        PushInput(new OperateDodgeStart());
    }

    private void DodgeAction_canceled(InputAction.CallbackContext obj) {
        PushInput(new OperateDodgeEnd());
    }

    private void MoveAction_started(InputAction.CallbackContext obj) {
        PushInput(new OperateMoveStart());
        moveFGGo.SetActive(true);
    }

    private void MoveAction_performed(InputAction.CallbackContext obj) {
        PushInput(new OperateMoveUpdate { toward = GetLookRad() + GetMoveRad() });
        var f = m_MoveAction.ReadValue<Vector2>();
        var a = Mathf.Atan2(f.y, f.x);
        moveFGGo.transform.localEulerAngles = new Vector3(0, 0, a * Mathf.Rad2Deg - 90);
    }

    private void MoveAction_canceled(InputAction.CallbackContext obj) {
        PushInput(new OperateMoveStop());
        moveFGGo.SetActive(false);
    }

    private void LookAction_performed(InputAction.CallbackContext obj) {
        var f = obj.ReadValue<Vector2>();
        if (!m_MouseCameraMode && f.x != 0) {
            var ld = GetLookRad();
            PushInput(new OperateCameraMove { toward = ld });
            if (m_MoveAction.IsPressed()) {
                var md = GetMoveRad();
                PushInput(new OperateMoveUpdate { toward = ld + md });
            }
        }
    }

    private void MouseLookAction_performed(InputAction.CallbackContext obj) {
        var f = obj.ReadValue<Vector2>();
        if (m_MouseCameraMode && f.x != 0) {
            var ld = GetLookRad();
            PushInput(new OperateCameraMove { toward = ld });
            if (m_MoveAction.IsPressed()) {
                var md = GetMoveRad();
                PushInput(new OperateMoveUpdate { toward = ld + md });
            }
        }
    }

    private void FightAction_started(InputAction.CallbackContext obj) {
        if (!m_MouseCameraMode)
            PushInput(new OperateAttackStart());
    }

    private void FightAction_canceled(InputAction.CallbackContext obj) {
        if (!m_MouseCameraMode)
            PushInput(new OperateAttackEnd());
    }

    private void MouseFightAction_started(InputAction.CallbackContext obj) {
        if (m_MouseCameraMode)
            PushInput(new OperateAttackStart());
    }

    private void MouseFightAction_canceled(InputAction.CallbackContext obj) {
        if (m_MouseCameraMode)
            PushInput(new OperateAttackEnd());
    }

    private void DefenseAction_started(InputAction.CallbackContext obj) {
        if (defBtnGo.activeInHierarchy)
            PushInput(new OperateDefense());
    }

    private void m_DiscardAction_started(InputAction.CallbackContext obj) {
        PushInput(new OperateDiscardWeapon());
    }

    private void OnSkill1(InputAction.CallbackContext obj) {
        PushInput(new OperateSelectCard { index = 0 });
    }

    private void OnSkill2(InputAction.CallbackContext obj) {
        PushInput(new OperateSelectCard { index = 1 });
    }

    private void OnSkill3(InputAction.CallbackContext obj) {
        PushInput(new OperateSelectCard { index = 2 });
    }

    private void OnReadData(byte[] data) {
        //if (data.Length > 6)
        //    Debug.Log("Recv:" + string.Join(",", data));
        m_Battle.OperateInput(data);
    }

    private void OnGetData(byte[] data) {
        //if (data.Length > 1)
        //    Debug.Log("Send:" + string.Join(",", data));
        m_Kcp.Send(data);
    }

    private void OnMsgGameRun(GameRunS2C msg) {
        m_GameRun = true;
    }

    private void OnMsgGameEnd(GameEndS2C msg) {
        m_GameRun = false;
        ChangeMouseMode(true);
        score.gameObject.SetActive(true);
        score.Set(m_Battle.Score, StartMsg.Index, OnReturn);
        battleGroup.SetActive(false);
    }

    private void OnPlayerUpdate(PlayerUpdateS2C msg) {
        if (msg.Mode == UpdatePBType.Remove && msg.Player.Seat != -1)
            Offlines.Add(msg.Player.Seat);
    }

    private FP GetMoveRad() {
        var f = m_MoveAction.ReadValue<Vector2>();
        return FP.FromFloat(Mathf.Atan2(f.x, f.y));
    }

    private FP GetLookRad() {
        var rad = m_Battle.PreCameraDir;
        var f = (m_MouseCameraMode ? m_MouseLookAction : m_LookAction).ReadValue<Vector2>();
        var a = f.x / Screen.height * MathF.PI;
        rad += a;
        m_Battle.PreCameraDir = rad;
        return rad;
    }

    //private void OnGUI() {
    //    GUI.Label(new Rect(0, 0, 200, 50), $"F:{m_Battle.Frame},D:{m_Battle.DataFrame},A:{m_Battle.ActFrame}");
    //}
}