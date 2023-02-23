using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Ship))]
public class PlayerController : MonoBehaviour, IController 
{
    private Mover mover;
    private Ship ship;

    bool controller_disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        ship = GetComponent<Ship>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(controller_disabled) return;
        if(InteractWithAttack()) return;
        if(InteractWithMovement()) return;
    }

    private bool InteractWithAttack()
    {
        if (Input.GetAxis("FrontShot") != 0)
        {
            return ship.FrontShot();
        }

        if (Input.GetAxis("LateralShot") != 0)
        {
            return ship.LateralShot(Mathf.Sign(Input.GetAxis("LateralShot")));
        }
        return false;
    }

    private bool InteractWithMovement()
    {
        float move_forward = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal");
        return mover.MovementAndRotation(move_forward, rotation);
    }

    public void DisableController()
    {
        controller_disabled = true;
    }

    public void EnableController()
    {
        controller_disabled = false;
    }
}
