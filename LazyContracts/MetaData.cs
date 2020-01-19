using OrbCreationExtensions;
using UnityEngine;
using UnityEngine.UI;

namespace LazyContracts
{
    public class MetaData : ModMeta
    {/*
        public static bool Enabled => _toggle.enabled;
        
        private static Toggle _toggle;*/
        private static Text _text;
        public override void ConstructOptionsScreen(RectTransform parent, bool inGame)
        {
            _text = WindowManager.SpawnLabel();
            _text.text = "Created with love by MarioGK.";
            /*_toggle = WindowManager.SpawnCheckbox();
            _toggle.name = "Enabled";
            WindowManager.AddElementToElement(_toggle.gameObject, parent.gameObject, new Rect(0f, 0f, 380f, 128f),
                new Rect(0f, 0f, 0f, 0f));*/
            WindowManager.AddElementToElement(_text.gameObject, parent.gameObject, new Rect(0f, 24f, 120f, 42f),
                new Rect(0f, 0f, 0f, 0f));

        }

        public override string Name => "LazyContracts";
    }
}