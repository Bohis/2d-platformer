using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedWalk = 10;

    public float jumpForce = 10;
    private bool isGrounded = true;

    private Vector2 speed;
    private Vector2 acceleration;
    public float timeSmooth = 0.12f;

    private Rigidbody2D rb;

    [Space]
    [Header("Защита от прохождение сквозь стены")]
    public bool isEnable = true;
    public float maxRay = 0.7f;
    public float minRay = 0.1f;
    public float radius = 0.5f;
    public LayerMask layerForCheckLevel;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
        Controller.controller.Inputs.Main.Jump.performed += _ => Jump();
	}

	private void Move() {
        float side = Controller.controller.Inputs.Main.Move.ReadValue<float>(); // -1 0 1

        speed = Vector2.SmoothDamp(speed, new Vector2(side, 0) * speedWalk, ref acceleration, timeSmooth);

        MoveRay();

        transform.Translate(speed * Time.fixedDeltaTime);
    }

    float dis;
	private void OnDrawGizmos() {
        Gizmos.DrawSphere((Vector2)transform.position + speed.normalized * dis, radius);
	}

	private void MoveRay() {
        if (!isEnable) return;

        float disRay = speed.magnitude;
        disRay = Mathf.Clamp(disRay, minRay, maxRay);
        Debug.DrawRay(transform.position,speed.normalized * disRay,Color.black);
        dis = disRay;
        if (Physics2D.CircleCast(transform.position, radius,speed, disRay, layerForCheckLevel)) {
            speed = Vector2.zero;
            acceleration = Vector2.zero;
        }
    }


    private void Jump() {
        if (isGrounded) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

	private void OnTriggerEnter2D(Collider2D collision) {
        isGrounded = true;
	}
	private void OnTriggerExit2D(Collider2D collision) {
        isGrounded = false;
	}
	private void OnTriggerStay2D(Collider2D collision) {
        isGrounded = true;
	}

	private void FixedUpdate() {
        Move();
	}
}
