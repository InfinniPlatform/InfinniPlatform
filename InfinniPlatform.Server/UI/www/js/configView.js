/** Отправляет POST-запрос с измененным файлом конфигурации AppExtension.json * 
 * @param {any} context Контекст
 */
function PostAppExtensionConfig(context) {
    //TODO Replace with server action
    var config = context.dataSources.AppExtensionSource.getSelectedItem();
    var data = JSON.stringify({
        "Config": JSON.parse(config)
    });

    var args = [window.InfinniUI.config.serverUrl,
        context.parameters.AgentAddress.getValue(),
        context.parameters.AgentPort.getValue(),
        context.parameters.AppFullName.getValue()
    ];

    var url = InfinniUI.StringUtils.format("{0}/server/config?Address={1}&Port={2}&AppFullName={3}&FileName=AppExtension.json", args);

    $.ajax({
        type: 'post',
        url: url,
        xhrFields: {
            withCredentials: true
        },
        data: data,
        contentType: "application/json;charset=UTF-8"
    });
}