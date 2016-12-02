/** Отправляет POST-запрос с измененным файлом конфигурации AppExtension.json * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function Subscribe(viewContext, viewArgs) {

    InfinniUI.global.notificationSubscription.startConnection(window.InfinniUI.config.signalRHubName);
    InfinniUI.global.notificationSubscription.subscribe("WorkLog",
        function (context, args) {
            toastr.success(args.message);
        },
        viewContext);

    InfinniUI.global.notificationSubscription.subscribe("Install",
        function (context, args) {
            toastr.success(args.message);
            RefreshAppsDataSource(context);
        },
        viewContext);

    InfinniUI.global.notificationSubscription.subscribe("Init",
        function (context, args) {
            toastr.success(args.message);
            RefreshAppsDataSource(context);
        },
        viewContext);
}

/** Обновляет содержимое источника данных о приложениях * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 */
function RefreshAppsDataSource(context) {
    context.dataSources.AppsDataSource.updateItems();
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

function OpenEventsLogInTab(context, args) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();
    var app = context.dataSources.AppsDataSource.getSelectedItem();

    var replacements = [
        agent.Address,
        agent.Port,
        app.AppFullName
    ];

    var url = InfinniUI.StringUtils.format("http://localhost:9901/server/appLog?Address={0}&Port={1}&AppFullName={2}", replacements);

    window.open(url);
}

function OpenPerfLogInTab(context, args) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();
    var app = context.dataSources.AppsDataSource.getSelectedItem();

    var replacements = [
        agent.Address,
        agent.Port,
        app.AppFullName
    ];

    var url = InfinniUI.StringUtils.format("http://localhost:9901/server/perfLog?Address={0}&Port={1}&AppFullName={2}", replacements);

    window.open(url);
}

function OpenNodeLogInTab(context, args) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();

    var replacements = [
        agent.Address,
        agent.Port
    ];

    var url = InfinniUI.StringUtils.format("http://localhost:9901/server/nodeLog?Address={0}&Port={1}", replacements);

    window.open(url);
}

function DownloadEventsLogFile(context, args) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();
    var app = context.dataSources.AppsDataSource.getSelectedItem();

    var replacements = [
        agent.Address,
        agent.Port,
        app.AppFullName
    ];

    var url = InfinniUI.StringUtils.format("http://localhost:9901/server/events.log?Address={0}&Port={1}&AppFullName={2}", replacements);

    window.open(url);
}

function DownloadPerfLogFile(context, args) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();
    var app = context.dataSources.AppsDataSource.getSelectedItem();

    var replacements = [
        agent.Address,
        agent.Port,
        app.AppFullName
    ];

    var url = InfinniUI.StringUtils.format("http://localhost:9901/server/performance.log?Address={0}&Port={1}&AppFullName={2}", replacements);

    window.open(url);
}

function DownloadNodeLogFile(context, args) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();

    var replacements = [
        agent.Address,
        agent.Port
    ];

    var url = InfinniUI.StringUtils.format("http://localhost:9901/server/infinniNode.log?Address={0}&Port={1}", replacements);

    window.open(url);
}

function GetProcessInfoStateIconValue(context, args) {
    switch (args.value) {
        case "Stopped":
            return "stop-circle";
        case "Running":
            return "play-circle";
        default:
            break;
    }
}

function GetProcessInfoStateStartButtonVisibility(context, args) {
    if(args.value==="Stopped"){
        return true;
    }
}

function GetProcessInfoStateStopButtonVisibility(context, args) {
    if(args.value==="Running"){
        return true;
    }
}

function GetProcessInfoStateIconForeground(context, args) {
    switch (args.value) {
        case "Stopped":
            return "accent2";
        case "Running":
            return "accent1";
        default:
            break;
    }
}

function ConvertTaskState(context, args) {
    if (args.value) {
        return 'Completed';
    } else {
        return 'Working';
    }
}