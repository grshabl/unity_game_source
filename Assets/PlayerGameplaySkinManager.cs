using UnityEngine;
using Zenject;


[DefaultExecutionOrder(-100)]
public class PlayerGameplaySkinManager : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth _playerHealth;

    [SerializeField]
    private PlayerShooting _playerWeapon;

    [SerializeField]
    private GameObject _gunObject, _playerObject;
    private GameplayService _service;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(GameplayService service, SignalBus bus)
    {
        Debug.Log("Construct PlayerGameplaySkinManager");
        _service = service;
        _signalBus = bus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<OnEqipmentChanged>(ApplyItems);
        ApplyItems();
    }


    private void ApplyItems()
    {
        Debug.Log("Applying skins");
        _service.ApplyGun(_gunObject, _playerWeapon);
        _service.ApplySkin(_playerObject, _playerHealth);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<OnEqipmentChanged>(ApplyItems);
    }
}
