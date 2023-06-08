using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using WebP;

[System.Serializable]
public class ResponseModel
{
    public List<RawModel> ok;


    [System.Serializable]
    public class RawModel
    {
        public int id;
        public string name;
        public string description;
        public string cid;
        public string @params;
        public List<RawParamModel> parsedParams;

        public RawModel() => FUCKING_BICYCLE();

        public async void FUCKING_BICYCLE()
        {
            await UniTask.Yield();

            if (@params != null)
                parsedParams = JsonConvert.DeserializeObject<List<RawParamModel>>(@params);
        }


        public string GetParamValue(string paramName)
        {
            return parsedParams.Find(x => x.parameter == paramName).value;
        }

        [System.Serializable]
        public class RawParamModel
        {
            public string id;
            public string parameter;
            public string unit;
            public string value;
        }
    }
}


public class NFTGameModel
{
    public string Name;
    public string Description;
    public string Rarity;
    public Texture2D Texture;


    public string VisualId;
    public int Id;

    public NFTGameModel() { }

    public NFTGameModel(ResponseModel.RawModel model)
    {
        Name = model.name;
        Description = model.description;

        Rarity = model.GetParamValue("rare");
        VisualId = model.GetParamValue("visualId");
        Id = model.id;


    }


    public async UniTask LoadImage(string url)
    {
        //load texture
        try
        {

            var request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Failed downloading .webp image Error: {request.error} from {url}");
                return;
            }

            Debug.Log($"Downloaded {request.downloadHandler.data.Length} webp bytes from cid {url}");
            var imageBytes = request.downloadHandler.data;

            Texture2D texture = Texture2DExt.CreateTexture2DFromWebP(imageBytes, lMipmaps: true, lLinear: true, lError: out Error lError);

            if (lError == Error.Success)
                Texture = texture;
            else
                Debug.LogError("Webp Load Error : " + lError.ToString());
        }
        catch (System.Exception ex)
        {
            Texture = null;
            Debug.LogException(ex);
        }

    }

    public virtual string GetGameDescription()
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(GetRarityColor());
        return $"Rarity: <color=#{hexColor}>{Rarity}</color>\n{Description}";
    }

    protected Color GetRarityColor()
    {
        return Rarity switch
        {
            "common" => new Color32(119, 255, 255, 255),
            "rare" => new Color32(255, 0, 130, 255),
            "legendary" => new Color32(255, 116, 0, 255),
            _ => Color.white
        };
    }
}
public class GunModel : NFTGameModel
{
    public int BulletDamage;
    public int StartBullets;

    public GunModel() { }
    public GunModel(ResponseModel.RawModel model) : base(model)
    {
        BulletDamage = int.Parse(model.GetParamValue("damage"));
        StartBullets = int.Parse(model.GetParamValue("bullets"));
    }

    public override string GetGameDescription()
    {
        return base.GetGameDescription() + $"\nBullet Damage: <color=#FF0082>{BulletDamage}</color>\nStart Bullets: <color=#FF0082>{StartBullets}</color>";
    }
}
public class SkinModel : NFTGameModel
{
    public int HpBonus;
    public float Resistance;

    public SkinModel() { }
    public SkinModel(ResponseModel.RawModel model) : base(model)
    {
        HpBonus = int.Parse(model.GetParamValue("hp"));
        Resistance = float.Parse(model.GetParamValue("resistance"), CultureInfo.InvariantCulture);
    }

    public override string GetGameDescription()
    {
        return base.GetGameDescription() + $"\nHP Bonus: <color=#FF0082>{HpBonus}</color>\nDamage Resistance: <color=#FF0082>{Mathf.Round(Resistance * 100)}%</color>";
    }
}


