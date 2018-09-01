using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MovieRenamer.MVVM
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method to reduce redundancy when calling the setter of a MVVM Property
        /// Instead of calling: RaisePropertyChangedEvent("SomePropertyName");
        /// Just call: SetProperty(ref _somePropertyName, value);
        /// https://www.c-sharpcorner.com/blogs/how-to-use-callermembername-attribute-in-wpf-with-mvvm-pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">The private field to store the property value</param>
        /// <param name="value">The property value</param>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns true if the property value has been changed</returns>
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
                
            storage = value;
            RaisePropertyChangedEvent(propertyName);
            return true;
        }
        
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var eventHandler = PropertyChanged;
            var eventArgs = new PropertyChangedEventArgs(propertyName);
            eventHandler?.Invoke(this, eventArgs);
        }
    }
}
