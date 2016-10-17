function Subscribe(context) {
    var viewContext = context;

    InfinniUI.global.notificationSubsription.startConnection(window.InfinniUI.config.signalRHubName);

    InfinniUI.global.notificationSubsription.subscribe("HomePage",
        function (context, args) {
            context.controls.Label.setValue(args.message);
        },
        viewContext);
}

function ShowAppsPanel(context) {
    context.controls.AppsPanel.setVisible(true);
}