using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class UIHelpers
    {
        // ── Descenders orange palette ───────────────────────────────────────
        public static readonly Color WinOuter = new Color(0.078f, 0.078f, 0.094f, 0.98f);
        public static readonly Color WinPanel = new Color(0.098f, 0.098f, 0.118f, 1f);
        public static readonly Color WinBorder = new Color(0.180f, 0.180f, 0.220f, 1f);
        public static readonly Color HeaderBg = new Color(0.063f, 0.063f, 0.078f, 1f);
        public static readonly Color SidebarBg = new Color(0.051f, 0.051f, 0.067f, 1f);
        public static readonly Color NavActive = new Color(0.118f, 0.118f, 0.149f, 1f);
        public static readonly Color RowBg = new Color(0.133f, 0.133f, 0.165f, 1f);
        public static readonly Color RowBorder = new Color(0.196f, 0.196f, 0.235f, 1f);
        public static readonly Color BtnBg = new Color(0.165f, 0.165f, 0.200f, 1f);

        // Descenders #FF5500 orange
        public static readonly Color Accent = new Color(1.000f, 0.333f, 0.000f, 1f);
        public static readonly Color AccentDim = new Color(0.820f, 0.310f, 0.080f, 0.12f);
        public static readonly Color AccentBdr = new Color(0.820f, 0.310f, 0.080f, 0.35f);

        // Softer orange for small action buttons — less aggressive than full Accent
        public static readonly Color ActionBtnBg = new Color(0.800f, 0.280f, 0.040f, 1f);

        public static readonly Color TextLight = new Color(0.918f, 0.918f, 0.937f, 1f);
        public static readonly Color TextMid = new Color(0.580f, 0.580f, 0.659f, 1f);
        public static readonly Color TextDim = new Color(0.333f, 0.333f, 0.400f, 1f);
        public static readonly Color BtnText = new Color(0.700f, 0.700f, 0.750f, 1f);

        public static readonly Color OnColor = new Color(0.133f, 0.773f, 0.369f, 1f);
        public static readonly Color OnBg = new Color(0.133f, 0.773f, 0.369f, 0.12f);
        public static readonly Color OnBdr = new Color(0.133f, 0.773f, 0.369f, 0.35f);
        public static readonly Color OffColor = new Color(0.937f, 0.267f, 0.267f, 1f);

        public static readonly Color TogOffTrack = RowBorder;
        public static readonly Color TogOnTrack = Accent;
        public static readonly Color TogKnobOn = new Color(1.000f, 1.000f, 1.000f, 1f);
        public static readonly Color TogKnobOff = new Color(0.500f, 0.500f, 0.560f, 1f);

        public static readonly Color BarBg = RowBorder;
        public static readonly Color BarFill = Accent;

        // ── Layout ──────────────────────────────────────────────────────────
        public const float WinW = 760f;
        public const float WinH = 780f;
        public const float SidebarW = 120f;
        public const float HeaderH = 46f;
        public const float TabH = 36f;  // kept for compat, not used
        public const float RowH = 40f;
        public const float RowGap = 4f;
        public const float RowPad = 12f;
        public const float ContentPad = 12f;
        public const float BottomH = 42f;

        // ── Font ────────────────────────────────────────────────────────────
        private static Font _font;
        public static Font GetFont()
        {
            if (_font == null) _font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            return _font;
        }

        // ── Procedural rounded sprites ───────────────────────────────────────
        public static Texture2D RoundTex(int w, int h, int r, Color fill)
        {
            var tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Bilinear;
            var clr = new Color(0, 0, 0, 0);
            var px = new Color[w * h];
            for (int y2 = 0; y2 < h; y2++)
                for (int x = 0; x < w; x++)
                {
                    float dx = 0, dy = 0;
                    if (x < r && y2 < r) { dx = r - x; dy = r - y2; }
                    else if (x > w - r - 1 && y2 < r) { dx = x - (w - r - 1); dy = r - y2; }
                    else if (x < r && y2 > h - r - 1) { dx = r - x; dy = y2 - (h - r - 1); }
                    else if (x > w - r - 1 && y2 > h - r - 1) { dx = x - (w - r - 1); dy = y2 - (h - r - 1); }
                    float d = Mathf.Sqrt(dx * dx + dy * dy);
                    if (d > r + 0.5f) px[y2 * w + x] = clr;
                    else if (d > r - 0.5f) px[y2 * w + x] = new Color(fill.r, fill.g, fill.b, fill.a * (1f - (d - (r - 0.5f))));
                    else px[y2 * w + x] = fill;
                }
            tex.SetPixels(px); tex.Apply();
            return tex;
        }

        public static Sprite RoundSprite(int sz, int r, Color fill)
        {
            var tex = RoundTex(sz, sz, r, fill);
            return Sprite.Create(tex, new Rect(0, 0, sz, sz), new Vector2(.5f, .5f), 100f, 0,
                SpriteMeshType.FullRect, new Vector4(r, r, r, r));
        }

        private static Sprite _rowSp, _btnSp, _winSp, _togSp, _knobSp, _barSp, _dotSp;
        public static Sprite RowSp { get { if (_rowSp == null) _rowSp = RoundSprite(64, 10, Color.white); return _rowSp; } }
        public static Sprite BtnSp { get { if (_btnSp == null) _btnSp = RoundSprite(48, 6, Color.white); return _btnSp; } }
        public static Sprite WinSp { get { if (_winSp == null) _winSp = RoundSprite(64, 14, Color.white); return _winSp; } }
        public static Sprite TogSp { get { if (_togSp == null) _togSp = RoundSprite(44, 6, Color.white); return _togSp; } }
        public static Sprite KnobSp { get { if (_knobSp == null) _knobSp = RoundSprite(16, 8, Color.white); return _knobSp; } }
        public static Sprite BarSp { get { if (_barSp == null) _barSp = RoundSprite(16, 3, Color.white); return _barSp; } }
        // DotSp — perfect circle, NO 9-slice, used with Image.Type.Simple
        public static Sprite DotSp
        {
            get
            {
                if (_dotSp == null)
                {
                    var tex = RoundTex(16, 16, 8, Color.white);
                    _dotSp = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(.5f, .5f), 100f);
                }
                return _dotSp;
            }
        }

        // ── Core helpers ─────────────────────────────────────────────────────
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

        public static GameObject Panel(string n, Transform p, Color c, Sprite sp = null)
        {
            var g = Obj(n, p);
            var i = g.AddComponent<Image>(); i.color = c;
            if (sp) { i.sprite = sp; i.type = Image.Type.Sliced; }
            return g;
        }

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

        public static void SmallBtn(Transform p, string lbl, UnityEngine.Events.UnityAction clk)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(22, 22), 12, clk);
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = 22; le.preferredHeight = 22;
            le.minWidth = 22; le.minHeight = 22; le.flexibleHeight = 0;
        }

        public static void ActionBtn(Transform p, string lbl, UnityEngine.Events.UnityAction clk, float w = 72)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(w, 24), 10, clk, ActionBtnBg, Color.white);
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = w; le.preferredHeight = 24;
            le.minWidth = w; le.minHeight = 24; le.flexibleHeight = 0;
        }

        public static Image MakeBar(string n, Transform p, float pct)
        {
            var w = Obj(n, p);
            var wi = w.AddComponent<Image>();
            wi.sprite = BarSp; wi.type = Image.Type.Sliced;
            wi.color = BarBg; wi.raycastTarget = false;
            var le = w.AddComponent<LayoutElement>();
            le.preferredWidth = 70; le.preferredHeight = 4;
            le.minWidth = 70; le.minHeight = 4; le.flexibleHeight = 0;

            var f = Obj("F", w.transform);
            var fi = f.AddComponent<Image>();
            fi.sprite = BarSp; fi.type = Image.Type.Sliced; fi.color = BarFill;
            var frt = RT(f);
            frt.anchorMin = new Vector2(0, 0.5f); frt.anchorMax = new Vector2(0, 0.5f);
            frt.pivot = new Vector2(0, 0.5f);
            frt.sizeDelta = new Vector2(70f * Mathf.Clamp01(pct), 4);
            frt.anchoredPosition = Vector2.zero;
            return fi;
        }

        public static void SetBar(Image fi, float pct)
        {
            if (fi) RT(fi.gameObject).sizeDelta = new Vector2(70f * Mathf.Clamp01(pct), 4);
        }

        public static GameObject StatRow(string label, Transform p)
        {
            var row = Panel(label + "R", p, RowBg, RowSp);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = RowH; le.minHeight = RowH;

            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = 8; hlg.padding = new RectOffset((int)RowPad, (int)RowPad, 0, 0);
            hlg.childAlignment = TextAnchor.MiddleCenter;
            hlg.childForceExpandWidth = false; hlg.childForceExpandHeight = false;

            var bd = Panel("Bd", row.transform, RowBorder, RowSp);
            bd.GetComponent<Image>().raycastTarget = false;
            Fill(RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;

            var t = Txt(label + "L", row.transform, label, 11, FontStyle.Bold, TextAnchor.MiddleLeft, TextLight);
            var tle = t.gameObject.AddComponent<LayoutElement>();
            tle.flexibleWidth = 1; tle.preferredHeight = RowH;
            return row;
        }

        public static void Toggle(Transform p, string n, UnityEngine.Events.UnityAction clk,
            out Image track, out RectTransform knob)
        {
            var g = Obj(n, p);
            track = g.AddComponent<Image>();
            track.sprite = TogSp; track.type = Image.Type.Sliced; track.color = TogOffTrack;

            var b = g.AddComponent<Button>(); b.onClick.AddListener(clk);
            var cb = b.colors;
            cb.normalColor = Color.white; cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white; cb.colorMultiplier = 1;
            b.colors = cb;

            var le = g.AddComponent<LayoutElement>();
            le.preferredWidth = 40; le.preferredHeight = 22;
            le.minWidth = 40; le.minHeight = 22; le.flexibleHeight = 0;

            var k = Obj("K", g.transform);
            var ki = k.AddComponent<Image>();
            ki.sprite = KnobSp; ki.type = Image.Type.Sliced; ki.color = TogKnobOff;
            ki.raycastTarget = false;

            knob = RT(k);
            knob.anchorMin = new Vector2(0, 0.5f); knob.anchorMax = new Vector2(0, 0.5f);
            knob.pivot = new Vector2(0, 0.5f);
            knob.sizeDelta = new Vector2(18, 18);
            knob.anchoredPosition = new Vector2(2, 0);
        }

        public static void SetToggle(Image track, RectTransform knob, bool on)
        {
            if (track) track.color = on ? TogOnTrack : TogOffTrack;
            if (knob)
            {
                knob.anchoredPosition = on ? new Vector2(20, 0) : new Vector2(2, 0);
                knob.GetComponent<Image>().color = on ? TogKnobOn : TogKnobOff;
            }
        }

        public static void SectionHeader(string title, Transform p)
        {
            // Left accent bar + uppercase label
            var row = Obj(title + "H", p);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = 26; le.minHeight = 26; le.flexibleHeight = 0;

            var bar = Panel("Bar", row.transform, Accent);
            var brt = RT(bar);
            brt.anchorMin = new Vector2(0, 0.5f); brt.anchorMax = new Vector2(0, 0.5f);
            brt.pivot = new Vector2(0, 0.5f); brt.sizeDelta = new Vector2(3, 13);
            brt.anchoredPosition = Vector2.zero;

            var t = Txt(title + "T", row.transform, title.ToUpper(), 10,
                FontStyle.Bold, TextAnchor.MiddleLeft, Accent);
            var trt = RT(t.gameObject);
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = new Vector2(10, 0); trt.offsetMax = Vector2.zero;
        }

        public static void Divider(Transform p)
        {
            Panel("Dv", p, new Color(0.22f, 0.22f, 0.28f, 0.5f))
                .AddComponent<LayoutElement>().preferredHeight = 1;
        }

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
            ble.preferredWidth = 38; ble.preferredHeight = 20; ble.minWidth = 38; ble.flexibleHeight = 0;

            var kt = Txt("K", badge.transform, key, 11, FontStyle.Bold, TextAnchor.MiddleCenter, Accent);
            Fill(RT(kt.gameObject));
        }
    }
}