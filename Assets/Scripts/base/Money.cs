using System;
using UnityEngine;

public class Money : MonoBehaviour
{

    private int _currMoney;
    [SerializeField] private int _startMoney=0;
     public event Action OnGetMoney;
    public void AddMoney(int money){
    _currMoney+=money;
    OnGetMoney?.Invoke();
    }
    public void SubMoney(int money){
    if (_currMoney -money <=0){ _currMoney=0;}
    else{_currMoney-=money; }
    OnGetMoney?.Invoke();
    }
    public int GetCurrMoney(){return _currMoney;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      _currMoney=_startMoney;
            OnGetMoney?.Invoke();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
