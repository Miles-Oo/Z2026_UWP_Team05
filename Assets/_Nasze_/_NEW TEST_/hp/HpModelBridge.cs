using UnityEngine;

public class HpModelBridge : MonoBehaviour
{
    private HpModel _model;

    public void Init(HpModel model)
    {
        _model = model;
    }

    public HpModel GetModel()
    {
        return _model;
    }
}