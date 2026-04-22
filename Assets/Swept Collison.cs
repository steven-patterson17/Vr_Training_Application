using UnityEngine;

namespace VRTraining
{
    public class PaddleSweptCollision : MonoBehaviour
    {
        private Vector3 lastPos;
    
        void Start()
        {
            lastPos = transform.position;
        }
    
        void FixedUpdate()
        {
            Vector3 frameMovement = transform.position - lastPos;
            float distance = frameMovement.magnitude;
    
            Vector3 paddleVelocity = frameMovement / Time.fixedDeltaTime;
    
            if (distance > 0f)
            {
                if (Physics.Raycast(lastPos, frameMovement.normalized, out RaycastHit hit, distance))
                {
                    BallHitHandler ball = hit.collider.GetComponent<BallHitHandler>();
                    if (ball != null)
                    {
                        Vector3 paddleNormal = transform.forward;
                        ball.OnHitByPaddle(hit.point, paddleVelocity, paddleNormal);
                    }
                }
            }
    
            lastPos = transform.position;
        }
    }
}
