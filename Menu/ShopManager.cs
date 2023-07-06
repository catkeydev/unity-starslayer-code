using Catkey.StarSlayer.Managers;
using Catkey.StarSlayer.Server;
using System.Collections;
using UnityEngine;

namespace Catkey.StarSlayer.Menu 
{
    public class ShopManager : Utils.Singleton<ShopManager>
    {
        [SerializeField] ShopCard[] _spaceShipShopCards;
        [SerializeField] ShopCard[] _colorsShopCards;
        [SerializeField] Sprite _unlockedSprite;

        [SerializeField] PopUpController _popUpController;

        [Header("Audio")]
        [SerializeField] AudioSource _audioSource;
        [SerializeField] AudioClip _buyAudioClip;
        public void UpdateSpaceshipShopCards(PlayerModel model) 
        {
            foreach (var data in model.spaceships)
            {
                for (int i = 0; i < _spaceShipShopCards.Length; i++) 
                {
                    if (string.Equals(_spaceShipShopCards[i].ID, data.name))
                    {
                        _spaceShipShopCards[i].Unlocked = data.unlocked;
                        _spaceShipShopCards[i].UpdateUnlockedImage(_unlockedSprite, data.unlocked);
                    }
                }
            }
        }

        public void UpdateColorsShopCards(PlayerModel model)
        {
            foreach (var data in model.colors)
            {
                for (int i = 0; i < _colorsShopCards.Length; i++)
                {
                    if (string.Equals(_colorsShopCards[i].ID, data.name))
                    {
                        _colorsShopCards[i].Unlocked = data.unlocked;
                        _colorsShopCards[i].UpdateUnlockedImage(_unlockedSprite, data.unlocked);
                    }
                }
            }
        }

        public void PurchaseSpaceshipItem(string id) 
        {
            int price = 0;

            for (int i = 0; i < DataManager.Instance.SpaceshipData.Length; i++)
            {
                if (string.Equals(DataManager.Instance.SpaceshipData[i].id, id))
                {
                    price = DataManager.Instance.SpaceshipData[i].price;
                    break;
                }
            }

            if (BackendConnection.Instance.GetCurrency() >= price)
            {
                for (int i = 0; i < _spaceShipShopCards.Length; i++)
                {
                    if (string.Equals(_spaceShipShopCards[i].ID, id))
                    {
                        _spaceShipShopCards[i].Unlocked = true;
                        _spaceShipShopCards[i].UpdateUnlockedImage(_unlockedSprite, true);
                    }
                }

                _popUpController.InitPopUp("gz!", "you unlocked a new skin!\r\ngo home and take a look at your new item");
                PlayBuyAudioClip();
                BackendConnection.Instance.UpdateCurrency(-price);
                BackendConnection.Instance.UnlockSpaceshipItem(id);
            }
        }

        public void PurchaseWithMoneyColorItem(string id) 
        {

            for (int i = 0; i < _colorsShopCards.Length; i++)
            {
                if (string.Equals(_colorsShopCards[i].ID, id))
                {
                    _colorsShopCards[i].Unlocked = true;
                    _colorsShopCards[i].UpdateUnlockedImage(_unlockedSprite, true);
                }
            }

            _popUpController.InitPopUp("gz!", "you unlocked a new skin!\r\ngo home and take a look at your new item");

            PlayBuyAudioClip();
            BackendConnection.Instance.UnlockColorItem(id);
        }
       
        public void FailedToPorchaseWithMoneyColorItem() 
        {
            _popUpController.InitPopUp("Oh...","sorry.... something went wrong.");
        }

        public void PurchaseWithMoneySpaceship(string id) 
        {
            for (int i = 0; i < _spaceShipShopCards.Length; i++)
            {
                if (string.Equals(_spaceShipShopCards[i].ID, id))
                {
                    _spaceShipShopCards[i].Unlocked = true;
                    _spaceShipShopCards[i].UpdateUnlockedImage(_unlockedSprite, true);
                }
            }

            _popUpController.InitPopUp("gz!", "you unlocked a new skin!\r\ngo home and take a look at your new item");

            PlayBuyAudioClip();
            BackendConnection.Instance.UnlockSpaceshipItem(id);
        }
        public void FailedToPorchaseWithMoneySpaceshipItem()
        {
            _popUpController.InitPopUp("Oh...", "sorry.... something went wrong.");
        }

        public void PurchaseColorItem(string id) 
        {
            int price = 0;

            for (int i = 0; i < DataManager.Instance.SpaceshipColorData.Length; i++)
            {
                if (string.Equals(DataManager.Instance.SpaceshipColorData[i].id, id))
                {
                    price = DataManager.Instance.SpaceshipColorData[i].price;
                    break;
                }
            }

            if (BackendConnection.Instance.GetCurrency() >= price)
            {
                for (int i = 0; i < _colorsShopCards.Length; i++)
                {
                    if (string.Equals(_colorsShopCards[i].ID, id))
                    {
                        _colorsShopCards[i].Unlocked = true;
                        _colorsShopCards[i].UpdateUnlockedImage(_unlockedSprite, true);
                    }
                }

                _popUpController.InitPopUp("gz!", "you unlocked a new skin!\r\ngo home and take a look at your new item");
                PlayBuyAudioClip();
                BackendConnection.Instance.UpdateCurrency(-price);
                BackendConnection.Instance.UnlockColorItem(id);
            }
        }

        private void PlayBuyAudioClip() 
        {
            StartCoroutine(PlayBuyClip());
        }

        private IEnumerator PlayBuyClip() 
        {
            AudioClip clip = _audioSource.clip;
            _audioSource.clip = _buyAudioClip;
            _audioSource.Play();
            yield return new WaitForSeconds(_buyAudioClip.length);
            _audioSource.clip = clip;
        }
    }
}

