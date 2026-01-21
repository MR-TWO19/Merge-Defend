using UnityEngine;

namespace Hawky.Math
{
    public static class Math2D
    {
        public static float CalculateDistanceFromPointToLine(Vector2 point, Vector2 linePoint1, Vector2 linePoint2)
        {
            float numerator = Mathf.Abs((linePoint2.y - linePoint1.y) * point.x - (linePoint2.x - linePoint1.x) * point.y + linePoint2.x * linePoint1.y - linePoint2.y * linePoint1.x);
            float denominator = Mathf.Sqrt(Mathf.Pow(linePoint2.y - linePoint1.y, 2) + Mathf.Pow(linePoint2.x - linePoint1.x, 2));

            return numerator / denominator;
        }
        public static float CalculateDistanceFromPointToLine(Vector2 point, Vector2 linePoint1, Vector2 linePoint2, float maxDistance)
        {
            float numerator = Mathf.Abs((linePoint2.y - linePoint1.y) * point.x - (linePoint2.x - linePoint1.x) * point.y + linePoint2.x * linePoint1.y - linePoint2.y * linePoint1.x);
            float denominator = Mathf.Sqrt(Mathf.Pow(linePoint2.y - linePoint1.y, 2) + Mathf.Pow(linePoint2.x - linePoint1.x, 2));
            float distance = numerator / denominator;

            Vector2 lineVector = linePoint2 - linePoint1;
            Vector2 pointVector1 = point - linePoint1;

            float t = Vector2.Dot(pointVector1, lineVector) / Vector2.Dot(lineVector, lineVector);
            if (t < 0.0f || t > 1.0f)
            {
                float distToEndPoint1 = Vector2.Distance(point, linePoint1);
                float distToEndPoint2 = Vector2.Distance(point, linePoint2);

                if (distToEndPoint1 > maxDistance && distToEndPoint2 > maxDistance)
                {
                    return -1;
                }
            }

            return distance;
        }

        public static Vector2 RotationZToDirection(float rotationZDegrees)
        {
            float rotationZRadians = rotationZDegrees * Mathf.Deg2Rad;

            float x = Mathf.Cos(rotationZRadians);
            float y = Mathf.Sin(rotationZRadians);

            return new Vector2(x, y).normalized;
        }

        public static float DirectionToRotationZ(Vector2 direction)
        {
            float angleRadians = Mathf.Atan2(direction.y, direction.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            return angleDegrees;
        }

        public static bool FindIntersection(Vector2 A1, Vector2 dA, Vector2 B1, Vector2 dB, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            float denominator = dA.x * dB.y - dA.y * dB.x;

            if (Mathf.Approximately(denominator, 0))
            {
                Debug.LogError("Denominator is 0");
                return false;
            }

            float t = ((B1.x - A1.x) * dB.y - (B1.y - A1.y) * dB.x) / denominator;
            intersection = A1 + t * dA;

            return true;
        }
    }
}
