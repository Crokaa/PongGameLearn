using UnityEngine;

public class BoostItem : MonoBehaviour
{
    private static readonly float MAXDURATION = 3.0f;
    private float timer;
    private bool _isShowing = false;
    public bool HasShown { get; private set; }

    void Update()
    {
        if (_isShowing && GameManager.instance.CurrentGameState == GameState.InGame)
        {
            timer += Time.deltaTime;
            if (timer >= MAXDURATION)
            {
                gameObject.SetActive(false);
                _isShowing = false;
            }
        }
    }

    public void Show()
    {
        HasShown = true;
        gameObject.SetActive(true);
        _isShowing = true;
        timer = 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Character>().IsBoosted = true;
            gameObject.SetActive(false);
        }

    }

}
