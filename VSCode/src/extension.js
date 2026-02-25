/*const vscode = require('vscode');

    function activate(context) {

    // Color provide.
    const colorProvider = vscode.languages.registerColorProvider('cscd', {

        provideDocumentColors(document) {
            const text = document.getText();
            const regex = /#[0-9a-fA-F]{3,8}\b/g;

            const colors = [];
            let match;

            while ((match = regex.exec(text))) {

                const start = document.positionAt(match.index);
                const end = document.positionAt(match.index + match[0].length);
                const range = new vscode.Range(start, end);

                const color = parseHexColor(match[0]);

                if (color) {
                    colors.push(new vscode.ColorInformation(range, color));
                }
            }

            return colors;
        },

        provideColorPresentations(color, context) {

            const hex = toHex(color);

            return [
                new vscode.ColorPresentation(hex)
            ];
        }
    });

    context.subscriptions.push(colorProvider);
    
    // Hover provider.
    const hoverProvider = vscode.languages.registerHoverProvider('cscd', {
        provideHover(document, position) {

            const range = document.getWordRangeAtPosition(position, /[^,\s\[\]\{\}\<\>:]+/);
            if (!range) return;

            const text = document.getText(range);

            if (/^-?\d+$/.test(text)) {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Integer**\n\n`);
                md.appendMarkdown(`Value: ${parseInt(text, 10)}\n\n`);
                md.appendMarkdown(`Hex: \`0x${parseInt(text, 10).toString(16)}\``);
                return new vscode.Hover(md);
            }

            if (text === '#') {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Color**\n\n`);
                md.appendMarkdown(`\`#000000\``);
                return new vscode.Hover(md);
            }
            if (/^#[0-9a-fA-F]{3}$/.test(text)
            || /^#[0-9a-fA-F]{4}$/.test(text)
            || /^#[0-9a-fA-F]{6}$/.test(text)
            || /^#[0-9a-fA-F]{8}$/.test(text)) {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Color**\n\n`);
                md.appendMarkdown(`\`${text}\``);
                return new vscode.Hover(md);
            }

            if (text === 'null') {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Null**`);
                return new vscode.Hover(md);
            }

            if (text === 'true' || text === 'false') {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Boolean**\n\n`);
                md.appendMarkdown(`Value: ${text}`);
                return new vscode.Hover(md);
            }

            if (text === 'nan') {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Not A Number**`);
                return new vscode.Hover(md);
            }

            if (text === 'inf' || text === '-inf') {
                const md = new vscode.MarkdownString();
                md.appendMarkdown(`**Infinity**\n\n`);
                md.appendMarkdown(`Value: ${text}`);
                return new vscode.Hover(md);
            }
        }
    });

    context.subscriptions.push(hoverProvider);
}

function deactivate() {}

module.exports = { activate, deactivate };*/