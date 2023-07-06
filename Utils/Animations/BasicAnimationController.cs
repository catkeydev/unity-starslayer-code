using UnityEngine;

namespace Catkey.StarSlayer.Utils 
{
    public class BasicAnimationController : MonoBehaviour
    {
        [Header("Animation components")]
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _animationClip;
        [SerializeField] private AnimationClip _exitAnimationClip;

        [Header("Settings")]
        [SerializeField] private bool _playOnStart;

        private void OnEnable()
        {
            if(_playOnStart) 
            {
                PlayAnimation();
            }
        }
        public void PlayAnimation() 
        {
            _animation.clip = _animationClip;
            _animation.Play();
        }

        public void PlayExitAnimation() 
        {
            _animation.clip = _exitAnimationClip;
            _animation.Play();
        }
    } 

}
