using UnityEngine;

public class BaseMoneyPresenter : MonoBehaviour
{
    [SerializeField] private Money model;
    [SerializeField] private BaseMoneyView view;

    void Start()
    {
        model.OnMoneyChange += UpdateView;
        UpdateView();
    }

    void OnDestroy()
    {
        model.OnMoneyChange -= UpdateView;
    }

    void UpdateView()
    {
        view.SetText(model.GetCurrMoney());
    }
}