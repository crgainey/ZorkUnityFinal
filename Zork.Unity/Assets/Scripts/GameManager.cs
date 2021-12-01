using Newtonsoft.Json;
using UnityEngine;
using Zork;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CurrentLocationText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    private Game _game;

    void Awake()
    {
        TextAsset gameTextAsset = Resources.Load<TextAsset>("Zork");
        _game = JsonConvert.DeserializeObject<Game>(gameTextAsset.text);
        _game.Player.LocationChanged += (sender, newLocation) => CurrentLocationText.text = newLocation.ToString();
        _game.Player.MovesChanged += (sender, moves) => MovesText.text = moves.ToString();
        _game.Player.ScoreChanged += (sender, score) => ScoreText.text = score.ToString();

        OutputService.WriteLine(string.IsNullOrWhiteSpace(_game.WelcomeMessage) ? "Welcome to Zork!" : _game.WelcomeMessage);
        _game.Start(InputService, OutputService);

        CurrentLocationText.text = _game.Player.Location.ToString();
        Game.Look(_game);

    }

    private void Update()
    {
        if (_game.IsRunning == false)
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

}
