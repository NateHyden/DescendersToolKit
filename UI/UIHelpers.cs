using UnityEngine;
using UnityEngine.UI;

namespace DescendersModMenu.UI
{
    public static class UIHelpers
    {
        // ════════════════════════════════════════════════════
        //  REDESIGN PALETTE — v3.0 Grungy MTB + Neon Lime
        // ════════════════════════════════════════════════════

        // Backgrounds — gunmetal grey
        public static readonly Color WinOuter = new Color(0.055f, 0.059f, 0.063f, 0.98f);
        public static readonly Color WinPanel = new Color(0.098f, 0.102f, 0.110f, 1.00f);
        public static readonly Color WinBorder = new Color(0.160f, 0.165f, 0.175f, 1.00f);
        public static readonly Color HeaderBg = new Color(0.067f, 0.071f, 0.078f, 1.00f);
        public static readonly Color SidebarBg = new Color(0.067f, 0.071f, 0.078f, 1.00f);
        public static readonly Color NavActive = new Color(0.090f, 0.110f, 0.055f, 1.00f);
        public static readonly Color RowBg = new Color(0.118f, 0.122f, 0.130f, 1.00f);
        public static readonly Color RowBorder = new Color(0.145f, 0.150f, 0.160f, 1.00f);
        public static readonly Color BtnBg = new Color(0.145f, 0.150f, 0.160f, 1.00f);

        // Primary accent — NEON LIME
        public static readonly Color Accent = new Color(0.800f, 1.000f, 0.000f, 1.00f);
        public static readonly Color AccentDim = new Color(0.110f, 0.125f, 0.055f, 1.00f);  // pre-blended lime on dark
        public static readonly Color AccentBdr = new Color(0.220f, 0.280f, 0.055f, 1.00f);  // pre-blended lime border

        // Button colour — NEON BLUE
        public static readonly Color NeonBlue = new Color(0.000f, 0.600f, 1.000f, 1.00f);

        // Secondary accent — ORANGE (destructive actions only)
        public static readonly Color Orange = new Color(1.000f, 0.267f, 0.000f, 1.00f);
        public static readonly Color OrangeDim = new Color(0.140f, 0.075f, 0.047f, 1.00f);  // pre-blended orange on dark
        public static readonly Color OrangeBdr = new Color(0.280f, 0.110f, 0.050f, 1.00f);  // pre-blended orange border
        public static readonly Color ActionBtnBg = new Color(0.145f, 0.150f, 0.160f, 1.00f);

        // Text
        public static readonly Color TextLight = new Color(0.900f, 0.910f, 0.920f, 1.00f);
        public static readonly Color TextMid = new Color(0.530f, 0.545f, 0.570f, 1.00f);
        public static readonly Color TextDim = new Color(0.320f, 0.330f, 0.350f, 1.00f);
        public static readonly Color BtnText = new Color(0.900f, 0.910f, 0.920f, 1.00f);

        // States
        public static readonly Color OnColor = new Color(0.000f, 1.000f, 0.533f, 1.00f);
        public static readonly Color OnBg = new Color(0.063f, 0.145f, 0.098f, 1.00f);  // pre-blended green on dark
        public static readonly Color OnBdr = new Color(0.055f, 0.310f, 0.192f, 1.00f);  // pre-blended green border
        public static readonly Color OffColor = new Color(1.000f, 0.133f, 0.267f, 1.00f);
        public static readonly Color RedDim = new Color(0.130f, 0.063f, 0.067f, 1.00f);  // pre-blended red on dark
        public static readonly Color RedBdr = new Color(0.255f, 0.075f, 0.098f, 1.00f);  // pre-blended red border

        // Toggles
        public static readonly Color TogOffTrack = new Color(0.145f, 0.150f, 0.160f, 1.00f);
        public static readonly Color TogOnTrack = new Color(0.145f, 0.175f, 0.078f, 1.00f);
        public static readonly Color TogKnobOn = new Color(0.800f, 1.000f, 0.000f, 1.00f);
        public static readonly Color TogKnobOff = new Color(0.310f, 0.320f, 0.340f, 1.00f);

        // Bars
        public static readonly Color BarBg = new Color(0.145f, 0.150f, 0.160f, 1.00f);
        public static readonly Color BarFill = new Color(0.800f, 1.000f, 0.000f, 1.00f);

        // ── Layout ──────────────────────────────────────────────────────────
        public const float WinW = 800f;
        public const float WinH = 660f;
        public const float SidebarW = 130f;
        public const float HeaderH = 58f;
        public const float TabH = 36f;
        public const float RowH = 40f;
        public const float RowGap = 4f;
        public const float RowPad = 12f;
        public const float ContentPad = 14f;
        public const float BottomH = 46f;

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

        // Angular corners — radius 2 for rows/buttons, radius 3 for window
        private static Sprite _rowSp, _btnSp, _winSp, _togSp, _knobSp, _barSp, _dotSp;
        public static Sprite RowSp { get { if (_rowSp == null) _rowSp = RoundSprite(64, 2, Color.white); return _rowSp; } }
        public static Sprite BtnSp { get { if (_btnSp == null) _btnSp = RoundSprite(48, 2, Color.white); return _btnSp; } }
        public static Sprite WinSp { get { if (_winSp == null) _winSp = RoundSprite(64, 3, Color.white); return _winSp; } }
        public static Sprite TogSp { get { if (_togSp == null) _togSp = RoundSprite(44, 2, Color.white); return _togSp; } }
        public static Sprite KnobSp { get { if (_knobSp == null) _knobSp = RoundSprite(16, 2, Color.white); return _knobSp; } }
        public static Sprite BarSp { get { if (_barSp == null) _barSp = RoundSprite(16, 3, Color.white); return _barSp; } }
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

        public static Button SmallBtn(Transform p, string lbl, UnityEngine.Events.UnityAction clk)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(24, 24), 13, clk, NeonBlue, new Color(0, 0, 0, 1));
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = 24; le.preferredHeight = 24;
            le.minWidth = 24; le.minHeight = 24; le.flexibleHeight = 0;
            return b;
        }

        public static void ActionBtn(Transform p, string lbl, UnityEngine.Events.UnityAction clk, float w = 72)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(w, 26), 11, clk, NeonBlue, new Color(0, 0, 0, 1));
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = w; le.preferredHeight = 26;
            le.minWidth = w; le.minHeight = 26; le.flexibleHeight = 0;
        }

        public static void ActionBtnOrange(Transform p, string lbl, UnityEngine.Events.UnityAction clk, float w = 72)
        {
            var b = Btn(lbl + "B", p, lbl, new Vector2(w, 26), 11, clk, NeonBlue, new Color(0, 0, 0, 1));
            var le = b.gameObject.AddComponent<LayoutElement>();
            le.preferredWidth = w; le.preferredHeight = 26;
            le.minWidth = w; le.minHeight = 26; le.flexibleHeight = 0;
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

            var t = Txt(label + "L", row.transform, label, 12, FontStyle.Bold, TextAnchor.MiddleLeft, TextLight);
            var tle = t.gameObject.AddComponent<LayoutElement>();
            tle.flexibleWidth = 1; tle.preferredHeight = RowH;
            return row;
        }

        // ── Active row highlight ──────────────────────────────────────
        // Call from RefreshAll to tint the row bg when a mod is ON.
        public static void SetRowActive(GameObject row, bool active)
        {
            if ((object)row == null) return;
            var img = row.GetComponent<Image>();
            if (img) img.color = active ? NavActive : RowBg;
        }

        public static void Toggle(Transform p, string n, UnityEngine.Events.UnityAction clk,
            out Image track, out RectTransform knob)
        {
            var g = Obj(n, p);
            track = g.AddComponent<Image>();
            track.sprite = TogSp; track.type = Image.Type.Sliced;
            track.color = TogOffTrack;

            // Border overlay on track
            var tbdr = Panel("TBdr", g.transform, RowBorder, TogSp);
            tbdr.GetComponent<Image>().raycastTarget = false;
            Fill(RT(tbdr));
            tbdr.AddComponent<LayoutElement>().ignoreLayout = true;

            var b = g.AddComponent<Button>(); b.onClick.AddListener(clk);
            var cb = b.colors;
            cb.normalColor = Color.white; cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white; cb.colorMultiplier = 1;
            b.colors = cb;

            var le = g.AddComponent<LayoutElement>();
            le.preferredWidth = 44; le.preferredHeight = 24;
            le.minWidth = 44; le.minHeight = 24; le.flexibleHeight = 0;

            var k = Obj("K", g.transform);
            var ki = k.AddComponent<Image>();
            ki.sprite = KnobSp; ki.type = Image.Type.Sliced;
            ki.color = TogKnobOff;
            ki.raycastTarget = false;

            knob = RT(k);
            knob.anchorMin = new Vector2(0, 0.5f); knob.anchorMax = new Vector2(0, 0.5f);
            knob.pivot = new Vector2(0, 0.5f);
            knob.sizeDelta = new Vector2(18, 18);
            knob.anchoredPosition = new Vector2(2, 0);
        }

        public static void SetToggle(Image track, RectTransform knob, bool on)
        {
            if (track)
            {
                track.color = on ? TogOnTrack : TogOffTrack;
                // Update border colour
                Transform tbdr = track.transform.Find("TBdr");
                if (tbdr != null)
                {
                    var tbdrImg = tbdr.GetComponent<Image>();
                    if (tbdrImg) tbdrImg.color = on
                        ? AccentBdr
                        : RowBorder;
                }
            }
            if (knob)
            {
                knob.anchoredPosition = on ? new Vector2(22, 0) : new Vector2(2, 0);
                knob.GetComponent<Image>().color = on ? TogKnobOn : TogKnobOff;
            }
        }

        public static void SectionHeader(string title, Transform p)
        {
            var row = Obj(title + "H", p);
            var le = row.AddComponent<LayoutElement>();
            le.preferredHeight = 28; le.minHeight = 28; le.flexibleHeight = 0;

            // Left accent bar — neon lime
            var bar = Panel("Bar", row.transform, Accent);
            var brt = RT(bar);
            brt.anchorMin = new Vector2(0, 0.5f); brt.anchorMax = new Vector2(0, 0.5f);
            brt.pivot = new Vector2(0, 0.5f); brt.sizeDelta = new Vector2(3, 14);
            brt.anchoredPosition = Vector2.zero;

            // Text — neon lime, bold, uppercase
            var t = Txt(title + "T", row.transform, title.ToUpper(), 11,
                FontStyle.Bold, TextAnchor.MiddleLeft, Accent);
            var trt = RT(t.gameObject);
            trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one;
            trt.offsetMin = new Vector2(10, 0); trt.offsetMax = Vector2.zero;
        }

        public static void Divider(Transform p)
        {
            Panel("Dv", p, RowBorder)
                .AddComponent<LayoutElement>().preferredHeight = 1;
        }

        public static void InfoBox(Transform p, string txt)
        {
            var bx = Panel("Inf", p, RowBg, RowSp);
            bx.AddComponent<LayoutElement>().preferredHeight = 34;

            // Border overlay
            var bd = Panel("Bd", bx.transform, RowBorder, RowSp);
            bd.GetComponent<Image>().raycastTarget = false; Fill(RT(bd));
            bd.AddComponent<LayoutElement>().ignoreLayout = true;

            // Left accent bar
            var lbar = Panel("LBar", bx.transform, TextDim);
            var lbRT = RT(lbar);
            lbRT.anchorMin = Vector2.zero; lbRT.anchorMax = new Vector2(0, 1);
            lbRT.pivot = new Vector2(0, 0.5f);
            lbRT.sizeDelta = new Vector2(2, 0); lbRT.offsetMin = new Vector2(0, 4);
            lbRT.offsetMax = new Vector2(2, -4);
            lbar.AddComponent<LayoutElement>().ignoreLayout = true;

            var t = Txt("IT", bx.transform, txt, 10, FontStyle.Italic, TextAnchor.MiddleLeft, TextDim);
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Truncate;
            Fill(RT(t.gameObject), 14, 12, 4, 4);
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

            // Accent badge — width scales with text so long labels like "RS Dbl Click" fit
            var badge = Panel("KB", row.transform, AccentDim, BtnSp);
            var ble = badge.AddComponent<LayoutElement>();
            float badgeW = Mathf.Max(38f, key.Length * 8f + 16f);
            ble.preferredWidth = badgeW; ble.minWidth = badgeW; ble.preferredHeight = 20; ble.flexibleHeight = 0;

            // Badge border
            var bbdr = Panel("BBdr", badge.transform, AccentBdr, BtnSp);
            bbdr.GetComponent<Image>().raycastTarget = false;
            Fill(RT(bbdr));
            bbdr.AddComponent<LayoutElement>().ignoreLayout = true;

            var kt = Txt("K", badge.transform, key, 11, FontStyle.Bold, TextAnchor.MiddleCenter, Accent);
            Fill(RT(kt.gameObject));
        }

        // ── Scroll fix: forward scroll events from buttons to parent ScrollRect ──
        public static void AddScrollForwarders(Transform root)
        {
            if ((object)root == null) return;
            ScrollRect parentSR = root.GetComponentInParent<ScrollRect>();
            if ((object)parentSR == null) return;
            AddForwardersRecursive(root, parentSR);
        }

        private static void AddForwardersRecursive(Transform t, ScrollRect sr)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                // Add forwarder to anything with a Graphic that receives raycasts
                var graphic = child.GetComponent<UnityEngine.UI.Graphic>();
                if ((object)graphic != null && graphic.raycastTarget)
                {
                    var fwd = child.GetComponent<ScrollForwarder>();
                    if ((object)fwd == null)
                    {
                        fwd = child.gameObject.AddComponent<ScrollForwarder>();
                        fwd.target = sr;
                    }
                }
                AddForwardersRecursive(child, sr);
            }
        }
    }

    // MonoBehaviour that catches scroll events on buttons/toggles and
    // forwards them to the parent ScrollRect so scrolling works everywhere
    public class ScrollForwarder : MonoBehaviour, UnityEngine.EventSystems.IScrollHandler
    {
        public ScrollRect target;

        public void OnScroll(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if ((object)target != null)
                target.OnScroll(eventData);
        }
    }
}