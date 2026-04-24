using UnityEngine;
using UnityEngine.UI;


public class GameMain : MonoBehaviour
{
    public Button attackButton;
    public Samurai samurai;
    public string slashFxId = "fx_samurai_slash";
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (attackButton != null)
        {
            attackButton.onClick.AddListener(() =>
            {
                samurai.Attack();
            });
        }
        else
        {
            Debug.LogError("GameMain: attackButton is not assigned in the Inspector.");
        }

        if (samurai == null)
        {
            samurai = FindObjectOfType<Samurai>();
        }

        if (samurai == null)
        {
            Debug.LogError("GameMain: samurai is not assigned and no Samurai object was found in the scene.");
            return;
        }

        samurai.OnAttackStarted += HandleSamuraiAttackStarted;

        samurai.MoveToCenter();
    }

    private void OnDestroy()
    {
        if (samurai != null)
        {
            samurai.OnAttackStarted -= HandleSamuraiAttackStarted;
        }
    }

    private void HandleSamuraiAttackStarted(Vector3 position)
    {
        if (FxManager.Instance == null)
        {
            Debug.LogError("GameMain: FxManager instance not found in scene.");
            return;
        }

        FxManager.Instance.PlayFx(slashFxId, position);
    }

    
}
