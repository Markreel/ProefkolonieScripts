using UnityEngine;

public interface IMiniGame
{
    void StartMiniGame(GameObject _objectThatActivatedTheMinigame = null);
    void WinMiniGame();
    void LoseMiniGame();
}
