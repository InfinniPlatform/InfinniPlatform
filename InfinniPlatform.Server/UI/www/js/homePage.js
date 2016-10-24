function ShowAppsPanel(context) {
    context.controls.AppsPanel.setVisible(true);
}

function PostConfigFile(context) {
    var item = context.dataSources.ConfigDataSource.getSelectedItem();
    var data = { "Config": JSON.parse(item) };

    $.ajax({
        type: 'post',
        url: "http://localhost:9901/server/config?Address=localhost&Port=9901&AppFullName=UpravdomGkh.1.1.0.2133-default&FileName=AppExtension.json",
        xhrFields: {
            withCredentials: true
        },
        data: JSON.stringify(data),
        contentType: "application/json;charset=UTF-8"
    });
}
