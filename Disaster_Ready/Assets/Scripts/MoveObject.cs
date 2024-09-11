using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    Rigidbody rb;

    public float timer = 120;

    [SerializeField] float targetY;
    [SerializeField] float moveSpeed;

    bool moved = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (!moved && transform.position.y != targetY)
        {
            float oldPosY = transform.position.y;
            float newPosY = transform.position.y + moveSpeed;

            Debug.Log("oldpos: " + oldPosY);

            rb.MovePosition(new Vector3(transform.position.x, newPosY, transform.position.z));
            Debug.Log("pos2: " + transform.position.y);
            if ((oldPosY > targetY && newPosY <= targetY) || 
                (oldPosY < targetY && newPosY >= targetY))
            {
                Debug.Log("true");
                rb.MovePosition(new Vector3(transform.position.x, targetY, transform.position.z));
                moved = true;
            }
        }
    }
}
