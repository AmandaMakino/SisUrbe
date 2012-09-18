using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Core;
using SysCEF.Model;

namespace SysCEF.Web.Helpers
{
    
    public static class HtmlHelpers
    {
        public static MvcHtmlString HiddenForEnum<TModel>(this HtmlHelper<TModel> htmlHelper, Type type, object selected, string id)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum.");
            }

            var enums = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(type))
            {
                var item = new SelectListItem
                {
                    Value = value.ToString(CultureInfo.InvariantCulture),
                    Text = Enum.GetName(type, value)
                };

                if (selected != null)
                {
                    item.Selected = (int)selected == value;
                }

                enums.Add(item);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string hiddenObject = String.Format("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" />", id, jss.Serialize(enums).Replace("\"", "'"));
            return new MvcHtmlString(hiddenObject);
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, bool canEdit)
        {
            return canEdit ?
                htmlHelper.DropDownListFor(expression, selectList) :
                htmlHelper.DropDownListFor(expression, selectList, new { disabled = "disabled" });
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type type, object selected)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type is not an enum.");
            }

            if (selected != null && selected.GetType() != type)
            {
                throw new ArgumentException(String.Format("Selected object is not a {0}.", type));
            }

            var enums = new List<SelectListItem>();

            foreach (int value in Enum.GetValues(type))
            {
                var item = new SelectListItem
                {
                    Value = value.ToString(CultureInfo.InvariantCulture),
                    Text = GetTypeDescriptionIfAvailable(type, Enum.GetName(type, value))
                };

                if (selected != null)
                    item.Selected = (int)selected == value;

                enums.Add(item);
            }

            return htmlHelper.DropDownListFor(expression, enums);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonList<TRadioButtonListValue>>> expression) where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, EnumDisposicaoRadioButtons.Horizontal, null);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonList<TRadioButtonListValue>>> expression, EnumDisposicaoRadioButtons disposicaoRadioButtons) where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, disposicaoRadioButtons, null);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonList<TRadioButtonListValue>>> expression, object htmlAttributes) where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, EnumDisposicaoRadioButtons.Horizontal, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonList<TRadioButtonListValue>>> expression, EnumDisposicaoRadioButtons disposicaoRadioButtons, object htmlAttributes) where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, disposicaoRadioButtons, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonList<TRadioButtonListValue>>> expression, EnumDisposicaoRadioButtons disposicaoRadioButtons, IDictionary<string, object> htmlAttributes) where TModel : class
        {
            var inputName = GetInputName(expression);

            RadioButtonList<TRadioButtonListValue> radioButtonList = GetValue(htmlHelper, expression);
            
            var tableTag = new TagBuilder("table");
            tableTag.MergeAttribute("id", inputName);
            tableTag.MergeAttribute("class", "radio");

            if (disposicaoRadioButtons == EnumDisposicaoRadioButtons.Horizontal)
            {
                tableTag.InnerHtml += "<tr>";

                foreach (var item in radioButtonList.ListItems)
                {
                    var radioButtonTag = HorizontalRadioButton(htmlHelper, inputName,
                                                               new SelectListItem
                                                                   {
                                                                       Text = item.Text,
                                                                       Selected = item.Selected,
                                                                       Value = item.Value.ToString()
                                                                   }, htmlAttributes);

                    tableTag.InnerHtml += radioButtonTag;
                }

                tableTag.InnerHtml += "</tr>";
            }
            else
            {
                foreach (var item in radioButtonList.ListItems)
                {
                    var radioButtonTag = VerticalRadioButton(htmlHelper, inputName,
                                                               new SelectListItem
                                                               {
                                                                   Text = item.Text,
                                                                   Selected = item.Selected,
                                                                   Value = item.Value.ToString()
                                                               }, htmlAttributes);

                    tableTag.InnerHtml += radioButtonTag;
                }
            }

            return new MvcHtmlString(tableTag.ToString());
        }

        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodCallExpression = (MethodCallExpression)expression.Body;
                string name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);

            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            // p => p.Foo.Bar().Baz.ToString() => p.Foo OR throw...

            var methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetInputName(methodCallExpression);
            }
            return expression.Object.ToString();
        }

        public static string HorizontalRadioButton(this HtmlHelper htmlHelper, string name, SelectListItem listItem,
                             IDictionary<string, object> htmlAttributes)
        {
            var inputIdSb = new StringBuilder();
            inputIdSb.Append(name)
                .Append("_")
                .Append(listItem.Value);

            var sb = new StringBuilder("<td>");

            var builder = new TagBuilder("input");
            if (listItem.Selected) builder.MergeAttribute("checked", "checked");
            builder.MergeAttribute("type", "radio");
            builder.MergeAttribute("value", listItem.Value);
            builder.MergeAttribute("id", inputIdSb.ToString());
            builder.MergeAttribute("name", name + ".SelectedValue");
            builder.MergeAttributes(htmlAttributes);
            sb.Append(builder.ToString(TagRenderMode.SelfClosing));
            sb.Append("</td><td>");
            sb.Append(RadioButtonLabel(inputIdSb.ToString(), listItem.Text, htmlAttributes));
            sb.Append("</td>");

            return sb.ToString();
        }

        public static string VerticalRadioButton(this HtmlHelper htmlHelper, string name, SelectListItem listItem,
                             IDictionary<string, object> htmlAttributes)
        {
            var inputIdSb = new StringBuilder();
            inputIdSb.Append(name)
                .Append("_")
                .Append(listItem.Value);

            var sb = new StringBuilder("<tr><td>");

            var builder = new TagBuilder("input");
            if (listItem.Selected) builder.MergeAttribute("checked", "checked");
            builder.MergeAttribute("type", "radio");
            builder.MergeAttribute("value", listItem.Value);
            builder.MergeAttribute("id", inputIdSb.ToString());
            builder.MergeAttribute("name", name + ".SelectedValue");
            builder.MergeAttributes(htmlAttributes);
            sb.Append(builder.ToString(TagRenderMode.SelfClosing));
            sb.Append("</td><td>");
            sb.Append(RadioButtonLabel(inputIdSb.ToString(), listItem.Text, htmlAttributes));
            sb.Append("</td></tr>");

            return sb.ToString();
        }
        public static string RadioButtonLabel(string inputId, string displayText,
                                     IDictionary<string, object> htmlAttributes)
        {
            var labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", inputId);
            labelBuilder.MergeAttributes(htmlAttributes);
            labelBuilder.InnerHtml = displayText;

            return labelBuilder.ToString(TagRenderMode.Normal);
        }
        
        public static TProperty GetValue<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            TModel model = htmlHelper.ViewData.Model;
            if (model == null)
            {
                return default(TProperty);
            }
            Func<TModel, TProperty> func = expression.Compile();
            return func(model);
        }

        private static string GetTypeDescriptionIfAvailable(Type type, string typeName)
        {
            var enumValue = (Enum) Enum.Parse(type, typeName);
            var fi = enumValue.GetType().GetField(typeName);
            var attr = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);
            if (attr.Length > 0)
            {
                return attr[0].Description;
            }
            else
            {
                return typeName;
            }
        }
    }
}