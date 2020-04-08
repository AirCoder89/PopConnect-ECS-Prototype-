using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/UI Line Connector")]
    [RequireComponent(typeof(UILineRenderer))]
    [ExecuteInEditMode]
    public class UILineConnector : MonoBehaviour
    {
        public List<RectTransform> transforms;
        private Vector2[] previousPositions;
        private RectTransform canvas;
        private RectTransform rt;
        private UILineRenderer lr;
        private bool updateLine;
        private void Awake()
        {
            canvas = GetComponentInParent<RectTransform>().GetParentCanvas().GetComponent<RectTransform>();
            rt = GetComponent<RectTransform>();
            lr = GetComponent<UILineRenderer>();
        }

        public void SetLineColor(Color color)
        {
            this.lr.color = color;
        }
        
        public void AddBubble(RectTransform bubble)
        {
            if(transforms == null) transforms = new List<RectTransform>();
            transforms.Add(bubble);
        }

        public void ClearLine()
        {
            if(transforms == null) transforms = new List<RectTransform>();
            transforms.Clear();
        }

        public void UpdateLine()
        {
            // Get the pivot points
            var thisPivot = rt.pivot;
            var canvasPivot = canvas.pivot;

            // Set up some arrays of coordinates in various reference systems
            var worldSpaces = new Vector3[transforms.Count];
            var canvasSpaces = new Vector3[transforms.Count];
            var points = new Vector2[transforms.Count];

            // First, convert the pivot to worldspace
            for (var i = 0; i < transforms.Count; i++)
            {
                worldSpaces[i] = transforms[i].TransformPoint(thisPivot);
            }

            // Then, convert to canvas space
            for (var i = 0; i < transforms.Count; i++)
            {
                canvasSpaces[i] = canvas.InverseTransformPoint(worldSpaces[i]);
            }

            // Calculate delta from the canvas pivot point
            for (var i = 0; i < transforms.Count; i++)
            {
                points[i] = new Vector2(canvasSpaces[i].x, canvasSpaces[i].y);
            }

            // And assign the converted points to the line renderer
            lr.Points = points;
            lr.RelativeSize = false;
            lr.drivenExternally = true;

            previousPositions = new Vector2[transforms.Count];
            for (var i = 0; i < transforms.Count; i++)
            {
                previousPositions[i] = transforms[i].anchoredPosition;
            }
            
        }
        // Update is called once per frame
        void Update()
        {
            if (transforms == null || transforms.Count < 1)
            {
                return;
            }
            //Performance check to only redraw when the child transforms move
            if (previousPositions != null && previousPositions.Length == transforms.Count)
            {
                updateLine = false;
                for (var i = 0; i < transforms.Count; i++)
                {
                    if (!updateLine && previousPositions[i] != transforms[i].anchoredPosition)
                    {
                        updateLine = true;
                    }
                }
                if (!updateLine) return;
            }

            UpdateLine();
        }
    }
}