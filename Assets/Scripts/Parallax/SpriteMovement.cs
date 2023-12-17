using UnityEngine;

    public class SpriteMovement : MonoBehaviour
    {
        public float speed = 1f;

        private void Update()
        {
            //Save the current position, so we can edit it
            var newPosition = transform.position;
            //Move the position along the x axis by an amount that depends on the
            //defined speed and the deltaTime, so we can get a framerate independent movement
            newPosition.x -= speed * Time.deltaTime;
            //Update position
            transform.position = newPosition;
        }
    }