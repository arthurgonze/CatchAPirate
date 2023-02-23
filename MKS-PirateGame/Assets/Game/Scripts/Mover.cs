using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float rotation_rate = 50.0f;

    [SerializeField] RectTransform ship_canvas_rect;

    Rigidbody2D ship_rigidbody;
    Vector3 target_position;

    private void Start() {
        ship_rigidbody = GetComponent<Rigidbody2D>();
    }

    public bool MovementAndRotation(float move_forward, float rotation)
    {
        bool moved = false;

        if (move_forward > 0)
        {
            ship_rigidbody.MovePosition(ship_rigidbody.position - (Vector2)this.transform.up * speed * Time.deltaTime);
            moved = true;
        }
        if(rotation != 0){
            float rotation_dir = -Mathf.Sign(rotation);
            this.transform.Rotate(0, 0, rotation_rate*rotation_dir*Time.deltaTime);
            ship_canvas_rect.Rotate(0, 0, -rotation_rate*rotation_dir*Time.deltaTime);
            moved = true;
        }
        return moved;
    }

    public bool MoveToLocation(Vector3 location)
    {
        bool moved = false;
        float distance = Vector3.Distance(this.transform.position, location);
        if(distance > 0.1f){
            if(!RotateToTarget(location)) 
                ship_rigidbody.MovePosition(ship_rigidbody.position - (Vector2)this.transform.up * speed * Time.deltaTime);
            
            moved = true;
        }
        return moved;
    }

    public bool RotateToTarget(Vector3 target){
        float theta = AngleBetweenBoatAndTarget(target);
            
        if(Mathf.Abs(theta) >= 0.1f){
            RotateBoat(Mathf.Sign(theta));
            return true;
        }
        return false;
    }
    private float AngleBetweenBoatAndTarget(Vector3 target){
        Vector3 target_direction = target - this.transform.position;
        return (-this.transform.up.x) * target_direction.y - target_direction.x * (-this.transform.up.y);
    }

    private void RotateBoat(float dir){
        this.transform.Rotate(0, 0, rotation_rate*Time.deltaTime*dir);
        ship_canvas_rect.Rotate(0, 0, -rotation_rate*Time.deltaTime*dir);
    }

    public bool CanFrontalShoot(Vector3 target){
        Vector3 targetDirection = target - this.transform.position;
        float theta = (-this.transform.up.x) * targetDirection.y - targetDirection.x * (-this.transform.up.y);// cross product
        return Mathf.Abs(theta) <= 0.1f;
    }

    public bool CanLateralShoot(Vector3 target, ref float dir){
        Vector3 targetDirection = target - this.transform.position;
        float theta = (-this.transform.up.x)*targetDirection.x + (-this.transform.up.y)*targetDirection.y; // dot product
        dir = -Mathf.Sign((-this.transform.up.x) * targetDirection.y - targetDirection.x * (-this.transform.up.y));
        return Mathf.Abs(theta) <= 0.1f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target_position, 0.1f);
    }
}


