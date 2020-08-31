using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public class CamChangeEvent : UnityEvent<StateManager.cameraStates> { }
    public CamChangeEvent camChangeEvent;

    [System.Serializable]
    public class MovementStageChangeEvent : UnityEvent<StateManager.movementStates> { }
    public MovementStageChangeEvent movementStateChangeEvent;

    [System.Serializable]
    public class GameStateChangeEvent : UnityEvent<StateManager.gameStates> { }
    public GameStateChangeEvent gameStateChangeEvent;

    [System.Serializable]
    public class PlayerAttemptShootEvent : UnityEvent<W_Gun, PlayerAttemptShootEvent.result, GameObject>
	{
        public enum result
		{
            SUCCESSFUL, FAILED_NOAMMO
		}
	}
    public PlayerAttemptShootEvent playerAttemptShootEvent;

    #region Singleton

    public static EventManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
}
