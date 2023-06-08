using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static ShopService;

public class PlayerConfigService
{
    private GunModel _gunModel;
    private SkinModel _skinModel;

    public ScheduledNotifier<NFTGameModel> OnModelChanged = new ScheduledNotifier<NFTGameModel>();

    public GunModel GetGun()
    {
        return _gunModel;
    }

    public SkinModel GetSkin()
    {
        return _skinModel;
    }


    public void SetModel(NFTGameModel model)
    {
        switch (model)
        {
            case GunModel gun:
                _gunModel = gun;
                break;
            case SkinModel skin:
                _skinModel = skin;
                break;
            default:
                throw new System.ArgumentException("Invalid model type");
        }
        OnModelChanged.Report(model);
    }

    public bool ModelsComplete()
    {
        return _gunModel != null && _skinModel != null;
    }
}
