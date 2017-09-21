window.InfinniUI = window.InfinniUI || {};
window.InfinniUI.config = window.InfinniUI.config || {};

// перекрываем дефолтные конфиги, лежащие в InfinniUI/app/config.js

window.InfinniUI.config.cacheMetadata = false;
window.InfinniUI.config.serverUrl = 'http://' + window.location.host;
window.InfinniUI.config.configName = 'Server.UI';
window.InfinniUI.config.signalRHubName = "SignalRPushNotificationServiceHub";

window.InfinniUI.config.homePage = '/views/homeView.json';
window.InfinniUI.config.disableGetCurrentUser = true;
window.InfinniUI.config.disableSignInExternalForm = true;
window.InfinniUI.config.lang = 'en-US';