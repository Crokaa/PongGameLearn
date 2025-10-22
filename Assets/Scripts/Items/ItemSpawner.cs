using UnityEngine;

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
    private static int HARDCHANCE = 10;
    private int _enemyChance = MEDIUMCHANCE;
    private BoostItem _playerItem;
    private BoostItem _enemyItem;
    private float _time;
    private float _paddleHeight;
    void Start()
    {
        // Player or enemy have the same paddle height
        _paddleHeight = FindAnyObjectByType<Character>().transform.localScale.y;
    }
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
                SpawnItem(_playerTransform);
            }

            if (enemyRandom < _enemyChance && _enemyItem == null)
            {
                SpawnItem(_enemyTransform);
            }
            
            _time = 0;
        }
    }

    private void SpawnItem(Transform characterTransform)
    {
        float topY = _topWallTransform.position.y - 0.5f;
        float bottomY = _bottomWallTransform.position.y + 0.5f;
        float characterY = characterTransform.position.y;
        float characterX = characterTransform.position.x;
        float spawnY;
        // if anything it goes right in the center
        Vector3 spawnPos = new Vector3(characterX, (topY + bottomY) / 2);

        if (characterY - _paddleHeight / 2 - 0.5f < bottomY)
        {
            spawnY = Random.Range(characterY + _paddleHeight / 2 + 0.5f , topY);
            spawnPos = new Vector3(characterX, spawnY);
        }
        else if (characterY + _paddleHeight / 2 + 0.5f > topY)
        {
            spawnY = Random.Range(bottomY, characterY - _paddleHeight / 2 - 0.5f);
            spawnPos = new Vector3(characterX, spawnY);
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                spawnY = Random.Range(bottomY, characterY - _paddleHeight / 2 - 0.5f);
            else
                spawnY = Random.Range(characterY - _paddleHeight / 2 + 0.5f, topY);
            spawnPos = new Vector3(characterX, spawnY);
        }
        BoostItem newItem = Instantiate(_itemPrefab, spawnPos, Quaternion.identity).GetComponent<BoostItem>();

        if (characterTransform == _playerTransform)
            _playerItem = newItem;
        else
            _enemyItem = newItem;

        newItem.Show();
    }

    public void ResetItems()
    {
        if (_playerItem != null)
            Destroy(_playerItem.gameObject);
        if (_enemyItem != null)
            Destroy(_enemyItem.gameObject);
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
