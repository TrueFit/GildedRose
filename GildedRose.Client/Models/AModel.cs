using System.ComponentModel;

namespace GildedRose.Client.Models
{
    /// <summary>
    /// Base implementation of any model
    /// </summary>
    public abstract class AModel : IModel
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
