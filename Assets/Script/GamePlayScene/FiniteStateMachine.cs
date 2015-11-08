using UnityEngine;
using System.Collections;
using System.Collections.Generic;   

public class FiniteStateMachine<T>
{
    #region Data Members

    private Stack<T> states;
    private T currentState;

    #endregion

    #region Setters & Getters

    //None

    #endregion

    #region Built-in Unity Methods

    //None
   
    #endregion

    #region Main Methods

    public FiniteStateMachine()
    {
        states = new Stack<T>();
    }

    public T CurrentState()
    {
        return states.Peek();
    }

    public void PushState(T state)
    {
        states.Push(state);
    }

    public T PopState()
    {
        return states.Pop();
    }
    
    #endregion
}
