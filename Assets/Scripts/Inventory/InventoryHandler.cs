using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _gadgetPrefab;
    public FirearmBase[] firearms;
    public BagBase bag;
    public float throwForce = 25f;

    public int selected;

    public GameObject bagImage;
    
    [SerializeField]
    private List<CollectibleData> _collectibles;
    
    private void Start()
    {
        _collectibles = new List<CollectibleData>();
        var v = GetComponentsInChildren<FirearmBase>();
        firearms = new FirearmBase[v.Length];
        for (int i = 0; i < v.Length; i++) {
            firearms[i] = v[i];
        }
        UpdateShownWeapon();
        bagImage.SetActive(false);
    }

    public void ShowSelected(bool state) {
        if (firearms.Length == 0) return;
        firearms[selected].spriteRoot.gameObject.SetActive(state);
    }

    public bool IsReloading() {
        if (firearms.Length == 0) return false;
        return firearms[selected].reloading;
    }

    public void UpdateShownWeapon() {
        if (firearms.Length == 0) return;
        for (int i = 0; i < firearms.Length; i++) {
            firearms[i].gameObject.SetActive(false);
            if (i == selected) firearms[i].gameObject.SetActive(true);
        }
    }

    public void Reload()
    {
        firearms[selected].TryReload();
    }
    
    public void UseItem() {
        if (firearms.Length == 0) return;
        if (selected < 2) {
            firearms[selected].OnShoot();
        }
    }

    [SerializeField]
    private int _gadgetCount;
    public void DeployGadget()
    {
        if (_gadgetPrefab == null || _gadgetCount <= 0) return;
        Instantiate(_gadgetPrefab, transform.position, Quaternion.identity);
        _gadgetCount--;
    }

    public int GetWearingWeaponType() {
        if (firearms.Length == 0) return -1;
        return (int)firearms[selected].type;
    }

    public FirearmBase.Type GetWearingWeaponType2() {
        if (firearms.Length == 0) return FirearmBase.Type.Pistol;
        return firearms[selected].type;
    }

    public FirearmBase GetCurrent() {
        if (firearms.Length == 0) return null;
        return firearms[selected];
    }

    public int GetWearingWeaponStability() {
        if (firearms.Length == 0) return 100;
        return (int)firearms[selected].stability;
    }

    public bool AddBag(BagBase b) {
        if(bag == null) {
            bag = b;
            bagImage.SetActive(true);
            return true;
        }
        return false;
    }

    public void ThrowBag(Vector3 dir, float force) {
        if(bag != null) {
            bag.transform.position = transform.position;
            bag.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 180f));
            bag.InInventory = false;
            bagImage.SetActive(false);
            int a = Random.Range(0, 2);

            int i = 0;
            foreach (var r in bag.GetComponentsInChildren<Rigidbody2D>()) {
                r.AddForce(dir.normalized * (force * r.mass * (a == i? 0.8f : 1f)), ForceMode2D.Impulse);
                i++;
            }
            
            bag = null;
        }
    }

    public void AddCollectible(CollectibleData data)
    {
        _collectibles.Add(data);
    }
    
    public bool HasCollectible(CollectibleData data)
    {
        return _collectibles.Contains(data);
    }
    
    public void RemoveCollectible(CollectibleData data)
    {
        _collectibles.Remove(data);
    }
}
