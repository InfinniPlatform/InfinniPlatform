function Subscribe(context, args) {

    var viewContext = context;

    InfinniUI.global.notificationSubsription.startConnection(window.InfinniUI.config.signalRHubName);

    InfinniUI.global.notificationSubsription.subscribe("HomePage",
        function (context, args) {            
            context.controls.Label.setValue(args.message);
        },
        viewContext);
}

function ShowInfo(context, args) {
    
    debugger;
    var agents = context.dataSources.AgentsDataSource.getItems();
    
}