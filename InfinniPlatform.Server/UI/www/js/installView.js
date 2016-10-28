/** Регистрирует обработчик события сохранения документа для источника данных о приложении * 
 * @param {any} context Контекст
 * @param {any} args Аргументы 
 */
function RegisterOnSavedHandler(context, args) {
    context.dataSources.AppsDataSource.onItemSaved(function (context, args) {
        SetButtonsAppearance(context, args);

        context.controls.InstallLogTextBox.setVisible(true);
        context.controls.InstallLogTextBox.setValue(args.value.result.Result.Output);
    });
}

/** Управляет внешним видом кнопки установки * 
 * @param {any} context Контекст
 * @param {any} args Аргументы 
 */
function SetButtonsAppearance(context, args) {
    var buttonText = context.controls.InstallButton.getText();

    if (buttonText === "Install") {
        context.controls.InstallButton.setText("Installing...");
        setTimeout(function () {
            context.controls.InstallButton.setEnabled(false);
        }, 0);
    }

    if (buttonText === "Installing...") {
        context.controls.InstallButton.setVisible(false);
        context.controls.CloseButton.setVisible(true);        
    }
}