using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Ship), typeof(PointGiver))]
public class AIController : MonoBehaviour, IController 
{
    enum AttackBehavior {
        Chaser,
        Shooter,
        None
    }
    [SerializeField] AttackBehavior AIBehavior = AttackBehavior.None;
    [SerializeField] Sprite[] chaser_sprites;

    private Mover mover;
    private Ship ship;
    private Health health;
    bool controller_disabled = false;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();
        ship = GetComponent<Ship>();
        health = GetComponent<Health>();

        if(AIBehavior != AttackBehavior.None) return;
        int rand = Random.Range(1, 100);
        if(rand % 2 == 0){
            AIBehavior = AttackBehavior.Chaser;
            health.SetBoatSprites(chaser_sprites);
        }
        else{
            AIBehavior = AttackBehavior.Shooter;
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if(controller_disabled) return;
    }

    public void DisableController()
    {
        controller_disabled = true;
    }

    public void EnableController()
    {
        controller_disabled = false;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag.Equals("Player") && !health.IsDead()){
            if(AIBehavior == AttackBehavior.Chaser){
                mover.MoveToLocation(other.gameObject.transform.position);
            }

            if(AIBehavior == AttackBehavior.Shooter){
                if(mover.CanFrontalShoot(other.transform.position)){
                    ship.FrontShot();
                    return;
                }

                float dir = 0;
                if(mover.CanLateralShoot(other.transform.position, ref dir)){
                    ship.LateralShot(dir);
                    return;
                }

                mover.RotateToTarget(other.transform.position);
            }
        }
    }
}
