using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using Catkey.StarSlayer.Menu;
using System.Text;
using System.Collections.Generic;
using Catkey.StarSlayer.Managers;

namespace Catkey.StarSlayer.Server 
{
    public class BackendConnection : Utils.Singleton<BackendConnection>
    {
        [Header("BASE URL")]
        public string BaseURl;

        [Header("GET URL's")]
        public string GetValidUserUrl;
        public string GetDataUrl;
        public string GetCurrencyUrl;
        public string GetUnlockedColorsUrl;
        public string GetUnlockedSpaceshipsUrl;

        [Header("POST/UPDATE URL's")]
        public string PostNewUserUrl;
        public string UpdateCurrencyUrl;
        public string UpdateColorUrl;
        public string UpdateSpaceshipUrl;
        public string UpdateUnlockedColorsUrl;
        public string UpdateUnlockedSpaceshipsUrl;

        [Header("ID")]
        [SerializeField] private string _testID = "unityuser";
        [SerializeField] private PlayerModel _playerData;

        private string _id;

        private void Start()
        {
#if UNITY_EDITOR
            _id = _testID;
#else
            _id = Social.localUser.id;
#endif
        }
        private void InitializePlayerData(string id)
        {
            _playerData = new PlayerModel();
            _playerData.player = id;
            _playerData.color = "default";
            _playerData.spaceship = "default";

            _playerData.colors = new List<ColorsModel>()
            {
                new ColorsModel("default", true),
                new ColorsModel("purple", false),
                new ColorsModel("blue", false),
                new ColorsModel("green", false),
                new ColorsModel("yellow", false),
            };

            _playerData.spaceships = new List<SpaceshipsModel>()
            {
                new SpaceshipsModel("default", true),
                new SpaceshipsModel("AX00", false),
                new SpaceshipsModel("AX01", false),
                new SpaceshipsModel("BX02", false),
                new SpaceshipsModel("BX03", false),
            };

            _playerData.currency = 0;
        }
        public void CheckUser(string id) 
        {
            StartCoroutine(CheckUserCoroutine(id));
        }

        private IEnumerator CheckUserCoroutine(string id)
        {

            using (UnityWebRequest www = UnityWebRequest.Get(BaseURl + GetValidUserUrl + id))
            {
                yield return www.SendWebRequest();

                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    // If user is valid fetch data and update UI
                    if (result == "true")
                        StartCoroutine(FetchDataByID(id));
                    else if (result == "false")
                    {
                        CreateNewUserById(id);
                        SplashSceneManager.Instance.LoadMainMenuScene(1);
                    }
                    else 
                    {
                        //handle the problem
                        InitializePlayerData("unknown");
                        SplashSceneManager.Instance.LoadMainMenuScene(1);
                    }

                    www.Dispose();
                }
                else
                {
                    //handle the problem
                    InitializePlayerData("unknown");
                    SplashSceneManager.Instance.LoadMainMenuScene(1);
                }
            }
        }

        private IEnumerator FetchDataByID(string id)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(BaseURl + GetDataUrl + id))
            {
                yield return www.SendWebRequest();

                if (www.isDone)
                {
                    // handle the result
                    var result = Encoding.UTF8.GetString(www.downloadHandler.data);

                    _playerData = JsonUtility.FromJson<PlayerModel>(result);
                    SplashSceneManager.Instance.LoadMainMenuScene(1);
                }
                else
                {
                    //handle the problem
                    InitializePlayerData("unknown");
                    SplashSceneManager.Instance.LoadMainMenuScene(1);
                }
                www.Dispose();
            }
        }

        public void UpdateMenuData() 
        {
            ShopManager.Instance.UpdateColorsShopCards(_playerData);
            ShopManager.Instance.UpdateSpaceshipShopCards(_playerData);

            HomeManager.Instance.UpdateCurrency(_playerData.currency);
            HomeManager.Instance.UpdateSpinButtons(_playerData);
        }

        public void CreateNewUserById(string id)
        {
            InitializePlayerData(id);

            var jsonData = JsonUtility.ToJson(_playerData);
            StartCoroutine(PostRequest(jsonData, BaseURl + PostNewUserUrl + id));
        }
        public void UnlockColorItem(string id) 
        {
            for (int i = 0; i < _playerData.colors.Count; i++)
            {
                if (string.Equals(_playerData.colors[i].name, id))
                {
                    _playerData.colors[i].unlocked = true;
                }
            }

            var jsonData = JsonUtility.ToJson(_playerData);

            StartCoroutine(PostRequest(jsonData, BaseURl + UpdateUnlockedColorsUrl + _id));
        }

        public void UnlockSpaceshipItem(string id) 
        {
            for (int i = 0; i < _playerData.colors.Count; i++)
            {
                if (string.Equals(_playerData.spaceships[i].name, id))
                {
                    _playerData.spaceships[i].unlocked = true;
                }
            }

            var jsonData = JsonUtility.ToJson(_playerData);
            StartCoroutine(PostRequest(jsonData, BaseURl + UpdateUnlockedSpaceshipsUrl + _id));
        }

        public void UpdateCurrency(int value) 
        {
            _playerData.currency += value;
            if (HomeManager.Instance) 
                HomeManager.Instance.UpdateCurrency(_playerData.currency);

            var jsonData = JsonUtility.ToJson(_playerData);
            StartCoroutine(PostRequest(jsonData, BaseURl + UpdateCurrencyUrl + _id));
        }

        public void UpdateCurrencyGivenValue(int value) 
        {
            _playerData.currency = value;
            if (HomeManager.Instance)
                HomeManager.Instance.UpdateCurrency(_playerData.currency);

            var jsonData = JsonUtility.ToJson(_playerData);
            StartCoroutine(PostRequest(jsonData, BaseURl + UpdateCurrencyUrl + _id));
        }

        public void CheckUserData(string id) 
        {
            StartCoroutine(CheckUserCoroutine(id));
        }

        public bool IsSpaceshipUnLocked(string id) 
        {
            foreach (SpaceshipsModel data in _playerData.spaceships)
                if (data.name == id) return data.unlocked;

            return false;
        }

        public bool IsColorUnLocked(string id)
        {
            foreach (ColorsModel data in _playerData.colors)
                if (data.name == id) return data.unlocked;

            return false;
        }

        public void UpdateColorByID(string colorid)
        {
            _playerData.color = colorid;
            var jsonData = JsonUtility.ToJson(_playerData);
            StartCoroutine(PostRequest(jsonData, BaseURl + UpdateColorUrl + _id));
        }

        public void UpdateSpaceshipByID(string spaceshipid)
        {
            _playerData.spaceship = spaceshipid;
            var jsonData = JsonUtility.ToJson(_playerData);
            StartCoroutine(PostRequest(jsonData, BaseURl + UpdateSpaceshipUrl + _id));
        }

        public IEnumerator GetRequest<T>(string requestURL)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(requestURL);
            yield return webRequest.SendWebRequest();
        }

        public IEnumerator PostRequest(string postData, string requestURL)
        {
            using UnityWebRequest webRequest = new(requestURL);
            webRequest.method = UnityWebRequest.kHttpVerbPOST;
            using UploadHandlerRaw uploadHandler = new(Encoding.ASCII.GetBytes(postData));
            webRequest.uploadHandler = uploadHandler;
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.disposeUploadHandlerOnDispose = true;
            webRequest.disposeDownloadHandlerOnDispose = true;
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();
        }

        public string GetColor() { return _playerData.color; }

        public string GetSpaceship() { return _playerData.spaceship; }

        public int GetCurrency() { return _playerData.currency;}
    }
}

