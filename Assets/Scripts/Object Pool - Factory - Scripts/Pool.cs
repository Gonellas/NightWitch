using System.Collections.Generic;
using System;

public class Pool<T>
{
    //Delegado que devuelve tipo T, por lo que aca voy a guardar el metodo de COMO se crea el objeto
    private Func<T> _factoryMethod;

    //Delegados que toman por parametro tipo T, donde voy a guardar COMO se prende/apaga el objeto una vez lo llame el cliente o tenga que regresarse al pool
    private Action<T> _turnOnCallback;
    private Action<T> _turnOffCallback;

    //Mi "cajon" donde voy a guardar los objetos disponibles para su uso
    private List<T> _currentStock;

    //[Constructor] que se llama cuando se crea una referencia de un pool nuevo
    public Pool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialAmount)
    {
        //Inicializo mi lista
        _currentStock = new List<T>();
        
        //Guardo COMO se crea el objeto
        _factoryMethod = factoryMethod;

        //Guardo COMO se prende el objeto
        _turnOnCallback = turnOnCallback;

        //Guardo COMO se apaga el objeto
        _turnOffCallback = turnOffCallback;

        for (int i = 0; i < initialAmount; i++)
        {
            //Uso el delegado donde tenia guardado el metodo para crear el objeto
            T obj = _factoryMethod();

            //Lo apago
            _turnOffCallback(obj);
            
            //Lo guardo en mi "cajon"
            _currentStock.Add(obj);
        }
    }

    public T GetObject()
    {
        T result;

        if (_currentStock.Count == 0)
        {
            result = _factoryMethod();
        }
        else
        {
            result = _currentStock[0];
            _currentStock.RemoveAt(0);
        }

        _turnOnCallback(result);
        
        return result;
    }
    
    public void ReturnObjectToPool(T obj)
    {
        _turnOffCallback(obj);
        _currentStock.Add(obj);
    }
}
