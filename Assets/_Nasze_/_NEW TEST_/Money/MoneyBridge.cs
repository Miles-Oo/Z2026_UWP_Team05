using UnityEngine;

public class MoneyBridge : MonoBehaviour
{
    private MoneyModel _model;

    public void Init(MoneyModel model)
    {
        _model = model;
    }

    public MoneyModel GetModel()
    {
        return _model;
    }
}