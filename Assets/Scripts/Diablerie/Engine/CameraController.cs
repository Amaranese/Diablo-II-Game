﻿using Diablerie.Engine.World;
using UnityEngine;

namespace Diablerie.Engine
{
    public class CameraController : MonoBehaviour
    {
        public float sizeChangeSpeed = 3f;
        public float horizontalShift = 0;

        new Camera camera;

        void Start()
        {
            camera = GetComponent<Camera>();
            camera.orthographicSize = CalcDesiredSize();
        }

        void LateUpdate()
        {
            UpdateSize();

            if (WorldState.instance == null || WorldState.instance.Player == null)
                return;

            transform.position = CalcTargetPos();
        }

        void UpdateSize()
        {
            float desiredSize = CalcDesiredSize();
            float diff = desiredSize - camera.orthographicSize;
            float speed = sizeChangeSpeed * Time.deltaTime;
            speed = Mathf.Min(speed, Mathf.Abs(diff)) * Mathf.Sign(diff);
            camera.orthographicSize += speed;
        }

        float CalcDesiredSize()
        {
            return camera.pixelHeight / Iso.pixelsPerUnit / 2;
        }

        Vector3 CalcTargetPos()
        {
            Vector3 targetPos = WorldState.instance.Player.transform.position;
            targetPos.z = transform.position.z;

            targetPos.x += camera.orthographicSize * camera.pixelWidth / camera.pixelHeight * horizontalShift;

            return targetPos;
        }
    }
}