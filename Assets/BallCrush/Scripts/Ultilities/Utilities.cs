using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;


namespace BallCrush
{
    public static class Utilities
    {
        public static T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        public static IEnumerator WaitAfter(float time, System.Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public static IEnumerator WaitAfterRealtime(float time, System.Action callback)
        {
            yield return new WaitForSecondsRealtime(time);
            callback?.Invoke();
        }
        public static bool IsObjectOutOfCameraView(Transform targetTransform)
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetTransform.position);

            return viewportPosition.x < 0 || viewportPosition.x > 1 ||
                   viewportPosition.y < 0 || viewportPosition.y > 1;
        }


        #region CHECK MOUSE UI
        public static bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        ///Returns 'true' if we touched or hovering on Unity UI element.
        public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];

                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }

            return false;
        }

        ///Gets all event systen raycast results of current mouse or touch position.
        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);

            return raysastResults;
        }
        #endregion


        #region RECT
        public static Vector2 CalculateRectangleCenter(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            // Calculate the average x and y coordinates of the points
            float centerX = (p1.x + p2.x + p3.x + p4.x) / 4f;
            float centerY = (p1.y + p2.y + p3.y + p4.y) / 4f;

            return new Vector2(centerX, centerY);
        }

        public static bool IsPointInsideRectangle(Vector2 point, Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD, float offset)
        {
            // Find the minimum and maximum x and y coordinates of the rectangle with offset
            float minX = Mathf.Min(pA.x, pB.x, pC.x, pD.x) - offset;
            float maxX = Mathf.Max(pA.x, pB.x, pC.x, pD.x) + offset;
            float minY = Mathf.Min(pA.y, pB.y, pC.y, pD.y) - offset;
            float maxY = Mathf.Max(pA.y, pB.y, pC.y, pD.y) + offset;

            // Check if the point is inside the rectangle with offset
            return point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY;
        }

        public static float CalculateRectangleArea(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD, float offset)
        {
            // Find the minimum and maximum x and y coordinates of the rectangle with offset
            float minX = Mathf.Min(pA.x, pB.x, pC.x, pD.x) - offset;
            float maxX = Mathf.Max(pA.x, pB.x, pC.x, pD.x) + offset;
            float minY = Mathf.Min(pA.y, pB.y, pC.y, pD.y) - offset;
            float maxY = Mathf.Max(pA.y, pB.y, pC.y, pD.y) + offset;

            // Calculate the length and width of the rectangle
            float length = maxX - minX;
            float width = maxY - minY;

            // Calculate and return the area of the rectangle
            float area = length * width;
            return area;
        }

        public static Vector2 GetRandomPositionInRectangle(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD, float offset)
        {
            // Calculate the minimum and maximum x and y coordinates of the rectangle with offset
            float minX = Mathf.Min(pA.x, pB.x, pC.x, pD.x) + offset;
            float maxX = Mathf.Max(pA.x, pB.x, pC.x, pD.x) - offset;
            float minY = Mathf.Min(pA.y, pB.y, pC.y, pD.y) + offset;
            float maxY = Mathf.Max(pA.y, pB.y, pC.y, pD.y) - offset;

            // Generate random x and y coordinates within the rectangle with offset
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            return new Vector2(randomX, randomY);
        }

        public static List<Vector2> GetRandomPointsInRectangle(Vector2 pA, Vector2 pB, Vector2 pC, Vector2 pD, float offset, float minDistance, int numberOfPoints)
        {
            List<Vector2> randomPoints = new List<Vector2>();

            // Calculate the minimum and maximum x and y coordinates of the rectangle with offset
            float minX = Mathf.Min(pA.x, pB.x, pC.x, pD.x) + offset;
            float maxX = Mathf.Max(pA.x, pB.x, pC.x, pD.x) - offset;
            float minY = Mathf.Min(pA.y, pB.y, pC.y, pD.y) + offset;
            float maxY = Mathf.Max(pA.y, pB.y, pC.y, pD.y) - offset;

            int attempts = 0;
            int maxAttempts = 100;

            while (randomPoints.Count < numberOfPoints && attempts < maxAttempts)
            {
                // Generate random x and y coordinates within the rectangle with offset
                float randomX = Random.Range(minX, maxX);
                float randomY = Random.Range(minY, maxY);
                Vector2 randomPoint = new Vector2(randomX, randomY);

                bool validPoint = true;

                // Check if the new random point is at least 'minDistance' away from existing points
                foreach (Vector2 existingPoint in randomPoints)
                {
                    if (Vector2.Distance(randomPoint, existingPoint) < minDistance)
                    {
                        validPoint = false;
                        break;
                    }
                }

                if (validPoint)
                {
                    randomPoints.Add(randomPoint);
                }

                attempts++;
            }

            return randomPoints;
        }
        #endregion
    }
}
