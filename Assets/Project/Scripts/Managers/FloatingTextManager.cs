using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviourSingleton<FloatingTextManager> {
    public class FloatingText {
        public bool active;
        public GameObject go;
        public TMP_Text txt;
        public Vector3 motion;
        public float duration;
        public float lastShown;

        //comment
        public void Show() {
            active = true;
            lastShown = Time.time;
            go.SetActive(active);
        }

        public void Hide() {
            active = false;
            go.SetActive(active);
        }

        public void Update() {
            if (!active) return;

            if (Time.time - lastShown > duration) Hide();

            go.transform.position += motion * Time.deltaTime;
        }
    }

    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> _floatingTexts = new List<FloatingText>();

    private void Update() {
        foreach (FloatingText i in _floatingTexts) i.Update();
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        FloatingText _ft = GetFloatingText();

        _ft.txt.text = msg;
        _ft.txt.fontSize = fontSize;
        _ft.txt.color = color;

        if (!(Camera.main is null)) _ft.go.transform.position = Camera.main.WorldToScreenPoint(position);
        _ft.motion = motion;

        _ft.duration = duration;

        _ft.Show();
    }

    private FloatingText GetFloatingText() {
        FloatingText _ft = _floatingTexts.Find(t => !t.active);

        if (_ft == null) {
            _ft = new FloatingText();
            _ft.go = Instantiate(textPrefab, textContainer.transform, true);
            _ft.txt = _ft.go.GetComponent<TMP_Text>();

            _floatingTexts.Add(_ft);
        }

        _ft.go.transform.localScale = Vector3.one;

        return _ft;
    }
}