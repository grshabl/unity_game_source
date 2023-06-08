using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField]
    private GameplayService _gameplayService;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AuthService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ShopService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerConfigService>().AsSingle().NonLazy();


        Container.BindInterfacesAndSelfTo<GameplayService>().FromComponentInNewPrefab(_gameplayService).AsSingle().NonLazy();
        Debug.Log("Installed Gameplay Service");


        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<GameStartSignal>();
        Container.DeclareSignal<GameEndSignal>();
        Container.DeclareSignal<OnEqipmentChanged>();
    }
}
