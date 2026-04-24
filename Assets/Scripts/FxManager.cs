using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public static FxManager Instance { get; private set; }

    [System.Serializable]
    public class FxEntry
    {
        public string id;
        public GameObject prefab;
    }

    [SerializeField] private List<FxEntry> fxEntries = new List<FxEntry>();

    private readonly Dictionary<string, GameObject> fxMap = new Dictionary<string, GameObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInstance()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject go = new GameObject("FxManager");
        go.AddComponent<FxManager>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildFxMap();
    }

    public void PlayFx(string fxId, Vector3 position)
    {
        if (!fxMap.TryGetValue(fxId, out GameObject fxPrefab) || fxPrefab == null)
        {
            // Inspector에 등록이 없으면 Resources 폴더에서 자동 로드
            fxPrefab = Resources.Load<GameObject>(fxId);

            if (fxPrefab == null)
            {
                Debug.LogError($"FxManager: FX '{fxId}' not found.\n" +
                    $"해결 방법:\n" +
                    $"1. FxManager 오브젝트의 Fx Entries에 id='{fxId}', prefab 할당  또는\n" +
                    $"2. Assets/Resources/{fxId}.prefab 위치에 프리팹 배치");
                return;
            }

            fxMap[fxId] = fxPrefab;
        }

        GameObject spawnedFx = Instantiate(fxPrefab, position, Quaternion.identity);
        StartCoroutine(DestroyWhenFinished(spawnedFx));
    }

    private void BuildFxMap()
    {
        fxMap.Clear();

        foreach (FxEntry entry in fxEntries)
        {
            if (string.IsNullOrWhiteSpace(entry.id) || entry.prefab == null)
            {
                continue;
            }

            fxMap[entry.id] = entry.prefab;
        }
    }

    private IEnumerator DestroyWhenFinished(GameObject fxObject)
    {
        if (fxObject == null)
        {
            yield break;
        }

        float waitTime = 1.0f;
        ParticleSystem particle = fxObject.GetComponentInChildren<ParticleSystem>();
        Animator anim = fxObject.GetComponentInChildren<Animator>();

        if (particle != null)
        {
            waitTime = particle.main.duration + particle.main.startLifetime.constantMax;
        }
        else if (anim != null && anim.runtimeAnimatorController != null)
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            if (state.length > 0f)
            {
                waitTime = state.length;
            }
        }

        yield return new WaitForSeconds(waitTime);

        if (fxObject != null)
        {
            Destroy(fxObject);
        }
    }
}
