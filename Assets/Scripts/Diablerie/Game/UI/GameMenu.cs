using System.Collections.Generic;
using Diablerie.Engine;
using Diablerie.Engine.Entities;
using Diablerie.Engine.IO.D2Formats;
using Diablerie.Engine.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Diablerie.Game.UI
{
    public class GameMenu
    {
        private static readonly float StarPadding = 70;
        private static readonly int ItemHeight = 50;
        
        private static GameMenu instance;
        private GraphicRaycaster raycaster;
        private GameObject root;
        private RectTransform layoutGroupTransform;
        private GameObject leftStar;
        private GameObject rightStar;
        private List<MenuItem> items = new List<MenuItem>();
        private int selectedIndex = -1;

        public static void Show()
        {
            Time.timeScale = 0;
            GetInstance().ShowInternal();
        }

        public static void Hide()
        {
            Time.timeScale = 1;
            GetInstance().HideInternal();
        }

        public static bool IsVisible()
        {
            return GetInstance().root.activeSelf;
        }

        private static GameMenu GetInstance()
        {
            if (instance == null)
                instance = new GameMenu();
            return instance;
        }

        private GameMenu()
        {
            root = new GameObject("Game Menu");
            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerID = SortingLayers.UI;
            raycaster = root.AddComponent<GraphicRaycaster>();
            var behaviour = root.AddComponent<InternalBehaviour>();
            behaviour.menu = this;
            var layoutGroupObject = new GameObject("Vertical Layout");
            layoutGroupObject.transform.SetParent(root.transform, false);
            var layoutGroup = layoutGroupObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = 0;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlHeight = true;
            layoutGroupTransform = layoutGroupObject.GetComponent<RectTransform>();
            layoutGroupTransform.anchorMin = new Vector2(0, 0.5f);
            layoutGroupTransform.anchorMax = new Vector2(1, 0.5f);
            layoutGroupTransform.pivot = new Vector2(0.5f, 0.5f);
            layoutGroupTransform.anchoredPosition = new Vector2(0, 0);
            AddMenuItem("OPTIONS", enabled: false);
            AddMenuItem("EXIT GAME", GameManager.QuitGame);
            AddMenuItem("RETURN TO GAME", Hide);
            HideInternal();
            leftStar = CreateStar(true);
            rightStar = CreateStar(false);
            SetSelectedIndex(1);
        }
        
        private void ShowInternal()
        {
            root.SetActive(true);
        }

        private void HideInternal()
        {
            root.SetActive(false);
        }

        private void AddMenuItem(string itemName, UnityAction action = null, bool enabled = true)
        {
            var menuItem = new MenuItem(itemName, enabled);
            menuItem.rectTransform.SetParent(layoutGroupTransform, false);
            menuItem.action = action;
            items.Add(menuItem);
            layoutGroupTransform.sizeDelta = new Vector2(0, ItemHeight * items.Count);
        }

        private GameObject CreateStar(bool left)
        {
            var spritesheet = Spritesheet.Load(@"data\global\ui\CURSOR\pentspin");
            var sprites = spritesheet.GetSprites(0);
            float width = sprites[0].rect.width;
            float height = sprites[0].rect.height;
            var star = new GameObject("star");
            star.transform.localScale = new Vector3(Iso.pixelsPerUnit, Iso.pixelsPerUnit);
            float offset = GetMenuWidth() / 2 + StarPadding;
            star.transform.localPosition = new Vector3((left ? -offset : offset) - width / 2, -height / 2);
            var animator = star.AddComponent<SpriteAnimator>();
            animator.SetSprites(sprites);
            animator.fps = 20;
            animator.reversed = left;
            animator.useUnscaledTime = true;
            animator.OffsetTime(left ? 0.1f : 0);
            var spriteRenderer = star.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerID = SortingLayers.UI;
            return star;
        }

        private void SetSelectedIndex(int selectedIndex)
        {
            if (this.selectedIndex != selectedIndex)
            {
                if (!items[selectedIndex].enabled)
                    return;
                this.selectedIndex = selectedIndex;
                UpdateStarsPositions();
            }
        }
        
        private void UpdateStarsPositions()
        {
            MenuItem selectedItem = items[selectedIndex];
            leftStar.transform.SetParent(selectedItem.gameObject.transform, false);
            rightStar.transform.SetParent(selectedItem.gameObject.transform, false);
        }

        private float GetMenuWidth()
        {
            string longestItem = "";
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].name.Length > longestItem.Length)
                {
                    longestItem = items[i].name;
                }
            }

            var settings = new TextGenerationSettings
            {
                textAnchor = TextAnchor.MiddleCenter,
                generationExtents = new Vector2(1000.0F, 1000.0F),
                pivot = Vector2.zero,
                font = Fonts.GetFont42()
            };
            TextGenerator generator = new TextGenerator();
            return generator.GetPreferredWidth(longestItem, settings);
        }

        private class InternalBehaviour : MonoBehaviour
        {
            public GameMenu menu;
            private List<RaycastResult> raycastResults = new List<RaycastResult>();
            private PointerEventData pointerEventData;

            void Start()
            {
                pointerEventData = new PointerEventData(EventSystem.current);
            }
            
            void Update()
            {
                UpdateSelectedItem();
                if (Input.GetKeyDown(KeyCode.Escape))
                    Hide();
            }

            private void UpdateSelectedItem()
            {
                raycastResults.Clear();
                pointerEventData.position = Input.mousePosition;
                menu.raycaster.Raycast(pointerEventData, raycastResults);
                if (raycastResults.Count != 0)
                {
                    int itemIndex = GetItemIndex(raycastResults[0].gameObject);
                    if (itemIndex != -1)
                    {
                        menu.SetSelectedIndex(itemIndex);
                    }
                }
            }

            private int GetItemIndex(GameObject gameObject)
            {
                for (int i = 0; i < menu.items.Count; ++i)
                {
                    if (menu.items[i].gameObject == gameObject)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        private class MenuItem
        {
            public GameObject gameObject;
            public RectTransform rectTransform;
            public UnityAction action;
            public bool enabled;
            public string name;
            
            public MenuItem(string name, bool enabled)
            {
                gameObject = new GameObject(name);
                var text = gameObject.AddComponent<Text>();
                rectTransform = gameObject.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 1);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchoredPosition = new Vector2(0, 0);
                text.alignment = TextAnchor.LowerCenter;
                text.color = enabled ? Color.white : Color.grey;
                text.font = Fonts.GetFont42();
                text.text = name;
                this.enabled = enabled;
                this.name = name;
                
                if (enabled)
                {
                    var button = gameObject.AddComponent<Button>();
                    button.transition = Selectable.Transition.None;
                    button.onClick.AddListener(OnClick);
                }
            }

            private void OnClick()
            {
                AudioManager.instance.Play("cursor_select");
                if (action != null)
                    action();
            }
        }
    }
}
