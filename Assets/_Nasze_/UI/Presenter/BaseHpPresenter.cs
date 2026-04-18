using UnityEngine;

public class BaseHpPresenter : MonoBehaviour
{
    [SerializeField] private baseHp model;
    [SerializeField] private BaseHpView view;

    void Start()
    {
        model.OnHpChanged += UpdateView;
        UpdateView();
    }

    void OnDestroy()
    {
        model.OnHpChanged -= UpdateView;
    }

    void UpdateView()
    {
        view.SetText(model.GetCurrHp(), model.GetMaxHp());
    }
}