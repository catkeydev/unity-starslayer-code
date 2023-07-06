using Catkey.StarSlayer.Utils;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Catkey.StarSlayer.Menu 
{
    public class PopUpController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private BasicAnimationController _animationController;

        public void InitPopUp(string title, string text) 
        {
            _titleText.text = title;
            _text.text = text;
            _animationController.PlayAnimation();
            StartCoroutine(ExitPopUp());
        }

        public void ClosePopUp() 
        {
            _animationController.PlayExitAnimation();
        }

        private IEnumerator ExitPopUp() 
        {
            yield return new WaitForSeconds(2.5f);
            ClosePopUp();
        }
    }
}

