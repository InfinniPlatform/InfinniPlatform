/** Отправляет POST-запрос с измененным файлом конфигурации AppExtension.json * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function Subscribe(context, args) {
    var viewContext = context;

    InfinniUI.global.notificationSubscription.startConnection(window.InfinniUI.config.signalRHubName);
    InfinniUI.global.notificationSubscription.subscribe("WorkLog",
        function (context, args) {
            toastr.success(args.message);
        },
        viewContext);

    InfinniUI.global.notificationSubscription.subscribe("Install",
        function (context, args) {
            var message = JSON.parse(args.message);
            toastr.success("Installing " + message.AppName + " started.");
            context.dataSources.AppsDataSource.setSelectedItem(message);
        },
        viewContext);
}

/** Форматирует статус для отображения в заголовке TabPage * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 * @returns {string} Текст заголовка TabPage
 */
function ConvertAgentInfoHeader(context, args) {
    var replacements = [
        args.value.Name,
        args.value.Address,
        args.value.Port
    ];

    var headerText = InfinniUI.StringUtils.format("{0} ({1}:{2})", replacements);

    return headerText;
}

/** Обновляет содержимое источника данных о приложениях * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function RefreshAppsDataSource(context) {
    context.dataSources.AppsDataSource.updateItems();
}

/** Обновляет данные в гриде данных о приложениях * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function UpdateAppsGrid(context, args) {
    var selectedItem = context.controls.AgentsTabPanel.getSelectedItem();

    if (selectedItem !== null && selectedItem !== undefined) {
        var selectedText = selectedItem.getText();
        //В Tag должен содержаться объект, соответствующий выбранной странице в TabPanel
        var associatedItem = args.getTag();
        context.dataSources.AgentsDataSource.setSelectedItem(associatedItem);
    }
}

/** Обновляет данные в гриде о задачах * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function UpdateTasksGrid(context, args) {
    if (args.getText() === "Tasks") {
        context.dataSources.TasksDataSource.updateItems();
    }
    if (args.getText() === "Apps") {
        context.dataSources.AppsDataSource.updateItems();
    }
}

/** Показывает/скрывает кнопки для управления приложением * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function EnableAppButtons(context, args) {
    if (args.value === null || args.value === undefined) {
        context.controls.AppButtonsStackPanel.setVisible(false);
    } else {
        context.controls.AppButtonsStackPanel.setVisible(true);
    }
}

/** Показывает/скрывает кнопки для управления задачами * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function EnableTaskButtons(context, args) {
    if (args.value === null || args.value === undefined) {
        context.controls.OutputButton.setVisible(false);
    } else {
        context.controls.OutputButton.setVisible(true);
    }
}

/** Обновляет статус выбранной задачи * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function GetTasksStatus(context, args) {
    context.dataSources.TasksDataSource.updateItems();
}