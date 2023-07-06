using TMPro;
using UnityEngine;
using Catkey.StarSlayer.Managers;
using UnityEngine.UI;
using Catkey.StarSlayer.Server;
using System;

namespace Catkey.StarSlayer.Menu 
{
    public class HomeManager : Catkey.StarSlayer.Utils.Singleton<HomeManager>
    {
        [Header("Spin buttons")]
        [SerializeField] private Button _spaceshipSpinButtonLeft;
        [SerializeField] private Button _spaceshipSpinButtonRight;
        [SerializeField] private Button _spaceshipColorSpinButtonLeft;
        [SerializeField] private Button _spaceshipColorSpinButtonRight;

        [SerializeField] private Color _disabledColor = Color.white;
        [SerializeField] private Color _enabledColor = Color.white;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _spinSpaceshipText;
        [SerializeField] private TextMeshProUGUI _spinSpaceshipColorText;
        [SerializeField] private TextMeshProUGUI _currencyText;

        [Header("Image")]
        [SerializeField] private Image _lockedImage;

        [Header("Spaceship components")]
        [SerializeField] private MeshFilter _mainMenuSpaceshipFilter;
        [SerializeField] private MeshRenderer _mainMenuSpaceshipRenderer;
        [SerializeField] private MeshFilter _spaceshipMeshFilter;
        [SerializeField] private MeshRenderer _spaceshipMeshRenderer;

        private int _selectedIndex = -1;
        private int _colorSelectedIndex = -1;

        private Mesh _selectedMesh;
        private Material _selectedMaterial;

        private bool _isLockedSelectedMesh = false;
        private bool _isLockedSelectedMaterial = false;

        #region Public methods
        public void UpdateCurrency(int value)
        {
            _currencyText.text = value.ToString();
        }

        /// <summary>
        /// This method is called when backend connection is done
        /// </summary>
        /// <param name="playerModel"></param>
        public void UpdateSpinButtons(PlayerModel playerModel) 
        {
            UpdateMeshes(playerModel);
            UpdateMaterials(playerModel);
            UpdateSpinButtons();
        }

        public void OnSpaceshipSpinClick(bool rightButtonClick) 
        {
            // Gather input
            if(rightButtonClick) 
                GatherSpaceshipSpinRightInput();
            else 
                GatherSpaceshipSpinLeftInput();

            // Update backend data
            UpdateSpaceship();

            // Update mesh with given index
            UpdateMeshes(_selectedIndex);

            // Update home UI
            UpdateUI();
        }

        public void OnSpaceshipColorClick(bool rightButtonClick) 
        {
            // Gather input
            if (rightButtonClick)
                GatherColorSpinRightInput();
            else
                GatherColorSpinLeftInput();
            // Update backend data
            UpdateColor();

            // Update material given index
            UpdateMaterial(_colorSelectedIndex);

            // Update home UI
            UpdateUI();
        }

        public void CloseHome() 
        {
            UpdateBackendData();

            UpdateSpaceshipWithSelectedItems();

            CloseUI();
        }

        #endregion

        #region Private methods
        private void UpdateBackendData()
        {
            if (BackendConnection.Instance == null)
                return;

            if (BackendConnection.Instance.IsSpaceshipUnLocked(DataManager.Instance.SpaceshipData[_selectedIndex].id) == true)
                BackendConnection.Instance.UpdateSpaceshipByID(DataManager.Instance.SpaceshipData[_selectedIndex].id);

            if (BackendConnection.Instance.IsColorUnLocked(DataManager.Instance.SpaceshipColorData[_colorSelectedIndex].id) == true)
                BackendConnection.Instance.UpdateColorByID(DataManager.Instance.SpaceshipColorData[_colorSelectedIndex].id);
        }

        private void UpdateMeshes(PlayerModel playerModel) 
        {
            foreach (SpaceshipData data in DataManager.Instance.SpaceshipData)
            {
                _selectedIndex++;

                if (data.id == playerModel.spaceship)
                {
                    _spinSpaceshipText.text = data.id;
                    _spaceshipMeshFilter.mesh = data.mesh;
                    _mainMenuSpaceshipFilter.mesh = data.mesh;
                    _selectedMesh = data.mesh;
                    break;
                }
            }
        }

        private void UpdateMaterials(PlayerModel playerModel) 
        {
            foreach (SpaceshipColorData data in DataManager.Instance.SpaceshipColorData)
            {
                _colorSelectedIndex++;

                if (data.id == playerModel.color)
                {
                    _spinSpaceshipColorText.text = data.id;
                    _spaceshipMeshRenderer.material = data.material;
                    _mainMenuSpaceshipRenderer.material = data.material;
                    _selectedMaterial = data.material;
                    break;
                }
            }
        }
        private void UpdateSpinButtons() 
        {
            if (_selectedIndex == DataManager.Instance.SpaceshipData.Length - 1)
                SetActiveSpaceshipSpinButtonRight(false);
            else if (_selectedIndex == 0)
                SetActiveSpaceshipSpinButtonLeft(false);
            else
            {
                SetActiveSpaceshipSpinButtonLeft(true);
                SetActiveSpaceshipSpinButtonRight(true);
            }

            if (_colorSelectedIndex == DataManager.Instance.SpaceshipColorData.Length - 1)
                SetActiveColorSpinButtonRight(false);
            else if (_colorSelectedIndex == 0)
                SetActiveColorSpinButtonLeft(false);
            else
            {
                SetActiveColorSpinButtonLeft(true);
                SetActiveColorSpinButtonRight(true);
            }
        }
        private void SetActiveSpaceshipSpinButtonLeft(bool activate) 
        {
            _spaceshipSpinButtonLeft.interactable = activate;
            _spaceshipSpinButtonLeft.image.color = activate ? _enabledColor : _disabledColor;
        }

        private void SetActiveSpaceshipSpinButtonRight(bool activate) 
        {
            _spaceshipSpinButtonRight.interactable = activate;
            _spaceshipSpinButtonRight.image.color = activate ? _enabledColor : _disabledColor;
        }

        private void SetActiveColorSpinButtonLeft(bool activate) 
        {
            _spaceshipColorSpinButtonLeft.interactable = activate;
            _spaceshipColorSpinButtonLeft.image.color = activate ? _enabledColor : _disabledColor;
        }

        private void SetActiveColorSpinButtonRight(bool activate)
        {
            _spaceshipColorSpinButtonRight.interactable = activate;
            _spaceshipColorSpinButtonRight.image.color = activate ? _enabledColor : _disabledColor;
        }

        private void GatherSpaceshipSpinRightInput() 
        {
            // If is last index just return
            if (_selectedIndex == DataManager.Instance.SpaceshipData.Length - 1)
                return;

            // Add one to index
            _selectedIndex++;

            // Set spin left interactable
            SetActiveSpaceshipSpinButtonLeft(true);

            // Set spin right interactable
            if (_selectedIndex == DataManager.Instance.SpaceshipData.Length - 1)
                SetActiveSpaceshipSpinButtonRight(false);
            else
                SetActiveSpaceshipSpinButtonRight(true);
        }

        private void GatherColorSpinRightInput() 
        {
            // If is last index just return
            if (_colorSelectedIndex == DataManager.Instance.SpaceshipColorData.Length - 1)
                return;

            // Add one to index
            _colorSelectedIndex++;

            // Set spin left interactabe
            SetActiveColorSpinButtonLeft(true);

            // Update right spin
            if (_colorSelectedIndex == DataManager.Instance.SpaceshipData.Length - 1)
                SetActiveColorSpinButtonRight(false);
            else
                SetActiveColorSpinButtonRight(true);
        }

        private void GatherSpaceshipSpinLeftInput() 
        {
            // If it's last index just return
            if (_selectedIndex == 0)
                return;

            // Substract one to index
            _selectedIndex--;

            // Set spin left interactable
            SetActiveSpaceshipSpinButtonRight(true);

            // Upate left spin
            if (_selectedIndex == 0)
                SetActiveSpaceshipSpinButtonLeft(false);
            else
                SetActiveSpaceshipSpinButtonLeft(true);
        }

        private void GatherColorSpinLeftInput() 
        {
            // If it's last index just return
            if (_colorSelectedIndex == 0)
                return;

            // Substract one to index
            _colorSelectedIndex--;

            // Set spin left interactable
            SetActiveColorSpinButtonRight(true);

            // Update left spin
            if (_colorSelectedIndex == 0)
                SetActiveColorSpinButtonLeft(false);
            else
                SetActiveColorSpinButtonLeft(true);
        }

        private void UpdateMeshes(int index) 
        {
            _spinSpaceshipText.text = DataManager.Instance.SpaceshipData[index].id;
            _spaceshipMeshFilter.mesh = DataManager.Instance.SpaceshipData[index].mesh;
            _mainMenuSpaceshipFilter.mesh = DataManager.Instance.SpaceshipData[index].mesh;
        }
        
        private void UpdateMaterial(int index) 
        {
            _spinSpaceshipColorText.text = DataManager.Instance.SpaceshipColorData[index].id;
            _spaceshipMeshRenderer.material = DataManager.Instance.SpaceshipColorData[index].material;
            _mainMenuSpaceshipRenderer.material = DataManager.Instance.SpaceshipColorData[index].material;
        }

        private void UpdateSpaceshipWithSelectedItems() 
        {
            _spaceshipMeshFilter.mesh = _selectedMesh;
            _mainMenuSpaceshipFilter.mesh = _selectedMesh;
            _spaceshipMeshRenderer.material = _selectedMaterial;
            _mainMenuSpaceshipRenderer.material = _selectedMaterial;
        }

        private void CloseUI() 
        {
            _lockedImage.color = new Color(255, 255, 255, 0);

            _selectedIndex = -1;
            _colorSelectedIndex = -1;

            // Update UI with slected items (This is done because player can select unlocked items to preview)
            foreach (SpaceshipData data in DataManager.Instance.SpaceshipData)
            {
                _selectedIndex++;

                if (data.id == BackendConnection.Instance.GetSpaceship())
                {
                    _spinSpaceshipText.text = data.id;
                    _spinSpaceshipText.color = Color.white;
                    break;
                }
            }

            foreach (SpaceshipColorData data in DataManager.Instance.SpaceshipColorData)
            {
                _colorSelectedIndex++;

                if (data.id == BackendConnection.Instance.GetColor())
                {
                    _spinSpaceshipColorText.text = data.id;
                    _spinSpaceshipColorText.color = Color.white;
                    break;
                }
            }
        }
        private void UpdateUI() 
        {
            if (_isLockedSelectedMaterial || _isLockedSelectedMesh)
                _lockedImage.color = Color.white;
            else
                _lockedImage.color = new Color(255, 255, 255, 0);
        }

        private void UpdateSpaceship() 
        {
            if (BackendConnection.Instance == null) 
            {
                _selectedMesh = DataManager.Instance.SpaceshipData[_selectedIndex].mesh;
                return;
            }
                

            if (BackendConnection.Instance.IsSpaceshipUnLocked(DataManager.Instance.SpaceshipData[_selectedIndex].id) == true)
            {
                _isLockedSelectedMaterial = false;
                _spinSpaceshipText.color = Color.white;

                _selectedMesh = DataManager.Instance.SpaceshipData[_selectedIndex].mesh;
            }
            else
            {
                _isLockedSelectedMaterial = true;
                _spinSpaceshipText.color = Color.red;

            }
        }

        private void UpdateColor() 
        {
            if (BackendConnection.Instance == null)
            {
                _selectedMaterial = DataManager.Instance.SpaceshipColorData[_colorSelectedIndex].material;
                return;
            }

            if (BackendConnection.Instance.IsColorUnLocked(DataManager.Instance.SpaceshipColorData[_colorSelectedIndex].id) == true)
            {
                _spinSpaceshipColorText.color = Color.white;
                _isLockedSelectedMesh = false;

                _selectedMaterial = DataManager.Instance.SpaceshipColorData[_colorSelectedIndex].material;
            }
            else
            {
                _isLockedSelectedMesh = true;
                _spinSpaceshipColorText.color = Color.red;
            }
        }
        #endregion
    }
}

