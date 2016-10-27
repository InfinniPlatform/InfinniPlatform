function UpdateAppsGrid(context, args) {
    var selectedItem = context.controls.AgentsTabPanel.getSelectedItem();
    
    if (selectedItem !== null && selectedItem !== undefined) {
        var selectedText = selectedItem.getText();
        var associatedItem = args.getTag();
        context.dataSources.AgentsDataSource.setSelectedItem(associatedItem);
    }
}

function AgentInfoHeaderConverter(context, args) {
    var replacements = [
        args.value.Name,
        args.value.Address,
        args.value.Port
    ];

    var headerText = InfinniUI.StringUtils.format("{0} ({1}:{2})", replacements);

    return headerText;
}

function PostAppExtensionConfig(context) {
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

function ShowInstallationLog(context, args) {
    context.controls.InstallLogTextBox.setVisible(true);
    context.controls.InstallLogTextBox.setValue(args.data.Result.Output);
}

function RefreshAppsDataSource(context) {
    context.dataSources.AppsDataSource.updateItems();
}