using System.Collections;
using TMPro;
using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class FloatingText : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _animationClip;

        [Header("Settings")]
        [SerializeField] private float _moveUpForce = 0.5f;

        #region Public methods
        /// <summary>
        /// Method that initializes floating text ingame. Must be called when instantianting floating text
        /// </summary>
        /// <param name="value">Text to be shown</param>
        /// <param name="position">Starting position</param>
        public void InitializeText(string text, Vector3 position)
        {
            _rectTransform.position = position;
            _text.text = text;
            _animation.clip = _animationClip;
            _animation.Play();

            StartCoroutine(MoveUp());
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Move floating text. Once animation is finished destroy game object.
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveUp()
        {
            float elapsedTime = 0.0f;

            while (elapsedTime < _animationClip.length)
            {
                elapsedTime += Time.deltaTime;
                _rectTransform.localPosition += Vector3.up * _moveUpForce;
                yield return null;
            }

            Destroy(gameObject);
        }
        #endregion
    }
}

