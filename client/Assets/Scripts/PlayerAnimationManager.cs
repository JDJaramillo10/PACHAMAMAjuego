using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    private Vector3 lastPosition;

    public void RunAnimation()
    {
        lastPosition.x = transform.position.x;
        lastPosition.y = transform.position.y;
    }
}
