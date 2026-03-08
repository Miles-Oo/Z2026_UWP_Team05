using UnityEngine;

public class enemyHp : MonoBehaviour
{
 
  [SerializeField] private int _currHp;
   [SerializeField] private int _maxHp;    

    public void SetMaxHp(int hp){
        if (hp > 0){_maxHp=hp;}
        else{
        print("nie wolno używać ujemnej wartości!");}
    }
    public void AddHp(int hp){
    if (_currHp + hp >= _maxHp) { _currHp=_maxHp;}
    else{ _currHp+=hp;}
 
    }
    public void SubHp(int hp){
    if (_currHp -hp <=0){ _currHp=0;}
    else{_currHp-=hp; }
        if (_currHp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public int GetCurrHp(){return _currHp;}
    public int GetMaxHp(){return _maxHp;}
  
    void Start()
    {
        if (_maxHp <=0)
        {
          _maxHp=1;
        }
        _currHp=_maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
