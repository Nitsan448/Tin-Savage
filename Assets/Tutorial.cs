using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Tutorial : ASingleton<Tutorial>
{
    [SerializeField] private GameObject _overlay;
    [SerializeField] private GameObject _movementText;
    [SerializeField] private GameObject _keyText;
    [SerializeField] private GameObject _dashText;
    [SerializeField] private int _moveDistanceToFinishMovementTutorial = 10;
    [SerializeField] private GameObject _tutorialKey;

    public async UniTask PlayTutorial()
    {
        _overlay.SetActive(true);
        _movementText.SetActive(true);
        _keyText.SetActive(false);
        _dashText.SetActive(false);

        Vector3 playerStartingPosition = SceneReferencer.Instance.Player.transform.position;
        await UniTask.WaitUntil(() =>
            Vector3.Distance(playerStartingPosition, SceneReferencer.Instance.Player.transform.position) >
            _moveDistanceToFinishMovementTutorial);

        _movementText.SetActive(false);
        _keyText.SetActive(true);
        //Spawn key
        _tutorialKey.SetActive(true);
        Vector3 tutorialKeyStartingPosition = _tutorialKey.transform.position;
        _tutorialKey.transform.position = tutorialKeyStartingPosition + Vector3.up * 10;
        DOTween.To(() => _tutorialKey.transform.position, value => _tutorialKey.transform.position = value, tutorialKeyStartingPosition,
            0.25f);
        await UniTask.WaitUntil(() => SceneReferencer.Instance.Player.HoldingKey);

        _keyText.SetActive(false);
        _dashText.SetActive(true);
        await SceneReferencer.Instance.WaveManager.PlayTutorialWave();

        _overlay.SetActive(false);
        GameConfiguration.Instance.PlayTutorial = false;
    }
}