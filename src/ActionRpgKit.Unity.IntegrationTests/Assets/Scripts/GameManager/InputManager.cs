using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool _pointerIsDown;

    public void OnPointerDown(PointerEventData data)
    {
        _pointerIsDown = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        _pointerIsDown = false;
    }

    void Update()
    {
        if (_pointerIsDown)
        {
            HandleInput();
        }
    }

    /// <summary>
    /// Handle User input to the Character.
    /// Basic implementation handles the Moving and Attacking based on
    /// the User input.</summary>
    public virtual void HandleInput()
    {
        var hit = HitFromInput();

        // Idle State if hit outside of terrain
        if (hit.collider == null)
        {
            return;
        }
        else if (hit.collider.CompareTag("Enemy"))
        {
            HitEnemy(hit);
        }
        else if (hit.collider.CompareTag("Terrain"))
        {
            HitTerrain(hit);
        }
    }

    /// <summary>
    /// Raycast the user input to retrieve a possible hit.</summary>
    private RaycastHit HitFromInput()
    {
#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //for touch device
#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Character",
                                                             "Terrain"));
        return hit;
    }

    /// <summary>
    /// Change to attack or chase State when an enemy was hit with
    /// user input.
    /// Also set the enemy as the first enemy in the list of enemies.</summary>
    private void HitEnemy(RaycastHit hit)
    {
        GameBaseCharacter enemy = hit.collider.gameObject.GetComponent<GameBaseCharacter>();
        GamePlayer.Instance.Character.AddEnemy(enemy.Character, 0);
        GamePlayer.Instance.Character.TargetedEnemy = enemy.Character;
    }

    /// <summary>
    /// Move to the hit destination on the terrain.</summary>
    private void HitTerrain(RaycastHit hit)
    {
        GamePlayer.Instance.NavMeshAgent.SetDestination(hit.point);
        GamePlayer.Instance.Character.ChangeState(GamePlayer.Instance.Character.MoveState);
        GamePlayer.Instance.Character.TargetedEnemy = null;
        GamePlayer.Instance.Character.Enemies.Clear();
    }
}
