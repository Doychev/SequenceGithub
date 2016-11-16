using UnityEngine;
using System.Collections;

public class Tween : MonoBehaviour
{
    public float time = 2;
    public Vector2 startPoint;
    public Vector2 endPoint;
    private float startTime;
    public AnimationCurve curve;

    private bool tweening = true;

    public enum Tweens
    {
        LINEAR,
        EASE_IN,
        CURVE
    }

    public Tweens tweenType;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (tweening)
        {
            ExecuteTween();
        }
    }

    void ExecuteTween()
    {
        float moveX, moveY;
        switch (tweenType)
        {
            case Tweens.LINEAR:
                moveX = Linear(DeltaTime(), startPoint.x, endPoint.x);
                moveY = Linear(DeltaTime(), startPoint.y, endPoint.y);
                break;
            case Tweens.EASE_IN:
                moveX = EaseIn(DeltaTime(), startPoint.x, endPoint.x);
                moveY = EaseIn(DeltaTime(), startPoint.y, endPoint.y);
                break;
            case Tweens.CURVE:
                moveX = Curve(DeltaTime(), startPoint.x, endPoint.x);
                moveY = Curve(DeltaTime(), startPoint.y, endPoint.y);
                break;
            default:
                moveX = 0;
                moveY = 0;
                break;
        }

        transform.position = new Vector3(
            moveX,
            moveY,
            transform.position.z
        );

        Bounce();
    }

    float DeltaTime()
    {
        float timeDelta = Time.time - startTime;

        if (timeDelta < time)
        {
            return timeDelta / time;
        }
        else
        {
            return 1;
        }
    }

    void Bounce()
    {
        if (DeltaTime() == 1)
        {
            tweening = false;
            Vector2 temp = endPoint;

            endPoint = startPoint;
            startPoint = temp;
            startTime = Time.time;
        }
    }

    float Linear(float delta, float start, float end)
    {
        return Mathf.Lerp(start, end, delta);
    }

    float EaseIn(float delta, float start, float end)
    {
        return Mathf.Lerp(start, end, delta * delta);
    }

    float Curve(float delta, float start, float end)
    {
        return (end - start) * curve.Evaluate(delta) + start;
    }
}