using System.Collections.Generic;

namespace Core
{
    public class RadioButtonList<T>
    {
        public string Id { get; set; }
        private T selectedValue;
        public T SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                UpdatedSelectedItems();
            }
        }

        private void UpdatedSelectedItems()
        {
            if (ListItems == null)
                return;

            ListItems.ForEach(li => li.Selected = Equals(li.Value, SelectedValue));
        }

        private List<RadioButtonListItem<T>> listItems;
        public List<RadioButtonListItem<T>> ListItems
        {
            get { return listItems; }
            set
            {
                listItems = value;
                UpdatedSelectedItems();
            }
        }
    }

    public class RadioButtonListItem<T>
    {
        public bool Selected { get; set; }

        public string Text { get; set; }

        public T Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}