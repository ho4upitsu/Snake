using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] public Transform segmentPrefab;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI bestScoreText;
    [SerializeField] public TextMeshProUGUI diedText;
    
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;
    private int _score;
    private int _bestScore;
    private bool _paused;
    private bool _firstStart;

    
    private void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
        bestScoreText.text = "";
        _bestScore = 0;
        _paused = true;
        _firstStart = true;
    }
    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.A) && _direction != Vector2.right)
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.S) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }

        _score = _segments.Count - 1;
        scoreText.text = _score.ToString();
        diedText.text = "";
        if (_paused)
        {
            Time.timeScale = 0f;
            if (_firstStart)
            {
                diedText.text = "Press Space\nTo start";
            }
            else
            {
                diedText.text = "Press Space\nTo restart";
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetGame();
                Time.timeScale = 1;
                _paused = !_paused;
                diedText.text = "";
                _direction = Vector2.right;
                _firstStart = !_firstStart;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        var position = this.transform.position;
        position = new Vector2(
            Mathf.Round(position.x) + _direction.x,
            Mathf.Round(position.y) + _direction.y
        );
        this.transform.position = position;
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Food"))
        {
            Grow();
            Time.timeScale += 0.03f;
        }
        else if (col.CompareTag("Obstacle"))
        {
            _paused = !_paused;
        }
    }

    private void ResetGame()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        
        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = new Vector2(0, 0);
        if (_bestScore < _score)
        {
            _bestScore = _score;
            bestScoreText.text = "Best Score - " + _bestScore.ToString();
        }
    }
}
