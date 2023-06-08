using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using IInitializable = Zenject.IInitializable;

public class ShopService : IInitializable
{



    public ScheduledNotifier<IReadOnlyList<SkinModel>> OnSkinsLoaded = new ScheduledNotifier<IReadOnlyList<SkinModel>>();
    public ScheduledNotifier<IReadOnlyList<GunModel>> OnGunsLoaded = new ScheduledNotifier<IReadOnlyList<GunModel>>();
    public IReadOnlyList<SkinModel> LoadedSkins => _skinModels;
    public IReadOnlyList<GunModel> LoadedGuns => _gunModels;



    private List<SkinModel> _skinModels = new List<SkinModel>();
    private List<GunModel> _gunModels = new List<GunModel>();
    [Inject]
    private AuthService _authService;
    

    public void Initialize()
    {
        //load raw models
        GetRemoteModels().Forget();
    }

    private async UniTask GetRemoteModels()
    {

        string url = $"https://api.getcrystal.org/api/game/getNftsByUserId?id={_authService.UserId}&contract={_authService.UserContract}";

        var request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            return;
        }

        string response = request.downloadHandler.text;
        Debug.Log(response);
        //parse response body
        ResponseModel rawModels = JsonConvert.DeserializeObject<ResponseModel>(response);



        //wait 2 frames, because of FUCKING_BICYCLE method in RawModel class
        for (int i = 0; i < 2; i++)
            await UniTask.Yield();


        //convert raw models to game models
        _skinModels.Clear();
        _gunModels.Clear();

        foreach (var model in rawModels.ok)
        {
            string modelType = model.parsedParams.Find(x => x.parameter == "type").value;

            NFTGameModel gameModel = null;

            if (modelType == "weapon")
            {
                gameModel = new GunModel(model);
                _gunModels.Add(gameModel as GunModel);
            }
            else if (modelType == "skin")
            {
                gameModel = new SkinModel(model);
                _skinModels.Add(gameModel as SkinModel);
            }

            if (gameModel != null)
                await gameModel.LoadImage(model.cid);
        }

        OnGunsLoaded.Report(_gunModels);
        OnSkinsLoaded.Report(_skinModels);
    }

}
