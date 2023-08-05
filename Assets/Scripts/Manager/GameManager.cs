using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float playerRespawnTime;
    [SerializeField] private Transform playerRespawnPoint;
    [SerializeField] private GameObject playerRef;

    private float _playerRespawnTimeStart;
    private bool _respawn;

    private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        _virtualCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        CheckRespawn();
    }

    public void Respawn()
    {
        _playerRespawnTimeStart = Time.time;
        _respawn = true;
    }

    private void CheckRespawn()
    {
        if(Time.time >= _playerRespawnTimeStart + playerRespawnTime && _respawn)
        {
            var player = Instantiate(playerRef, playerRespawnPoint);
            _virtualCamera.m_Follow = player.transform;
            _respawn = false;
        }
    }
}