using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Catkey.StarSlayer.Menu 
{
    public class ShopCard : MonoBehaviour
    {
        [SerializeField] private Image _unlockedImage;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _priceText;
        
        public string ID;
        public bool Unlocked;

        public void UpdateUnlockedImage(Sprite sprite, bool unlocked) 
        {
            if(!unlocked) 
            {
                _unlockedImage.sprite = sprite;
                _unlockedImage.color = Color.white;
            }
            else 
            {
                _unlockedImage.sprite = sprite;
                _unlockedImage.color = new Color(255,255,255,0);
                _button.interactable = false;
            }
        }

        public void UpdatePriceText(int value) 
        {
            _priceText.text = value.ToString();
        }

    }
}

