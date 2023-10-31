using MR.Net.Proto.Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestRoomItemSeat : MonoBehaviour {
    public GameObject playerGroup;
    public TMP_Text txtName;
    public GameObject readyGo;
    public GameObject selfGo;

    public Button btnSit;
    public Button btnMove;

    private TestRoom m_Room;
    private int m_Seat;

    private void Start() {
        btnSit.onClick.AddListener(OnSit);
        btnMove.onClick.AddListener(OnMove);
    }

    public void Init(TestRoom room, int seat) {
        m_Room = room;
        m_Seat = seat;
    }

    public void UpdateData(TestRoom.Player player, bool onSeat, bool isSelf) {
        if (player == null) {
            playerGroup.SetActive(false);
            btnSit.gameObject.SetActive(!onSeat);
            btnMove.gameObject.SetActive(onSeat);
        } else {
            playerGroup.SetActive(true);
            btnSit.gameObject.SetActive(false);
            btnMove.gameObject.SetActive(false);
            txtName.text = player.name;
            selfGo.gameObject.SetActive(isSelf);
            readyGo.gameObject.SetActive(player.ready);
        }
    }

    private void OnSit() {
        m_Room.OnSit(m_Seat);
    }

    private void OnMove() {
        m_Room.OnMove(m_Seat);
    }
}
