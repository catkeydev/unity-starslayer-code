using UnityEngine;
using Catkey.StarSlayer.Utils;
using Catkey.StarSlayer.Managers;
using UnityEngine.SceneManagement;
using Catkey.StarSlayer.Server;
using System.Collections;
using UnityEngine.Advertisements;
using Catkey.StarSlayer.GPGS;
using StarsSlayer;

namespace Catkey.StarSlayer.Core
{
    public class GameManager : Catkey.StarSlayer.Utils.Singleton<GameManager>, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        // Public for external hooks
        public int Lifes { private set; get; }
        public int Score { private set; get; }
        public int Crowns { private set; get; }
        public int HighScore { private set; get; }
        public GameState Gamestate { private set; get; }
        public float SpeedBoost { private set; get; }
        public int LifeBoost { private set; get; }
        public int CrownProbability { private set; get; }
        public int BigProbability { private set; get; }
        public int UndestructibleProbability { private set; get; }
        public PlayerController PlayerController { private set; get; }

        [Header("Game settings")]
        [SerializeField] private int _frameRate = 60;
        [SerializeField] private int _startingDefaultLifes = 3;
        [SerializeField] private float _increaseDifficultyInTime = 60;
        [SerializeField] private float _dificultySpeedBoostModifier = 0.2f;
        [SerializeField] private int _dificultyLifeBoostModifier = 1;
        [SerializeField] private int _maxLifeBoostModifier = 4;
        [SerializeField] private float _spawnCooldownReducerModifier = 0.4f;
        [SerializeField] private int _crownProbability = 5;
        [SerializeField] private int _undestructibleProbability = 10;
        [SerializeField] private int _bigProbability = 10;
        [SerializeField] private int _earnCoinsWatchAd = 20;
        [SerializeField] private int _difficultyTimeModifier = 10;
        [Header("Scenes")]
        [SerializeField] private int _menuSceneIndex = 1;

        [Header("References")]
        [SerializeField] private PlayerController _playerController;

        private int _difficulty;
        private float _difficultyIncreaseTime;
        private float _elapsedTime;
        private bool _hasPressedPlayAgain = false;
        private bool _watchAd = false;

        #region Unity events

        private void Start()
        {
            Application.targetFrameRate = _frameRate;

            InitializeInterestitial();
            _hasPressedPlayAgain = false;
            _watchAd = false;

            UpdateSpaceshipMesh();
            UpdateSpaceshipMaterial();

            Gamestate = GameState.Playing;

            PlayerController = _playerController;
            CrownProbability = _crownProbability;
            UndestructibleProbability = _undestructibleProbability;
            BigProbability = _bigProbability;

            SpeedBoost = 0;
            LifeBoost = 0;
            _elapsedTime = 0;
            _difficulty = 0;

            _difficultyIncreaseTime = Time.time + _increaseDifficultyInTime;

            Lifes = _startingDefaultLifes;

            //This line of code is to avoid testing issues when playing only game scene
            if (BackendConnection.Instance) 
            {
                Crowns = BackendConnection.Instance.GetCurrency();
                UIManager.Instance.UpdateCurrencyText(Crowns);
            }

            if (PlayerPrefs.HasKey(PlayerPrefKeys.HighScoreKey))
            {
                HighScore = PlayerPrefs.GetInt(PlayerPrefKeys.HighScoreKey);
                UIManager.Instance.UpdateHighScoreText(HighScore);
            }
        }

        private void Update()
        {
            IncreaseDifficulty();
            UpdateElapsedTime();
        }
        #endregion

        #region Public methods
        public void UpdateElapsedTime()
        {
            _elapsedTime += Time.deltaTime;
        }

        /// <summary>
        /// Substract lifes with given value and check if lifes is equal to 0 then set game state as game over.
        /// </summary>
        /// <param name="value"></param>
        public void SubstractPlayerLife()
        {
            _playerController.PlayDieAniamtion();
            ChangeGameState(GameState.GameOver);
        }

        /// <summary>
        /// IncreaseDifficulty method add speedboostmodifier to speedbost and update UI
        /// </summary>
        public void IncreaseDifficulty()
        {
            if (_difficultyIncreaseTime < Time.time)
            {
                SpawnManager.Instance.ReduceSpawnDelay(_spawnCooldownReducerModifier);
                _increaseDifficultyInTime += _difficultyTimeModifier;
                _increaseDifficultyInTime = Mathf.Clamp(_increaseDifficultyInTime, 0, 20);
                _difficultyIncreaseTime = Time.time + _increaseDifficultyInTime;
                _difficulty++;

                SpeedBoost += _dificultySpeedBoostModifier;

                LifeBoost += _dificultyLifeBoostModifier;
                LifeBoost = Mathf.Clamp(LifeBoost, 0, _maxLifeBoostModifier);
            }
        }
        public int GetDifficulty() { return _difficulty; }
        /// <summary>
        /// Add value to the score and update UI
        /// </summary>
        /// <param name="value"></param>
        public void AddScore(int value)
        {
            Score += value;

            UIManager.Instance.UpdateScoreText(Score);
            UIManager.Instance.PlayScoreTextAnimation();
        }

        public void AddCrown(int value) 
        {
            Crowns += value;

            UIManager.Instance.UpdateCurrencyText(Crowns);
            UIManager.Instance.PlayLifeTextAnimation();
        }

        public void ShowLeaderboard() 
        {
            Social.ShowLeaderboardUI();
        }

        /// <summary>
        /// This method is called by pause button. Uses ChangeGameState method declared below.
        /// </summary>
        public void PauseGame()
        {
            ChangeGameState(GameState.Paused);
        }

        /// <summary>
        /// This method is called by replay button when the game is paused. Uses ChangeGameState method declared below.
        /// </summary>
        public void ResumeGame()
        {
            ChangeGameState(GameState.Playing);
        }

        /// <summary>
        /// Load game scene. This is called when user press play again button
        /// </summary>
        public void RestartGame()
        {
            _hasPressedPlayAgain = true;

            // Check if it's first time playing
            if (PlayerPrefs.HasKey(PlayerPrefKeys.AdTime))
            {
                // Add number of current games
                int numberOfGames = PlayerPrefs.GetInt(PlayerPrefKeys.AdTime);
                numberOfGames++;
                PlayerPrefs.SetInt(PlayerPrefKeys.AdTime, numberOfGames);

                if (numberOfGames >= 3)
                    StartAdvertisment();
                else 
                    StartCoroutine(ReloadScene());
            }
            else
                PlayerPrefs.SetInt(PlayerPrefKeys.AdTime, 0);

        }

        /// <summary>
        /// ChangeGameState method deals with all the logic needed when the state of the game is changed
        /// </summary>
        /// <param name="newState">New state to be set</param>
        public void ChangeGameState(GameState newState)
        {
            switch (Gamestate)
            {
                case GameState.Playing:
                    UIManager.Instance.SetActiveInGameUIParent(false);
                    break;
                case GameState.GameOver:
                    UIManager.Instance.SetActiveGameOverUIParent(false);
                    break;
            }

            switch (newState)
            {
                case GameState.Playing:
                    UIManager.Instance.SetActiveInGameUIParent(true);
                    break;
                case GameState.GameOver:

                    UpdateHighScore();
                    UpdateLeaderBoard();

                    UIManager.Instance.UpdateCrownGameOverTexT(Crowns);
                    UIManager.Instance.UpdateGameOverElements(Score);
                    UIManager.Instance.SetActiveGameOverUIParent(true);
                    //This line of code is to avoid testing erros when playign in game scene
                    if(BackendConnection.Instance)
                        BackendConnection.Instance.UpdateCurrencyGivenValue(Crowns);

                    break;
            }

            Gamestate = newState;
        }

        /// <summary>
        /// Method called when game is finish and user press exit button.
        /// </summary>
        public void ReturnToMenu()
        {
            _hasPressedPlayAgain = false;

            if (PlayerPrefs.HasKey(PlayerPrefKeys.AdTime))
            {
                int numberOfGames = PlayerPrefs.GetInt(PlayerPrefKeys.AdTime);
                numberOfGames++;

                PlayerPrefs.SetInt(PlayerPrefKeys.AdTime, numberOfGames);

                if (numberOfGames >= 3)
                    StartAdvertisment();
                else 
                    StartCoroutine(LoadMenuScene());
            }
            else
                PlayerPrefs.SetInt(PlayerPrefKeys.AdTime, 0);
        }

 
        public void PlayEarnCrownAd() 
        {
            _watchAd = true;
            LoadAd();

            if (!BackendConnection.Instance)
                return;

            BackendConnection.Instance.UpdateCurrency(_earnCoinsWatchAd);
        }


        #endregion

        #region Private methods
        private void UpdateLeaderBoard()
        {
            Social.ReportScore(Score, GPGSIds.leaderboard_score_leaderboard, (bool success) =>
            {
                if (success)
                    Debug.Log("[GameManager] Leaderboard posted." + Score);
                else
                    Debug.Log("[GameManager] Leaderboard field to post.");
            });
        }

        private IEnumerator LoadMenuScene()
        {
            UIManager.Instance.DeactivateGameOverButtons();
            UIManager.Instance.GetAnimationControler().PlayFadeOut();
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(_menuSceneIndex);
        }

        private IEnumerator ReloadScene()
        {
            UIManager.Instance.DeactivateGameOverButtons();
            UIManager.Instance.GetAnimationControler().PlayFadeOut();
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void StartAdvertisment()
        {
            UIManager.Instance.DeactivateGameOverButtons();
            StartCoroutine(PlayAd());
            PlayerPrefs.SetInt(PlayerPrefKeys.AdTime, 0);
        }

        private IEnumerator PlayAd()
        {
            UIManager.Instance.OpenPopUp("Hey", "time for our sponsors...");
            yield return new WaitForSeconds(3.5f);
            LoadAd();
        }

        /// <summary>
        /// Updates highscore in playerprefs. Only if current socre is high than Highscore
        /// </summary>
        private void UpdateHighScore()
        {
            if (HighScore < Score)
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.HighScoreKey, Score);
                UIManager.Instance.UpdateHighScoreText(Score);
            }
        }

        private void UpdateSpaceshipMesh() 
        {
            // This line of code is to avoid errors when testing in game scene
            if (BackendConnection.Instance == null)
                return;

            for (int i = 0; i < DataManager.Instance.SpaceshipData.Length; i++)
            {
                if (DataManager.Instance.SpaceshipData[i].id == BackendConnection.Instance.GetSpaceship())
                {
                    _playerController.GetMeshFilter().mesh = DataManager.Instance.SpaceshipData[i].mesh;
                }
            }
        }

        private void UpdateSpaceshipMaterial() 
        {
            // This line of code is to avoid errors when testing in game scene
            if (BackendConnection.Instance == null)
                return;

            for (int i = 0; i < DataManager.Instance.SpaceshipColorData.Length; i++)
            {
                if (DataManager.Instance.SpaceshipColorData[i].id == BackendConnection.Instance.GetColor())
                {
                    _playerController.GetMeshRenderer().material = DataManager.Instance.SpaceshipColorData[i].material;
                }
            }
        }
        #endregion

        #region Advertisment

        [SerializeField] string _androidAdUnitId = "Interstitial_Android";
        [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
        string _adUnitId;

        void InitializeInterestitial()
        {
            // Get the Ad Unit ID for the current platform:
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // Show the loaded content in the Ad Unit:
        public void ShowAd()
        {
            // Note that if the ad content wasn't previously loaded, this method will fail
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }

        // Implement Load Listener and Show Listener interface methods: 
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            // Optionally execute code if the Ad Unit successfully loads content.
            Debug.Log("Ad loaded");
            ShowAd();
        }

        public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }

        public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        }

        public void OnUnityAdsShowStart(string _adUnitId) { }
        public void OnUnityAdsShowClick(string _adUnitId) { }
        public void OnUnityAdsShowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (_watchAd) 
            {
                _watchAd = false;
                UIManager.Instance.ActivateGameOverButtons();
                UIManager.Instance.UpdateCrownGameOverTexT(Crowns + 20);
                return;
            }
               
            if (!_hasPressedPlayAgain) 
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(_menuSceneIndex);
            }
            else 
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        #endregion
    }
}

