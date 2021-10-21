using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Beamable.AccountManagement;
using Beamable.Server.Clients;
using TMPro;

public class RoomManagementUI : MonoBehaviour
{
    public Button createRoomButton;
    public Button shareRoomButton;
    public Button joinRoomPublicButton;
    public Button joinRoomPrivateButton;
    public Button joinWithPasswordButton;

    public Toggle privateRoomToggle;
    public TMP_InputField roomNameField;
    public TMP_InputField passwordField;

    public GameObject joinWithPasswordPanel;
    public GameObject createRoomPanel;
    public GameObject roomListPanel;
    public GameObject roomListEntry;
    public Transform roomListRoot;

    private AccountManagementSignals _accountManagement;
    private ArnaMicroServiceClient _arnaClient;
    private Dictionary<string, GameObject> _roomEntries;

    private long userId;

    private void Start()
    {
        _roomEntries = new Dictionary<string, GameObject>();
        _accountManagement = FindObjectOfType<AccountManagementSignals>();
        _accountManagement.UserAvailable.AddListener((user) =>
        {
            userId = user.id;
        });

        createRoomButton.onClick.AddListener(() => createRoomPanel.SetActive(true));
        shareRoomButton.onClick.AddListener(ShareRoom);
        joinRoomPublicButton.onClick.AddListener(ShowRoomList);
        joinRoomPrivateButton.onClick.AddListener(() => joinWithPasswordPanel.SetActive(true));
        joinWithPasswordButton.onClick.AddListener(() => JoinPrivateRoom(passwordField.text));

        SetupBeamable();
    }

    private async void SetupBeamable()
    {
        var beamableAPI = await Beamable.API.Instance;

        Debug.Log($"beamableAPI.User.id = {beamableAPI.User.id}");

        _arnaClient = new ArnaMicroServiceClient();
        await _arnaClient.StartSession();
        Debug.Log("Room management ready.");
    }

    private async void ShareRoom()
    {
        string passwordId = await _arnaClient.CreateRoom(userId, roomNameField.text, privateRoomToggle.isOn);
        Debug.Log($"Created new room, passwordId: " + passwordId);
        createRoomPanel.SetActive(false);
    }

    private async void ShowRoomList()
    {
        var rooms = await _arnaClient.GetRooms();
        var keys = _roomEntries.Keys.ToArray();
        foreach(var e in keys)
        {
            Destroy(_roomEntries[e]);
            if (!rooms.Contains(e))
                _roomEntries.Remove(e);
        }
        foreach(var e in rooms)
        {
            if(!_roomEntries.ContainsKey(e))
            {
                var entry = Instantiate(roomListEntry, roomListRoot);
                entry.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    JoinPublicRoom(e);
                });
                _roomEntries.Add(e, entry);
            }
        }
        roomListPanel.SetActive(true);
    }

    private async void JoinPublicRoom(string id)
    {
        bool success = await _arnaClient.JoinRoomPublic(id);
        Debug.Log($"Joined room {id} success: {success}");
        roomListPanel.SetActive(false);
    }

    private async void JoinPrivateRoom(string pw)
    {
        string roomId = await _arnaClient.JoinRoom(pw);
        joinWithPasswordPanel.SetActive(false);
        Debug.Log($"Joined private room id: {roomId}");
    }
}
