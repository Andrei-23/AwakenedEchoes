using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    public GameObject bulletFolder;
    [SerializeField] private List<GameObject> _bulletTypes;

    [HideInInspector] public GameObject lastBullet {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject GetType(int type){
        return _bulletTypes[type];
    }

    public void UpdateLastBullet(GameObject bullet)
    {
        if(bullet.GetComponent<Bullet>() == null) { return; }
        lastBullet = bullet;
    }

    public void DestroyAllBullets()
    {
        foreach (Transform child in bulletFolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void DestroyAllPlayerBullets()
    {
        foreach (Transform child in bulletFolder.transform)
        {
            Bullet b = child.GetComponent<Bullet>();
            if (b == null) continue;
            if(b.GetTeamType() == 0)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
