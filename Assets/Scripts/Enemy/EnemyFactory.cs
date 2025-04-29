using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance {  get; private set; }
    [SerializeField] private Transform _folder;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject SpawnEnemy(GameObject enemyPrefab, Vector2 pos)
    {
        return Instantiate(enemyPrefab, pos, Quaternion.identity, _folder);
    }
    public void KillAllEnemies()
    {
        foreach (Transform child in _folder)
        {
            Destroy(child.gameObject);
        }
    }
}
