using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _topWallTransform;
    [SerializeField] private Transform _bottomWallTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _enemyTransform;
    private static int SPAWNCHANCE = 5;
    private static int EASYCHANCE = 3;
    private static int MEDIUMCHANCE = SPAWNCHANCE;
    private static int HARDCHANCE = 3;
    private int _enemyChance = MEDIUMCHANCE;
    private BoostItem _playerItem;
    private BoostItem _enemyItem;
    private float _time;
    void Update()
    {

        if (GameManager.instance.CurrentGameState != GameState.InGame)
            return;

        _time += Time.deltaTime;
        if (_time > 1 )
        {
            int playerRandom = Random.Range(0, 100);
            int enemyRandom = Random.Range(0, 100);


            if (playerRandom < SPAWNCHANCE && _playerItem == null)
            {
                Debug.Log(_time);
                SpawnItem(_playerTransform);
            }

            if (enemyRandom < _enemyChance && _enemyItem == null)
            {
                Debug.Log(_time);
                SpawnItem(_enemyTransform);
            }
            
            _time = 0;
        }
    }

    private void SpawnItem(Transform characterTransform)
    {
        float topY = _topWallTransform.position.y;
        float bottomY = _bottomWallTransform.position.y;

        float spawnY = Random.Range(bottomY + 0.5f, topY - 0.5f);
        Vector3 spawnPos = new Vector3(characterTransform.position.x, spawnY, 0);

        BoostItem newItem = Instantiate(_itemPrefab, spawnPos, Quaternion.identity).GetComponent<BoostItem>();

        if (characterTransform == _playerTransform)
            _playerItem = newItem;
        else
            _enemyItem = newItem;

        newItem.Show();
    }

    public void EnemyDifficultyChange(EnemyDifficulty difficulty)
    {
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                _enemyChance = EASYCHANCE;
                break;
            case EnemyDifficulty.Medium:
                _enemyChance = MEDIUMCHANCE;
                break;
            case EnemyDifficulty.Hard:
                _enemyChance = HARDCHANCE;
                break;
        }
    }


}
