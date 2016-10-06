function Subscribe(context, args) {

    //Сохраняем контекст view'шки для передачи в функцию обработки нотификации.
    var viewContext = context;

    //Подключаемся к сервису нотификаций.
    //Переменная 'window.InfinniUI.config.signalRHubName' задается в файле конфигурации (см .config.js).
    InfinniUI.global.notificationSubsription.startConnection(window.InfinniUI.config.signalRHubName);

    //Подписываемся на нотификации по ключу 'HomePage'.
    InfinniUI.global.notificationSubsription.subscribe("HomePage",
        function (context, args) {
            //Обрабатываем событие получения нотификации.
            context.controls.Label.setValue(args.message);
        },
        viewContext);
}