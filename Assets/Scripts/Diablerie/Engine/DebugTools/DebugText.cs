﻿using UnityEngine;

namespace Diablerie.Engine.DebugTools
{
    public class DebugText : MonoBehaviour
    {
        public string text = null;

        void OnGUI()
        {
            GUI.color = Color.white;
            var center = Camera.main.WorldToScreenPoint(Camera.main.transform.position);
            var pos = Camera.main.WorldToScreenPoint(transform.position);
            pos.z = 0;
            pos.y = center.y * 2 - pos.y;
            var renderText = (text == null) || text == "" ? gameObject.name : text;
            GUI.Label(new Rect(pos, new Vector2(200, 200)), renderText);
        }
    }
}
