let appSettings: any = null;
export const loadConfig = async () => {
    const response = await fetch("/appsettings.json");
    appSettings = await response.json();
};

export const getConfig = () => {
    if (!appSettings) {
        throw new Error("Config not loaded");
    }
    return appSettings;
};


export const ApiServerBaseUrl = () =>{
    if (!appSettings) {
        throw new Error("Config not loaded");
    }
    return appSettings.ApiServerBaseUrl;
}