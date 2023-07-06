using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Catkey.StarSlayer.Utils 
{
    internal enum TFade { FadeOut, FadeIn, All }
    public class FadeAnimationController : MonoBehaviour
    {
        public float FadeInDuration {  get; private set; }
        public float FadeOutDuration { get; private set; }

        [Header("Properties")]
        [SerializeField] private TFade _fadeType;
        [SerializeField] private bool _startOnAwake;

        [Header("Animation components")]
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _fadeInAnimation;
        [SerializeField] private AnimationClip _fadeOutAnimation;

        [Header("Events")]
        [SerializeField] private UnityEvent _onFinishFadeInEvent;
        [SerializeField] private UnityEvent _onFinishFadeEvent;

        #region Unity events
        private void Awake()
        {
            FadeInDuration = _fadeInAnimation.length;
            FadeOutDuration = _fadeOutAnimation.length;
        }

        private void Start()
        {
            if (_startOnAwake)
                PlayDefaultFadeAnimation();
        }
        #endregion

        #region Public methods
        public void PlayDefaultFadeAnimation() 
        {
            switch (_fadeType)
            {
                case TFade.FadeOut:
                    StartCoroutine(PlayFadeOut(true));
                    break;
                case TFade.FadeIn:
                    StartCoroutine(PlayFadeIn(true));
                    break;
                case TFade.All:
                    StartCoroutine(PlayFadeOutAndFadeIn());
                    break;
            }
        }

        public IEnumerator PlayFadeOut(bool invokeEvent) 
        {
            _animation.clip = _fadeOutAnimation;
            _animation.Play();
            yield return new WaitForSeconds(_fadeOutAnimation.length);

            if (invokeEvent)
                InvokeFinishFadeEvent();
        }

        public void PlayFadeOut() 
        {
            Debug.Log("Play fade out");
            _animation.clip = _fadeOutAnimation;
            _animation.Play();
        }

        public IEnumerator PlayFadeIn(bool invokeEvent) 
        {
            _animation.clip = _fadeInAnimation;
            _animation.Play();
            yield return new WaitForSeconds(_fadeInAnimation.length);
            if (invokeEvent)
                InvokeFadeInEvent();
        }

        public IEnumerator PlayFadeOutAndFadeIn() 
        {
            StartCoroutine(PlayFadeIn(false));
            yield return new WaitForSeconds(_fadeOutAnimation.length);
            StartCoroutine(PlayFadeOut(false));
            yield return new WaitForSeconds(_fadeOutAnimation.length);
        }

        private void InvokeFadeInEvent()
        {
            _onFinishFadeInEvent.Invoke();
        }
        private void InvokeFinishFadeEvent() 
        {
            _onFinishFadeEvent.Invoke();
        }
        #endregion
    }
}

