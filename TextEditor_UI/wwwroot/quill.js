var cursorPosition = 0;

(function () {
    window.QuillFunctions = {
        createQuill: function (
            quillElement, toolBar, readOnly,
            placeholder, theme, debugLevel) {

            Quill.register('modules/blotFormatter', QuillBlotFormatter.default);

            var options = {
                debug: debugLevel,
                modules: {
                    toolbar: toolBar,
                    blotFormatter: {}
                },
                placeholder: placeholder,
                readOnly: readOnly,
                theme: theme
            };

            new Quill(quillElement, options);

            quillElement.__quill.on('text-change', function (delta, oldDelta, source) {
                if (source == 'api') {
                    console.log("An API call triggered this change.");
                } else if (source == 'user') {
                    console.log("A user action triggered this change.");
                    DotNet.invokeMethodAsync('OurTextEditor', 'TextChanged');
                }
            });
        },
        getQuillContent: function (quillElement) {
            return JSON.stringify(quillElement.__quill.getContents());
        },
        getQuillText: function (quillElement) {
            return quillElement.__quill.getText();
        },
        getQuillHTML: function (quillElement) {
            return quillElement.__quill.root.innerHTML;
        },
        loadQuillContent: function (quillElement, quillContent) {
            content = JSON.parse(quillContent);
            return quillElement.__quill.setContents(content, 'api');
        },
        loadQuillHTMLContent: function (quillElement, quillHTMLContent) {
            cursorPosition = quillElement.__quill.getSelection();
            if (cursorPosition) {
                cursorPosition = cursorPosition.index;
            }
            return quillElement.__quill.root.innerHTML = quillHTMLContent;
            //return quillElement.__quill.clipboard.dangerouslyPasteHTML(quillHTMLContent, 'api');
        },
        loadQuillHTMLContent2: function (quillElement, quillHTMLContent) {
            // return quillElement.__quill.root.innerHTML = quillHTMLContent;
           // const delta = quillElement.__quill.clipboard.convert(quillHTMLContent);
            //return quillElement.__quill.setText(delta);
            return quillElement.__quill.insertText(0, 'Hello', 'bold', true);
        },
        focus: function (quillElement) {
            quillElement.__quill.focus();
            return quillElement.__quill.setSlection(cursorPosition, 0);
        },
        enableQuillEditor: function (quillElement, mode) {
            quillElement.__quill.enable(mode);
        },
        insertQuillImage: function (quillElement, imageURL) {
            var Delta = Quill.import('delta');
            editorIndex = 0;

            if (quillElement.__quill.getSelection() !== null) {
                editorIndex = quillElement.__quill.getSelection().index;
            }

            return quillElement.__quill.updateContents(
                new Delta()
                    .retain(editorIndex)
                    .insert({ image: imageURL },
                        { alt: imageURL }));
        }
    };
})();