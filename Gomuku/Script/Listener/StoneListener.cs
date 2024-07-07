using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace Gomuku
{
    public class StoneListener : ButtonListener
    {
        [SerializeField]
        private Sprite _Stone;

        public Sprite Stone => _Stone;
    }

    public class StonePool : MemoryPool<int, StoneListener> 
    {
        public StonePool() 
        {
            DespawnRoot = new GameObject(typeof(StonePool).Name).transform;
        }

        [Inject]
        public RectTransform Content     { get; }
        public Transform     DespawnRoot { get; }

        protected override void Reinitialize(int id, StoneListener stone)
        {
            stone.Id = id;

            stone.transform.SetParent(Content);
        }

        protected override void OnDespawned(StoneListener stone)
        {
            stone.transform.SetParent(DespawnRoot);
        }
    }

    public static class StoneListenerExtensions 
    {
        public static void Click(this StoneListener self, EStoneType stoneType)
        {
            var image = self.Listener.image;
            var color = stoneType == EStoneType.Black ? Color.black : Color.white;
            
            image.SetSprite(self.Stone, color);

            self.Listener.interactable = false;
        }

        public static void PointerEnter(this StoneListener self, EStoneType stoneType) 
        {
            if (self.Listener.interactable)
            {
                var image = self.Listener.image;
                var color = stoneType == EStoneType.Black ? new Color(0, 0, 0, 0.5f) : new Color(1, 1, 1, 0.5f);

                image.SetSprite(self.Stone, color);
            }
        }

        public static void PointerExit(this StoneListener self)
        {
            if (self.Listener.interactable) 
            {
                var image = self.Listener.image;

                image.SetSprite(null, new Color(0, 0, 0, 0));
            }
        }

        private static void SetSprite(this Image self, Sprite sprite, Color color) 
        {
            self.sprite = sprite;
            self.color  = color;
        }
    }
}
