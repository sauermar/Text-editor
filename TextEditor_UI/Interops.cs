using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace OurTextEditor
{
    public static class CursorInterop
    {
        public static IJSRuntime JsRuntime = null;

        public static ValueTask<bool> Alert(string msg)
        {
            return JsRuntime.InvokeAsync<bool>(
                "CursorPositionFunctions.Alert", msg);
        }

        public static ValueTask<string> GetCaretCoordinates(string elementId)
        {
            return JsRuntime.InvokeAsync<string>(
                "CursorPositionFunctions.getCaretCoordinates", elementId);
        }

        public static ValueTask<double> GetElementActualTop(string elementId)
        {
            return JsRuntime.InvokeAsync<double>(
                "CursorPositionFunctions.GetElementActualTop", elementId);
        }

        public static ValueTask<double> GetElementActualLeft(string elementId)
        {
            return JsRuntime.InvokeAsync<double>(
                "CursorPositionFunctions.GetElementActualLeft", elementId);
        }

        public static ValueTask<int> GetCharCursorPosition(string elementId)
        {
            return JsRuntime.InvokeAsync<int>("CursorPositionFunctions.GetCharCursorPosition", elementId);
        }
    }

    public static class QuillEditorInterops
    {
        internal static ValueTask<object> CreateQuill(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            ElementReference toolbar,
            bool readOnly,
            string placeholder,
            string theme,
            string debugLevel)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.createQuill",
                quillElement, toolbar, readOnly,
                placeholder, theme, debugLevel);
        }

        internal static ValueTask<string> GetText(
            IJSRuntime jsRuntime,
            ElementReference quillElement)
        {
            return jsRuntime.InvokeAsync<string>(
                "QuillFunctions.getQuillText",
                quillElement);
        }

        internal static ValueTask<string> Focus(
            IJSRuntime jsRuntime,
            ElementReference quillElement)
        {
            return jsRuntime.InvokeAsync<string>(
                "QuillFunctions.focus",
                quillElement);
        }

        internal static ValueTask<string> GetHTML(
            IJSRuntime jsRuntime,
            ElementReference quillElement)
        {
            return jsRuntime.InvokeAsync<string>(
                "QuillFunctions.getQuillHTML",
                quillElement);
        }

        internal static ValueTask<string> GetContent(
            IJSRuntime jsRuntime,
            ElementReference quillElement)
        {
            return jsRuntime.InvokeAsync<string>(
                "QuillFunctions.getQuillContent",
                quillElement);
        }

        internal static ValueTask<object> LoadQuillContent(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string Content)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.loadQuillContent",
                quillElement, Content);
        }

        internal static ValueTask<object> LoadQuillHTMLContent(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string quillHTMLContent)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.loadQuillHTMLContent",
                quillElement, quillHTMLContent);
        }

        internal static ValueTask<object> LoadQuillHTMLContent2(
        IJSRuntime jsRuntime,
        ElementReference quillElement,
        string quillHTMLContent)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.loadQuillHTMLContent2",
                quillElement, quillHTMLContent);
        }

        internal static ValueTask<object> EnableQuillEditor(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            bool mode)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.enableQuillEditor",
                quillElement, mode);
        }

        internal static ValueTask<object> InsertQuillImage(
            IJSRuntime jsRuntime,
            ElementReference quillElement,
            string imageURL)
        {
            return jsRuntime.InvokeAsync<object>(
                "QuillFunctions.insertQuillImage",
                quillElement, imageURL);
        }
    }
}