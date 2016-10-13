window.InfinniUI = window.InfinniUI || {};
window.InfinniUI.config = window.InfinniUI.config || {};
window.InfinniUI.config.serverUrl = "http://localhost:9901";
window.InfinniUI.config.configName = "IP.Server";
window.InfinniUI.config.homePage = "/metadata/Views/HomePage.json";

window.InfinniUI.config.signalRHubName = "InfinniPlatformServerHub";

// TODO Выяснить зачем это:

window.InfinniUI.config.disableLayoutManager = true;
window.InfinniUI.config.disableGetCurrentUser = true;
window.InfinniUI.config.disableSignInExternalForm = true;
window.InfinniUI.config.HistoryAPI = {
    pushState: true
};

//window.InfinniUI.config.Routes = [
//  {
//      Name: "HomePageRoute",
//      Path: "/",
//      Action: "{ routeCallback(context, args) }"
//  },
//  {
//      Name: "UserPageRoute",
//      Path: "/user/<% userId %>",
//      Action: "{ routeCallback3(context, args) }"
//  }
//];