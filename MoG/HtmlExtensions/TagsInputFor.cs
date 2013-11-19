using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace MoG
{
    public static class TagsInput
    {
        /// <summary>
        /// render something like that 
        /// <input type="text" value="@Model.Tags" data-role="tagsinput" class="[cssClass]" />
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        public static MvcHtmlString TagsInputFor<TModel, TProperty>(
         this HtmlHelper<TModel> helper,
         Expression<Func<TModel, TProperty>> expression, string cssClass)
        {
            return helper.TextBoxFor(expression, new { @class = cssClass, data_role = "tagsinput"});

        }

        public static MvcHtmlString TagsLabelFor(
         this HtmlHelper helper,
        string value, string cssClass)
        {
            String[] values = value.Split( ',');
            string html = "";
            foreach (string tag in values)
            {
                TagBuilder element = new TagBuilder("span");
                element.AddCssClass("badge color_good");
                element.SetInnerText(tag);
                html += element.ToString();
            }
           

            return new MvcHtmlString(html);

        }
    }
}