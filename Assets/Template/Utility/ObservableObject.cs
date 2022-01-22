using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class ObservableObject : INotifyPropertyChanged
{
    /// <summary>
    /// Raised when a property on this object has a new value.
    /// </summary>
    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Raises the PropertyChange event for the property specified
    /// </summary>
    /// <param name="propertyName">Property name to update. Is case-sensitive.</param>
    public virtual void RaisePropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }

    /// <summary>
    /// Raises this object's PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The property that has a new value.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    protected void Notify([CallerMemberName] string propertyName = "")
    {
        OnPropertyChanged(propertyName);
    }
}