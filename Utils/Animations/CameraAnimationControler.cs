using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Catkey.StarSlayer.Utils 
{
    public class CameraAnimationControler : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private bool _startOnAwake;

        [Header("Animation components")]
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _zoomOutAnimation;

        [Header("Events")]
        [SerializeField] private UnityEvent _onFinishAnimationEvent;

        #region Unity events
        private void Start()
        {
            if (_startOnAwake)
                StartCoroutine(PlayZoomOutAnimation(true));
        }
        #endregion

        #region Private methods
        private IEnumerator PlayZoomOutAnimation(bool invokeEvent)
        {
            // Play fadeout animation
            _animation.clip = _zoomOutAnimation;
            _animation.Play();
            //Wait to finish animation
            yield return new WaitForSeconds(_zoomOutAnimation.length);
            // Invoke event
            if (invokeEvent)
                InvokeAnimationEvent();
        }
        /// <summary>
        /// Invoke event when animation is finished. It's caleld in every animation end time.
        /// </summary>
        private void InvokeAnimationEvent()
        {
            _onFinishAnimationEvent.Invoke();
        }
        #endregion
    }
}

