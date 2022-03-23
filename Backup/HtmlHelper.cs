using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;


    public static class CustomHelpers
    {
        public static MvcHtmlString AccountBox(this HtmlHelper helper, string name, int MaxLength, int MinLength = 0, string value = "")
        {
            var builder = new StringBuilder();
            var table = new TagBuilder("table");
            table.Attributes["class"] = "tblacc";
            var tr = new TagBuilder("tr");
            StringBuilder sb = new StringBuilder();
            string modelvalue = value ?? "";
            
            char[] arry = modelvalue.PadLeft(9,'0').ToCharArray();
            int count = arry.Count();
            for (int i = 1; i <= MaxLength; i++)
            {
                var td = new TagBuilder("td");

                var tb = new TagBuilder("input");
                tb.Attributes["type"] = "text";
                tb.Attributes["id"] = name + "acc-" + i.ToString();
                tb.Attributes["class"] = "acc";
                tb.Attributes["onblur"] = "TbValidate(this)";
                if ((i - 1) < count)
                {
                    tb.Attributes["value"] = arry[i - 1].ToString();
                }
                td.InnerHtml = tb.ToString(TagRenderMode.SelfClosing);

                sb.Append(td);
            }

            tr.InnerHtml = sb.ToString();

            table.InnerHtml = tr.ToString();
            builder.Append(table);
            builder.Append("&nbsp;<span class='errmsg' id=" + name + "errmsg></span>");
            var txt = new TagBuilder("input");
            txt.Attributes["type"] = "text";
            txt.Attributes["id"] = name + "tbacc";
            txt.Attributes["style"] = "display:none;";
            txt.Attributes["value"] = modelvalue;
            txt.Attributes["name"] = name;


            builder.Append(txt.ToString(TagRenderMode.SelfClosing));

            var txt1 = new TagBuilder("input");
            txt1.Attributes["type"] = "text";
            txt1.Attributes["id"] = name + "min";
            txt1.Attributes["style"] = "display:none;";
            txt1.Attributes["value"] = MinLength.ToString();



            builder.Append(txt1.ToString(TagRenderMode.SelfClosing));

            var txt2 = new TagBuilder("input");
            txt2.Attributes["type"] = "text";
            txt2.Attributes["id"] = name + "max";
            txt2.Attributes["style"] = "display:none;";
            txt2.Attributes["value"] = MaxLength.ToString();

            builder.Append(txt2.ToString(TagRenderMode.SelfClosing));

            //builder.Append("<input type='text' Id='tbacc' name=" + name + " style='display:none;' value=" + value + " />");

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString AccountBoxFor<TModel, TProperty>
(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int MaxLength)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return AccountBox(htmlHelper, name, MaxLength, 0, metadata.Model as string);
        }

        public static MvcHtmlString AccountBoxFor<TModel, TProperty>
(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int MaxLength, int MinLength)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return AccountBox(htmlHelper, name, MaxLength, MinLength, metadata.Model as string);
        }
    }
