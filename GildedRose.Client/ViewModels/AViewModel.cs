using GildedRose.Client.Models;
using System.ComponentModel;

namespace GildedRose.Client.ViewModels
{
    /// <summary>
    /// Base class for any kind of view model
    /// </summary>
    public abstract class AViewModel : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    /// <summary>
    /// Base class for any kind of view model that encapsulates a specific model.
    /// </summary>
    public abstract class AViewModel<T> : AViewModel where T : IModel
    {
        /// <summary>
        /// The model that is encapsulated by this view model.
        /// </summary>
        public T Model { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">Model that is encapsulated by this view model.</param>
        public AViewModel(T model)
        {
            Model = model;
        }
    }
}