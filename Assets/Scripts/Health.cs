using UnityEngine;

public class Health : MonoBehaviour
{
    private float _value = 100.0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public float GetValue()
    {
        return _value;
    }

    public void SetValue(float value)
    {
        _value = value;
    }
}