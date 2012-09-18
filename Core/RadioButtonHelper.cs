using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core
{
    public static class RadioButtonHelper
    {
        public static RadioButtonList<T> ParseEnumToRadioButtonList<T>(T enumDefaultValue) where T : struct
        {
            var enumType = typeof (T);

            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("This method is only valid for Enumerations");
            }

            T value = default(T);
            var fields = enumType.GetFields();
            var radioButtonList = new RadioButtonList<T>
                                      {
                                          Id = enumType.ToString(),
                                          ListItems = new List<RadioButtonListItem<T>>()
                                      };

            foreach (var field in fields)
            {
                var attributes =
                    (DescriptionAttribute[]) field.GetCustomAttributes(typeof (DescriptionAttribute), false);
                if (attributes.Length <= 0) 
                    continue;

                value = (T) field.GetRawConstantValue();

                radioButtonList.ListItems.Add(new RadioButtonListItem<T>
                                                  {
                                                      Text = attributes[0].Description,
                                                      Selected = value.ToString() == enumDefaultValue.ToString(),
                                                      Value = value
                                                  });
            }

            return radioButtonList;
        }
    }
}