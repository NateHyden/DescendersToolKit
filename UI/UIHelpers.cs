using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class UIHelpers
    {
        // ── Solid Carbon palette ────────────────────────────────────────────

        public static readonly Color WinOuter    = new Color(0.102f, 0.102f, 0.118f, 0.97f);
        public static readonly Color WinPanel    = new Color(0.133f, 0.133f, 0.149f, 1f);
        public static readonly Color WinBorder   = new Color(0.200f, 0.200f, 0.220f, 1f);
        public static readonly Color HeaderBg    = new Color(0.149f, 0.149f, 0.169f, 1f);
        public static readonly Color RowBg       = new Color(0.165f, 0.165f, 0.184f, 1f);
        public static readonly Color RowBorder   = new Color(0.220f, 0.220f, 0.243f, 1f);
        public static readonly Color BtnBg       = new Color(0.200f, 0.200f, 0.220f, 1f);

        public static readonly Color Accent      = new Color(0.910f, 0.627f, 0.220f, 1f);
        public static readonly Color AccentDim   = new Color(0.910f, 0.627f, 0.220f, 0.10f);
        public static readonly Color AccentBdr   = new Color(0.910f, 0.627f, 0.220f, 0.30f);

        public static readonly Color TextLight   = new Color(0.800f, 0.800f, 0.800f, 1f);
        public static readonly Color TextMid     = new Color(0.600f, 0.600f, 0.600f, 1f);
        public static readonly Color TextDim     = new Color(0.400f, 0.400f, 0.400f, 1f);
        public static readonly Color BtnText     = new Color(0.533f, 0.533f, 0.533f, 1f);

        public static readonly Color OnColor     = new Color(0.306f, 0.765f, 0.431f, 1f);
        public static readonly Color OnBg        = new Color(0.306f, 0.765f, 0.431f, 0.15f);
        public static readonly Color OnBdr       = new Color(0.306f, 0.765f, 0.431f, 0.40f);
        public static readonly Color OffColor    = new Color(0.973f, 0.443f, 0.443f, 1f);

        public static readonly Color TogOffTrack = RowBorder;
        public static readonly Color TogOnTrack  = Accent;
        public static readonly Color TogKnobOn   = new Color(0.867f, 0.867f, 0.867f, 1f);
        public static readonly Color TogKnobOff  = new Color(0.600f, 0.600f, 0.600f, 1f);

        public static readonly Color BarBg       = RowBorder;
        public static readonly Color BarFill     = Accent;

        // ── Layout ──────────────────────────────────────────────────────────

        public const float WinW = 490f, WinH = 520f;
        public const float HeaderH = 48f, TabH = 36f;
        public const float RowH = 42f, RowGap = 5f, RowPad = 14f, ContentPad = 14f;
        public const float BottomH = 42f;

        // ── Font ────────────────────────────────────────────────────────────

        private static Font _font;
        public static Font GetFont()
        {
            if (_font == null) _font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            return _font;
        }

        // ── Procedural rounded sprite ───────────────────────────────────────
        // Bigger textures + bigger radii = actually visible rounding

        public static Texture2D RoundTex(int w, int h, int r, Color fill)
        {
            var tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Bilinear;
            var clr = new Color(0, 0, 0, 0);
            var px = new Color[w * h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    float dx = 0, dy = 0;
                    if      (x < r     && y < r)     { dx = r - x;       dy = r - y; }
                    else if (x > w-r-1 && y < r)     { dx = x-(w-r-1);   dy = r - y; }
                    else if (x < r     && y > h-r-1) { dx = r - x;       dy = y-(h-r-1); }
                    else if (x > w-r-1 && y > h-r-1) { dx = x-(w-r-1);   dy = y-(h-r-1); }
                    float d = Mathf.Sqrt(dx * dx + dy * dy);
                    if (d > r + 0.5f)
                        px[y * w + x] = clr;
                    else if (d > r - 0.5f)
                        px[y * w + x] = new Color(fill.r, fill.g, fill.b, fill.a * (1f - (d - (r - 0.5f))));
                    else
                        px[y * w + x] = fill;
                }
            tex.SetPixels(px);
            tex.Apply();
            return tex;
        }

        public static Sprite RoundSprite(int sz, int r, Color fill)
        {
            var tex = RoundTex(sz, sz, r, fill);
            return Sprite.Create(tex, new Rect(0, 0, sz, sz), new Vector2(.5f, .5f), 100f, 0,
                SpriteMeshType.FullRect, new Vector4(r, r, r, r));
        }

        // Larger textures with bigger radii for visible rounding
        private static Sprite _rowSp, _btnSp, _winSp, _togSp, _knobSp, _barSp;

        public static Sprite RowSp  { get { if (_rowSp  == null) _rowSp  = RoundSprite(64, 12, Color.white); return _rowSp; } }
        public static Sprite BtnSp  { get { if (_btnSp  == null) _btnSp  = RoundSprite(48, 8,  Color.white); return _btnSp; } }
        public static Sprite WinSp  { get { if (_winSp  == null) _winSp  = RoundSprite(64, 14, Color.white); return _winSp; } }
        public static Sprite TogSp  { get { if (_togSp  == null) _togSp  = RoundSprite(36, 18, Color.white); return _togSp; } }  // pill shape
        public static Sprite KnobSp { get { if (_knobSp == null) _knobSp = RoundSprite(28, 14, Color.white); return _knobSp; } } // circle
        public static Sprite BarSp  { get { if (_barSp  == null) _barSp  = RoundSprite(16, 3,  Color.white); return _barSp; } }  // thin rounded

        // ── Core helpers ────────────────────────────────────────────────────

        public static GameObject Obj(string n, Transform p)
        {
            var g = new GameObject(n, typeof(RectTransform));
            g.transform.SetParent(p, false);
            return g;
        }

        public static RectTransform RT(GameObject g) { return g.GetComponent<RectTransform>(); }

        public static void Fill(RectTransform rt, float l = 0, float r = 0, float t = 0, float b = 0)
        {
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(l, b); rt.offsetMax = new Vector2(-r, -t);
        }

        public static void Pin(RectTransform rt, Vector2 a, Vector2 pv, Vector2 pos, Vector2 sz)
        {
            rt.anchorMin = a; rt.anchorMax = a; rt.pivot = pv;
            rt.anchoredPosition = pos; rt.sizeDelta = sz;
        }

        // Panel
        public static GameObject Panel(string n, Transform p, Color c, Sprite sp = null)
        {
            var g = Obj(n, p);
            var i = g.AddComponent<Image>(); i.color = c;
            if (sp) { i.sprite = sp; i.type = Image.Type.Sliced; }
            return g;
        }

        // Text
        public static Text Txt(string n, Transform p, string txt, int sz, FontStyle fs, TextAnchor a, Color c)
        {
            var g = Obj(n, p);
            var t = g.AddComponent<Text>();
            t.font = GetFont(); t.text = txt; t.fontSize = sz; t.fontStyle = fs;
            t.alignment = a; t.color = c;
            t.horizontalOverflow = HorizontalWrapMode.Overflow;
            t.verticalOverflow = VerticalWrapMode.Overflow;
            t.raycastTarget = false;
            return t;
        }

        // Button (smaller, rounded)
        public static Button Btn(string n, Transform p, string lbl, Vector2 sz, int fs,
            UnityEngine.Events.UnityAction clk, Color? bg = null, Color? tc = null)
        {
            var g = Obj(n, p);
            var im = g.AddComponent<Image>();
            im.sprite = BtnSp; im.type = Image.Type.Sliced; im.color = bg ?? BtnBg;
            var b = g.AddComponent<Button>();
            var cb = b.colors;
            cb.normalColor = Color.white; cb.highlightedColor = new Color(1, 1, 1, 1.15f);
            cb.pressedColor = new Color(.7f, .7f, .7f, 1);
            cb.colorMultiplier = 1; cb.fadeDuration = .08f;
            b.colors = cb;
            RT(g).sizeDelta = sz;
            b.onClick.AddListener(clk);
            var t = Txt("L", g.transform, lbl, fs, FontStyle.Bold, TextAnchor.MiddleCenter, tc ?? BtnText);
            Fill(RT(t.gameObject));
            return b;
        }

        // +/- buttons — smaller (20x20)
        public static void SmallBtn(Transform p, string lbl, UnityEngine.Events.UnityAction clk)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(20, 20), 12, clk);
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = 20; le.preferredHeight = 20;
            le.minWidth = 20; le.minHeight = 20;
            le.flexibleHeight = 0;
        }

        // Action button (accent)
        public static void ActionBtn(Transform p, string lbl, UnityEngine.Events.UnityAction clk, float w = 72)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(w, 24), 10, clk, AccentDim, Accent);
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = w; le.preferredHeight = 24;
            le.minWidth = w; le.minHeight = 24;
            le.flexibleHeight = 0;
        }

        // ── Progress bar (thin, rounded, won't stretch) ─────────────────────

        public static Image MakeBar(string n, Transform p, float pct)
        {
            var w = Obj(n, p);
            var wi = w.AddComponent<Image>();
            wi.sprite = BarSp; wi.type = Image.Type.Sliced;
            wi.color = BarBg; wi.raycastTarget = false;

            var le = w.AddComponent<LayoutElement>();
            le.preferredWidth = 70; le.preferredHeight = 4;
            le.minWidth = 70; le.minHeight = 4;
            le.flexibleHeight = 0;  // CRITICAL: prevents stretching to row height

            var f = Obj("F", w.transform);
            var fi = f.AddComponent<Image>();
            fi.sprite = BarSp; fi.type = Image.Type.Sliced;
            fi.color = BarFill;

            // Fill anchored to left, fixed height of 4
            var frt = RT(f);
            frt.anchorMin = new Vector2(0, 0.5f);
            frt.anchorMax = new Vector2(0, 0.5f);
            frt.pivot = new Vector2(0, 0.5f);
            frt.sizeDelta = new Vector2(70f * Mathf.Clamp01(pct), 4);
            frt.anchoredPosition = Vector2.zero;

            return fi;
        }

        public static void SetBar(Image fi, float pct)
        {
            if (fi) RT(fi.gameObject).sizeDelta = new Vector2(70f * Mathf.Clamp01(pct), 4);
        }

        // ── Stat row (childForceExpandHeight OFF) ───────────────────────────

        public static GameObject StatRow(string label, Transform p)
        {
            var row = Panel(label + "R", p, RowBg, RowSp);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = RowH; le.minHeight = RowH;

            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 8;
            hlg.padding = new RectOffset((int)RowPad, (int)RowPad, 0, 0);
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childForceExpandWidth = false;
            hlg.childForceExpandHeight = false;  // FIX: don't stretch children

            // Border (rounded) — must ignore layout so it overlays without taking space
            var bd = Panel("Bd", row.transform, RowBorder, RowSp);
            bd.GetComponent<Image>().raycastTarget = false;
            Fill(RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;

            // Label
            var t = Txt(label + "L", row.transform, label, 12, FontStyle.Bold, TextAnchor.MiddleLeft, TextLight);
            var tle = t.gameObject.AddComponent<LayoutElement>();
            tle.flexibleWidth = 1;
            tle.preferredHeight = RowH;  // give label explicit height since expand is off
            return row;
        }

        // ── Toggle (rounded pill track + circle knob) ───────────────────────

        public static void Toggle(Transform p, string n, UnityEngine.Events.UnityAction clk,
            out Image track, out RectTransform knob)
        {
            var g = Obj(n, p);
            track = g.AddComponent<Image>();
            track.sprite = TogSp;           // pill-shaped sprite
            track.type = Image.Type.Sliced;
            track.color = TogOffTrack;

            var b = g.AddComponent<Button>(); b.onClick.AddListener(clk);
            var cb = b.colors;
            cb.normalColor = Color.white; cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white; cb.colorMultiplier = 1;
            b.colors = cb;

            var le = g.AddComponent<LayoutElement>();
            le.preferredWidth = 34; le.preferredHeight = 18;
            le.minWidth = 34; le.minHeight = 18;
            le.flexibleHeight = 0;

            var k = Obj("K", g.transform);
            var ki = k.AddComponent<Image>();
            ki.sprite = KnobSp;            // circle sprite
            ki.type = Image.Type.Sliced;
            ki.color = TogKnobOff;
            ki.raycastTarget = false;

            knob = RT(k);
            knob.anchorMin = new Vector2(0, 0.5f);
            knob.anchorMax = new Vector2(0, 0.5f);
            knob.pivot = new Vector2(0, 0.5f);
            knob.sizeDelta = new Vector2(14, 14);
            knob.anchoredPosition = new Vector2(2, 0);
        }

        public static void SetToggle(Image track, RectTransform knob, bool on)
        {
            if (track) track.color = on ? TogOnTrack : TogOffTrack;
            if (knob)
            {
                knob.anchoredPosition = on ? new Vector2(18, 0) : new Vector2(2, 0);
                knob.GetComponent<Image>().color = on ? TogKnobOn : TogKnobOff;
            }
        }

        // ── Section header ──────────────────────────────────────────────────

        public static void SectionHeader(string title, Transform p)
        {
            var t = Txt(title + "H", p, title, 11, FontStyle.Bold, TextAnchor.MiddleCenter, Accent);
            t.gameObject.AddComponent<LayoutElement>().preferredHeight = 28;
        }

        public static void Divider(Transform p)
        {
            Panel("Dv", p, new Color(1, 1, 1, 0.04f)).AddComponent<LayoutElement>().preferredHeight = 1;
        }

        // ── Info box ────────────────────────────────────────────────────────

        public static void InfoBox(Transform p, string txt)
        {
            var bx = Panel("Inf", p, RowBg, RowSp);
            bx.AddComponent<LayoutElement>().preferredHeight = 34;
            var bd = Panel("Bd", bx.transform, RowBorder, RowSp);
            bd.GetComponent<Image>().raycastTarget = false; Fill(RT(bd));
            var t = Txt("IT", bx.transform, txt, 10, FontStyle.Italic, TextAnchor.MiddleLeft, TextDim);
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Truncate;
            Fill(RT(t.gameObject), 12, 12, 4, 4);
        }

        // ── Hotkey row ──────────────────────────────────────────────────────

        public static void HotkeyRow(Transform p, string desc, string key)
        {
            var row = Panel("HK" + key, p, RowBg, RowSp);
            row.AddComponent<LayoutElement>().preferredHeight = 30;
            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 8; hlg.padding = new RectOffset(14, 14, 0, 0);
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;
            var bd = Panel("Bd", row.transform, RowBorder, RowSp);
            bd.GetComponent<Image>().raycastTarget = false; Fill(RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;

            var dt = Txt("D", row.transform, desc, 11, FontStyle.Normal, TextAnchor.MiddleLeft, TextMid);
            var dle = dt.gameObject.AddComponent<LayoutElement>();
            dle.flexibleWidth = 1; dle.preferredHeight = 30;

            var badge = Panel("KB", row.transform, BtnBg, BtnSp);
            var ble = badge.AddComponent<LayoutElement>();
            ble.preferredWidth = 38; ble.preferredHeight = 20; ble.minWidth = 38;
            ble.flexibleHeight = 0;

            var kt = Txt("K", badge.transform, key, 11, FontStyle.Bold, TextAnchor.MiddleCenter, TextMid);
            Fill(RT(kt.gameObject));
        }
    }
}
