using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static ShopService;

public class GameplayService : MonoBehaviour
{
    [SerializeField]
    private List<VisualInfo> _assets = new List<VisualInfo>();





    private PlayerConfigService _playerService;


    [Inject]
    private void Construct(PlayerConfigService playerService)
    {
        _playerService = playerService;
    }

    public void ApplySkin(GameObject gameObject, PlayerHealth health = null)
    {
        SkinModel skin = _playerService.GetSkin();

        if (skin == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        GetAssetInfo(skin.VisualId)?.ApplyToObject(ref gameObject);

        if (health == null)
            return;

        health.startingHealth = health.currentHealth = 100 + skin.HpBonus;
        health.damageResistance = skin.Resistance;
    }

    public void ApplyGun(GameObject gameObject, PlayerShooting weapon = null)
    {
        GunModel gun = _playerService.GetGun();

        if (gun == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        GetAssetInfo(gun.VisualId)?.ApplyToObject(ref gameObject);

        if (weapon == null)
            return;

        weapon.numberOfBullets = gun.StartBullets;
        weapon.damagePerShot = gun.BulletDamage;
    }




    private VisualInfo GetAssetInfo(string visualId)
    {
        var info = _assets.Find(x => x.Id == visualId);

        if (info == null)
            Debug.LogError($"Can't find visual info with ID {visualId}");

        return info;
    }


    [Serializable]
    public class VisualInfo
    {
        public string Id;
        public Material Material;
        public Mesh Mesh;

        public void ApplyToObject(ref GameObject gameObject)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            MeshFilter filter = gameObject.GetComponent<MeshFilter>();


            if (skinnedMeshRenderer != null)
            {
                if (Material != null)
                    skinnedMeshRenderer.material = Material;
                if (Mesh != null)
                    skinnedMeshRenderer.sharedMesh = Mesh;
            }

            if (filter != null)
                if (Mesh != null)
                    filter.mesh = Mesh;

            if (meshRenderer != null)
                if (Material != null)
                    meshRenderer.material = Material;
        }
    }





}
