using UnityEngine;
using Catkey.StarSlayer.Utils;
using TMPro;
using Catkey.StarSlayer.Menu;
using UnityEngine.UI;

namespace Catkey.StarSlayer.Managers
{
    public class UIManager : Utils.Singleton<UIManager>
    {
        [Header("UI Parents")]
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private GameObject _inGameUIParent;
        [SerializeField] private GameObject _gameOverUIParent;
        [SerializeField] private FadeAnimationController _fadeAnimationController;

        [Header("In game UI elements")]
        [SerializeField] private TextMeshProUGUI _highScoreText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private PopUpController _popUpController;

        [Header("Game over UI elements")]
        [SerializeField] private TextMeshProUGUI _scoreGameOverText;
        [SerializeField] private TextMeshProUGUI _crownGameOverText;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _watchAdButton;
        [SerializeField] private Button _leaderboardButton;

        [Header("Animations")]
        [SerializeField] private Animation _scoreTextAnimation;
        [SerializeField] private Animation _currencyTextAnimation;
        [SerializeField] private AnimationClip _addScoreAnimationClip;
        public FadeAnimationController GetAnimationControler() 
        {
            return _fadeAnimationController;
        }
        public void DeactivateGameOverButtons() 
        {
            _playButton.interactable = false;
            _exitButton.interactable = false;
            _watchAdButton.interactable = false;
            _leaderboardButton.interactable = false;
        }

        public void ActivateGameOverButtons() 
        {
            _playButton.interactable = true;
            _exitButton.interactable = true;
            _watchAdButton.interactable = true;
        }
        public void OpenPopUp(string title,string message) 
        {
            _popUpController.InitPopUp(title, message);
        }

        public void UpdateCrownGameOverTexT(int value) 
        {
            _crownGameOverText.text = value.ToString();
        }
        public void ClosePopUp() 
        {
            _popUpController.ClosePopUp();
        }
        public void PlayLifeTextAnimation() 
        {
            _currencyTextAnimation.clip = _addScoreAnimationClip;
            _currencyTextAnimation.Play();
        }

        public void PlayScoreTextAnimation() 
        {
            _scoreTextAnimation.clip = _addScoreAnimationClip;
            _scoreTextAnimation.Play();
        }

        public void UpdateGameOverElements(int score)
        {
            _scoreGameOverText.text =  score.ToString();
        }

        public void UpdateHighScoreText(int score)
        {
            _highScoreText.text = "best " + score.ToString();
        }

        public void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void UpdateCurrencyText(int currency)
        {
            _currencyText.text = currency.ToString();
        }

        public void SetActiveInGameUIParent(bool active)
        {
            _inGameUIParent.SetActive(active);
        }

        public void SetActiveGameOverUIParent(bool active)
        {
            _gameOverUIParent.SetActive(active);
        }
    }
}

