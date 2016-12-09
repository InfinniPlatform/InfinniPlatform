/** Осуществляет подписку на сообщения SignalR * 
 * @param {any} viewContext Контекст
 * @param {any} viewArgs Аргументы
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
            context.dataSources.AppsDataSource.updateItems();
            context.dataSources.TasksDataSource.updateItems();
        },
        viewContext);

    InfinniUI.global.notificationSubscription.subscribe("Init",
        function (context, args) {
            toastr.success(args.message);
            context.dataSources.AppsDataSource.updateItems();
            context.dataSources.TasksDataSource.updateItems();
        },
        viewContext);
}

/** Открывает/скачивает файл лога * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 * @param {any} name Имя сценария
 */
function GetNodeLog(context, args, name) {
    var agent = context.dataSources.AgentsDataSource.getSelectedItem();
    var template = window.InfinniUI.config.serverUrl + "/server";

    switch (name) {
        case 'tab':
            template = template + "/nodeLog?Address={0}&Port={1}";
            break;
        case 'file':
            template = template + "/infinniNode.log?Address={0}&Port={1}";
            break;
        default:
            break;
    }

    var replacements = [
        agent.Address,
        agent.Port
    ];

    var url = InfinniUI.StringUtils.format(template, replacements);

    window.open(url);
}

/** Открывает/скачивает файл лога * 
 * @param {any} context Контекст
 * @param {any} args Аргументы
 * @param {any} name Имя сценария
 */
function GetLog(context, args, name) {
    var app = context.dataSources.AppsDataSource.getSelectedItem();
    if (app === null || app === undefined) {
        return;
    }

    var agent = context.dataSources.AgentsDataSource.getSelectedItem();
    var template = window.InfinniUI.config.serverUrl + "/server";

    switch (name) {
        case 'eventsTab':
            template = template + "/appLog?Address={0}&Port={1}&FullName={2}";
            break;
        case 'eventsFile':
            template = template + "/events.log?Address={0}&Port={1}&FullName={2}";
            break;
        case 'perfTab':
            template = template + "/perfLog?Address={0}&Port={1}&FullName={2}";
            break;
        case 'perfFile':
            template = template + "/performance.log?Address={0}&Port={1}&FullName={2}";
            break;
        default:
            break;
    }

    var replacements = [
        agent.Address,
        agent.Port,
        app.FullName
    ];

    var url = InfinniUI.StringUtils.format(template, replacements);

    window.open(url);
}

function OnRestDataSourceError(context, args) {
    toastr.error(args.data.data.responseJSON.Error);
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
    if (args.value === "Stopped") {
        return true;
    }
}

function GetProcessInfoStateStopButtonVisibility(context, args) {
    if (args.value === "Running") {
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
        return "Completed";
    } else {
        return "Working";
    }
}